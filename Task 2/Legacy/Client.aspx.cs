using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClientManagementWebforms.Data;
using ClientManagementWebforms.Infrastructure;

namespace ClientManagementWebforms
{
    public partial class Client : Page
    {
        private int ClientId
        {
            get
            {
                object value = ViewState["ClientId"];
                if (value == null)
                {
                    return 0;
                }

                return (int)value;
            }
            set { ViewState["ClientId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id;
                if (int.TryParse(Request["id"], out id))
                {
                    ClientId = id;
                }

                BindStatusLookup();
                BindNoteTypeLookup();

                if (ClientId > 0)
                {
                    LoadClient();
                    BindAllRelated(0);
                    btnDelete.Visible = true;
                    pnlRelated.Visible = true;
                }
                else
                {
                    if (ddlStatus.Items.Count > 0)
                    {
                        ddlStatus.SelectedIndex = 0;
                    }

                    btnDelete.Visible = false;
                    pnlRelated.Visible = false;
                }

                if (string.Equals(Request["saved"], "1", StringComparison.Ordinal))
                {
                    ShowMessage("Saved.");
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            DateTime? dob = null;
            if (!string.IsNullOrWhiteSpace(txtDob.Text))
            {
                DateTime parsed;
                if (!DateTime.TryParseExact(txtDob.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
                {
                    ShowMessage("Enter date of birth in dd/MM/yyyy format.");
                    return;
                }

                dob = parsed;
            }

            string firstName = txtFirst.Text.Trim();
            string lastName = txtLast.Text.Trim();
            string email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
            string phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim();

            int? statusId = null;
            if (!string.IsNullOrWhiteSpace(ddlStatus.SelectedValue))
            {
                int id;
                if (int.TryParse(ddlStatus.SelectedValue, out id))
                {
                    statusId = id;
                }
            }

            try
            {
                DataTable result;

                if (ClientId > 0)
                {
                    result = Repositories.Client_Update(ClientId, firstName, lastName, dob, email, phone, statusId);
                }
                else
                {
                    result = Repositories.Client_Create(firstName, lastName, dob, email, phone, statusId);
                }

                int newId = ClientId;
                if (result.Rows.Count > 0)
                {
                    object idValue = result.Rows[0]["ClientId"];
                    if (idValue != null && idValue != DBNull.Value)
                    {
                        int.TryParse(idValue.ToString(), out newId);
                    }
                }

                if (newId <= 0)
                {
                    ShowMessage("An error occurred while saving the client.");
                    return;
                }

                Response.Redirect("Client.aspx?id=" + newId + "&saved=1");
            }
            catch (Exception ex)
            {
                Logger.Error("Error saving client", ex);
                ShowMessage("An error occurred while saving the client.");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (ClientId <= 0)
            {
                return;
            }

            try
            {
                Repositories.Client_Delete(ClientId, true);
                Response.Redirect("Clients.aspx");
            }
            catch (Exception ex)
            {
                Logger.Error("Error deleting client from details page", ex);
                ShowMessage("An error occurred while deleting the client.");
            }
        }

        protected void btnAddJournal_Click(object sender, EventArgs e)
        {
            if (ClientId <= 0)
            {
                ShowMessage("Save the client before adding related records.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtJournalBody.Text))
            {
                ShowMessage("Journal body is required.");
                return;
            }

            int noteTypeId;
            if (!int.TryParse(ddlNoteType.SelectedValue, out noteTypeId))
            {
                ShowMessage("Select a journal note type.");
                return;
            }

            try
            {
                string body = txtJournalBody.Text.Trim();
                string author = string.IsNullOrWhiteSpace(txtJournalAuthor.Text) ? null : txtJournalAuthor.Text.Trim();

                Repositories.Journal_Create(ClientId, noteTypeId, DateTime.UtcNow, body, author);

                txtJournalBody.Text = string.Empty;
                txtJournalAuthor.Text = string.Empty;

                gvJournal.PageIndex = 0;
                BindJournal(0);
            }
            catch (Exception ex)
            {
                Logger.Error("Error adding journal entry", ex);
                ShowMessage("An error occurred while adding the journal entry.");
            }
        }

        protected void gvJournal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvJournal.PageIndex = e.NewPageIndex;
            BindJournal(e.NewPageIndex);
        }

        protected void gvJournal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.Equals(e.CommandName, "DeleteJournal", StringComparison.OrdinalIgnoreCase))
            {
                int id;
                if (int.TryParse(Convert.ToString(e.CommandArgument), out id))
                {
                    try
                    {
                        Repositories.Journal_Delete(id);
                        BindJournal(gvJournal.PageIndex);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error deleting journal entry", ex);
                        ShowMessage("An error occurred while deleting the journal entry.");
                    }
                }
            }
        }

        protected void btnAddAsset_Click(object sender, EventArgs e)
        {
            if (ClientId <= 0)
            {
                ShowMessage("Save the client before adding related records.");
                return;
            }

            string assetType = txtAssetType.Text.Trim();

            decimal value;
            if (!decimal.TryParse(txtAssetValue.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out value))
            {
                ShowMessage("Enter numeric asset value.");
                return;
            }

            string provider = string.IsNullOrWhiteSpace(txtAssetProvider.Text) ? null : txtAssetProvider.Text.Trim();

            try
            {
                Repositories.Asset_Create(ClientId, assetType, value, provider, DateTime.UtcNow);

                txtAssetType.Text = string.Empty;
                txtAssetValue.Text = string.Empty;
                txtAssetProvider.Text = string.Empty;

                gvAssets.PageIndex = 0;
                BindAssets(0);
            }
            catch (Exception ex)
            {
                Logger.Error("Error adding asset", ex);
                ShowMessage("An error occurred while adding the asset.");
            }
        }

        protected void gvAssets_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAssets.PageIndex = e.NewPageIndex;
            BindAssets(e.NewPageIndex);
        }

        protected void gvAssets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.Equals(e.CommandName, "DeleteAsset", StringComparison.OrdinalIgnoreCase))
            {
                int id;
                if (int.TryParse(Convert.ToString(e.CommandArgument), out id))
                {
                    try
                    {
                        Repositories.Asset_Delete(id);
                        BindAssets(gvAssets.PageIndex);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error deleting asset", ex);
                        ShowMessage("An error occurred while deleting the asset.");
                    }
                }
            }
        }

        protected void btnAddLiab_Click(object sender, EventArgs e)
        {
            if (ClientId <= 0)
            {
                ShowMessage("Save the client before adding related records.");
                return;
            }

            string type = txtLiabType.Text.Trim();

            decimal balance;
            if (!decimal.TryParse(txtLiabBalance.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out balance))
            {
                ShowMessage("Enter numeric liability balance.");
                return;
            }

            decimal? rate = null;
            if (!string.IsNullOrWhiteSpace(txtLiabRate.Text))
            {
                decimal parsedRate;
                if (!decimal.TryParse(txtLiabRate.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out parsedRate))
                {
                    ShowMessage("Enter numeric liability rate.");
                    return;
                }

                rate = parsedRate;
            }

            try
            {
                Repositories.Liability_Create(ClientId, type, balance, rate, DateTime.UtcNow);

                txtLiabType.Text = string.Empty;
                txtLiabBalance.Text = string.Empty;
                txtLiabRate.Text = string.Empty;

                gvLiabilities.PageIndex = 0;
                BindLiabilities(0);
            }
            catch (Exception ex)
            {
                Logger.Error("Error adding liability", ex);
                ShowMessage("An error occurred while adding the liability.");
            }
        }

        protected void gvLiabilities_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLiabilities.PageIndex = e.NewPageIndex;
            BindLiabilities(e.NewPageIndex);
        }

        protected void gvLiabilities_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.Equals(e.CommandName, "DeleteLiability", StringComparison.OrdinalIgnoreCase))
            {
                int id;
                if (int.TryParse(Convert.ToString(e.CommandArgument), out id))
                {
                    try
                    {
                        Repositories.Liability_Delete(id);
                        BindLiabilities(gvLiabilities.PageIndex);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error deleting liability", ex);
                        ShowMessage("An error occurred while deleting the liability.");
                    }
                }
            }
        }

        protected void btnAddIncome_Click(object sender, EventArgs e)
        {
            if (ClientId <= 0)
            {
                ShowMessage("Save the client before adding related records.");
                return;
            }

            string source = txtIncomeSource.Text.Trim();

            decimal amount;
            if (!decimal.TryParse(txtIncomeAmount.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out amount))
            {
                ShowMessage("Enter numeric income amount.");
                return;
            }

            try
            {
                Repositories.Income_Create(ClientId, source, amount, DateTime.UtcNow);

                txtIncomeSource.Text = string.Empty;
                txtIncomeAmount.Text = string.Empty;

                gvIncome.PageIndex = 0;
                BindIncome(0);
            }
            catch (Exception ex)
            {
                Logger.Error("Error adding income", ex);
                ShowMessage("An error occurred while adding the income.");
            }
        }

        protected void gvIncome_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvIncome.PageIndex = e.NewPageIndex;
            BindIncome(e.NewPageIndex);
        }

        protected void gvIncome_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.Equals(e.CommandName, "DeleteIncome", StringComparison.OrdinalIgnoreCase))
            {
                int id;
                if (int.TryParse(Convert.ToString(e.CommandArgument), out id))
                {
                    try
                    {
                        Repositories.Income_Delete(id);
                        BindIncome(gvIncome.PageIndex);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error deleting income", ex);
                        ShowMessage("An error occurred while deleting the income.");
                    }
                }
            }
        }

        protected void btnAddEx_Click(object sender, EventArgs e)
        {
            if (ClientId <= 0)
            {
                ShowMessage("Save the client before adding related records.");
                return;
            }

            string category = txtExCat.Text.Trim();

            decimal amount;
            if (!decimal.TryParse(txtExAmt.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out amount))
            {
                ShowMessage("Enter numeric expenditure amount.");
                return;
            }

            try
            {
                Repositories.Expenditure_Create(ClientId, category, amount, DateTime.UtcNow);

                txtExCat.Text = string.Empty;
                txtExAmt.Text = string.Empty;

                gvExpenditure.PageIndex = 0;
                BindExpenditure(0);
            }
            catch (Exception ex)
            {
                Logger.Error("Error adding expenditure", ex);
                ShowMessage("An error occurred while adding the expenditure.");
            }
        }

        protected void gvExpenditure_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvExpenditure.PageIndex = e.NewPageIndex;
            BindExpenditure(e.NewPageIndex);
        }

        protected void gvExpenditure_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.Equals(e.CommandName, "DeleteExpenditure", StringComparison.OrdinalIgnoreCase))
            {
                int id;
                if (int.TryParse(Convert.ToString(e.CommandArgument), out id))
                {
                    try
                    {
                        Repositories.Expenditure_Delete(id);
                        BindExpenditure(gvExpenditure.PageIndex);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error deleting expenditure", ex);
                        ShowMessage("An error occurred while deleting the expenditure.");
                    }
                }
            }
        }

        private void BindStatusLookup()
        {
            try
            {
                var table = Repositories.LookupValue_Search("CLIENT_STATUS", true);
                ddlStatus.DataSource = table;
                ddlStatus.DataTextField = "Label";
                ddlStatus.DataValueField = "LookupValueId";
                ddlStatus.DataBind();
            }
            catch (Exception ex)
            {
                Logger.Error("Error loading client status lookup", ex);
                ShowMessage("An error occurred while loading status options.");
            }
        }

        private void BindNoteTypeLookup()
        {
            try
            {
                var table = Repositories.LookupValue_Search("JOURNAL_NOTE_TYPE", true);
                ddlNoteType.DataSource = table;
                ddlNoteType.DataTextField = "Label";
                ddlNoteType.DataValueField = "LookupValueId";
                ddlNoteType.DataBind();
            }
            catch (Exception ex)
            {
                Logger.Error("Error loading journal note type lookup", ex);
                ShowMessage("An error occurred while loading journal note types.");
            }
        }

        private void LoadClient()
        {
            try
            {
                var table = Repositories.Client_Read(ClientId);
                if (table.Rows.Count == 0)
                {
                    ShowMessage("Client not found.");
                    btnDelete.Visible = false;
                    pnlRelated.Visible = false;
                    return;
                }

                var row = table.Rows[0];

                txtFirst.Text = Convert.ToString(row["FirstName"]);
                txtLast.Text = Convert.ToString(row["LastName"]);

                if (row["DateOfBirth"] != DBNull.Value)
                {
                    var dob = (DateTime)row["DateOfBirth"];
                    txtDob.Text = dob.ToString("dd/MM/yyyy");
                }
                else
                {
                    txtDob.Text = string.Empty;
                }

                txtEmail.Text = row["Email"] != DBNull.Value ? Convert.ToString(row["Email"]) : string.Empty;
                txtPhone.Text = row["Phone"] != DBNull.Value ? Convert.ToString(row["Phone"]) : string.Empty;

                if (row["StatusLookupValueId"] != DBNull.Value)
                {
                    var statusId = Convert.ToInt32(row["StatusLookupValueId"]);
                    var item = ddlStatus.Items.FindByValue(statusId.ToString());
                    if (item != null)
                    {
                        ddlStatus.ClearSelection();
                        item.Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error loading client details", ex);
                ShowMessage("An error occurred while loading the client.");
            }
        }

        private void BindAllRelated(int pageIndex)
        {
            BindJournal(pageIndex);
            BindAssets(pageIndex);
            BindLiabilities(pageIndex);
            BindIncome(pageIndex);
            BindExpenditure(pageIndex);
        }

        private void BindJournal(int pageIndex)
        {
            if (ClientId <= 0)
            {
                gvJournal.DataSource = null;
                gvJournal.DataBind();
                return;
            }

            try
            {
                int pageSize = GetRelatedPageSize();
                gvJournal.PageSize = pageSize;

                var dataSet = Repositories.Journal_Search(ClientId, null, null, null, null, pageIndex + 1, pageSize, "OccurredAt", "DESC");

                DataTable rows = null;
                if (dataSet.Tables.Count > 0)
                {
                    rows = dataSet.Tables[0];
                }

                int totalCount = 0;
                if (dataSet.Tables.Count > 1 && dataSet.Tables[1].Rows.Count > 0)
                {
                    var value = dataSet.Tables[1].Rows[0][0];
                    if (value != null && value != DBNull.Value)
                    {
                        int.TryParse(value.ToString(), out totalCount);
                    }
                }

                gvJournal.VirtualItemCount = totalCount;
                gvJournal.DataSource = rows;
                gvJournal.DataBind();
            }
            catch (Exception ex)
            {
                Logger.Error("Error binding journal grid", ex);
                ShowMessage("An error occurred while loading journal entries.");
            }
        }

        private void BindAssets(int pageIndex)
        {
            if (ClientId <= 0)
            {
                gvAssets.DataSource = null;
                gvAssets.DataBind();
                return;
            }

            try
            {
                int pageSize = GetRelatedPageSize();
                gvAssets.PageSize = pageSize;

                var dataSet = Repositories.Asset_Search(ClientId, null, null, null, pageIndex + 1, pageSize, "AsOf", "DESC");

                DataTable rows = null;
                if (dataSet.Tables.Count > 0)
                {
                    rows = dataSet.Tables[0];
                }

                int totalCount = 0;
                if (dataSet.Tables.Count > 1 && dataSet.Tables[1].Rows.Count > 0)
                {
                    var value = dataSet.Tables[1].Rows[0][0];
                    if (value != null && value != DBNull.Value)
                    {
                        int.TryParse(value.ToString(), out totalCount);
                    }
                }

                gvAssets.VirtualItemCount = totalCount;
                gvAssets.DataSource = rows;
                gvAssets.DataBind();
            }
            catch (Exception ex)
            {
                Logger.Error("Error binding assets grid", ex);
                ShowMessage("An error occurred while loading assets.");
            }
        }

        private void BindLiabilities(int pageIndex)
        {
            if (ClientId <= 0)
            {
                gvLiabilities.DataSource = null;
                gvLiabilities.DataBind();
                return;
            }

            try
            {
                int pageSize = GetRelatedPageSize();
                gvLiabilities.PageSize = pageSize;

                var dataSet = Repositories.Liability_Search(ClientId, null, null, null, pageIndex + 1, pageSize, "AsOf", "DESC");

                DataTable rows = null;
                if (dataSet.Tables.Count > 0)
                {
                    rows = dataSet.Tables[0];
                }

                int totalCount = 0;
                if (dataSet.Tables.Count > 1 && dataSet.Tables[1].Rows.Count > 0)
                {
                    var value = dataSet.Tables[1].Rows[0][0];
                    if (value != null && value != DBNull.Value)
                    {
                        int.TryParse(value.ToString(), out totalCount);
                    }
                }

                gvLiabilities.VirtualItemCount = totalCount;
                gvLiabilities.DataSource = rows;
                gvLiabilities.DataBind();
            }
            catch (Exception ex)
            {
                Logger.Error("Error binding liabilities grid", ex);
                ShowMessage("An error occurred while loading liabilities.");
            }
        }

        private void BindIncome(int pageIndex)
        {
            if (ClientId <= 0)
            {
                gvIncome.DataSource = null;
                gvIncome.DataBind();
                return;
            }

            try
            {
                int pageSize = GetRelatedPageSize();
                gvIncome.PageSize = pageSize;

                var dataSet = Repositories.Income_Search(ClientId, null, null, null, pageIndex + 1, pageSize, "AsOf", "DESC");

                DataTable rows = null;
                if (dataSet.Tables.Count > 0)
                {
                    rows = dataSet.Tables[0];
                }

                int totalCount = 0;
                if (dataSet.Tables.Count > 1 && dataSet.Tables[1].Rows.Count > 0)
                {
                    var value = dataSet.Tables[1].Rows[0][0];
                    if (value != null && value != DBNull.Value)
                    {
                        int.TryParse(value.ToString(), out totalCount);
                    }
                }

                gvIncome.VirtualItemCount = totalCount;
                gvIncome.DataSource = rows;
                gvIncome.DataBind();
            }
            catch (Exception ex)
            {
                Logger.Error("Error binding income grid", ex);
                ShowMessage("An error occurred while loading income.");
            }
        }

        private void BindExpenditure(int pageIndex)
        {
            if (ClientId <= 0)
            {
                gvExpenditure.DataSource = null;
                gvExpenditure.DataBind();
                return;
            }

            try
            {
                int pageSize = GetRelatedPageSize();
                gvExpenditure.PageSize = pageSize;

                var dataSet = Repositories.Expenditure_Search(ClientId, null, null, null, pageIndex + 1, pageSize, "AsOf", "DESC");

                DataTable rows = null;
                if (dataSet.Tables.Count > 0)
                {
                    rows = dataSet.Tables[0];
                }

                int totalCount = 0;
                if (dataSet.Tables.Count > 1 && dataSet.Tables[1].Rows.Count > 0)
                {
                    var value = dataSet.Tables[1].Rows[0][0];
                    if (value != null && value != DBNull.Value)
                    {
                        int.TryParse(value.ToString(), out totalCount);
                    }
                }

                gvExpenditure.VirtualItemCount = totalCount;
                gvExpenditure.DataSource = rows;
                gvExpenditure.DataBind();
            }
            catch (Exception ex)
            {
                Logger.Error("Error binding expenditure grid", ex);
                ShowMessage("An error occurred while loading expenditure.");
            }
        }

        private int GetRelatedPageSize()
        {
            var value = ConfigurationManager.AppSettings["RelatedPageSize"];
            int pageSize;
            if (!int.TryParse(value, out pageSize) || pageSize <= 0)
            {
                pageSize = 10;
            }

            return pageSize;
        }

        private void ShowMessage(string message)
        {
            var dialog = Master as Dialog;
            if (dialog != null)
            {
                dialog.ShowMessage(message);
            }
        }
    }
}

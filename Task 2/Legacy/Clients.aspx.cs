using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClientManagementWebforms.Data;
using ClientManagementWebforms.Infrastructure;

namespace ClientManagementWebforms
{
    public partial class Clients : Page
    {
        private static readonly string[] AllowedSortColumns = { "CreatedAt", "LastName", "FirstName", "Email" };

        private string SortBy
        {
            get { return (string)(ViewState["SortBy"] ?? GetDefaultSortBy()); }
            set { ViewState["SortBy"] = value; }
        }

        private string SortDir
        {
            get { return (string)(ViewState["SortDir"] ?? GetDefaultSortDir()); }
            set { ViewState["SortDir"] = value; }
        }

        private int TotalCount
        {
            get
            {
                object value = ViewState["TotalCount"];
                if (value == null)
                {
                    return 0;
                }

                return (int)value;
            }
            set { ViewState["TotalCount"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvClients.PageSize = GetDefaultPageSize();
                SortBy = GetDefaultSortBy();
                SortDir = GetDefaultSortDir();
                BindStatus();
                BindGrid();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvClients.PageIndex = 0;
            BindGrid();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            if (ddlStatus.Items.Count > 0)
            {
                ddlStatus.SelectedIndex = 0;
            }

            SortBy = GetDefaultSortBy();
            SortDir = GetDefaultSortDir();
            gvClients.PageIndex = 0;
            BindGrid();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("Client.aspx");
        }

        protected void gvClients_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClients.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvClients_Sorting(object sender, GridViewSortEventArgs e)
        {
            var sortExpression = GetSafeSortBy(e.SortExpression);

            if (string.Equals(SortBy, sortExpression, StringComparison.OrdinalIgnoreCase))
            {
                SortDir = string.Equals(SortDir, "ASC", StringComparison.OrdinalIgnoreCase) ? "DESC" : "ASC";
            }
            else
            {
                SortBy = sortExpression;
                SortDir = "ASC";
            }

            gvClients.PageIndex = 0;
            BindGrid();
        }

        protected void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (gvClients.PageIndex > 0)
            {
                gvClients.PageIndex--;
                BindGrid();
            }
        }

        protected void btnNextPage_Click(object sender, EventArgs e)
        {
            int pageSize = gvClients.PageSize;
            int totalCount = TotalCount;

            int pageCount = pageSize > 0 ? (int)Math.Ceiling((double)totalCount / pageSize) : 0;
            if (pageCount < 1)
            {
                pageCount = 1;
            }

            if (gvClients.PageIndex < pageCount - 1)
            {
                gvClients.PageIndex++;
            }

            BindGrid();
        }

        protected void gvClients_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.Equals(e.CommandName, "DeleteClient", StringComparison.OrdinalIgnoreCase))
            {
                int clientId;
                if (int.TryParse(Convert.ToString(e.CommandArgument), out clientId))
                {
                    try
                    {
                        Repositories.Client_Delete(clientId, true);
                        BindGrid();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error deleting client", ex);
                        ShowError("An error occurred while deleting the client.");
                    }
                }
            }
        }

        private void BindStatus()
        {
            ddlStatus.Items.Clear();
            ddlStatus.Items.Add(new ListItem("All statuses", string.Empty));

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
                Logger.Error("Error loading status lookup for clients list", ex);
                ShowError("An error occurred while loading statuses.");
            }
        }

        private void BindGrid()
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;

            try
            {
                var search = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text.Trim();

                int? statusLookupValueId = null;
                if (!string.IsNullOrWhiteSpace(ddlStatus.SelectedValue))
                {
                    int parsed;
                    if (int.TryParse(ddlStatus.SelectedValue, out parsed))
                    {
                        statusLookupValueId = parsed;
                    }
                }

                var pageSize = GetDefaultPageSize();
                gvClients.PageSize = pageSize;
                var pageNumber = gvClients.PageIndex + 1;

                var sortBy = GetSafeSortBy(SortBy);
                var sortDir = GetSafeSortDir(SortDir);

                var result = Repositories.Client_Search(
                    search,
                    statusLookupValueId,
                    null,
                    null,
                    pageNumber,
                    pageSize,
                    sortBy,
                    sortDir);

                DataTable rows = null;
                if (result.Tables.Count > 0)
                {
                    rows = result.Tables[0];
                }

                var totalCount = 0;
                if (result.Tables.Count > 1 && result.Tables[1].Rows.Count > 0)
                {
                    var value = result.Tables[1].Rows[0][0];
                    if (value != null && value != DBNull.Value)
                    {
                        int.TryParse(value.ToString(), out totalCount);
                    }
                }

                TotalCount = totalCount;

                gvClients.VirtualItemCount = totalCount;
                gvClients.DataSource = rows;
                gvClients.DataBind();

                UpdatePager(totalCount, pageSize);
            }
            catch (Exception ex)
            {
                Logger.Error("Error binding clients grid", ex);
                ShowError("An error occurred while loading clients.");
            }
        }

        private int GetDefaultPageSize()
        {
            var value = ConfigurationManager.AppSettings["DefaultPageSize"];
            int pageSize;
            if (!int.TryParse(value, out pageSize) || pageSize <= 0)
            {
                pageSize = 25;
            }

            return pageSize;
        }

        private string GetDefaultSortBy()
        {
            var sortBy = ConfigurationManager.AppSettings["ClientDefaultSortBy"];
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                sortBy = "CreatedAt";
            }

            return GetSafeSortBy(sortBy);
        }

        private string GetDefaultSortDir()
        {
            var sortDir = ConfigurationManager.AppSettings["ClientDefaultSortDir"];
            if (string.IsNullOrWhiteSpace(sortDir))
            {
                sortDir = "DESC";
            }

            return GetSafeSortDir(sortDir);
        }

        private string GetSafeSortBy(string sortBy)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return "CreatedAt";
            }

            foreach (var allowed in AllowedSortColumns)
            {
                if (string.Equals(allowed, sortBy, StringComparison.OrdinalIgnoreCase))
                {
                    return allowed;
                }
            }

            return "CreatedAt";
        }

        private string GetSafeSortDir(string sortDir)
        {
            if (string.Equals(sortDir, "ASC", StringComparison.OrdinalIgnoreCase))
            {
                return "ASC";
            }

            return "DESC";
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = !string.IsNullOrWhiteSpace(message);
        }

        private void UpdatePager(int totalCount, int pageSize)
        {
            int pageIndex = gvClients.PageIndex;

            int pageCount = pageSize > 0 ? (int)Math.Ceiling((double)totalCount / pageSize) : 0;
            if (pageCount < 1)
            {
                pageCount = 1;
            }

            lblPageInfo.Text = string.Format("Page {0} of {1}", pageIndex + 1, pageCount);

            btnPrevPage.Enabled = pageIndex > 0;
            btnNextPage.Enabled = true;
        }
    }
}

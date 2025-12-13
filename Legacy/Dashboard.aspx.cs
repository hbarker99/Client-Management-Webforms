using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using ClientManagementWebforms.Data;
using ClientManagementWebforms.Infrastructure;

namespace ClientManagementWebforms
{
    public partial class Dashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrWhiteSpace(txtDays.Text))
                {
                    txtDays.Text = "60";
                }

                if (string.IsNullOrWhiteSpace(txtTopN.Text))
                {
                    txtTopN.Text = "10";
                }

                BindStatus();
                BindStale();
                BindNetWorth();
                BindAge();
            }
        }

        protected void btnRefreshStatus_Click(object sender, EventArgs e)
        {
            BindStatus();
        }

        protected void btnRefreshStale_Click(object sender, EventArgs e)
        {
            BindStale();
        }

        protected void btnRefreshNetWorth_Click(object sender, EventArgs e)
        {
            BindNetWorth();
        }

        protected void btnRefreshAge_Click(object sender, EventArgs e)
        {
            BindAge();
        }

        private void BindStatus()
        {
            try
            {
                var table = Repositories.DashboardStatusBreakdown();
                gvStatus.DataSource = table;
                gvStatus.DataBind();
            }
            catch (Exception ex)
            {
                Logger.Error("BindStatus failed", ex);
            }
        }

        private void BindStale()
        {
            try
            {
                int days;
                if (!int.TryParse(txtDays.Text, out days) || days <= 0)
                {
                    days = 60;
                }

                var table = Repositories.DashboardStaleClients(days);
                gvStale.DataSource = table;
                gvStale.DataBind();
            }
            catch (Exception ex)
            {
                Logger.Error("BindStale failed", ex);
            }
        }

        private void BindNetWorth()
        {
            try
            {
                int topN;
                if (!int.TryParse(txtTopN.Text, out topN) || topN <= 0)
                {
                    topN = 10;
                }

                DateTime? minAsOfDate = null;
                if (!string.IsNullOrWhiteSpace(txtMinAsOf.Text))
                {
                    DateTime parsed;
                    if (DateTime.TryParseExact(txtMinAsOf.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
                    {
                        minAsOfDate = parsed;
                    }
                }

                var table = Repositories.DashboardEstimatedNetWorth(topN, minAsOfDate);
                gvNetWorth.DataSource = table;
                gvNetWorth.DataBind();
            }
            catch (Exception ex)
            {
                Logger.Error("BindNetWorth failed", ex);
            }
        }

        private void BindAge()
        {
            try
            {
                int? minAge = null;
                int? maxAge = null;

                int parsed;
                if (!string.IsNullOrWhiteSpace(txtMinAge.Text) && int.TryParse(txtMinAge.Text, out parsed))
                {
                    minAge = parsed;
                }

                if (!string.IsNullOrWhiteSpace(txtMaxAge.Text) && int.TryParse(txtMaxAge.Text, out parsed))
                {
                    maxAge = parsed;
                }

                DataSet dataSet = Repositories.DashboardClientsByAge(minAge, maxAge);

                DataTable detail = null;
                DataTable bands = null;

                if (dataSet != null)
                {
                    if (dataSet.Tables.Count > 0)
                    {
                        detail = dataSet.Tables[0];
                    }

                    if (dataSet.Tables.Count > 1)
                    {
                        bands = dataSet.Tables[1];
                    }
                }

                gvAgeDetail.DataSource = detail;
                gvAgeDetail.DataBind();

                gvAgeBands.DataSource = bands;
                gvAgeBands.DataBind();
            }
            catch (Exception ex)
            {
                Logger.Error("BindAge failed", ex);
            }
        }
    }
}

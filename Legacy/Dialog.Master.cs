using System;
using System.Web.UI;

namespace ClientManagementWebforms
{
    public partial class Dialog : MasterPage
    {
        public void ShowMessage(string message)
        {
            lblMessage.Text = message ?? string.Empty;
            lblMessage.Visible = !string.IsNullOrWhiteSpace(lblMessage.Text);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}


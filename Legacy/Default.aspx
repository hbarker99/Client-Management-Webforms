<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ClientManagementWebforms._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row" aria-labelledby="appTitle">
            <div class="col-md-8">
                <h1 id="appTitle">Client Management Demo</h1>
                <p class="lead">
                    Simple client management with journals, assets, liabilities, income and expenditure.
                </p>
                <p>
                    <a href="Clients.aspx" class="btn btn-primary btn-md">Go to Clients &raquo;</a>
                </p>
            </div>
        </section>
    </main>

</asp:Content>

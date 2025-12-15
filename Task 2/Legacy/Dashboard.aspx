<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="ClientManagementWebforms.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Dashboard</h2>

    <h3>Status Breakdown</h3>
    <asp:Button ID="btnRefreshStatus" runat="server" Text="Refresh" CssClass="btn btn-secondary mb-2" OnClick="btnRefreshStatus_Click" />
    <asp:GridView ID="gvStatus" runat="server" AutoGenerateColumns="True" CssClass="table table-striped table-sm" />

    <hr />

    <h3>Stale Clients</h3>
    <div class="mb-2">
        <asp:Label ID="lblDays" runat="server" AssociatedControlID="txtDays" Text="Days without activity:" CssClass="form-label" />
        <asp:TextBox ID="txtDays" runat="server" CssClass="form-control d-inline-block" Style="width: 100px;" />
        <asp:Button ID="btnRefreshStale" runat="server" Text="Refresh" CssClass="btn btn-secondary ms-2" OnClick="btnRefreshStale_Click" />
    </div>
    <asp:GridView ID="gvStale" runat="server" AutoGenerateColumns="True" CssClass="table table-striped table-sm" />

    <hr />

    <h3>Estimated Net Worth</h3>
    <div class="mb-2">
        <asp:Label ID="lblTopN" runat="server" AssociatedControlID="txtTopN" Text="Top N clients:" CssClass="form-label" />
        <asp:TextBox ID="txtTopN" runat="server" CssClass="form-control d-inline-block" Style="width: 100px;" />
        <asp:Label ID="lblMinAsOf" runat="server" AssociatedControlID="txtMinAsOf" Text="Min As Of (yyyy-MM-dd):" CssClass="form-label ms-3" />
        <asp:TextBox ID="txtMinAsOf" runat="server" CssClass="form-control d-inline-block" Style="width: 150px;" />
        <asp:Button ID="btnRefreshNetWorth" runat="server" Text="Refresh" CssClass="btn btn-secondary ms-2" OnClick="btnRefreshNetWorth_Click" />
    </div>
    <asp:GridView ID="gvNetWorth" runat="server" AutoGenerateColumns="True" CssClass="table table-striped table-sm" />

    <hr />

    <h3>Clients by Age</h3>
    <div class="mb-2">
        <asp:Label ID="lblMinAge" runat="server" AssociatedControlID="txtMinAge" Text="Min age:" CssClass="form-label" />
        <asp:TextBox ID="txtMinAge" runat="server" CssClass="form-control d-inline-block" Style="width: 100px;" />
        <asp:Label ID="lblMaxAge" runat="server" AssociatedControlID="txtMaxAge" Text="Max age:" CssClass="form-label ms-3" />
        <asp:TextBox ID="txtMaxAge" runat="server" CssClass="form-control d-inline-block" Style="width: 100px;" />
        <asp:Button ID="btnRefreshAge" runat="server" Text="Refresh" CssClass="btn btn-secondary ms-2" OnClick="btnRefreshAge_Click" />
    </div>

    <h4>Client Detail</h4>
    <asp:GridView ID="gvAgeDetail" runat="server" AutoGenerateColumns="True" CssClass="table table-striped table-sm" />

    <h4>Age Bands</h4>
    <asp:GridView ID="gvAgeBands" runat="server" AutoGenerateColumns="True" CssClass="table table-striped table-sm" />
</asp:Content>

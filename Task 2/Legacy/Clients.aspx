<%@ Page Title="Clients" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clients.aspx.cs" Inherits="ClientManagementWebforms.Clients" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Clients</h2>

    <asp:Label ID="lblError" runat="server" ForeColor="Red" EnableViewState="false" />

    <div class="row mb-3">
        <div class="col-md-4">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Placeholder="Search name or email..." />
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" />
        </div>
        <div class="col-md-5">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-default" OnClick="btnClear_Click" />
            <asp:Button ID="btnAdd" runat="server" Text="+ Add Client" CssClass="btn btn-success" OnClick="btnAdd_Click" />
        </div>
    </div>

    <asp:GridView ID="gvClients" runat="server" AutoGenerateColumns="False"
        AllowPaging="True" AllowCustomPaging="True" AllowSorting="True"
        CssClass="table table-striped" PageSize="25"
        PagerSettings-Mode="NumericFirstLast"
        PagerSettings-FirstPageText="First"
        PagerSettings-LastPageText="Last"
        PagerSettings-NextPageText="Next"
        PagerSettings-PreviousPageText="Prev"
        OnPageIndexChanging="gvClients_PageIndexChanging"
        OnSorting="gvClients_Sorting"
        OnRowCommand="gvClients_RowCommand">
        <Columns>
            <asp:BoundField DataField="ClientId" HeaderText="ID" ReadOnly="True" />
            <asp:TemplateField HeaderText="Name" SortExpression="LastName">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkClient" runat="server"
                        Text='<%# Eval("LastName") + ", " + Eval("FirstName") %>'
                        NavigateUrl='<%# "Client.aspx?id=" + Eval("ClientId") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="DateOfBirth" HeaderText="DOB" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="Phone" HeaderText="Phone" />
            <asp:BoundField DataField="CreatedAt" HeaderText="Created" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" SortExpression="CreatedAt" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="DeleteClient"
                        CommandArgument='<%# Eval("ClientId") %>'
                        OnClientClick="return confirm('Are you sure you want to delete this client and all related records?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div class="d-flex justify-content-between align-items-center mt-2">
        <asp:Label ID="lblPageInfo" runat="server" />
        <div>
            <asp:Button ID="btnPrevPage" runat="server" Text="Prev" CssClass="btn btn-secondary btn-sm"
                OnClick="btnPrevPage_Click" />
            <asp:Button ID="btnNextPage" runat="server" Text="Next" CssClass="btn btn-secondary btn-sm ms-1"
                OnClick="btnNextPage_Click" />
        </div>
    </div>
</asp:Content>

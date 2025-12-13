<%@ Page Title="Client" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="Client.aspx.cs" Inherits="ClientManagementWebforms.Client" %>

<asp:Content ID="Content1" ContentPlaceHolderID="DialogContent" runat="server">
    <h2 class="mb-3">Client</h2>

    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="text-danger mb-3" />

    <div class="card mb-4">
        <div class="card-header">
            Details
        </div>
        <div class="card-body">
            <div class="row mb-3">
                <div class="col-md-6">
                    <label for="txtFirst" class="form-label">First name</label>
                    <asp:TextBox ID="txtFirst" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvFirst" runat="server"
                        ControlToValidate="txtFirst"
                        ErrorMessage="First name is required."
                        Display="Dynamic"
                        CssClass="text-danger" />
                </div>
                <div class="col-md-6">
                    <label for="txtLast" class="form-label">Last name</label>
                    <asp:TextBox ID="txtLast" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvLast" runat="server"
                        ControlToValidate="txtLast"
                        ErrorMessage="Last name is required."
                        Display="Dynamic"
                        CssClass="text-danger" />
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-4">
                    <label for="txtDob" class="form-label">Date of birth</label>
                    <asp:TextBox ID="txtDob" runat="server" CssClass="form-control" Placeholder="dd/MM/yyyy" />
                </div>
                <div class="col-md-4">
                    <label for="txtEmail" class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-4">
                    <label for="txtPhone" class="form-label">Phone</label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-4">
                    <label for="ddlStatus" class="form-label">Status</label>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" />
                </div>
            </div>

            <div class="mt-3">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary me-2" OnClick="btnSave_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger"
                    OnClick="btnDelete_Click"
                    OnClientClick="return confirm('Are you sure you want to delete this client and all related records?');" />
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlRelated" runat="server">
        <div class="card mb-4">
            <div class="card-header">
                Journal
            </div>
            <div class="card-body">
                <div class="row g-2 mb-3">
                    <div class="col-md-3">
                        <label for="ddlNoteType" class="form-label">Note type</label>
                        <asp:DropDownList ID="ddlNoteType" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-5">
                        <label for="txtJournalBody" class="form-label">Body</label>
                        <asp:TextBox ID="txtJournalBody" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
                    </div>
                    <div class="col-md-2">
                        <label for="txtJournalAuthor" class="form-label">Author</label>
                        <asp:TextBox ID="txtJournalAuthor" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-2 d-flex align-items-end">
                        <asp:Button ID="btnAddJournal" runat="server" Text="Add Journal" CssClass="btn btn-secondary w-100" OnClick="btnAddJournal_Click" />
                    </div>
                </div>

                <asp:GridView ID="gvJournal" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                    CssClass="table table-striped table-sm"
                    OnPageIndexChanging="gvJournal_PageIndexChanging"
                    OnRowCommand="gvJournal_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="JournalId" HeaderText="ID" ReadOnly="True" />
                        <asp:BoundField DataField="OccurredAt" HeaderText="Occurred" DataFormatString="{0:dd/MM/yyyy HH:mm}" HtmlEncode="False" />
                        <asp:BoundField DataField="Author" HeaderText="Author" />
                        <asp:BoundField DataField="Body" HeaderText="Body" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDeleteJournal" runat="server" Text="Delete"
                                    CommandName="DeleteJournal"
                                    CommandArgument='<%# Eval("JournalId") %>'
                                    OnClientClick="return confirm('Delete this journal entry?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div class="card mb-4">
            <div class="card-header">
                Assets
            </div>
            <div class="card-body">
                <div class="row g-2 mb-3">
                    <div class="col-md-4">
                        <label for="txtAssetType" class="form-label">Asset type</label>
                        <asp:TextBox ID="txtAssetType" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-3">
                        <label for="txtAssetValue" class="form-label">Value</label>
                        <asp:TextBox ID="txtAssetValue" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-3">
                        <label for="txtAssetProvider" class="form-label">Provider</label>
                        <asp:TextBox ID="txtAssetProvider" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-2 d-flex align-items-end">
                        <asp:Button ID="btnAddAsset" runat="server" Text="Add Asset" CssClass="btn btn-secondary w-100" OnClick="btnAddAsset_Click" />
                    </div>
                </div>

                <asp:GridView ID="gvAssets" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                    CssClass="table table-striped table-sm"
                    OnPageIndexChanging="gvAssets_PageIndexChanging"
                    OnRowCommand="gvAssets_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="AssetId" HeaderText="ID" ReadOnly="True" />
                        <asp:BoundField DataField="AssetType" HeaderText="Type" />
                        <asp:BoundField DataField="Value" HeaderText="Value" DataFormatString="{0:N2}" HtmlEncode="False" />
                        <asp:BoundField DataField="Provider" HeaderText="Provider" />
                        <asp:BoundField DataField="AsOf" HeaderText="As Of" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDeleteAsset" runat="server" Text="Delete"
                                    CommandName="DeleteAsset"
                                    CommandArgument='<%# Eval("AssetId") %>'
                                    OnClientClick="return confirm('Delete this asset?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div class="card mb-4">
            <div class="card-header">
                Liabilities
            </div>
            <div class="card-body">
                <div class="row g-2 mb-3">
                    <div class="col-md-4">
                        <label for="txtLiabType" class="form-label">Liability type</label>
                        <asp:TextBox ID="txtLiabType" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-3">
                        <label for="txtLiabBalance" class="form-label">Balance</label>
                        <asp:TextBox ID="txtLiabBalance" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-3">
                        <label for="txtLiabRate" class="form-label">Rate</label>
                        <asp:TextBox ID="txtLiabRate" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-2 d-flex align-items-end">
                        <asp:Button ID="btnAddLiab" runat="server" Text="Add Liability" CssClass="btn btn-secondary w-100" OnClick="btnAddLiab_Click" />
                    </div>
                </div>

                <asp:GridView ID="gvLiabilities" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                    CssClass="table table-striped table-sm"
                    OnPageIndexChanging="gvLiabilities_PageIndexChanging"
                    OnRowCommand="gvLiabilities_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="LiabilityId" HeaderText="ID" ReadOnly="True" />
                        <asp:BoundField DataField="LiabilityType" HeaderText="Type" />
                        <asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString="{0:N2}" HtmlEncode="False" />
                        <asp:BoundField DataField="Rate" HeaderText="Rate" DataFormatString="{0:N2}" HtmlEncode="False" />
                        <asp:BoundField DataField="AsOf" HeaderText="As Of" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDeleteLiab" runat="server" Text="Delete"
                                    CommandName="DeleteLiability"
                                    CommandArgument='<%# Eval("LiabilityId") %>'
                                    OnClientClick="return confirm('Delete this liability?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div class="card mb-4">
            <div class="card-header">
                Income
            </div>
            <div class="card-body">
                <div class="row g-2 mb-3">
                    <div class="col-md-6">
                        <label for="txtIncomeSource" class="form-label">Source</label>
                        <asp:TextBox ID="txtIncomeSource" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-4">
                        <label for="txtIncomeAmount" class="form-label">Amount monthly</label>
                        <asp:TextBox ID="txtIncomeAmount" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-2 d-flex align-items-end">
                        <asp:Button ID="btnAddIncome" runat="server" Text="Add Income" CssClass="btn btn-secondary w-100" OnClick="btnAddIncome_Click" />
                    </div>
                </div>

                <asp:GridView ID="gvIncome" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                    CssClass="table table-striped table-sm"
                    OnPageIndexChanging="gvIncome_PageIndexChanging"
                    OnRowCommand="gvIncome_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="IncomeId" HeaderText="ID" ReadOnly="True" />
                        <asp:BoundField DataField="Source" HeaderText="Source" />
                        <asp:BoundField DataField="AmountMonthly" HeaderText="Amount" DataFormatString="{0:N2}" HtmlEncode="False" />
                        <asp:BoundField DataField="AsOf" HeaderText="As Of" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDeleteIncome" runat="server" Text="Delete"
                                    CommandName="DeleteIncome"
                                    CommandArgument='<%# Eval("IncomeId") %>'
                                    OnClientClick="return confirm('Delete this income?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div class="card mb-4">
            <div class="card-header">
                Expenditure
            </div>
            <div class="card-body">
                <div class="row g-2 mb-3">
                    <div class="col-md-6">
                        <label for="txtExCat" class="form-label">Category</label>
                        <asp:TextBox ID="txtExCat" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-4">
                        <label for="txtExAmt" class="form-label">Amount monthly</label>
                        <asp:TextBox ID="txtExAmt" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-2 d-flex align-items-end">
                        <asp:Button ID="btnAddEx" runat="server" Text="Add Expenditure" CssClass="btn btn-secondary w-100" OnClick="btnAddEx_Click" />
                    </div>
                </div>

                <asp:GridView ID="gvExpenditure" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                    CssClass="table table-striped table-sm"
                    OnPageIndexChanging="gvExpenditure_PageIndexChanging"
                    OnRowCommand="gvExpenditure_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="ExpenditureId" HeaderText="ID" ReadOnly="True" />
                        <asp:BoundField DataField="Category" HeaderText="Category" />
                        <asp:BoundField DataField="AmountMonthly" HeaderText="Amount" DataFormatString="{0:N2}" HtmlEncode="False" />
                        <asp:BoundField DataField="AsOf" HeaderText="As Of" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDeleteEx" runat="server" Text="Delete"
                                    CommandName="DeleteExpenditure"
                                    CommandArgument='<%# Eval("ExpenditureId") %>'
                                    OnClientClick="return confirm('Delete this expenditure?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </asp:Panel>
</asp:Content>


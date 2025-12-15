Build the Dashboard (WebForms, .NET Framework 4.7.2)



Implement the dashboard below using SqlClient (no EF) and full postbacks/ViewState.



Data sources (already created procs)



dbo.Dashboard\_StatusBreakdown\_Bad



Returns: StatusLabel NVARCHAR(200), ClientCount INT.



dbo.Dashboard\_StaleClients\_Bad @Days INT



Returns: ClientId INT, FirstName NVARCHAR(150), LastName NVARCHAR(150), Email NVARCHAR(254), Phone NVARCHAR(30), LastJournalAt DATETIME2(3).



dbo.Dashboard\_EstimatedNetWorth\_Bad @TopN INT, @MinAsOfDate DATE



Returns: ClientId INT, FirstName NVARCHAR(150), LastName NVARCHAR(150), EstimatedNetWorth DECIMAL(18,2), TotalMonthlyIncome DECIMAL(18,2).



dbo.Dashboard\_ClientsByAge\_Bad @MinAge INT, @MaxAge INT



Two result sets



RS1 (detail): ClientId, FirstName, LastName, DateOfBirth, AgeYears INT, AgeBand NVARCHAR(10)



RS2 (bands): AgeBand NVARCHAR(10), ClientCount INT.



1\) Add a nav link



File: Site.Master

Add a link to Dashboard.aspx next to “Clients”.



2\) Repository wrappers



File: App\_Code/Data/Repositories.cs

Append these methods (use existing Db.ReadTable / Db.ReadDataSet):



public static System.Data.DataTable DashboardStatusBreakdown()

&nbsp;   => Db.ReadTable("dbo.Dashboard\_StatusBreakdown\_Bad");



public static System.Data.DataTable DashboardStaleClients(int days)

&nbsp;   => Db.ReadTable("dbo.Dashboard\_StaleClients\_Bad",

&nbsp;       new System.Data.SqlClient.SqlParameter("@Days", days));



public static System.Data.DataTable DashboardEstimatedNetWorth(int topN, System.DateTime? minAsOfDate)

&nbsp;   => Db.ReadTable("dbo.Dashboard\_EstimatedNetWorth\_Bad",

&nbsp;       new System.Data.SqlClient.SqlParameter("@TopN", topN),

&nbsp;       new System.Data.SqlClient.SqlParameter("@MinAsOfDate", (object)minAsOfDate ?? System.DBNull.Value));



public static System.Data.DataSet DashboardClientsByAge(int? minAge, int? maxAge)

&nbsp;   => Db.ReadDataSet("dbo.Dashboard\_ClientsByAge\_Bad",

&nbsp;       new System.Data.SqlClient.SqlParameter("@MinAge", (object)minAge ?? System.DBNull.Value),

&nbsp;       new System.Data.SqlClient.SqlParameter("@MaxAge", (object)maxAge ?? System.DBNull.Value));



3\) Create Dashboard page



New files:



Dashboard.aspx (uses Site.Master)



Dashboard.aspx.cs



Dashboard.aspx (full postbacks, no AJAX). Create four sections:



Status Breakdown



Controls: Button btnRefreshStatus, GridView gvStatus (AutoGenerateColumns=true)



Stale Clients



Controls: TextBox txtDays (default 60), Button btnRefreshStale, GridView gvStale



Estimated Net Worth



Controls: TextBox txtTopN (default 10), TextBox txtMinAsOf (optional ISO yyyy-MM-dd), Button btnRefreshNetWorth, GridView gvNetWorth



Clients by Age



Controls: TextBox txtMinAge, TextBox txtMaxAge, Button btnRefreshAge,

GridView gvAgeDetail (detail RS1), GridView gvAgeBands (bands RS2)



Lay out with simple headings and HRs. Use built-in validators only if trivial; otherwise skip.



4\) Code-behind logic



Dashboard.aspx.cs



On Page\_Load, if (!IsPostBack) call: BindStatus(), BindStale(), BindNetWorth(), BindAge().



Implement:



BindStatus() → gvStatus.DataSource = Repos.DashboardStatusBreakdown(); DataBind();



BindStale() → parse txtDays (int; default 60) → bind gvStale.



BindNetWorth() → parse txtTopN (int; default 10) and txtMinAsOf (DateTime? parsed as yyyy-MM-dd) → bind gvNetWorth.



BindAge() → parse txtMinAge/txtMaxAge (nullable ints) → call Repos.DashboardClientsByAge → bind gvAgeDetail to Tables\[0], gvAgeBands to Tables\[1].



Add click handlers:



btnRefreshStatus\_Click → BindStatus()



btnRefreshStale\_Click → BindStale()



btnRefreshNetWorth\_Click → BindNetWorth()



btnRefreshAge\_Click → BindAge()



Wrap each bind in try/catch; on exception call Logger.Error("BindX failed", ex) and keep the page rendering.



5\) Formatting



Let GridViews auto-generate columns (simple).



For dates (LastJournalAt, DateOfBirth) you may set DataFormatString via BoundField if you choose to add explicit columns; otherwise leave default.



6\) Acceptance criteria



Dashboard.aspx is reachable from the top nav.



Status Breakdown shows StatusLabel + ClientCount.



Stale Clients respects the Days textbox and refresh button.



Estimated Net Worth respects TopN and optional Min AsOf (ISO) inputs.



Clients by Age shows two populated grids (detail and bands) and respects optional Min/Max age.



All calls use SqlClient via Db/Repos. No EF, no AJAX. Full postbacks only.



7\) Output



Show a concise diff of added/changed files and print:

DASHBOARD PAGE IMPLEMENTED


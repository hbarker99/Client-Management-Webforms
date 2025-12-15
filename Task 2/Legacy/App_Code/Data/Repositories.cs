using System;
using System.Data;
using System.Data.SqlClient;

namespace ClientManagementWebforms.Data
{
    public static class Repositories
    {
        // Lookup

        public static DataTable LookupValue_Search(string lookupCode, bool onlyActive)
        {
            var parameters = new[]
            {
                new SqlParameter("@LookupCode", SqlDbType.NVarChar, 100) { Value = (object)lookupCode ?? DBNull.Value },
                new SqlParameter("@OnlyActive", SqlDbType.Bit) { Value = onlyActive }
            };

            return Db.ReadTable("LookupValue_Search", parameters);
        }

        // Client

        public static DataTable Client_Create(string firstName, string lastName, DateTime? dateOfBirth, string email, string phone, int? statusLookupValueId)
        {
            var parameters = new[]
            {
                new SqlParameter("@FirstName", SqlDbType.NVarChar, 150) { Value = (object)firstName ?? DBNull.Value },
                new SqlParameter("@LastName", SqlDbType.NVarChar, 150) { Value = (object)lastName ?? DBNull.Value },
                new SqlParameter("@DateOfBirth", SqlDbType.Date) { Value = (object)dateOfBirth ?? DBNull.Value },
                new SqlParameter("@Email", SqlDbType.NVarChar, 254) { Value = (object)email ?? DBNull.Value },
                new SqlParameter("@Phone", SqlDbType.NVarChar, 30) { Value = (object)phone ?? DBNull.Value },
                new SqlParameter("@StatusLookupValueId", SqlDbType.Int) { Value = (object)statusLookupValueId ?? DBNull.Value }
            };

            return Db.ReadTable("Client_Create", parameters);
        }

        public static DataTable Client_Read(int clientId)
        {
            var parameters = new[]
            {
                new SqlParameter("@ClientId", SqlDbType.Int) { Value = clientId }
            };

            return Db.ReadTable("Client_Read", parameters);
        }

        public static DataTable Client_Update(int clientId, string firstName, string lastName, DateTime? dateOfBirth, string email, string phone, int? statusLookupValueId)
        {
            var parameters = new[]
            {
                new SqlParameter("@ClientId", SqlDbType.Int) { Value = clientId },
                new SqlParameter("@FirstName", SqlDbType.NVarChar, 150) { Value = (object)firstName ?? DBNull.Value },
                new SqlParameter("@LastName", SqlDbType.NVarChar, 150) { Value = (object)lastName ?? DBNull.Value },
                new SqlParameter("@DateOfBirth", SqlDbType.Date) { Value = (object)dateOfBirth ?? DBNull.Value },
                new SqlParameter("@Email", SqlDbType.NVarChar, 254) { Value = (object)email ?? DBNull.Value },
                new SqlParameter("@Phone", SqlDbType.NVarChar, 30) { Value = (object)phone ?? DBNull.Value },
                new SqlParameter("@StatusLookupValueId", SqlDbType.Int) { Value = (object)statusLookupValueId ?? DBNull.Value }
            };

            return Db.ReadTable("Client_Update", parameters);
        }

        public static int Client_Delete(int clientId, bool force)
        {
            var parameters = new[]
            {
                new SqlParameter("@ClientId", SqlDbType.Int) { Value = clientId },
                new SqlParameter("@Force", SqlDbType.Bit) { Value = force }
            };

            return Db.Exec("Client_Delete", parameters);
        }

        public static DataSet Client_Search(string search, int? statusLookupValueId, DateTime? dobFrom, DateTime? dobTo, int pageNumber, int pageSize, string sortBy, string sortDir)
        {
            var parameters = new[]
            {
                new SqlParameter("@Search", SqlDbType.NVarChar, 200) { Value = (object)search ?? DBNull.Value },
                new SqlParameter("@StatusLookupValueId", SqlDbType.Int) { Value = (object)statusLookupValueId ?? DBNull.Value },
                new SqlParameter("@DobFrom", SqlDbType.Date) { Value = (object)dobFrom ?? DBNull.Value },
                new SqlParameter("@DobTo", SqlDbType.Date) { Value = (object)dobTo ?? DBNull.Value },
                new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber },
                new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize },
                new SqlParameter("@SortBy", SqlDbType.NVarChar, 50) { Value = (object)sortBy ?? DBNull.Value },
                new SqlParameter("@SortDir", SqlDbType.NVarChar, 4) { Value = (object)sortDir ?? DBNull.Value }
            };

            return Db.ReadDataSet("Client_Search", parameters);
        }

        // Journal

        public static DataTable Journal_Create(int clientId, int noteTypeLookupValueId, DateTime occurredAt, string body, string author)
        {
            var parameters = new[]
            {
                new SqlParameter("@ClientId", SqlDbType.Int) { Value = clientId },
                new SqlParameter("@NoteTypeLookupValueId", SqlDbType.Int) { Value = noteTypeLookupValueId },
                new SqlParameter("@OccurredAt", SqlDbType.DateTime2, 7) { Value = occurredAt },
                new SqlParameter("@Body", SqlDbType.NVarChar) { Value = (object)body ?? DBNull.Value },
                new SqlParameter("@Author", SqlDbType.NVarChar, 100) { Value = (object)author ?? DBNull.Value }
            };

            return Db.ReadTable("Journal_Create", parameters);
        }

        public static DataTable Journal_Read(int journalId)
        {
            var parameters = new[]
            {
                new SqlParameter("@JournalId", SqlDbType.Int) { Value = journalId }
            };

            return Db.ReadTable("Journal_Read", parameters);
        }

        public static DataTable Journal_Update(int journalId, int noteTypeLookupValueId, DateTime occurredAt, string body, string author)
        {
            var parameters = new[]
            {
                new SqlParameter("@JournalId", SqlDbType.Int) { Value = journalId },
                new SqlParameter("@NoteTypeLookupValueId", SqlDbType.Int) { Value = noteTypeLookupValueId },
                new SqlParameter("@OccurredAt", SqlDbType.DateTime2, 7) { Value = occurredAt },
                new SqlParameter("@Body", SqlDbType.NVarChar) { Value = (object)body ?? DBNull.Value },
                new SqlParameter("@Author", SqlDbType.NVarChar, 100) { Value = (object)author ?? DBNull.Value }
            };

            return Db.ReadTable("Journal_Update", parameters);
        }

        public static int Journal_Delete(int journalId)
        {
            var parameters = new[]
            {
                new SqlParameter("@JournalId", SqlDbType.Int) { Value = journalId }
            };

            return Db.Exec("Journal_Delete", parameters);
        }

        public static DataSet Journal_Search(int clientId, int? noteTypeLookupValueId, DateTime? fromUtc, DateTime? toUtc, string search, int pageNumber, int pageSize, string sortBy, string sortDir)
        {
            var parameters = new[]
            {
                new SqlParameter("@ClientId", SqlDbType.Int) { Value = clientId },
                new SqlParameter("@NoteTypeLookupValueId", SqlDbType.Int) { Value = (object)noteTypeLookupValueId ?? DBNull.Value },
                new SqlParameter("@FromUtc", SqlDbType.DateTime2, 7) { Value = (object)fromUtc ?? DBNull.Value },
                new SqlParameter("@ToUtc", SqlDbType.DateTime2, 7) { Value = (object)toUtc ?? DBNull.Value },
                new SqlParameter("@Search", SqlDbType.NVarChar, 400) { Value = (object)search ?? DBNull.Value },
                new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber },
                new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize },
                new SqlParameter("@SortBy", SqlDbType.NVarChar, 50) { Value = (object)sortBy ?? DBNull.Value },
                new SqlParameter("@SortDir", SqlDbType.NVarChar, 4) { Value = (object)sortDir ?? DBNull.Value }
            };

            return Db.ReadDataSet("Journal_Search", parameters);
        }

        // Asset

        public static DataTable Asset_Create(int clientId, string assetType, decimal value, string provider, DateTime asOf)
        {
            var parameters = new[]
            {
                new SqlParameter("@ClientId", SqlDbType.Int) { Value = clientId },
                new SqlParameter("@AssetType", SqlDbType.NVarChar, 50) { Value = (object)assetType ?? DBNull.Value },
                new SqlParameter("@Value", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = value },
                new SqlParameter("@Provider", SqlDbType.NVarChar, 200) { Value = (object)provider ?? DBNull.Value },
                new SqlParameter("@AsOf", SqlDbType.DateTime2, 7) { Value = asOf }
            };

            return Db.ReadTable("Asset_Create", parameters);
        }

        public static DataTable Asset_Read(int assetId)
        {
            var parameters = new[]
            {
                new SqlParameter("@AssetId", SqlDbType.Int) { Value = assetId }
            };

            return Db.ReadTable("Asset_Read", parameters);
        }

        public static DataTable Asset_Update(int assetId, string assetType, decimal value, string provider, DateTime asOf)
        {
            var parameters = new[]
            {
                new SqlParameter("@AssetId", SqlDbType.Int) { Value = assetId },
                new SqlParameter("@AssetType", SqlDbType.NVarChar, 50) { Value = (object)assetType ?? DBNull.Value },
                new SqlParameter("@Value", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = value },
                new SqlParameter("@Provider", SqlDbType.NVarChar, 200) { Value = (object)provider ?? DBNull.Value },
                new SqlParameter("@AsOf", SqlDbType.DateTime2, 7) { Value = asOf }
            };

            return Db.ReadTable("Asset_Update", parameters);
        }

        public static int Asset_Delete(int assetId)
        {
            var parameters = new[]
            {
                new SqlParameter("@AssetId", SqlDbType.Int) { Value = assetId }
            };

            return Db.Exec("Asset_Delete", parameters);
        }

        public static DataSet Asset_Search(int clientId, string assetType, DateTime? fromUtc, DateTime? toUtc, int pageNumber, int pageSize, string sortBy, string sortDir)
        {
            var parameters = new[]
            {
                new SqlParameter("@ClientId", SqlDbType.Int) { Value = clientId },
                new SqlParameter("@AssetType", SqlDbType.NVarChar, 50) { Value = (object)assetType ?? DBNull.Value },
                new SqlParameter("@FromUtc", SqlDbType.DateTime2, 7) { Value = (object)fromUtc ?? DBNull.Value },
                new SqlParameter("@ToUtc", SqlDbType.DateTime2, 7) { Value = (object)toUtc ?? DBNull.Value },
                new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber },
                new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize },
                new SqlParameter("@SortBy", SqlDbType.NVarChar, 50) { Value = (object)sortBy ?? DBNull.Value },
                new SqlParameter("@SortDir", SqlDbType.NVarChar, 4) { Value = (object)sortDir ?? DBNull.Value }
            };

            return Db.ReadDataSet("Asset_Search", parameters);
        }

        // Liability

        public static DataTable Liability_Create(int clientId, string liabilityType, decimal balance, decimal? rate, DateTime asOf)
        {
            var balanceParameter = new SqlParameter("@Balance", SqlDbType.Decimal)
            {
                Precision = 18,
                Scale = 2,
                Value = balance
            };

            var rateParameter = new SqlParameter("@Rate", SqlDbType.Decimal)
            {
                Precision = 9,
                Scale = 4,
                Value = (object)rate ?? DBNull.Value
            };

            var parameters = new[]
            {
                new SqlParameter("@ClientId", SqlDbType.Int) { Value = clientId },
                new SqlParameter("@LiabilityType", SqlDbType.NVarChar, 50) { Value = (object)liabilityType ?? DBNull.Value },
                balanceParameter,
                rateParameter,
                new SqlParameter("@AsOf", SqlDbType.DateTime2, 7) { Value = asOf }
            };

            return Db.ReadTable("Liability_Create", parameters);
        }

        public static DataTable Liability_Read(int liabilityId)
        {
            var parameters = new[]
            {
                new SqlParameter("@LiabilityId", SqlDbType.Int) { Value = liabilityId }
            };

            return Db.ReadTable("Liability_Read", parameters);
        }

        public static DataTable Liability_Update(int liabilityId, string liabilityType, decimal balance, decimal? rate, DateTime asOf)
        {
            var balanceParameter = new SqlParameter("@Balance", SqlDbType.Decimal)
            {
                Precision = 18,
                Scale = 2,
                Value = balance
            };

            var rateParameter = new SqlParameter("@Rate", SqlDbType.Decimal)
            {
                Precision = 9,
                Scale = 4,
                Value = (object)rate ?? DBNull.Value
            };

            var parameters = new[]
            {
                new SqlParameter("@LiabilityId", SqlDbType.Int) { Value = liabilityId },
                new SqlParameter("@LiabilityType", SqlDbType.NVarChar, 50) { Value = (object)liabilityType ?? DBNull.Value },
                balanceParameter,
                rateParameter,
                new SqlParameter("@AsOf", SqlDbType.DateTime2, 7) { Value = asOf }
            };

            return Db.ReadTable("Liability_Update", parameters);
        }

        public static int Liability_Delete(int liabilityId)
        {
            var parameters = new[]
            {
                new SqlParameter("@LiabilityId", SqlDbType.Int) { Value = liabilityId }
            };

            return Db.Exec("Liability_Delete", parameters);
        }

        public static DataSet Liability_Search(int clientId, string liabilityType, DateTime? fromUtc, DateTime? toUtc, int pageNumber, int pageSize, string sortBy, string sortDir)
        {
            var parameters = new[]
            {
                new SqlParameter("@ClientId", SqlDbType.Int) { Value = clientId },
                new SqlParameter("@LiabilityType", SqlDbType.NVarChar, 50) { Value = (object)liabilityType ?? DBNull.Value },
                new SqlParameter("@FromUtc", SqlDbType.DateTime2, 7) { Value = (object)fromUtc ?? DBNull.Value },
                new SqlParameter("@ToUtc", SqlDbType.DateTime2, 7) { Value = (object)toUtc ?? DBNull.Value },
                new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber },
                new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize },
                new SqlParameter("@SortBy", SqlDbType.NVarChar, 50) { Value = (object)sortBy ?? DBNull.Value },
                new SqlParameter("@SortDir", SqlDbType.NVarChar, 4) { Value = (object)sortDir ?? DBNull.Value }
            };

            return Db.ReadDataSet("Liability_Search", parameters);
        }

        // Income

        public static DataTable Income_Create(int clientId, string source, decimal amountMonthly, DateTime asOf)
        {
            var amountParameter = new SqlParameter("@AmountMonthly", SqlDbType.Decimal)
            {
                Precision = 18,
                Scale = 2,
                Value = amountMonthly
            };

            var parameters = new[]
            {
                new SqlParameter("@ClientId", SqlDbType.Int) { Value = clientId },
                new SqlParameter("@Source", SqlDbType.NVarChar, 50) { Value = (object)source ?? DBNull.Value },
                amountParameter,
                new SqlParameter("@AsOf", SqlDbType.DateTime2, 7) { Value = asOf }
            };

            return Db.ReadTable("Income_Create", parameters);
        }

        public static DataTable Income_Read(int incomeId)
        {
            var parameters = new[]
            {
                new SqlParameter("@IncomeId", SqlDbType.Int) { Value = incomeId }
            };

            return Db.ReadTable("Income_Read", parameters);
        }

        public static DataTable Income_Update(int incomeId, string source, decimal amountMonthly, DateTime asOf)
        {
            var amountParameter = new SqlParameter("@AmountMonthly", SqlDbType.Decimal)
            {
                Precision = 18,
                Scale = 2,
                Value = amountMonthly
            };

            var parameters = new[]
            {
                new SqlParameter("@IncomeId", SqlDbType.Int) { Value = incomeId },
                new SqlParameter("@Source", SqlDbType.NVarChar, 50) { Value = (object)source ?? DBNull.Value },
                amountParameter,
                new SqlParameter("@AsOf", SqlDbType.DateTime2, 7) { Value = asOf }
            };

            return Db.ReadTable("Income_Update", parameters);
        }

        public static int Income_Delete(int incomeId)
        {
            var parameters = new[]
            {
                new SqlParameter("@IncomeId", SqlDbType.Int) { Value = incomeId }
            };

            return Db.Exec("Income_Delete", parameters);
        }

        public static DataSet Income_Search(int clientId, string source, DateTime? fromUtc, DateTime? toUtc, int pageNumber, int pageSize, string sortBy, string sortDir)
        {
            var parameters = new[]
            {
                new SqlParameter("@ClientId", SqlDbType.Int) { Value = clientId },
                new SqlParameter("@Source", SqlDbType.NVarChar, 50) { Value = (object)source ?? DBNull.Value },
                new SqlParameter("@FromUtc", SqlDbType.DateTime2, 7) { Value = (object)fromUtc ?? DBNull.Value },
                new SqlParameter("@ToUtc", SqlDbType.DateTime2, 7) { Value = (object)toUtc ?? DBNull.Value },
                new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber },
                new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize },
                new SqlParameter("@SortBy", SqlDbType.NVarChar, 50) { Value = (object)sortBy ?? DBNull.Value },
                new SqlParameter("@SortDir", SqlDbType.NVarChar, 4) { Value = (object)sortDir ?? DBNull.Value }
            };

            return Db.ReadDataSet("Income_Search", parameters);
        }

        // Expenditure

        public static DataTable Expenditure_Create(int clientId, string category, decimal amountMonthly, DateTime asOf)
        {
            var amountParameter = new SqlParameter("@AmountMonthly", SqlDbType.Decimal)
            {
                Precision = 18,
                Scale = 2,
                Value = amountMonthly
            };

            var parameters = new[]
            {
                new SqlParameter("@ClientId", SqlDbType.Int) { Value = clientId },
                new SqlParameter("@Category", SqlDbType.NVarChar, 50) { Value = (object)category ?? DBNull.Value },
                amountParameter,
                new SqlParameter("@AsOf", SqlDbType.DateTime2, 7) { Value = asOf }
            };

            return Db.ReadTable("Expenditure_Create", parameters);
        }

        public static DataTable Expenditure_Read(int expenditureId)
        {
            var parameters = new[]
            {
                new SqlParameter("@ExpenditureId", SqlDbType.Int) { Value = expenditureId }
            };

            return Db.ReadTable("Expenditure_Read", parameters);
        }

        public static DataTable Expenditure_Update(int expenditureId, string category, decimal amountMonthly, DateTime asOf)
        {
            var amountParameter = new SqlParameter("@AmountMonthly", SqlDbType.Decimal)
            {
                Precision = 18,
                Scale = 2,
                Value = amountMonthly
            };

            var parameters = new[]
            {
                new SqlParameter("@ExpenditureId", SqlDbType.Int) { Value = expenditureId },
                new SqlParameter("@Category", SqlDbType.NVarChar, 50) { Value = (object)category ?? DBNull.Value },
                amountParameter,
                new SqlParameter("@AsOf", SqlDbType.DateTime2, 7) { Value = asOf }
            };

            return Db.ReadTable("Expenditure_Update", parameters);
        }

        public static int Expenditure_Delete(int expenditureId)
        {
            var parameters = new[]
            {
                new SqlParameter("@ExpenditureId", SqlDbType.Int) { Value = expenditureId }
            };

            return Db.Exec("Expenditure_Delete", parameters);
        }

        public static DataSet Expenditure_Search(int clientId, string category, DateTime? fromUtc, DateTime? toUtc, int pageNumber, int pageSize, string sortBy, string sortDir)
        {
            var parameters = new[]
            {
                new SqlParameter("@ClientId", SqlDbType.Int) { Value = clientId },
                new SqlParameter("@Category", SqlDbType.NVarChar, 50) { Value = (object)category ?? DBNull.Value },
                new SqlParameter("@FromUtc", SqlDbType.DateTime2, 7) { Value = (object)fromUtc ?? DBNull.Value },
                new SqlParameter("@ToUtc", SqlDbType.DateTime2, 7) { Value = (object)toUtc ?? DBNull.Value },
                new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber },
                new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize },
                new SqlParameter("@SortBy", SqlDbType.NVarChar, 50) { Value = (object)sortBy ?? DBNull.Value },
                new SqlParameter("@SortDir", SqlDbType.NVarChar, 4) { Value = (object)sortDir ?? DBNull.Value }
            };

            return Db.ReadDataSet("Expenditure_Search", parameters);
        }

        // Dashboard

        public static System.Data.DataTable DashboardStatusBreakdown()
        {
            return Db.ReadTable("dbo.Dashboard_StatusBreakdown_Bad");
        }

        public static System.Data.DataTable DashboardStaleClients(int days)
        {
            return Db.ReadTable(
                "dbo.Dashboard_StaleClients_Bad",
                new System.Data.SqlClient.SqlParameter("@Days", days));
        }

        public static System.Data.DataTable DashboardEstimatedNetWorth(int topN, System.DateTime? minAsOfDate)
        {
            return Db.ReadTable(
                "dbo.Dashboard_EstimatedNetWorth_Bad",
                new System.Data.SqlClient.SqlParameter("@TopN", topN),
                new System.Data.SqlClient.SqlParameter("@MinAsOfDate", (object)minAsOfDate ?? System.DBNull.Value));
        }

        public static System.Data.DataSet DashboardClientsByAge(int? minAge, int? maxAge)
        {
            return Db.ReadDataSet(
                "dbo.Dashboard_ClientsByAge_Bad",
                new System.Data.SqlClient.SqlParameter("@MinAge", (object)minAge ?? System.DBNull.Value),
                new System.Data.SqlClient.SqlParameter("@MaxAge", (object)maxAge ?? System.DBNull.Value));
        }
    }
}

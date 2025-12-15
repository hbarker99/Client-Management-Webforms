/* ============================================================
   Demo Data Seeder (fixed TRUNCATE/DELETE order + reseeds)
   Requires schema with singular tables + Lookup/LookupValue.
   ============================================================ */
USE ClientManagementDemo;
SET NOCOUNT ON;

BEGIN TRY
BEGIN TRAN;

---------------------------------------------------------------
-- CLEAR EXISTING DATA (children first)
-- TRUNCATE children is OK; parent must be DELETE due to FK refs.
---------------------------------------------------------------
TRUNCATE TABLE dbo.Journal;
TRUNCATE TABLE dbo.Asset;
TRUNCATE TABLE dbo.Liability;
TRUNCATE TABLE dbo.Income;
TRUNCATE TABLE dbo.Expenditure;

-- Parent cannot be TRUNCATE because it is referenced by FKs
DELETE FROM dbo.Client;

-- Reseed identities (start back at 1)
DBCC CHECKIDENT ('dbo.Journal', RESEED, 0);
DBCC CHECKIDENT ('dbo.Asset', RESEED, 0);
DBCC CHECKIDENT ('dbo.Liability', RESEED, 0);
DBCC CHECKIDENT ('dbo.Income', RESEED, 0);
DBCC CHECKIDENT ('dbo.Expenditure', RESEED, 0);
DBCC CHECKIDENT ('dbo.Client', RESEED, 0);

---------------------------------------------------------------
-- Lookup Ids
---------------------------------------------------------------
DECLARE @ClientStatusLookupId INT =
  (SELECT LookupId FROM dbo.Lookup WHERE Code = N'CLIENT_STATUS');
IF @ClientStatusLookupId IS NULL
    THROW 51000, 'Lookup CLIENT_STATUS not found. Seed lookups first.', 1;

DECLARE @JournalNoteTypeLookupId INT =
  (SELECT LookupId FROM dbo.Lookup WHERE Code = N'JOURNAL_NOTE_TYPE');
IF @JournalNoteTypeLookupId IS NULL
    THROW 51001, 'Lookup JOURNAL_NOTE_TYPE not found. Seed lookups first.', 1;

DECLARE @Status TABLE (LookupValueId INT PRIMARY KEY, Code NVARCHAR(100), Label NVARCHAR(200), SortOrder INT);
INSERT INTO @Status
SELECT LookupValueId, Code, Label, SortOrder
FROM dbo.LookupValue
WHERE LookupId = @ClientStatusLookupId AND IsActive = 1;

DECLARE @NoteTypes TABLE (LookupValueId INT PRIMARY KEY, Code NVARCHAR(100), Label NVARCHAR(200), SortOrder INT);
INSERT INTO @NoteTypes
SELECT LookupValueId, Code, Label, SortOrder
FROM dbo.LookupValue
WHERE LookupId = @JournalNoteTypeLookupId AND IsActive = 1;

---------------------------------------------------------------
-- Random source tables
---------------------------------------------------------------
DECLARE @FirstNames TABLE (Name NVARCHAR(150));
INSERT INTO @FirstNames(Name) VALUES
(N'Alice'),(N'Ben'),(N'Chloe'),(N'Daniel'),(N'Ella'),
(N'Freya'),(N'George'),(N'Harry'),(N'Isla'),(N'Jack'),
(N'Katie'),(N'Liam'),(N'Mia'),(N'Noah'),(N'Oliver'),
(N'Poppy'),(N'Quinn'),(N'Ruby'),(N'Sam'),(N'Theodore');

DECLARE @LastNames TABLE (Name NVARCHAR(150));
INSERT INTO @LastNames(Name) VALUES
(N'Brown'),(N'Carter'),(N'Davis'),(N'Evans'),(N'Fox'),
(N'Green'),(N'Hughes'),(N'Ives'),(N'Jones'),(N'King'),
(N'Lewis'),(N'Moore'),(N'Nelson'),(N'Owen'),(N'Parker'),
(N'Quinn'),(N'Reed'),(N'Scott'),(N'Taylor'),(N'Walker');

DECLARE @Authors TABLE (Name NVARCHAR(100));
INSERT INTO @Authors(Name) VALUES
(N'Advisor A'),(N'Advisor B'),(N'Advisor C'),
(N'Ops Team'),(N'Compliance'),(N'System');

DECLARE @AssetTypes TABLE (Code NVARCHAR(50));
INSERT INTO @AssetTypes(Code) VALUES (N'pension'),(N'isa'),(N'cash'),(N'property');

DECLARE @LiabilityTypes TABLE (Code NVARCHAR(50));
INSERT INTO @LiabilityTypes(Code) VALUES (N'mortgage'),(N'loan'),(N'credit_card');

DECLARE @IncomeSources TABLE (Code NVARCHAR(50));
INSERT INTO @IncomeSources(Code) VALUES (N'employment'),(N'pension'),(N'rental'),(N'self_employed');

DECLARE @ExpenditureCats TABLE (Code NVARCHAR(50));
INSERT INTO @ExpenditureCats(Code) VALUES (N'housing'),(N'utilities'),(N'food'),(N'transport'),
(N'leisure'),(N'healthcare'),(N'insurance');

DECLARE @Providers TABLE (Name NVARCHAR(200));
INSERT INTO @Providers(Name) VALUES
(N'Aviva'),(N'Legal & General'),(N'Scottish Widows'),
(N'BlackRock'),(N'Vanguard'),(N'Nationwide'),(N'HSBC');

-- counts
DECLARE @FNCount INT = (SELECT COUNT(*) FROM @FirstNames);
DECLARE @LNCount INT = (SELECT COUNT(*) FROM @LastNames);
DECLARE @AuthCount INT = (SELECT COUNT(*) FROM @Authors);
DECLARE @AssetTypeCount INT = (SELECT COUNT(*) FROM @AssetTypes);
DECLARE @LiabTypeCount INT = (SELECT COUNT(*) FROM @LiabilityTypes);
DECLARE @IncomeSrcCount INT = (SELECT COUNT(*) FROM @IncomeSources);
DECLARE @ExCatCount INT = (SELECT COUNT(*) FROM @ExpenditureCats);
DECLARE @ProviderCount INT = (SELECT COUNT(*) FROM @Providers);
DECLARE @NoteTypeCount INT = (SELECT COUNT(*) FROM @NoteTypes);
DECLARE @StatusCount INT = (SELECT COUNT(*) FROM @Status);

---------------------------------------------------------------
-- Generate ~100 clients + related data
---------------------------------------------------------------
DECLARE @TargetClients INT = 100;
DECLARE @i INT = 1;

WHILE @i <= @TargetClients
BEGIN
    -- pick names
    DECLARE @FirstName NVARCHAR(150), @LastName NVARCHAR(150), @rn INT;

    SET @rn = 1 + (ABS(CHECKSUM(NEWID())) % @FNCount);
    SELECT @FirstName = Name FROM (SELECT Name, ROW_NUMBER() OVER (ORDER BY (SELECT 1)) rn FROM @FirstNames) t WHERE t.rn = @rn;

    SET @rn = 1 + (ABS(CHECKSUM(NEWID())) % @LNCount);
    SELECT @LastName = Name FROM (SELECT Name, ROW_NUMBER() OVER (ORDER BY (SELECT 1)) rn FROM @LastNames) t WHERE t.rn = @rn;

    -- DOB 1955–1995
    DECLARE @DaysRange INT = DATEDIFF(DAY, '1955-01-01', '1995-12-31');
    DECLARE @DOB DATE = DATEADD(DAY, ABS(CHECKSUM(NEWID())) % @DaysRange, '1955-01-01');

    -- unique-ish email
    DECLARE @Email NVARCHAR(254) = LOWER(CONCAT(REPLACE(@FirstName,' ',''),
                                               '.', REPLACE(@LastName,' ',''),
                                               '.', @i, '@example.com'));
    -- UK mobile
    DECLARE @Phone NVARCHAR(30) = CONCAT('07', RIGHT('000000000' + CAST(ABS(CHECKSUM(NEWID())) % 1000000000 AS VARCHAR(10)), 9));

    -- status
    DECLARE @StatusLookupValueId INT;
    SET @rn = 1 + (ABS(CHECKSUM(NEWID())) % @StatusCount);
    SELECT @StatusLookupValueId = LookupValueId
    FROM (SELECT LookupValueId, ROW_NUMBER() OVER (ORDER BY SortOrder) rn FROM @Status) s
    WHERE s.rn = @rn;

    INSERT INTO dbo.Client(FirstName, LastName, DateOfBirth, Email, Phone, StatusLookupValueId)
    VALUES (@FirstName, @LastName, @DOB, @Email, @Phone, @StatusLookupValueId);

    DECLARE @ClientId INT = SCOPE_IDENTITY();

    -- journals: 3–8
    DECLARE @JCount INT = 3 + (ABS(CHECKSUM(NEWID())) % 6);
    DECLARE @j INT = 1;
    WHILE @j <= @JCount
    BEGIN
        DECLARE @NoteTypeId INT, @Author NVARCHAR(100);
        SET @rn = 1 + (ABS(CHECKSUM(NEWID())) % @NoteTypeCount);
        SELECT @NoteTypeId = LookupValueId
        FROM (SELECT LookupValueId, ROW_NUMBER() OVER (ORDER BY SortOrder) rn FROM @NoteTypes) t
        WHERE t.rn = @rn;

        SET @rn = 1 + (ABS(CHECKSUM(NEWID())) % @AuthCount);
        SELECT @Author = Name
        FROM (SELECT Name, ROW_NUMBER() OVER (ORDER BY (SELECT 1)) rn FROM @Authors) a
        WHERE a.rn = @rn;

        DECLARE @OccurredAt DATETIME2(3) = DATEADD(DAY, - (ABS(CHECKSUM(NEWID())) % 365), SYSUTCDATETIME());
        DECLARE @Body NVARCHAR(MAX) = CONCAT('Client ', @FirstName, ' ', @LastName, ' – follow-up note #', @j, '.');

        INSERT INTO dbo.Journal(ClientId, NoteTypeLookupValueId, OccurredAt, Body, Author)
        VALUES (@ClientId, @NoteTypeId, @OccurredAt, @Body, @Author);

        SET @j += 1;
    END

    -- assets: 0–3
    DECLARE @ACount INT = ABS(CHECKSUM(NEWID())) % 4;
    DECLARE @a INT = 1;
    WHILE @a <= @ACount
    BEGIN
        DECLARE @AssetType NVARCHAR(50), @Provider NVARCHAR(200);
        SET @rn = 1 + (ABS(CHECKSUM(NEWID())) % @AssetTypeCount);
        SELECT @AssetType = Code FROM (SELECT Code, ROW_NUMBER() OVER (ORDER BY (SELECT 1)) rn FROM @AssetTypes) t WHERE t.rn = @rn;

        SET @rn = 1 + (ABS(CHECKSUM(NEWID())) % @ProviderCount);
        SELECT @Provider = Name FROM (SELECT Name, ROW_NUMBER() OVER (ORDER BY (SELECT 1)) rn FROM @Providers) p WHERE p.rn = @rn;

        DECLARE @AssetValue DECIMAL(18,2) =
            CASE @AssetType
              WHEN 'pension'  THEN 20000 + (ABS(CHECKSUM(NEWID())) % 200000)
              WHEN 'property' THEN 120000 + (ABS(CHECKSUM(NEWID())) % 400000)
              WHEN 'isa'      THEN 5000 + (ABS(CHECKSUM(NEWID())) % 80000)
              ELSE                 1000 + (ABS(CHECKSUM(NEWID())) % 20000)
            END;

        DECLARE @AssetAsOf DATETIME2(3) = DATEADD(DAY, - (ABS(CHECKSUM(NEWID())) % 180), SYSUTCDATETIME());

        INSERT INTO dbo.Asset(ClientId, AssetType, [Value], Provider, AsOf)
        VALUES (@ClientId, @AssetType, @AssetValue, @Provider, @AssetAsOf);

        SET @a += 1;
    END

    -- liabilities: 0–2
    DECLARE @LCount INT = ABS(CHECKSUM(NEWID())) % 3;
    DECLARE @l INT = 1;
    WHILE @l <= @LCount
    BEGIN
        DECLARE @LiabType NVARCHAR(50);
        SET @rn = 1 + (ABS(CHECKSUM(NEWID())) % @LiabTypeCount);
        SELECT @LiabType = Code FROM (SELECT Code, ROW_NUMBER() OVER (ORDER BY (SELECT 1)) rn FROM @LiabilityTypes) t WHERE t.rn = @rn;

        DECLARE @Balance DECIMAL(18,2) =
            CASE @LiabType
              WHEN 'mortgage'    THEN 50000 + (ABS(CHECKSUM(NEWID())) % 350000)
              WHEN 'loan'        THEN 2000 + (ABS(CHECKSUM(NEWID())) % 30000)
              WHEN 'credit_card' THEN 200 + (ABS(CHECKSUM(NEWID())) % 8000)
              ELSE 1000 + (ABS(CHECKSUM(NEWID())) % 20000)
            END;

        DECLARE @Rate DECIMAL(9,4) = (ABS(CHECKSUM(NEWID())) % 900) / 100.0;

        DECLARE @LiabAsOf DATETIME2(3) = DATEADD(DAY, - (ABS(CHECKSUM(NEWID())) % 120), SYSUTCDATETIME());

        INSERT INTO dbo.Liability(ClientId, LiabilityType, Balance, Rate, AsOf)
        VALUES (@ClientId, @LiabType, @Balance, @Rate, @LiabAsOf);

        SET @l += 1;
    END

    -- income: 1–2
    DECLARE @ICount INT = 1 + (ABS(CHECKSUM(NEWID())) % 2);
    DECLARE @ix INT = 1;
    WHILE @ix <= @ICount
    BEGIN
        DECLARE @IncomeSrc NVARCHAR(50);
        SET @rn = 1 + (ABS(CHECKSUM(NEWID())) % @IncomeSrcCount);
        SELECT @IncomeSrc = Code FROM (SELECT Code, ROW_NUMBER() OVER (ORDER BY (SELECT 1)) rn FROM @IncomeSources) t WHERE t.rn = @rn;

        DECLARE @IncomeAmt DECIMAL(18,2) =
            CASE @IncomeSrc
              WHEN 'employment'    THEN 1500 + (ABS(CHECKSUM(NEWID())) % 4000)
              WHEN 'pension'       THEN 800  + (ABS(CHECKSUM(NEWID())) % 3000)
              WHEN 'rental'        THEN 500  + (ABS(CHECKSUM(NEWID())) % 2500)
              WHEN 'self_employed' THEN 1000 + (ABS(CHECKSUM(NEWID())) % 5000)
              ELSE 1000 + (ABS(CHECKSUM(NEWID())) % 3000)
            END;

        DECLARE @IncomeAsOf DATETIME2(3) = DATEADD(DAY, - (ABS(CHECKSUM(NEWID())) % 90), SYSUTCDATETIME());

        INSERT INTO dbo.Income(ClientId, [Source], AmountMonthly, AsOf)
        VALUES (@ClientId, @IncomeSrc, @IncomeAmt, @IncomeAsOf);

        SET @ix += 1;
    END

    -- expenditures: 3–6
    DECLARE @ECount INT = 3 + (ABS(CHECKSUM(NEWID())) % 4);
    DECLARE @ex INT = 1;
    WHILE @ex <= @ECount
    BEGIN
        DECLARE @ExCat NVARCHAR(50);
        SET @rn = 1 + (ABS(CHECKSUM(NEWID())) % @ExCatCount);
        SELECT @ExCat = Code FROM (SELECT Code, ROW_NUMBER() OVER (ORDER BY (SELECT 1)) rn FROM @ExpenditureCats) t WHERE t.rn = @rn;

        DECLARE @ExAmt DECIMAL(18,2) =
            CASE @ExCat
              WHEN 'housing'   THEN 600 + (ABS(CHECKSUM(NEWID())) % 900)
              WHEN 'utilities' THEN 150 + (ABS(CHECKSUM(NEWID())) % 250)
              WHEN 'food'      THEN 200 + (ABS(CHECKSUM(NEWID())) % 400)
              WHEN 'transport' THEN 80  + (ABS(CHECKSUM(NEWID())) % 220)
              WHEN 'leisure'   THEN 50  + (ABS(CHECKSUM(NEWID())) % 200)
              WHEN 'healthcare'THEN 30  + (ABS(CHECKSUM(NEWID())) % 120)
              WHEN 'insurance' THEN 20  + (ABS(CHECKSUM(NEWID())) % 150)
              ELSE 50 + (ABS(CHECKSUM(NEWID())) % 200)
            END;

        DECLARE @ExAsOf DATETIME2(3) = DATEADD(DAY, - (ABS(CHECKSUM(NEWID())) % 60), SYSUTCDATETIME());

        INSERT INTO dbo.Expenditure(ClientId, Category, AmountMonthly, AsOf)
        VALUES (@ClientId, @ExCat, @ExAmt, @ExAsOf);

        SET @ex += 1;
    END

    SET @i += 1;
END

COMMIT TRAN;
PRINT 'Seed complete: ~100 clients with related data generated.';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRAN;
    DECLARE @msg NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR('Seeder failed: %s', 16, 1, @msg);
END CATCH;

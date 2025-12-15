/* ============================================================
   Client Management Schema (singular names + generic lookups)
   SQL Server / LocalDB compatible
   ============================================================ */

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;

BEGIN TRAN;

---------------------------------------------------------------
-- Generic lookup tables
---------------------------------------------------------------
IF OBJECT_ID('dbo.Lookup', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Lookup
  (
    LookupId   INT IDENTITY(1,1) PRIMARY KEY,
    Code       NVARCHAR(100)  NOT NULL,   -- e.g., 'CLIENT_STATUS', 'JOURNAL_NOTE_TYPE'
    Name       NVARCHAR(200)  NOT NULL,
    IsActive   BIT            NOT NULL DEFAULT (1),
    CreatedAt  DATETIME2(3)   NOT NULL DEFAULT SYSUTCDATETIME()
  );

  CREATE UNIQUE INDEX UX_Lookup_Code ON dbo.Lookup(Code);
END;

IF OBJECT_ID('dbo.LookupValue', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.LookupValue
  (
    LookupValueId INT IDENTITY(1,1) PRIMARY KEY,
    LookupId      INT           NOT NULL REFERENCES dbo.Lookup(LookupId),
    Code          NVARCHAR(100) NOT NULL,   -- e.g., 'NEW', 'QUOTES_SENT'
    Label         NVARCHAR(200) NOT NULL,   -- e.g., 'New', 'Quotes Sent'
    SortOrder     INT           NOT NULL DEFAULT (0),
    IsActive      BIT           NOT NULL DEFAULT (1),
    CreatedAt     DATETIME2(3)  NOT NULL DEFAULT SYSUTCDATETIME()
  );

  -- Uniqueness of codes within a lookup set
  CREATE UNIQUE INDEX UX_LookupValue_Lookup_Code
    ON dbo.LookupValue(LookupId, Code);

  -- Helpful for ordered dropdowns
  CREATE INDEX IX_LookupValue_Lookup_Sort
    ON dbo.LookupValue(LookupId, SortOrder);
END;

---------------------------------------------------------------
-- Core entities (singular)
---------------------------------------------------------------
IF OBJECT_ID('dbo.Client', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Client
  (
    ClientId      INT IDENTITY(1,1) PRIMARY KEY,
    FirstName     NVARCHAR(150) NOT NULL,
    LastName      NVARCHAR(150) NOT NULL,
    DateOfBirth   DATE          NULL,
    Email         NVARCHAR(254) NULL,
    Phone         NVARCHAR(30)  NULL,

    -- Current journey status via LookupValue (CLIENT_STATUS)
    StatusLookupValueId INT     NULL REFERENCES dbo.LookupValue(LookupValueId),

    CreatedAt     DATETIME2(3)  NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt     DATETIME2(3)  NOT NULL DEFAULT SYSUTCDATETIME()
  );

  -- Search-friendly and common filters
  CREATE INDEX IX_Client_Name ON dbo.Client(LastName, FirstName);
  CREATE INDEX IX_Client_Status ON dbo.Client(StatusLookupValueId);

  -- Email unique when provided (allows multiple NULLs)
  CREATE UNIQUE INDEX UX_Client_Email
    ON dbo.Client(Email)
    WHERE Email IS NOT NULL;
END;

IF OBJECT_ID('dbo.Journal', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Journal
  (
    JournalId     INT IDENTITY(1,1) PRIMARY KEY,
    ClientId      INT           NOT NULL REFERENCES dbo.Client(ClientId),

    -- Note type via LookupValue (JOURNAL_NOTE_TYPE)
    NoteTypeLookupValueId INT  NOT NULL REFERENCES dbo.LookupValue(LookupValueId),

    OccurredAt    DATETIME2(3) NOT NULL,     -- when it happened
    Body          NVARCHAR(MAX) NOT NULL,
    Author        NVARCHAR(100) NULL,
    CreatedAt     DATETIME2(3) NOT NULL DEFAULT SYSUTCDATETIME()
  );

  CREATE INDEX IX_Journal_Client_OccurredAt
    ON dbo.Journal(ClientId, OccurredAt DESC);
END;

IF OBJECT_ID('dbo.Asset', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Asset
  (
    AssetId     INT IDENTITY(1,1) PRIMARY KEY,
    ClientId    INT            NOT NULL REFERENCES dbo.Client(ClientId),
    AssetType   NVARCHAR(50)   NOT NULL,    -- pension, isa, cash, property (could be another lookup later)
    Value       DECIMAL(18,2)  NOT NULL,
    Provider    NVARCHAR(200)  NULL,
    AsOf        DATETIME2(3)   NOT NULL DEFAULT SYSUTCDATETIME(),
    CreatedAt   DATETIME2(3)   NOT NULL DEFAULT SYSUTCDATETIME()
  );

  CREATE INDEX IX_Asset_Client_AsOf
    ON dbo.Asset(ClientId, AsOf DESC);
END;

IF OBJECT_ID('dbo.Liability', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Liability
  (
    LiabilityId    INT IDENTITY(1,1) PRIMARY KEY,
    ClientId       INT            NOT NULL REFERENCES dbo.Client(ClientId),
    LiabilityType  NVARCHAR(50)   NOT NULL,  -- mortgage, loan, cc (could also be lookup later)
    Balance        DECIMAL(18,2)  NOT NULL,
    Rate           DECIMAL(9,4)   NULL,      -- interest rate if relevant
    AsOf           DATETIME2(3)   NOT NULL DEFAULT SYSUTCDATETIME(),
    CreatedAt      DATETIME2(3)   NOT NULL DEFAULT SYSUTCDATETIME()
  );

  CREATE INDEX IX_Liability_Client_AsOf
    ON dbo.Liability(ClientId, AsOf DESC);
END;

IF OBJECT_ID('dbo.Income', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Income
  (
    IncomeId       INT IDENTITY(1,1) PRIMARY KEY,
    ClientId       INT            NOT NULL REFERENCES dbo.Client(ClientId),
    Source         NVARCHAR(50)   NOT NULL,   -- employment, pension, rental (could be lookup later)
    AmountMonthly  DECIMAL(18,2)  NOT NULL,
    AsOf           DATETIME2(3)   NOT NULL DEFAULT SYSUTCDATETIME(),
    CreatedAt      DATETIME2(3)   NOT NULL DEFAULT SYSUTCDATETIME()
  );

  CREATE INDEX IX_Income_Client_AsOf
    ON dbo.Income(ClientId, AsOf DESC);
END;

IF OBJECT_ID('dbo.Expenditure', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Expenditure
  (
    ExpenditureId   INT IDENTITY(1,1) PRIMARY KEY,
    ClientId        INT            NOT NULL REFERENCES dbo.Client(ClientId),
    Category        NVARCHAR(50)   NOT NULL,  -- housing, utilities, food (could be lookup later)
    AmountMonthly   DECIMAL(18,2)  NOT NULL,
    AsOf            DATETIME2(3)   NOT NULL DEFAULT SYSUTCDATETIME(),
    CreatedAt       DATETIME2(3)   NOT NULL DEFAULT SYSUTCDATETIME()
  );

  CREATE INDEX IX_Expenditure_Client_AsOf
    ON dbo.Expenditure(ClientId, AsOf DESC);
END;

---------------------------------------------------------------
-- Seed Lookups: CLIENT_STATUS and JOURNAL_NOTE_TYPE
---------------------------------------------------------------
-- CLIENT_STATUS
IF NOT EXISTS (SELECT 1 FROM dbo.Lookup WHERE Code = N'CLIENT_STATUS')
BEGIN
  INSERT INTO dbo.Lookup (Code, Name) VALUES (N'CLIENT_STATUS', N'Client Journey Status');
END;

DECLARE @StatusLookupId INT =
(
  SELECT LookupId FROM dbo.Lookup WHERE Code = N'CLIENT_STATUS'
);

-- Insert statuses if not present
INSERT INTO dbo.LookupValue (LookupId, Code, Label, SortOrder)
SELECT @StatusLookupId, v.Code, v.Label, v.SortOrder
FROM (VALUES
  (N'NEW',             N'New',                   10),
  (N'QUOTES_SENT',     N'Quotes Sent',           20),
  (N'IFA_CALL_BOOKED', N'IFA Call Booked',       30),
  (N'APP_PACK_SENT',   N'Application Pack Sent', 40),
  (N'APP_PACK_BACK',   N'Application Pack Back', 50),
  (N'COMPLETED',       N'Completed',             60),
  (N'NOT_PROCEEDING',  N'Not Proceeding',        70)
) AS v(Code, Label, SortOrder)
WHERE NOT EXISTS (
  SELECT 1
  FROM dbo.LookupValue lv
  WHERE lv.LookupId = @StatusLookupId AND lv.Code = v.Code
);

-- JOURNAL_NOTE_TYPE
IF NOT EXISTS (SELECT 1 FROM dbo.Lookup WHERE Code = N'JOURNAL_NOTE_TYPE')
BEGIN
  INSERT INTO dbo.Lookup (Code, Name) VALUES (N'JOURNAL_NOTE_TYPE', N'Journal Note Type');
END;

DECLARE @NoteTypeLookupId INT =
(
  SELECT LookupId FROM dbo.Lookup WHERE Code = N'JOURNAL_NOTE_TYPE'
);

INSERT INTO dbo.LookupValue (LookupId, Code, Label, SortOrder)
SELECT @NoteTypeLookupId, v.Code, v.Label, v.SortOrder
FROM (VALUES
  (N'CALL',   N'Call',   10),
  (N'EMAIL',  N'Email',  20),
  (N'NOTE',   N'Note',   30),
  (N'SYSTEM', N'System', 40)
) AS v(Code, Label, SortOrder)
WHERE NOT EXISTS (
  SELECT 1
  FROM dbo.LookupValue lv
  WHERE lv.LookupId = @NoteTypeLookupId AND lv.Code = v.Code
);

COMMIT TRAN;

/* ------------------------------------------------------------
   Notes:
   - Client.StatusLookupValueId and Journal.NoteTypeLookupValueId
     point to dbo.LookupValue. You can optionally validate the
     “type” in application code (i.e., ensure they belong to
     CLIENT_STATUS or JOURNAL_NOTE_TYPE respectively).
   - All timestamps are UTC (SYSUTCDATETIME()).
   - Email uniqueness allows multiple NULLs (filtered unique index).
   - Add further lookups (e.g., AssetType, LiabilityType) by
     inserting a new row into dbo.Lookup and rows into dbo.LookupValue.
   ------------------------------------------------------------ */
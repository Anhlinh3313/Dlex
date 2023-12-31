CREATE TABLE Core_Service(
	[Id] [INT] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[CreatedWhen] [DATETIME] NULL,
	[CreatedBy] [INT] NULL,
	[ModifiedWhen] [DATETIME] NULL,
	[ModifiedBy] [INT] NULL,
	[Code] [NVARCHAR](100) NOT NULL,
	[Name] [NVARCHAR](256) NOT NULL,
	[ServiceMain] [BIT] NULL,
	[ServiceAuxiliary] [BIT] NULL,
	[ServivePacking] [BIT] NULL,
	[IsEnabled] [BIT] NULL,
	[ConcurrencyStamp] [NVARCHAR](MAX) NULL,
	[CompanyId] [INT] NULL,
	)
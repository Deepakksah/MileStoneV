-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RequestLog]') AND type = 'U')
CREATE TABLE [RequestLog]
(
	[RequestId] BIGINT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[WebMethodName] VARCHAR(256) NOT NULL,
	[StartTime] DATETIME NOT NULL,
	[ExecutionTime] BIGINT NOT NULL,
	[IdentityName] NVARCHAR(256) NOT NULL,
	[IPAddress] VARCHAR(40) NOT NULL,
	[ExecutionCount] INT NOT NULL,
	[TempId] BIGINT NOT NULL
)
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RequestLogParameter]') AND type = 'U')
CREATE TABLE [RequestLogParameter]
(
	[RequestId] BIGINT NOT NULL,
	[ParameterName] VARCHAR(64) NOT NULL,
	[ParameterIndex] INT NOT NULL,
	[ParameterValue] NVARCHAR(MAX) NULL
)
GO

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.[name] = N'tvp_RequestLogTable' AND ss.name = N'dbo')
CREATE TYPE [tvp_RequestLogTable] AS TABLE
(
	[TempId] INT NOT NULL,
	[WebMethodName] VARCHAR(256) NOT NULL,
	[StartTime] DATETIME NOT NULL,
	[ExecutionTime] BIGINT NOT NULL,
	[IdentityName] NVARCHAR(256) NOT NULL,
	[IPAddress] VARCHAR(40) NOT NULL,
	[ExecutionCount] INT NOT NULL
)
GO

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.[name] = N'tvp_RequestParameterTable' AND ss.name = N'dbo')
CREATE TYPE [tvp_RequestParameterTable] AS TABLE
(
	[TempId] INT NOT NULL,
	[ParameterName]	VARCHAR(64) NOT NULL,
	[ParameterIndex] INT NOT NULL,
	[ParameterValue] NVARCHAR(MAX) NOT NULL
)
GO

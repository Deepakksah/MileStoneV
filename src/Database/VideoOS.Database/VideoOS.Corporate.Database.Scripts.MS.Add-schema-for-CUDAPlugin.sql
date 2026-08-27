-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'CUDAPlugin')
	EXECUTE('CREATE SCHEMA[CUDAPlugin]')
GO

IF NOT EXISTS (SELECT * FROM sys.types WHERE name = N'CUDAMessages')
BEGIN
	CREATE TYPE dbo.CUDAMessages AS TABLE
		( CUDAInfo xml, SourceId xml, EventId uniqueidentifier )
END
GO

-- CUDA Message source table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[CUDAPlugin].[Message]') AND type in (N'U'))
BEGIN
CREATE TABLE [CUDAPlugin].[Message](
	[Id] int identity,
	[ReceiveTime] datetime2 NOT NULL,
	[CUDAInfo] xml NOT NULL,
	[SourceId] xml NOT NULL,
	[EventId] uniqueidentifier NULL
 CONSTRAINT [PK_CudaMessageWrapper] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

-- CUDA refresh interval source table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[CUDAPlugin].[RefreshInterval]') AND type in (N'U'))
BEGIN
	CREATE TABLE [CUDAPlugin].[RefreshInterval](
	[IntervalSecs] int NOT NULL,
	 CONSTRAINT [PK_CudaRefreshInterval] PRIMARY KEY CLUSTERED 
	(
		[IntervalSecs] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
	-- default value 20m
	INSERT INTO [CUDAPlugin].[RefreshInterval] VALUES(20*60)
END
GO

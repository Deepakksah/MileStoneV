CREATE TYPE [dbo].[DescriptionType] FROM [nvarchar](max) NOT NULL
GO
CREATE TYPE [dbo].[DescriptionType2] FROM [nvarchar](4000) NOT NULL
GO
CREATE TYPE [dbo].[GuidType] FROM [char](36) NOT NULL
GO
CREATE TYPE [dbo].[InternalIdType] FROM [varchar](100) NOT NULL
GO
CREATE TYPE [dbo].[LongNameType] FROM [nvarchar](1024) NOT NULL
GO
CREATE TYPE [dbo].[NameType] FROM [nvarchar](256) NOT NULL
GO
CREATE TYPE [dbo].[PasswordType] FROM [nvarchar](256) NOT NULL
GO
CREATE TYPE [dbo].[SecurityValueType] FROM [nvarchar](256) NOT NULL
GO
CREATE TYPE [dbo].[ShortNameType] FROM [nvarchar](128) NOT NULL
GO
CREATE TYPE [dbo].[SIDType] FROM [varchar](256) NOT NULL
GO
CREATE TYPE [dbo].[UriType] FROM [nvarchar](max) NOT NULL
GO
CREATE TYPE [dbo].[UriTypeShort] FROM [nvarchar](1024) NOT NULL
GO
CREATE TYPE [dbo].[UserIdType] FROM [nvarchar](256) NOT NULL
GO
CREATE TYPE [dbo].[CUDAMessages] AS TABLE(
	[CUDAInfo] [xml] NULL,
	[SourceId] [xml] NULL,
	[EventId] [uniqueidentifier] NULL
)
GO
CREATE TYPE [dbo].[tvp_IdTableInt] AS TABLE(
	[ID] [int] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
CREATE TYPE [dbo].[tvp_IdTableUniqueIdentifier] AS TABLE(
	[ID] [uniqueidentifier] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
CREATE TYPE [dbo].[tvp_LicenseUpdateTable] AS TABLE(
	[Id] [uniqueidentifier] NOT NULL,
	[UniqueId] [varchar](700) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Expires] [bigint] NOT NULL,
	INDEX [IX_TVP_LicenseUpdateTable_ID] NONCLUSTERED 
(
	[Id] ASC
),
	PRIMARY KEY NONCLUSTERED 
(
	[UniqueId] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
CREATE TYPE [dbo].[tvp_RequestLogTable] AS TABLE(
	[TempId] [int] NOT NULL,
	[WebMethodName] [varchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[ExecutionTime] [bigint] NOT NULL,
	[IdentityName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IPAddress] [varchar](40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ExecutionCount] [int] NOT NULL
)
GO
CREATE TYPE [dbo].[tvp_RequestParameterTable] AS TABLE(
	[TempId] [int] NOT NULL,
	[ParameterName] [varchar](64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ParameterIndex] [int] NOT NULL,
	[ParameterValue] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
CREATE TYPE [dbo].[tvp_StickerSizeTable] AS TABLE(
	[DeviceId] [dbo].[GuidType] NOT NULL,
	[Size] [bigint] NULL
)
GO
CREATE TYPE [dbo].[tvp_StringTableGuidType] AS TABLE(
	[ID] [dbo].[GuidType] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
CREATE TYPE [dbo].[tvp_StringTableNvarchar] AS TABLE(
	[ID] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
CREATE TYPE [dbo].[tvp_StringTableVarchar] AS TABLE(
	[ID] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
CREATE TYPE [Mip].[IdTable] AS TABLE(
	[Id] [uniqueidentifier] NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ExtractDetectedModel](@xml XML)
RETURNS VARCHAR(256)
WITH schemabinding
AS
BEGIN
	RETURN @xml.value('(/properties/settings[@hwid="hardware"]/setting[@typeid="928B6039-4E4A-4B03-9A82-A1CF29748ACC"]/value)[1]', 'varchar(256)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ExtractDriverName](@xml XML)
RETURNS VARCHAR(256)
WITH schemabinding
AS
BEGIN
	RETURN @xml.value('(/definition/driver/name)[1]', 'varchar(256)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ExtractDriverVersion](@xml XML)
RETURNS VARCHAR(256)
WITH schemabinding
AS
BEGIN
	RETURN @xml.value('(/definition/driver/version)[1]', 'varchar(256)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ExtractMacAddress](@xml XML)
RETURNS VARCHAR(256)
WITH schemabinding
AS
BEGIN
	RETURN @xml.value('(/properties/settings[@hwid="hardware"]/setting[@typeid="F9D27CE8-8FA9-4B91-8C85-3E80DE7EF0AB"]/value)[1]', 'varchar(256)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ExtractSerialNumber](@xml XML)
RETURNS VARCHAR(256)
WITH schemabinding
AS
BEGIN
	RETURN @xml.value('(/properties/settings[@hwid="hardware"]/setting[@typeid="F1DED00E-9614-475F-A6F7-BA29FCA8DE39"]/value)[1]', 'varchar(256)') 
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetOriginalDeviceID](@DeviceSettings XML, @DeviceID  UNIQUEIDENTIFIER)
RETURNS UNIQUEIDENTIFIER
with schemabinding
AS   
BEGIN   
	DECLARE @TempOriginalDeviceID UNIQUEIDENTIFIER;    
	SELECT @TempOriginalDeviceID = @DeviceSettings.value('(properties/settings/setting[@typeid=''C7A3ADD5-2EDC-4852-A72E-8BA695156C99'']/value)[1]', 'char(36)');   
	IF(@TempOriginalDeviceID IS NULL)
		RETURN Convert(UNIQUEIDENTIFIER ,@DeviceID)
	RETURN Convert(UNIQUEIDENTIFIER ,@TempOriginalDeviceID)
END; 

GO

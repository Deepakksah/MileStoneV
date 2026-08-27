-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- ================ CLEAN UP =================================================================================================

IF OBJECT_ID ('TR_Devices_DevicePtzSettingData_AfterInsert','TR') IS NOT NULL
   DROP TRIGGER TR_Devices_DevicePtzSettingData_AfterInsert;
GO

IF OBJECT_ID ('TR_Devices_DevicePtzSettingData_AfterUpdate','TR') IS NOT NULL
   DROP TRIGGER TR_Devices_DevicePtzSettingData_AfterUpdate;
GO

IF EXISTS(SELECT 1 FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[DevicePtzSettingData]') AND [type] IN (N'U'))
BEGIN
	DROP TABLE [DevicePtzSettingData]
END
GO

IF EXISTS(SELECT 1 FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[fn_TRIM]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
	DROP FUNCTION dbo.fn_TRIM
GO

-- =============== CREATE ====================================================================================================

CREATE FUNCTION dbo.fn_TRIM(@s NVARCHAR(MAX))
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @length INT = DATALENGTH(@s) / 2
	DECLARE @firstCharPosition INT = PATINDEX('%[^' + CHAR(32) + CHAR(09) + CHAR(10) + CHAR(13) + ']%', @s)
	DECLARE @lastCharPosition INT = @length - (PATINDEX('%[^' + CHAR(32) + CHAR(09) + CHAR(10) + CHAR(13) + ']%', REVERSE(@s)) - 1)

	RETURN SUBSTRING(@s, @firstCharPosition, @lastCharPosition - @firstCharPosition + 1)
END
GO

CREATE TABLE [DevicePtzSettingData]
(
	[IDDevice] [GuidType] NOT NULL,
	[Hwid] VARCHAR(32) NOT NULL,
	[TypeId] UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(256) NOT NULL,
	[Value] NVARCHAR(2048) NOT NULL

	CONSTRAINT [PK_DevicePtzSettingData] PRIMARY KEY NONCLUSTERED
	(
		[IDDevice] ASC,
		[Hwid] ASC,
		[TypeId] ASC
	)

	CONSTRAINT [FK_DevicePtzSettingData_Devices] FOREIGN KEY([IDDevice]) REFERENCES [Devices] ([IDDevice]) ON DELETE CASCADE

) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM [DevicePtzSettingData])
BEGIN
	INSERT INTO [DevicePtzSettingData]
		SELECT	d.[IDDevice] AS [device_id],
				XmlPtzSettings.value('../@hwid', 'VARCHAR(32)'),
				XmlPtzSettings.value('./@typeid', 'UNIQUEIDENTIFIER'),
				XmlPtzSettings.value('(./name)[1]', 'NVARCHAR(256)'),
				dbo.fn_TRIM(XmlPtzSettings.value('(./value)[1]', 'NVARCHAR(2048)'))
		FROM	[Devices] d
		CROSS APPLY d.[Settings].nodes('properties/settings[substring(@hwid, 1, string-length("deviceptz")) = "deviceptz"]/*') S(XmlPtzSettings)
END
GO

CREATE TRIGGER TR_Devices_DevicePtzSettingData_AfterInsert ON [Devices]
AFTER INSERT
AS
BEGIN
	INSERT INTO [DevicePtzSettingData]
		SELECT	d.[IDDevice] AS [device_id],
				XmlPtzSettings.value('../@hwid', 'VARCHAR(32)'),
				XmlPtzSettings.value('./@typeid', 'UNIQUEIDENTIFIER'),
				XmlPtzSettings.value('(./name)[1]', 'NVARCHAR(256)'),
				dbo.fn_TRIM(XmlPtzSettings.value('(./value)[1]', 'NVARCHAR(2048)'))
		FROM	inserted d
		CROSS APPLY d.[Settings].nodes('properties/settings[substring(@hwid, 1, string-length("deviceptz")) = "deviceptz"]/*') S(XmlPtzSettings)
END
GO

-- Note: This also handles deletion of a device
CREATE TRIGGER TR_Devices_DevicePtzSettingData_AfterUpdate ON [Devices]
AFTER UPDATE
AS
BEGIN
	IF UPDATE([Settings])
	BEGIN
		DELETE	dps
		FROM	[DevicePtzSettingData] dps
				INNER JOIN deleted ON deleted.[IDDevice] = dps.[IDDevice]

		INSERT INTO [DevicePtzSettingData]
			SELECT	d.[IDDevice] AS [device_id],
					XmlPtzSettings.value('../@hwid', 'VARCHAR(32)'),
					XmlPtzSettings.value('./@typeid', 'UNIQUEIDENTIFIER'),
					XmlPtzSettings.value('(./name)[1]', 'NVARCHAR(256)'),
					dbo.fn_TRIM(XmlPtzSettings.value('(./value)[1]', 'NVARCHAR(2048)'))
			FROM	inserted d
			CROSS APPLY d.[Settings].nodes('properties/settings[substring(@hwid, 1, string-length("deviceptz")) = "deviceptz"]/*') S(XmlPtzSettings)
	END
END
GO

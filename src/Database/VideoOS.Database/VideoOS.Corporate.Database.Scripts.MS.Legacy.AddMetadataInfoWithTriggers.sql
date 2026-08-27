-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DeviceMetadataInfo]') AND type in (N'U'))
BEGIN
	DROP TABLE [DeviceMetadataInfo]
END
GO

-- Create new table DeviceMetadataInfo
CREATE TABLE [DeviceMetadataInfo]
(
	[IDDevice] [GuidType] NOT NULL,
	[ItemIndex] [int] NOT NULL,
	[IDReference] [uniqueidentifier] NOT NULL,
	[Name] [NameType] NOT NULL,
	[IDDisplayReference] [uniqueidentifier] NOT NULL,
	[DisplayName] [NameType] NOT NULL

	CONSTRAINT [PK_DeviceMetadataInfo] PRIMARY KEY NONCLUSTERED
	(
		[IDDevice] ASC,
		[IDReference] ASC
	)

	CONSTRAINT [FK_DeviceMetadataInfo_Devices] FOREIGN KEY([IDDevice]) REFERENCES [Devices] ([IDDevice]) ON DELETE CASCADE

) ON [PRIMARY]
GO


IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[LastHwidIndex]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [LastHwidIndex]
GO

CREATE FUNCTION [LastHwidIndex] ( @hwid VARCHAR(128) )
RETURNS INT 
AS
BEGIN 
	RETURN CAST(RIGHT(@hwid, CHARINDEX('.', REVERSE(@hwid))-1) AS INT)
END
GO


IF NOT EXISTS(SELECT TOP 1 * FROM [DeviceMetadataInfo])
BEGIN
	-- Update table for devices already in the dbo
	INSERT INTO [DeviceMetadataInfo]
		SELECT
			d.[IDDevice],
			dbo.LastHwidIndex(T1.deviceNode.value('(capabilities)[1]/@hwid', 'VARCHAR(128)')),
			T1.deviceNode.value('(@referenceid)[1]', 'uniqueidentifier'),
			T1.deviceNode.value('(name)[1]', 'NameType'),
			T1.deviceNode.value('(displayname/@referenceid)[1]', 'uniqueidentifier'),
			T1.deviceNode.value('(displayname)[1]', 'NameType')
		FROM
			[HardwareDefinition] hd
			INNER JOIN [Hardware] h ON h.[C18NDigest] = hd.[C18NDigest]
			INNER JOIN [Devices] d ON d.[IDHardware] = h.[IDHardware] AND d.[DeviceType] = 'Metadata' 
			CROSS APPLY hd.[Definition].nodes('(/definition/hardware/devices/device[capabilities/@hwid=concat(''device:8.'', xs:string(sql:column("d.DeviceIndex")))]/metadataitems/metadataitem)') T1(deviceNode)
END
GO


-- Create trigger on devices after insert
IF OBJECT_ID ('TR_Devices_AfterInsert','TR') IS NOT NULL
   DROP TRIGGER TR_Devices_AfterInsert;
GO

CREATE TRIGGER TR_Devices_AfterInsert ON Devices
AFTER INSERT
AS
BEGIN
	INSERT INTO [DeviceMetadataInfo]
		SELECT
			d.[IDDevice],
			dbo.LastHwidIndex(T1.deviceNode.value('(capabilities)[1]/@hwid', 'VARCHAR(128)')),
			T1.deviceNode.value('(@referenceid)[1]', 'uniqueidentifier'),
			T1.deviceNode.value('(name)[1]', 'NameType'),
			T1.deviceNode.value('(displayname/@referenceid)[1]', 'uniqueidentifier'),
			T1.deviceNode.value('(displayname)[1]', 'NameType')
		FROM
			[HardwareDefinition] hd
			INNER JOIN [Hardware] h ON h.[C18NDigest] = hd.[C18NDigest]
			INNER JOIN inserted d ON d.[IDHardware] = h.[IDHardware] AND d.[DeviceType] = 'Metadata' 
			CROSS APPLY hd.[Definition].nodes('(/definition/hardware/devices/device[capabilities/@hwid=concat(''device:8.'', xs:string(sql:column("d.DeviceIndex")))]/metadataitems/metadataitem)') T1(deviceNode)
END
GO


-- Create trigger on hardware after update of C18NDigest
IF OBJECT_ID ('TR_Hardware_AfterUpdate','TR') IS NOT NULL
   DROP TRIGGER TR_Hardware_AfterUpdate;
GO

CREATE TRIGGER TR_Hardware_AfterUpdate ON Hardware
AFTER UPDATE
AS
BEGIN
	IF UPDATE([C18NDigest])
	BEGIN
		DELETE FROM [DeviceMetadataInfo]
		WHERE [IDDevice] IN (
			SELECT d.[IDDevice]
				FROM
					deleted h 
					INNER JOIN [Devices] d ON d.[IDHardware] = h.[IDHardware] 
		)

		INSERT INTO [DeviceMetadataInfo]
			SELECT
				d.[IDDevice],
				dbo.LastHwidIndex(T1.deviceNode.value('(capabilities)[1]/@hwid', 'VARCHAR(128)')),
				T1.deviceNode.value('(@referenceid)[1]', 'uniqueidentifier'),
				T1.deviceNode.value('(name)[1]', 'NameType'),
				T1.deviceNode.value('(displayname/@referenceid)[1]', 'uniqueidentifier'),
				T1.deviceNode.value('(displayname)[1]', 'NameType')
			FROM
				[HardwareDefinition] hd
				INNER JOIN inserted h ON h.[C18NDigest] = hd.[C18NDigest]
				INNER JOIN [Devices] d ON d.[IDHardware] = h.[IDHardware] AND d.[DeviceType] = 'Metadata' 
				CROSS APPLY hd.[Definition].nodes('(/definition/hardware/devices/device[capabilities/@hwid=concat(''device:8.'', xs:string(sql:column("d.DeviceIndex")))]/metadataitems/metadataitem)') T1(deviceNode)
	END
END
GO

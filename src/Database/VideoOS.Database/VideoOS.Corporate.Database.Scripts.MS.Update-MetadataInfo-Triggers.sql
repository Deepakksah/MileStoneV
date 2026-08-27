-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Function is no longer used
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[LastHwidIndex]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [LastHwidIndex]
GO


-- Create trigger on devices after insert
IF OBJECT_ID ('TR_Devices_AfterInsert','TR') IS NOT NULL
   DROP TRIGGER TR_Devices_AfterInsert;
GO

CREATE TRIGGER TR_Devices_AfterInsert ON Devices
AFTER INSERT
AS
BEGIN
        DECLARE @deviceIndexStart INT = LEN('metadataitem:8.')+1

        DECLARE @metadataItems  TABLE (
                    [IDHardware] [GuidType] NOT NULL INDEX IX_IDHardware NONCLUSTERED,
                    [DeviceIndex] INT NOT NULL INDEX IX_DeviceIndex NONCLUSTERED,
                    [ItemIndex] INT NOT NULL,
                    [ReferenceId] UNIQUEIDENTIFIER NOT NULL,
                    [Name] [NameType] NOT NULL,
                    [DisplayNameReferenceId] UNIQUEIDENTIFIER NOT NULL,
                    [DisplayName] [NameType] NOT NULL);

        INSERT INTO @metadataItems
            SELECT
                h.[IDHardware],
                CAST(
                    SUBSTRING(
                        T1.deviceNode.value('(capabilities)[1]/@hwid', 'VARCHAR(128)'),
                        @deviceIndexStart,
                        CHARINDEX('.', T1.deviceNode.value('(capabilities)[1]/@hwid', 'VARCHAR(128)'), @deviceIndexStart) - @deviceIndexStart
                    )
                    AS INT ) AS DeviceIndex,
                CAST(
					RIGHT(
						T1.deviceNode.value('(capabilities)[1]/@hwid', 'VARCHAR(128)'),
						CHARINDEX('.', REVERSE(T1.deviceNode.value('(capabilities)[1]/@hwid', 'VARCHAR(128)')))-1
					)
					AS INT) AS ItemIndex,
	            T1.deviceNode.value('(@referenceid)[1]', 'UNIQUEIDENTIFIER') AS [ReferenceId],
	            T1.deviceNode.value('(name)[1]', 'NameType') AS [Name],
	            T1.deviceNode.value('(displayname/@referenceid)[1]', 'UNIQUEIDENTIFIER') AS [DisplayNameReferenceId],
	            T1.deviceNode.value('(displayname)[1]', 'NameType') AS [DisplayName]
            FROM
	            [HardwareDefinition] hd
	            INNER JOIN [Hardware] h ON h.[C18NDigest] = hd.[C18NDigest]
                INNER JOIN inserted d ON d.[IDHardware] = h.[IDHardware] AND d.[DeviceType] = 'Metadata'
                CROSS APPLY hd.[Definition].nodes('(/definition/hardware/devices/device/metadataitems/metadataitem)') T1(deviceNode)


		INSERT INTO [DeviceMetadataInfo]
            SELECT
	            d.[IDDevice],
	            m.[ItemIndex],
                m.[ReferenceId],
                m.[Name],
                m.[DisplayNameReferenceId],
                m.[DisplayName]
            FROM
                @metadataItems m
	            INNER JOIN inserted d ON d.[DeviceIndex] = m.DeviceIndex AND d.[IDHardware] = m.[IDHardware] AND d.[DeviceType] = 'Metadata'
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
        DECLARE @deviceIndexStart INT = LEN('metadataitem:8.')+1

        DELETE dmi
            FROM [DeviceMetadataInfo] dmi
            INNER JOIN [Devices] d ON d.[IDDevice] = dmi.[IDDevice] AND d.[DeviceType] = 'Metadata'
            INNER JOIN deleted h ON h.[IDHardware] = d.[IDHardware]


        DECLARE @metadataItems TABLE (
                    [IDHardware] [GuidType] NOT NULL INDEX IX_IDHardware NONCLUSTERED,
                    [DeviceIndex] INT NOT NULL INDEX IX_DeviceIndex NONCLUSTERED,
                    [ItemIndex] INT NOT NULL,
                    [ReferenceId] UNIQUEIDENTIFIER NOT NULL,
                    [Name] [NameType] NOT NULL,
                    [DisplayNameReferenceId] UNIQUEIDENTIFIER NOT NULL,
                    [DisplayName] [NameType] NOT NULL);

        INSERT INTO @metadataItems
            SELECT
                h.[IDHardware],
                CAST(
                    SUBSTRING(
                        T1.deviceNode.value('(capabilities)[1]/@hwid', 'VARCHAR(128)'),
                        @deviceIndexStart,
                        CHARINDEX('.', T1.deviceNode.value('(capabilities)[1]/@hwid', 'VARCHAR(128)'), @deviceIndexStart) - @deviceIndexStart
                    )
                    AS INT ) AS DeviceIndex,
                CAST(
					RIGHT(
						T1.deviceNode.value('(capabilities)[1]/@hwid', 'VARCHAR(128)'),
						CHARINDEX('.', REVERSE(T1.deviceNode.value('(capabilities)[1]/@hwid', 'VARCHAR(128)')))-1
					)
					AS INT) AS ItemIndex,
	            T1.deviceNode.value('(@referenceid)[1]', 'UNIQUEIDENTIFIER') AS [ReferenceId],
	            T1.deviceNode.value('(name)[1]', 'NameType') AS [Name],
	            T1.deviceNode.value('(displayname/@referenceid)[1]', 'UNIQUEIDENTIFIER') AS [DisplayNameReferenceId],
	            T1.deviceNode.value('(displayname)[1]', 'NameType') AS [DisplayName]
            FROM
	            [HardwareDefinition] hd
	            INNER JOIN inserted h ON h.[C18NDigest] = hd.[C18NDigest]
                CROSS APPLY hd.[Definition].nodes('(/definition/hardware/devices/device/metadataitems/metadataitem)') T1(deviceNode)


		INSERT INTO [DeviceMetadataInfo]
            SELECT
	            d.[IDDevice],
	            m.[ItemIndex],
                m.[ReferenceId],
                m.[Name],
                m.[DisplayNameReferenceId],
                m.[DisplayName]
            FROM
                @metadataItems m
	            INNER JOIN [Devices] d ON d.[DeviceIndex] = m.DeviceIndex AND d.[IDHardware] = m.[IDHardware] AND d.[DeviceType] = 'Metadata' 
	END
END
GO

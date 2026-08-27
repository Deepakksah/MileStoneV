SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
ALTER TABLE [dbo].[Devices] ENABLE TRIGGER [TR_Devices_AfterInsert]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
ALTER TABLE [dbo].[Devices] ENABLE TRIGGER [TR_Devices_DevicePtzSettingData_AfterInsert]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
ALTER TABLE [dbo].[Devices] ENABLE TRIGGER [TR_Devices_DevicePtzSettingData_AfterUpdate]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
ALTER TABLE [dbo].[Hardware] ENABLE TRIGGER [TR_Hardware_AfterUpdate]
GO

-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Devices]') AND name = N'MulticastSettings')
BEGIN
	ALTER TABLE [Devices]
	ADD [MulticastSettings] NTEXT NULL
END
GO

UPDATE [Devices]
SET [MulticastSettings] = '
	<properties>
		<settings>
			<setting>
				<name>Enabled</name>
				<value>False</value>
			</setting>
		</settings>
	</properties>
'
WHERE [MulticastSettings] IS NULL
GO

ALTER TABLE [Devices]
ALTER COLUMN [MulticastSettings] NTEXT NOT NULL
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Recorders]') AND name = N'MulticastSettings')
BEGIN
	ALTER TABLE Recorders
	ADD [MulticastSettings] NTEXT NULL
END
GO

DECLARE @counter INT
SET @counter = 0

DECLARE @recorderId [GuidType]
DECLARE @multicastSettings NVARCHAR(MAX)

DECLARE recorder_cursor CURSOR FOR 
SELECT [IDRecorder], [MulticastSettings] FROM [Recorders]
OPEN recorder_cursor

FETCH NEXT FROM recorder_cursor INTO @recorderId, @multicastSettings

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @multicastSettings = '
	<properties>
		<settings>
			<setting>
				<name>ClientIPRangeStart</name>
				<value>232.0.1.' + CAST(@counter AS VARCHAR(3)) + '</value>
			</setting>
			<setting>
				<name>ClientIPRangeEnd</name>
				<value>232.0.1.' + CAST(@counter AS VARCHAR(3)) + '</value>
			</setting>
			<setting>
				<name>ClientPortRangeStart</name>
				<value>6000</value>
			</setting>
			<setting>
				<name>ClientPortRangeEnd</name>
				<value>7000</value>
			</setting>
			<setting>
				<name>ServerAddress</name>
				<value>0.0.0.0</value>
			</setting>
			<setting>
				<name>ServerPort</name>
				<value>0</value>
			</setting>
			<setting>
				<name>MTU</name>
				<value>1500</value>
			</setting>
			<setting>
				<name>TTL</name>
				<value>128</value>
			</setting>
		</settings>
	</properties>
	'

	UPDATE [Recorders]
	SET [MulticastSettings] = @multicastSettings
	WHERE [IDRecorder] = @recorderId
				AND [MulticastSettings] IS NULL

	SET @counter = @counter + 1

	FETCH NEXT FROM recorder_cursor INTO @recorderId, @multicastSettings
END
GO

CLOSE recorder_cursor
DEALLOCATE recorder_cursor

ALTER TABLE [Recorders]
ALTER COLUMN [MulticastSettings] NTEXT NOT NULL
GO

DECLARE @AdministratorLiveMulticastID [GuidType]
SET @AdministratorLiveMulticastID = '1A35788B-5F4A-4efa-948D-D71D2795869F'

-- Add AdministratorLiveMulticast setting specifying if newly added cams should have live multicast enabled or not
IF NOT EXISTS (SELECT * FROM [SystemSettings] WHERE [IDSystemSetting] = @AdministratorLiveMulticastID)
INSERT [SystemSettings] ([LastModified], [IDSystemSetting], [Name], [Setting])
VALUES(
	GETUTCDATE(),
	@AdministratorLiveMulticastID,
	'AdministratorLiveMulticast',
	'false'
)
GO

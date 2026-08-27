-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Create new hosting tables
IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HostingServer]') AND type in (N'U'))
BEGIN
	CREATE TABLE [HostingServer] (
		[pkid] INT IDENTITY NOT NULL,
		[created_date] [datetime] NOT NULL,
		[last_modified] [datetime] NOT NULL,
		[type] NVARCHAR(256) NOT NULL,
		[name] [dbo].[NameType] NOT NULL,
		[description] [dbo].[DescriptionType] NOT NULL,
		[user_name] [dbo].[UserIdType] NOT NULL,
		[password] [dbo].[PasswordType] NOT NULL,
		[internal_address] NVARCHAR(256) NOT NULL,
		[external_address] NVARCHAR(256) NOT NULL
		
		CONSTRAINT [PK_HostingServer] PRIMARY KEY CLUSTERED
		(
			[pkid]
		),
	) ON [PRIMARY]

	CREATE UNIQUE NONCLUSTERED INDEX [HostingServer_UniqueName] ON [HostingServer] 
	(
		[name] ASC
	)ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HostedHardware]') AND type in (N'U'))
BEGIN
	CREATE TABLE [HostedHardware] (
		[pkid] INT IDENTITY NOT NULL,
		[created_date] [datetime] NOT NULL,
		[last_modified] [datetime] NOT NULL,
		[type] NVARCHAR(256) NOT NULL,
		[name] [dbo].[NameType] NOT NULL,
		[description] [dbo].[DescriptionType] NOT NULL,
		[user_name] [dbo].[UserIdType] NOT NULL,
		[password] [dbo].[PasswordType] NOT NULL,
		[hosting_info] NVARCHAR(MAX) NOT NULL,
		[manufacturer_id] NVARCHAR(MAX) NOT NULL,
		[custom_data] NVARCHAR(MAX) NOT NULL,
		[hosting_server_id] INT NOT NULL
		
		CONSTRAINT [PK_HostedHardware] PRIMARY KEY CLUSTERED
		(
			[pkid]
		),
		
		CONSTRAINT [FK_HostedHardware_HostingServer] FOREIGN KEY ([hosting_server_id])
		REFERENCES [dbo].[HostingServer] ([pkid]),
	) ON [PRIMARY]

	CREATE UNIQUE NONCLUSTERED INDEX [HostedHardware_UniqueName] ON [HostedHardware] 
	(
		[name] ASC,
		[hosting_server_id] ASC
	)ON [PRIMARY]
END
GO

-- Add HostedHardware column in Hardware table.
-- The new column should be foreign key to HostedHardware.pkid
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Hardware]') AND name = N'HostedHardware_id')
BEGIN
	ALTER TABLE [Hardware] ADD [HostedHardware_id] INT NULL
	ALTER TABLE [Hardware] ADD [CameraData] NVARCHAR(MAX) NULL
END
GO

IF  NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Hardware_HostedHardware]') AND parent_object_id = OBJECT_ID(N'[Hardware]'))
	ALTER TABLE [Hardware]  WITH CHECK ADD CONSTRAINT [FK_Hardware_HostedHardware] FOREIGN KEY([HostedHardware_id])
	REFERENCES [HostedHardware] ([pkid])
GO

ALTER TABLE [Hardware] CHECK CONSTRAINT [FK_Hardware_HostedHardware]
GO

-- Insert new nodes in navigation xml
DECLARE @navigation XML
SELECT @navigation = CAST(Setting as XML)
FROM [SystemSettings]
WHERE [IDSystemSetting] = '5B868950-9A55-4E0E-9F95-710CC42D184C'

-- First add the RemoteConnectServers node in the xml
IF (@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="RemoteConnectServers"][1]') = 0)
BEGIN
	SET @navigation.modify('
	insert
		<entry ID=''RemoteConnectServers''>
			<text>Remote Connect Services</text>
			<description>Remote Connect Services</description>
			<iconcollapsed>132</iconcollapsed>
			<iconexpanded>132</iconexpanded>
			<data>RemoteConnectServices.html</data>
			<launch>
				<instance>D4474C73-8924-41dc-B7DD-72559182C41E</instance>
				<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
				<classname>VideoOS.Administration.AddIn.UserControlHelp</classname>
			</launch>
		</entry>
	 before (/main/entry[@ID="Root"]/entry[@ID="Servers"])[1]')
	 
	 UPDATE SystemSettings
	 SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
	 OUTPUT CAST(DELETED.Setting AS XML) AS [Before], CAST(INSERTED.Setting AS XML) AS [After]
	 WHERE [IDSystemSetting] = '5B868950-9A55-4E0E-9F95-710CC42D184C'
END

-- Now add the AxisOneClick node in the xml
IF (@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="RemoteConnectServers"]/entry[@ID="AxisOneClick"][1]') = 0)
BEGIN
	SET @navigation.modify('
	insert
		<entry ID=''AxisOneClick''>
			<text>Axis One-click Camera Connection</text>
			<description>Axis One-click Camera Connection</description>
			<iconcollapsed>133</iconcollapsed>
			<iconexpanded>133</iconexpanded>
			<data/>
			<launch>
				<instance>77FCFE41-F463-4D5D-B11F-9800022F08A9</instance>
				<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
				<classname>VideoOS.Administration.AddIn.UserControlAvhs</classname>
			</launch>
		</entry>
	 as last into (/main/entry[@ID="Root"]/entry[@ID="RemoteConnectServers"])[1]')
	 
	 UPDATE SystemSettings
	 SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
	 OUTPUT CAST(DELETED.Setting AS XML) AS [Before], CAST(INSERTED.Setting AS XML) AS [After]
	 WHERE [IDSystemSetting] = '5B868950-9A55-4E0E-9F95-710CC42D184C'
END
GO

-- AVHSUser
IF NOT EXISTS (SELECT * from SystemSettings where IDSystemSetting = 'F5093733-E9AD-40D7-A6DA-A90DFA314B87')
BEGIN
	INSERT INTO SystemSettings ([LastModified], [IDSystemSetting], [Name], [Setting])
	VALUES(getdate(), 'F5093733-E9AD-40D7-A6DA-A90DFA314B87', 'AVHSUser', '')
END
GO

-- AVHSPass
IF NOT EXISTS (SELECT * from SystemSettings where IDSystemSetting = '9973295F-F7BB-48F3-ABBF-ECCC063F93EA')
BEGIN
	INSERT INTO SystemSettings ([LastModified], [IDSystemSetting], [Name], [Setting])
	VALUES(getdate(), '9973295F-F7BB-48F3-ABBF-ECCC063F93EA', 'AVHSPass', '')
END
GO

IF NOT EXISTS (SELECT * FROM [Feature] WHERE [Name] = 'AxisOneClick')
BEGIN
	INSERT INTO [Feature] VALUES ('AxisOneClick', '', 0)
END
GO
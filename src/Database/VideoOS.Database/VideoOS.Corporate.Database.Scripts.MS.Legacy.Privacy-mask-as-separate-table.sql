-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

 -- First establish the privacy mask table.
IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DevicePrivacyMask]') AND type in (N'U'))
CREATE TABLE [DevicePrivacyMask]
(
	[LastModified] DATETIME NOT NULL,				-- When the privacy mask was last modified.
	[IDDevice] [GuidType] NOT NULL,					-- ID (FK) of the device that has this privacy mask
	[PrivacyMask] XML NULL

	CONSTRAINT [PK_DevicePrivacyMask] PRIMARY KEY NONCLUSTERED
	(
		[IDDevice] ASC
	),

	CONSTRAINT [FK_DevicePrivacyMask_Devices] FOREIGN KEY
	(
		[IDDevice]
	)
	REFERENCES [Devices] ([IDDevice])

) ON [PRIMARY]
GO

-- If the privacymask column is in [Devices], we assume
--   a) move the contents to the DevicePrivacyMask table
--   b) Drop the privacy mask column in the Devices table

IF EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'PrivacyMask'
          AND Object_ID = Object_ID(N'Devices'))
BEGIN
	declare @status int
	begin tran
	-- Execute as dynamic SQL, otherwise script execution will complain that PrivacyMask is not there, if the db is already updated.
	-- Using lastmodified on Devices for the new rows - the most accurate? Alternative, GetUTCDate().
	exec sp_executesql N'insert [DevicePrivacyMask]
	(LastModified, IDDevice, PrivacyMask)						
	select lastmodified, IDDevice, PrivacyMask
	from Devices
	where DeviceType = ''Camera'''
	set @status = @@ERROR

	IF @status = 0
	BEGIN
		commit tran
		alter table [Devices] DROP column [PrivacyMask]
	END
	ELSE
		rollback tran
END
GO

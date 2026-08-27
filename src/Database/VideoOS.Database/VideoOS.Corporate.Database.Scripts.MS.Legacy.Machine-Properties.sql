-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[ServerType]') AND name = 'pkid' AND is_identity = 1)
BEGIN
	ALTER TABLE [ServerType]
	ADD [tempID] INT IDENTITY(1,1)

	ALTER TABLE [DataCollectorInstalledServices] 
	DROP CONSTRAINT [FK_DataCollectorInstalledServices_ServerType]

	ALTER TABLE [ServerType] 
	DROP CONSTRAINT [PK_server_type]

	ALTER TABLE [ServerType]
	DROP COLUMN [pkid]

	EXEC sp_rename '[ServerType].[tempID]', 'pkid', 'COLUMN'

	ALTER TABLE [ServerType] 
	ADD CONSTRAINT [PK_server_type] PRIMARY KEY ([pkid])

	ALTER TABLE [DataCollectorInstalledServices] WITH CHECK 
	ADD CONSTRAINT [FK_DataCollectorInstalledServices_ServerType] FOREIGN KEY([server_type_id]) REFERENCES [ServerType] ([pkid])
END
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MachineProperties]') AND type in (N'U'))
BEGIN
	CREATE TABLE [MachineProperties] (
		[data_collector_id] INT NOT NULL,
		[name] NVARCHAR(200) NOT NULL,
		[value] BIGINT NOT NULL,
		
		CONSTRAINT [PK_MachineProperties] PRIMARY KEY CLUSTERED
		(
			[data_collector_id] ASC,
			[name] ASC
		)
	)
END
GO

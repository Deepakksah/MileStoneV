-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[Hardware]') AND name = N'DriverNumber')
BEGIN
	ALTER TABLE [Hardware] ADD [DriverNumber] int NULL
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[Hardware]') AND name = N'CustomSettings')
BEGIN

	UPDATE 
		[Hardware] 
	SET 
		[DriverNumber] = CASE WHEN [CustomSettings].value('(/properties/settings/setting/value)[1]', 'varchar(100)') = '' THEN NULL ELSE [CustomSettings].value('(/properties/settings/setting/value)[1]', 'int') END


	UPDATE 
		[Hardware] 
	SET 
		[DriverNumber] = 10000
	WHERE
		[VendorProductID] = 'TestDriver'


	UPDATE 
		[Hardware] 
	SET 
		[DriverNumber] = 10001
	WHERE
		[VendorProductID] = 'PtzTestDriver'
	

	ALTER TABLE [Hardware] ALTER COLUMN [DriverNumber] int NOT NULL
	
	ALTER TABLE [Hardware] DROP COLUMN [CustomSettings]
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[Hardware]') AND name = N'Type')
BEGIN
	ALTER TABLE [Hardware] DROP COLUMN [Type]
END
GO



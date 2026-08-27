-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Recorders]') AND name = N'x64')
BEGIN
	ALTER TABLE Recorders
	ADD [x64] BIT NOT NULL CONSTRAINT [DF_x64] DEFAULT (0)
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Hardware]') AND name = N'DriverProxyName')
BEGIN
	ALTER TABLE Hardware
	ADD [DriverProxyName] [NameType] NULL
END
GO

IF EXISTS (SELECT [DriverProxyName] FROM [Hardware] WHERE [DriverProxyName] IS NULL)
BEGIN
	UPDATE 
		[Hardware] 
	SET 
		[DriverProxyName] = [DriverNumber]
END
GO


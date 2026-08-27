-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM [SecurityAction] WHERE [name] = 'AUTHORIZE_USER' AND [namespace_id] = 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15')
BEGIN
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	VALUES( 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15', 'AUTHORIZE_USER', 'Authorize users', 6, 1)
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SecurityRoles]') AND name = N'DualRequired')
BEGIN
	ALTER TABLE [SecurityRoles]
	ADD [DualRequired] BIT NULL
END
GO

UPDATE [SecurityRoles]
SET [DualRequired] = 0
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SecurityRoles]') AND name = N'DualRequired')
BEGIN
	ALTER TABLE [SecurityRoles] ALTER COLUMN [DualRequired] BIT NOT NULL
END
GO


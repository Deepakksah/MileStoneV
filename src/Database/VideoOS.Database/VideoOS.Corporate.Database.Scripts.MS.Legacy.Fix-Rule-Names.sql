---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT TOP 1 * FROM RULES WHERE [IDRule] = '4CE46D3E-C4C7-46b2-A580-FC98CDC24611' AND [Name] = 'Default Record on Bookmark')
BEGIN
	UPDATE Rules SET
	[Name] = 'Default Record on Bookmark Rule'
	WHERE
	[IDRule] = '4CE46D3E-C4C7-46b2-A580-FC98CDC24611' AND [Name] = 'Default Record on Bookmark' 
END
GO

IF EXISTS(SELECT TOP 1 * FROM RULES WHERE [IDRule] = 'FA2F8209-8D9B-4580-A6EB-17E58C99A610' AND [Name] = 'Default Record on Request')
BEGIN
	UPDATE Rules SET
	[Name] = 'Default Record on Request Rule'
	WHERE
	[IDRule] = 'FA2F8209-8D9B-4580-A6EB-17E58C99A610' AND [Name] = 'Default Record on Request' 
END
GO

IF EXISTS(SELECT TOP 1 * FROM RULES WHERE [IDRule] = '7AA28F82-F3FF-4398-9781-061299178C7F' AND [Name] = 'Default Goto Preset when PTZ is done')
BEGIN
	UPDATE Rules SET
	[Name] = 'Default Goto Preset when PTZ is done Rule'
	WHERE
	[IDRule] = '7AA28F82-F3FF-4398-9781-061299178C7F' AND [Name] = 'Default Goto Preset when PTZ is done' 
END
GO
-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- First we rename FailoverRecorders -> FailoverServers
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FailoverRecorders]') AND type = N'U')
BEGIN
	EXEC sp_rename @objname = 'FailoverRecorders', @newname = 'FailoverServers'
	EXEC sp_rename @objname = 'FailoverServers.IDFailoverRecorder', @newname = 'IDFailoverServer', @objtype = 'COLUMN'
END
GO

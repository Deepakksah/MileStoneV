-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[unregister_registered_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [unregister_registered_server]
GO

CREATE PROCEDURE [unregister_registered_server]
	@ServiceId [UNIQUEIDENTIFIER]
AS
BEGIN
	DELETE FROM [RegisteredServers] WHERE [IDServer] = @ServiceId
END
GO
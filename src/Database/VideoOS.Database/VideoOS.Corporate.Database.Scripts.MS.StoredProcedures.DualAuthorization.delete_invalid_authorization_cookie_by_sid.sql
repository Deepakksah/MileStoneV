-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_invalid_authorization_cookie_by_sid]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_invalid_authorization_cookie_by_sid]
GO

CREATE PROCEDURE [delete_invalid_authorization_cookie_by_sid]
	@UserSid VARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM [AuthorizationCookie]
	WHERE [SID] = @UserSid AND [ValidUntil] < GETUTCDATE() 
END
GO
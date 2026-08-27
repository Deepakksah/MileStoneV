-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_authorization_cookie]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_authorization_cookie]
GO

CREATE PROCEDURE [delete_authorization_cookie]
	@UserSid VARCHAR(256),
	@AuthorizationId NVARCHAR(256),
	@Action NVARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM [AuthorizationCookie]
	WHERE [SID] = @UserSid AND 
		[AuthorizationId] = @AuthorizationId AND
		[ActionType] = @Action
END
GO
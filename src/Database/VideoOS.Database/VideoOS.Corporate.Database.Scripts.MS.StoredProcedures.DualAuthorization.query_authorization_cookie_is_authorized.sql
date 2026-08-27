-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_authorization_cookie_is_authorized]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_authorization_cookie_is_authorized]
GO

CREATE PROCEDURE [query_authorization_cookie_is_authorized]
	@UserSid VARCHAR(256),
	@AuthorizationId NVARCHAR(256),
	@Action NVARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @isAuthorized BIT
	SET @isAuthorized = 0

	IF EXISTS(
		SELECT *
		FROM [AuthorizationCookie]
		WHERE [SID] = @UserSid AND 
		[AuthorizationId] = @AuthorizationId AND
		[ActionType] = @Action AND
		[ValidUntil] >= GETUTCDATE())
	 SET @isAuthorized = 1

	 SELECT @isAuthorized
END
GO
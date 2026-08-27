-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[extend_authorization_cookie]') AND type in (N'P', N'PC'))
DROP PROCEDURE [extend_authorization_cookie]
GO

CREATE PROCEDURE [extend_authorization_cookie]
	@AuthorizationId NVARCHAR(256),
	@AddedTimeSec INT
AS
BEGIN
	SET NOCOUNT ON
 
	UPDATE [AuthorizationCookie]
	SET [ValidUntil] = DATEADD(SECOND, @AddedTimeSec, GETUTCDATE())
	WHERE [AuthorizationId] = @AuthorizationId
END
GO
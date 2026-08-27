-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_token_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_token_by_id]
GO

CREATE PROCEDURE [query_token_by_id]
	@TokenId NVARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON

	SELECT 
		t.[TokenId],
		t.[SID], 
		t.[Expires], 
		t.[RegistrationTime],
		t.[ManagementClientToken] 		
	FROM [Token] t
	WHERE t.[TokenId] = @TokenId

END
GO

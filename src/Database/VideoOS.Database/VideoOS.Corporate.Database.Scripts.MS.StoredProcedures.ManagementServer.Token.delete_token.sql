-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_token]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_token]
GO

CREATE PROCEDURE [delete_token]
	@TokenId NVARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON

	DELETE Token 
	WHERE [TokenId] = @TokenId

END
GO

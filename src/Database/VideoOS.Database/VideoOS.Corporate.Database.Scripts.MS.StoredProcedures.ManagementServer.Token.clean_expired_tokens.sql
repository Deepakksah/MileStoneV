
-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[clean_expired_tokens]') AND type in (N'P', N'PC'))
DROP PROCEDURE [clean_expired_tokens]
GO

CREATE PROCEDURE [clean_expired_tokens]
AS
BEGIN
	SET NOCOUNT ON

	-- Deletes tokens which has expired more than 6 hours ago
	DELETE FROM Token
	WHERE GetUtcDate() > DateAdd(HH, ([Expires] / 1000 / 60 / 60) + 6, [RegistrationTime])
END
GO

-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_read_identity]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_read_identity]
GO

CREATE PROCEDURE [security_read_identity]
	@SID VARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON;

	-- Select the identity - this we be ResultSet 0
	SELECT
		[SID],
		[Type],
		[DisplayName],
		[Description],
		[Domain],
		[AccountName],
		[DistinguishedName],
		[Deleted]
	FROM [SecurityIdentity]
	WHERE [SID] = @SID
		AND [Deleted] = 0 -- We do not want to retrieve deleted identities
END
GO
-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_accesscontrol_member_groupid]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_accesscontrol_member_groupid]
GO

CREATE PROCEDURE [query_accesscontrol_member_groupid]
	@memberId [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	BEGIN
		SELECT
			[groupId], [memberId], [LastModified]
		FROM [AccessControlMembers]
		WHERE [memberId] = @memberId
	END
END
GO

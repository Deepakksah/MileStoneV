-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[remove_accesscontrol_member]') AND type in (N'P', N'PC'))
DROP PROCEDURE [remove_accesscontrol_member]
GO

CREATE PROCEDURE [remove_accesscontrol_member]
	@groupId [GuidType],
	@memberId [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	BEGIN
		DELETE FROM [AccessControlMembers] WHERE [groupId] = @groupId AND [memberId] = @memberId
	END
END
GO

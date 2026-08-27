-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[add_accesscontrol_member]') AND type in (N'P', N'PC'))
DROP PROCEDURE [add_accesscontrol_member]
GO

CREATE PROCEDURE [add_accesscontrol_member]
	@groupId [GuidType],
	@memberId [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT * FROM [AccessControlMembers] WHERE groupId = @groupId AND memberId = @memberId)
	BEGIN
			-- Member already exists in the group, no action needed
		RETURN
	END
	ELSE
	IF (EXISTS(SELECT * FROM [AccessControlMembers] WHERE memberId = @memberId))
	BEGIN
		UPDATE [AccessControlMembers] SET lastModified = GETUTCDATE(), [groupId] = @groupId
		WHERE memberId = @memberId
	END
	BEGIN
		INSERT [AccessControlMembers] (groupId, memberId, lastModified) 
		VALUES (@groupId, @memberId, GETUTCDATE())
	END
END
GO

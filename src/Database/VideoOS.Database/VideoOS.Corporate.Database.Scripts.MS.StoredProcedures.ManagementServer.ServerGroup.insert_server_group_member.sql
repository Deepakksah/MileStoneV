-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[insert_server_group_member]') AND type in (N'P', N'PC'))
DROP PROCEDURE [insert_server_group_member]
GO

CREATE PROCEDURE [insert_server_group_member]
	@IDObject [GuidType],
	@IDServerGroup [GuidType],
	@ObjectType NVARCHAR(512)
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	INSERT [ServerGroupMembership] (
		IDObject, 
		IDServerGroup, 
		ObjectType, 
		LastModified
	)
	VALUES (
		@IDObject,
		@IDServerGroup,
		@ObjectType,
		@CurrentUtcTime
	)
END
GO
-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_private_view_group]') AND type = 'P')
DROP PROCEDURE [query_private_view_group]
GO

CREATE PROCEDURE [query_private_view_group]
	@UserSid [SidType]
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @Domain NVARCHAR(512)
		DECLARE @AccountName NVARCHAR(512)
		SELECT @Domain = [DOMAIN],  @AccountName = [ACCOUNTNAME] FROM [SecurityIdentity] WHERE [SID] = @UserSid
	
		IF(@Domain IS NULL)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END
	
		DECLARE @UserDomainAccountName NVARCHAR(512) =  @Domain + N'\' + @AccountName
		DECLARE @Created BIT = 0
	
		DECLARE @IDServerGroup [GuidType]
		SELECT @IDServerGroup = [IDServerGroup] 
		FROM [ServerGroup] WITH(UPDLOCK) 
		WHERE [Name] = @userDomainAccountName AND [IDParentGroup] = '4BF8A8B0-54AB-4E82-994F-98D8E9584176' AND [StructureType] = 'VIEW_GROUP'

		IF(@IDServerGroup IS NULL)
		BEGIN
			SELECT @IDServerGroup = newid()
		
			EXEC [create_server_group_transaction] @IDServerGroup, @userDomainAccountName, '', '4BF8A8B0-54AB-4E82-994F-98D8E9584176', 'VIEW_GROUP', @UserSid
		
			SET @Created = 1
		END
	
		SELECT
				[IDServerGroup],
				[Name],
				[Description],
				[IDParentGroup],
				[CreatedDate],
				[LastModified],
				[StructureType],
				[RelationIds]
		FROM [ServerGroup]
		WHERE [IDServerGroup] = @IDServerGroup
	
		SELECT @Created AS Created

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
	END CATCH;
END
GO
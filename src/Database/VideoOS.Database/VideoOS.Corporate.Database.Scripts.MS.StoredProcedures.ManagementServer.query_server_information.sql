-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_server_information]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_server_information]
GO

CREATE PROCEDURE [query_server_information]
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

		DECLARE @SkipPrivateViewGroupCreation BIT = 0

		DECLARE @Domain NVARCHAR(512)
		DECLARE @AccountName NVARCHAR(512)
	
		SELECT @Domain = [DOMAIN],  @AccountName = [ACCOUNTNAME] 
		FROM [SecurityIdentity] 
		WHERE [SID] = @UserSid AND [Deleted] = 0

		IF(@UserSid='S-1-5-20')
		BEGIN
			SET @SkipPrivateViewGroupCreation = 1
		END
	
		DECLARE @PrivateViewGroupId [GuidType]
		DECLARE @Created BIT = 0

		SET @PrivateViewGroupId = NULL

		IF(@SkipPrivateViewGroupCreation = 0 AND @Domain IS NOT NULL AND @AccountName IS NOT NULL)
		BEGIN
			DECLARE @UserDomainAccountName NVARCHAR(512) =  @Domain + N'\' + @AccountName
		
			SELECT @PrivateViewGroupId = [IDServerGroup] 
			FROM [ServerGroup]
			WHERE [Name] = @userDomainAccountName AND [IDParentGroup] = '4BF8A8B0-54AB-4E82-994F-98D8E9584176' AND [StructureType] = 'VIEW_GROUP'


			IF(@PrivateViewGroupId IS NULL)
			BEGIN
				SELECT @PrivateViewGroupId = newid()		
				EXEC [create_server_group_transaction] @PrivateViewGroupId, @userDomainAccountName, '', '4BF8A8B0-54AB-4E82-994F-98D8E9584176', 'VIEW_GROUP', @UserSid

				SET @Created = 1
			END
		END

		SELECT
			s.[IDSite],
			s.[Name],
			s.[Description],
			s.[Addresses],
			s.[LastModified],
			sr.[IDSiteMaster],
			s.[Version],
			s.[LastStatusHandshake],
			s.[SynchronizationStatus],
			@PrivateViewGroupId AS PrivateViewGroupId
		FROM [Sites] s
		LEFT JOIN [SitesRelation] sr ON [IDSiteSlave] = s.[IDSite]
		WHERE [IDSiteType] = 1

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
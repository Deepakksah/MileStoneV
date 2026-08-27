-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_client_profile]') AND type = 'P')
DROP PROCEDURE [delete_client_profile]
GO

CREATE PROCEDURE [delete_client_profile]
	@IDClientProfile [GuidType]
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)
  
	BEGIN TRY
		DECLARE @currentName [NameType]

		BEGIN TRAN
		
		SELECT @currentName = Name
		FROM [ClientProfile] WITH(UPDLOCK)
		WHERE [IDClientProfile] = @IDClientProfile
	
		-- Does the client profile exists?
		IF(@currentName IS NULL)
		BEGIN
			;THROW 60704, @procedureName, 1;
		END
	
		DELETE FROM ClientProfileRoleRelation
		WHERE IDClientProfile = @IDClientProfile

		DELETE FROM ClientProfile
		WHERE IDClientProfile = @IDClientProfile
		AND DefaultProfile = 0 -- Only possible to delete non-default profiles
	
		UPDATE ClientProfile
		SET Priority = DT.p
		FROM
		(
			SELECT IDClientProfile AS Id, RANK() OVER(PARTITION BY profile_type ORDER BY Priority) - 1 as p FROM ClientProfile
		) AS DT
		WHERE IDClientProfile = DT.Id
	
		COMMIT TRAN
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
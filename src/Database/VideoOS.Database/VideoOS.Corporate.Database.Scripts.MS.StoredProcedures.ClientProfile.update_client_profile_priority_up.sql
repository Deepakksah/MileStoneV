-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_client_profile_priority_up]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_client_profile_priority_up]
GO

CREATE PROCEDURE [update_client_profile_priority_up]
	@IDClientProfile [GuidType]
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		DECLARE @currentPriority [INT]
		DECLARE @profile_type NVARCHAR(200)
		DECLARE @isDefault [BIT]

		SELECT @currentPriority = Priority, @profile_type = profile_type, @isDefault = DefaultProfile
		FROM [ClientProfile] WITH(UPDLOCK)
		WHERE [IDClientProfile] = @IDClientProfile
	
		-- Does the client profile exists?
		IF(@currentPriority IS NULL)
		BEGIN
			;THROW 60702, @procedureName, 1;
		END
	
		IF(@isDefault = 1)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END
	
		IF(EXISTS (SELECT Priority FROM ClientProfile WHERE Priority = @currentPriority + 1 AND profile_type = @profile_type))
		BEGIN
			UPDATE ClientProfile
			SET Priority = @currentPriority
			WHERE Priority = @currentPriority + 1
			AND profile_type = @profile_type
		
			UPDATE ClientProfile
			SET Priority = @currentPriority + 1
			WHERE IDClientProfile = @IDClientProfile
		END
	
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
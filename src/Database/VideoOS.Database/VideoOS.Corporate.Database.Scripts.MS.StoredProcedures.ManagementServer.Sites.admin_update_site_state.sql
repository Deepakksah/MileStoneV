-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_update_site_state]') AND type = 'P')
DROP PROCEDURE [admin_update_site_state]
GO

CREATE PROCEDURE [admin_update_site_state]
	@IDSite GuidType,
	@IDSiteState INT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
		DECLARE @SynchronizationStatus INT
		SET @SynchronizationStatus = 0
	
		-- Does the site exist?
		IF NOT EXISTS(
			SELECT *
			FROM [Sites] WITH(UPDLOCK)
			WHERE [IDSite] = @IDSite)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END
	
		-- Try to update state on home site?
		IF EXISTS(
			SELECT *
			FROM [Sites] WITH(UPDLOCK)
			WHERE [IDSite] = @IDSite
			AND [IDSiteState] = 1)
		BEGIN
			-- State can't be changed on home site
			ROLLBACK TRAN
			RETURN 0
		END
	
		BEGIN TRY
			-- Update the site state
			UPDATE [Sites] 
			SET [IDSiteState] = @IDSiteState, 
				[LastStatusHandshake] = @CurrentUtcTime,
				[SynchronizationStatus] = @SynchronizationStatus
			WHERE IDSite = @IDSite
		END TRY
		BEGIN CATCH
			ROLLBACK TRAN
			RETURN 0
		END CATCH
	
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
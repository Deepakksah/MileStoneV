-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[NextSequenceCounterValue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [NextSequenceCounterValue]
GO

CREATE PROCEDURE [NextSequenceCounterValue]
	@CounterName NVARCHAR(30)
AS	
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @next INT
		DECLARE @max INT

		BEGIN TRANSACTION

		UPDATE [SequenceCounter] SET [Value] = [Value] + 1 WHERE [Name] = @CounterName;
		SELECT @next = [Value], @max = [MaxValue] FROM [SequenceCounter] WHERE [Name] = @CounterName;

		IF @next > @max
		BEGIN
			UPDATE [SequenceCounter] SET [Value] = 1 WHERE [Name] = @CounterName;
			SET @next = 1;
		END

		COMMIT TRANSACTION
		SELECT @next
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
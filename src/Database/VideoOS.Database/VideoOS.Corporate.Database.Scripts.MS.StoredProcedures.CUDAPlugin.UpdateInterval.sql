IF OBJECT_ID ( '[CUDAPlugin].[UpdateInterval]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [CUDAPlugin].[UpdateInterval];
GO

CREATE PROCEDURE [CUDAPlugin].[UpdateInterval] (@Interval int)
AS
BEGIN
	SET NOCOUNT ON	
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
		BEGIN TRANSACTION UPDATEINTERVAL
		truncate table [CUDAPlugin].[RefreshInterval]
		insert into [CUDAPlugin].[RefreshInterval] values (@Interval)
		COMMIT TRANSACTION UPDATEINTERVAL
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION UPDATEINTERVAL
    SELECT
        ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;
	END CATCH
END
GO
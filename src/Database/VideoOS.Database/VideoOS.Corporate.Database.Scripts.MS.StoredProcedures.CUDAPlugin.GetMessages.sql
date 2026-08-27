-- GetMessages returns all CUDA messages stored in the CUDA plugin messages table
IF OBJECT_ID ( '[CUDAPlugin].[GetMessages]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [CUDAPlugin].[GetMessages];
GO

CREATE PROCEDURE [CUDAPlugin].[GetMessages]
AS
BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
	declare @timeNow as DATETIME
	declare @startTime as DATETIME
	set @timeNow = GETUTCDATE()
	set @startTime = DATEADD(SECOND, (select top 1 -ri.IntervalSecs from [CUDAPlugin].[RefreshInterval] ri), @timeNow)

		BEGIN TRANSACTION GETMESSAGES
		select m.CUDAInfo as CUDAInfo, m.SourceId as SourceId, m.EventId as EventId from [CUDAPlugin].[Message] m
		where m.ReceiveTime between @startTime and @timeNow
		COMMIT TRANSACTION GETMESSAGES
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION GETMESSAGES
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
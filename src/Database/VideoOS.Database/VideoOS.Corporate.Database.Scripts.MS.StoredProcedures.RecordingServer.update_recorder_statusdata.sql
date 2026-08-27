-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_recorder_statusdata]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_recorder_statusdata]
GO

CREATE PROCEDURE [update_recorder_statusdata]
	@IDRecorder [GuidType],
	@Type VARCHAR(256),
	@Data XML
 AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		-- Make sure the recording server exists
		IF NOT EXISTS (SELECT [IDRecorder] FROM [Recorders] WITH(UPDLOCK) WHERE [IDRecorder] = @IDRecorder)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END
	
		BEGIN TRY
			IF NOT EXISTS (SELECT [IDRecorder], [Type] FROM [RecorderStatusData] WITH(UPDLOCK) WHERE [IDRecorder] = @IDRecorder AND [Type] = @Type)
			BEGIN
				INSERT INTO [RecorderStatusData] ([IDRecorder], [Type], [Data], [LastModified]) 
				VALUES (@IDRecorder, @Type, @Data, GETUTCDATE())
			END
			ELSE
			BEGIN
				UPDATE [RecorderStatusData]
				SET	[Data] = @Data,
					[LastModified] = GETUTCDATE()
				WHERE 
					[IDRecorder] = @IDRecorder AND [Type] = @Type
			END
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
		THROW; -- Used from ServerService, where Error 60000 is NOT expected
	END CATCH;
END
GO
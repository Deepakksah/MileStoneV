-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_archive_storage_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_archive_storage_transaction]
GO

CREATE PROCEDURE [delete_archive_storage_transaction]
	@pkid [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

		-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	DECLARE @archive_storage_name [NameType]
	DECLARE @recordingstorage_id [GuidType]
	DECLARE @recordingserver_id [GuidType]
	DECLARE @recorder_service_id [GuidType]

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	-- Check that this a valid archive storage that we can delete.
	-- Look up values for creating task
	SELECT
		@archive_storage_name = arch.[Name], 
		@recordingstorage_id = arch.[recordingstorage_id], 
		@recordingserver_id = rs.[recordingserver_id],
		@recorder_service_id = rssr.[recording_server_service_id]
	FROM [ArchiveStorage] arch WITH(UPDLOCK)
	INNER JOIN [RecordingStorage] rs WITH(UPDLOCK) ON rs.[pkid] = arch.[recordingstorage_id]
	INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = rs.[recordingserver_id] AND rssr.[type] = 0
	WHERE arch.[pkid] = @pkid

	-- Does the archive storage exist?
	IF(@@ROWCOUNT = 0)
	BEGIN
		;THROW 60052, @procedureName, 1;
	END
	
	DECLARE @diffgram XML
	SELECT @diffgram = dbo.[archiveStorageAsDataSet](@pkid)

	-- Create diffgram
	EXEC [create_task_transaction]
		@type = 'diffgram_delete',
		@target = @recorder_service_id,
		@data_a = @diffgram,
		@current_time = @CurrentUtcTime

	-- TODO: implement audit logging...

	-- Delete the archive database
	DELETE FROM [ArchiveStorage]
	WHERE [pkid] = @pkid

	RETURN 0
END
GO

-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recording_storage_encryption_password]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recording_storage_encryption_password]
GO

CREATE PROCEDURE [query_recording_storage_encryption_password]
	@pkid [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	DECLARE @rowCount INT
	DECLARE @IDRecordingServer [GuidType]
	DECLARE @storedPassword [PasswordType]

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure the Recording storage exists
	SELECT
		@IDRecordingServer = rs.[recordingserver_id],
		@storedPassword = rs.[encryption_password]
	FROM [RecordingStorage] rs WITH(UPDLOCK)
	WHERE rs.[pkid] = @pkid

	SELECT @rowCount = @@ROWCOUNT

	IF(@rowCount = 0)
	BEGIN
		;THROW 60093, @procedureName, 1;
	END

	-- Make sure that no SetEncryptionPassword for the same recording server exist
	IF EXISTS (
		SELECT [pkid]
		FROM [Task] 
		WITH(UPDLOCK) WHERE ([Target] = @IDRecordingServer AND 
			[Type] = 'Database.SetEncryptionPassword'))
	BEGIN
		;THROW 60095, @procedureName, 1;
	END

	SELECT @storedPassword;
END
GO

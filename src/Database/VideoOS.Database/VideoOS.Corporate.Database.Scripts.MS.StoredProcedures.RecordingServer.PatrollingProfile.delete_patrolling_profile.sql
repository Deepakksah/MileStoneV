-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_patrolling_profile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_patrolling_profile]
GO

CREATE PROCEDURE [delete_patrolling_profile]
	@patrolling_id [GuidType],
	@name [NameType],
	@session_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

		-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @rowCount INT
		DECLARE @dataSetBefore XML
	
		DECLARE @device_id [GuidType]
		DECLARE @device_name [NameType]
		DECLARE @device_type [InternalIdType]
		DECLARE @recordingserver_id [GuidType]
		DECLARE @recordingserver_name [NameType]
		DECLARE @recorder_service_id [GuidType]

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRAN

		-- Make sure that patrolling profile exists
		SELECT	IDPatrollingProfile
		FROM [PatrollingProfiles] p WITH(UPDLOCK)
		WHERE [IDPatrollingProfile] = @patrolling_id
	
		SELECT @rowCount = @@ROWCOUNT
		IF(@rowCount = 0)
		BEGIN
			;THROW 64104, @procedureName, 1;
		END

		-- Make sure that device exists, and get properties for logging
		SELECT	@recordingserver_id = h.[IDRecorder],
				@recordingserver_name = r.[Name],
				@device_id = d.[IDDevice],
				@device_name = d.[Name],
				@device_type = d.[DeviceType],
				@recorder_service_id = rssr.[recording_server_service_id]
		FROM [Devices] d WITH(UPDLOCK)
		INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
		INNER JOIN [Recorders] r ON r.[IDRecorder] = h.[IDRecorder]
		INNER JOIN [RecordingServerServiceRecorder] rssr ON r.[IDRecorder] = rssr.recorder_id
		INNER JOIN [PatrollingProfiles] p ON p.IDDevice = d.IDDevice
		WHERE p.[IDPatrollingProfile] = @patrolling_id AND rssr.[type] IN (0, 2)
	
		SELECT @rowCount = @@ROWCOUNT
		IF(@rowCount = 0)
		BEGIN
			;THROW 60071, @procedureName, 1;
		END
	
		-- Get DataSet after changes
		SET @dataSetBefore = dbo.patrollingProfileAsDataSet(@patrolling_id)

		-- Create patrolling profile
		DELETE FROM PatrollingProfiles
		WHERE [IDPatrollingProfile] = @patrolling_id

		-- DeviceSettingsChanged: 13027; Log an entry similar to "Patrolling profile 'patrollingProfile_name' deleted."
		EXEC create_pending_log @session_id, 13027, @device_id, @device_name, @recordingserver_id, @recordingserver_name, @name

		-- Register diffgrams for management server
		EXEC [create_task_transaction]
						@type = 'diffgram_delete',
						@target = '909749AE-88C4-4780-A381-383D691E98FA',
						@data_a = @dataSetBefore,
						@current_time = @CurrentUtcTime
	
		COMMIT TRAN
		RETURN 0

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

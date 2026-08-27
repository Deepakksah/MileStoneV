-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[move_failover_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [move_failover_server]
GO

CREATE PROCEDURE [move_failover_server]
	@IDFailoverServer [GuidType],
	@IDTargetGroup [GuidType],
	@InsertAtPosition SMALLINT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @sourceGroupId [GuidType]
		DECLARE @failoverServerName [NameType]
		DECLARE @targetGroupName [NameType]
		DECLARE @currentSequence SMALLINT
		DECLARE @numberOfServers INT

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		SELECT @targetGroupName = [Name]
		FROM [ServerGroup] WITH(UPDLOCK)
		WHERE [IDServerGroup] = @IDTargetGroup

		-- Does the target failover group exist?
		IF(@targetGroupName IS NULL)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%IDTargetGroup=' + CAST(@IDTargetGroup AS NVARCHAR(36)) + N'%%'
			;THROW 60041, @ErrorArgs, 1;
		END
	
		-- Find the source failover group
		SELECT 
			@sourceGroupId = [IDServerGroup],
			@failoverServerName = [Name],
			@currentSequence = [GroupSequence]
		FROM [FailoverServers] fs, [ServerGroupMembership] sgm  WITH(UPDLOCK)
		WHERE [IDFailoverServer] = @IDFailoverServer
		AND [IDObject] = @IDFailoverServer

		IF(@sourceGroupId IS NULL)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END
	
		IF(@sourceGroupId <> @IDTargetGroup)
		BEGIN
			-- It is not possible to move a hot-standby server to a failover group in the MC wihtout first unassigning it.
			-- However if done directly via VMO, change the type to regular failover to preserve database integrity.
			UPDATE [RecordingServerServiceRecorder]
			SET [last_modified] = @CurrentUtcTime
			  ,[type] = 1
			WHERE [type] = 2
			AND [recording_server_service_id] = (
				SELECT TOP 1 rssr.[recording_server_service_id]
				FROM [FailoverServers] fs
				JOIN [RecordingServerServiceRecorder] rssr
				ON fs.[IDRecordingServerService] = rssr.[recording_server_service_id]
				AND fs.[IDFailoverServer] = @IDFailoverServer)
		END

		---- Is the source and target group and position specified the same?
		--IF((@sourceGroupId = @IDTargetGroup) AND (@currentSequence = @InsertAtPosition))
		--BEGIN
		--	ROLLBACK TRAN
		--	;THROW 60043, @procedureName, 1;
		--END

		SET @numberOfServers = (
			SELECT COUNT(fs.[GroupSequence]) 
			FROM [FailoverServers] fs, [ServerGroupMembership] sgm
			WHERE sgm.[IDServerGroup] = @IDTargetGroup
			AND sgm.[IDObject] = fs.[IDFailoverServer])
	
		IF(@InsertAtPosition = -1)
		BEGIN
			SET @InsertAtPosition = @numberOfServers
		END

		-- If we move within same group - the highest sequence number should be number of servers - 1
		IF(@sourceGroupId = @IDTargetGroup)
		BEGIN
			IF(@InsertAtPosition >= (@numberOfServers))
			BEGIN
				SET @InsertAtPosition = @numberOfServers - 1
			END
		END

		-- If we move between groups - the highest sequence number should number of servers
		ELSE
		BEGIN
			IF(@InsertAtPosition >= (@numberOfServers))
			BEGIN
				SET @InsertAtPosition = @numberOfServers 
			END
		END

		BEGIN TRY
			-- If source = target group - move (up or down)
			IF(@sourceGroupId = @IDTargetGroup)
			BEGIN
				-- Move Up
				IF(@currentSequence > @InsertAtPosition)
				BEGIN
					UPDATE [FailoverServers]
					SET [GroupSequence] = -1
					WHERE [IDFailoverServer] = @IDFailoverServer

					UPDATE fs
					SET fs.[GroupSequence] = fs.[GroupSequence] + 1
					FROM [FailoverServers] fs, [ServerGroupMembership] sgm
					WHERE ((fs.[GroupSequence] >= @InsertAtPosition) AND (fs.[GroupSequence] < @currentSequence))
						AND sgm.[IDServerGroup] = @IDTargetGroup
						AND sgm.[IDObject] = fs.[IDFailoverServer]
						AND fs.[IDFailoverServer] != @IDFailoverServer

					UPDATE [FailoverServers]
					SET [GroupSequence] = @InsertAtPosition
					WHERE [IDFailoverServer] = @IDFailoverServer
				END
				-- Move Down
				ELSE IF(@currentSequence < @InsertAtPosition)
				BEGIN
					UPDATE [FailoverServers]
					SET [GroupSequence] = -1
					WHERE [IDFailoverServer] = @IDFailoverServer

					UPDATE fs
					SET fs.[GroupSequence] = fs.[GroupSequence] - 1
					FROM [FailoverServers] fs, [ServerGroupMembership] sgm
					WHERE ((fs.[GroupSequence] > @currentSequence) AND (fs.[GroupSequence] <= @InsertAtPosition))
						AND sgm.[IDServerGroup] = @IDTargetGroup
						AND sgm.[IDObject] = fs.[IDFailoverServer]
						AND fs.[IDFailoverServer] != @IDFailoverServer
				
					UPDATE [FailoverServers]
					SET [GroupSequence] = @InsertAtPosition
					WHERE [IDFailoverServer] = @IDFailoverServer
				END
			END
			-- If source group != target group - remove from source and insert to target
			ELSE
			BEGIN
				UPDATE [FailoverServers]
				SET [GroupSequence] = -1
				WHERE [IDFailoverServer] = @IDFailoverServer

				-- Change sequences in source group
				UPDATE fs
				SET fs.[GroupSequence] = fs.[GroupSequence] - 1
				FROM [FailoverServers] fs, [ServerGroupMembership] sgm
				WHERE (fs.[GroupSequence] >= @currentSequence)
					AND sgm.[IDObject] = fs.[IDFailoverServer]
					AND sgm.[IDServerGroup] = @sourceGroupId

				-- Change sequences in target group
				UPDATE fs
				SET fs.[GroupSequence] = fs.[GroupSequence] + 1
				FROM [FailoverServers] fs, [ServerGroupMembership] sgm
				WHERE (fs.[GroupSequence] >= @InsertAtPosition)
					AND sgm.[IDObject] = fs.[IDFailoverServer]
					AND sgm.[IDServerGroup] = @IDTargetGroup

				-- Move failover server to target group
				UPDATE [ServerGroupMembership]
				SET [IDServerGroup] = @IDTargetGroup
				WHERE [IDObject] = @IDFailoverServer

				UPDATE [FailoverServers]
				SET GroupSequence = @InsertAtPosition
				WHERE IDFailoverServer = @IDFailoverServer
			END
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				SET @ErrorArgs = @procedureName + N': %%targetGroupName=' + @targetGroupName + N'%%; %%failoverServerName=' + @failoverServerName + N'%%'
				;THROW 60042, @ErrorArgs, 1;
			END
			;THROW
		END CATCH

		-- Mark the source and target failover groups as changed.
		UPDATE [ServerGroup]
		SET [LastModified] = @CurrentUtcTime
		WHERE [IDServerGroup] IN (@sourceGroupId, @IDTargetGroup)

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
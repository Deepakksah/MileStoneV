-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_video_wall_view_apply_view]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_video_wall_view_apply_view]
GO

CREATE PROCEDURE [update_video_wall_view_apply_view]
	@source_view_id [GuidType],
	@destination_view_id [GuidType],
	@session_id [GuidType],
	@client_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)
	DECLARE @shortcut NVARCHAR(16)

	BEGIN TRY
		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		-- Make sure the views exists
		IF NOT EXISTS (
			SELECT [pkid]
			FROM [VideoWallView] WITH(UPDLOCK)
			WHERE [pkid] = @source_view_id)
		BEGIN
			;THROW 60215, @procedureName, 1;
		END
	
		-- Make sure the views exists
		IF NOT EXISTS (
			SELECT [pkid]
			FROM [VideoWallView] WITH(UPDLOCK)
			WHERE [pkid] = @destination_view_id)
		BEGIN
			;THROW 60215, @procedureName, 1;
		END

		-- Save shortcut so that we can reapply it after the update	
		SELECT @shortcut = [definition_xml].value('(/view/@shortcut)[1]','varchar(16)')
		FROM [VideoWallView] WITH(UPDLOCK)
		WHERE [pkid] = @destination_view_id

		UPDATE VideoWallView
		SET definition_xml = (SELECT definition_xml
								FROM VideoWallView
								WHERE [pkid] = @source_view_id),
			last_modified = @CurrentUtcTime
		WHERE [pkid] = @destination_view_id
	
		-- Delete custom properties
		EXEC [delete_custom_properties_transaction]
			@object_id = @destination_view_id

		-- Delete view items
		EXEC [delete_video_wall_view_items_transaction]
			@view_id = @destination_view_id

		DECLARE @tempTable Table
		(
			[source_view_item_id] GuidType,
			[destination_view_item_id] GuidType
		)
	
		INSERT INTO @tempTable
		(
			[source_view_item_id],
			[destination_view_item_id]
		)
		SELECT [pkid], newid()
		FROM VideoWallViewItem
		WHERE [view_id] = @source_view_id

		INSERT [VideoWallViewItem] 
		(
			[pkid],
			[view_id],
			[position],
			[definition_xml]
		)
		SELECT t.destination_view_item_id, @destination_view_id, [position], [definition_xml]
		FROM VideoWallViewItem INNER JOIN @tempTable t ON [pkid] = t.source_view_item_id

		INSERT [CustomProperty] 
		(
			[object_id],
			[name],
			[value]
		)
		SELECT t.destination_view_item_id, [name], [value]
		FROM CustomProperty INNER JOIN @tempTable t ON [object_id] = t.source_view_item_id

		UPDATE [VideoWallView] 
		SET [definition_xml].modify('replace value of (/view/@id)[1] with sql:variable("@destination_view_id")')
		WHERE [pkid] = @destination_view_id

		UPDATE [VideoWallView] 
		SET [definition_xml].modify('replace value of (/view/@shortcut)[1] with sql:variable("@shortcut")')
		WHERE [pkid] = @destination_view_id

		UPDATE [VideoWallView] 
		SET [definition_xml].modify('replace value of (/view/@displayname)[1] with sql:column("name")')
		WHERE [pkid] = @destination_view_id

		UPDATE rowsToUpdate 
		SET [definition_xml].modify('replace value of (/viewitem/@id)[1] with sql:column("pkid")')
		FROM (SELECT pkid, definition_xml FROM [VideoWallViewItem] WHERE [view_id] = @destination_view_id) rowsToUpdate

	  -- update the smartClientId attribute in the view and viewitem xml
		IF(@client_id IS NOT NULL)
		BEGIN
			UPDATE [VideoWallView]
			SET [definition_xml].modify('replace value of (/view/@smartClientId)[1] with sql:variable("@client_id")')
			WHERE [pkid] = @destination_view_id AND [definition_xml].value('/view[@smartClientId][1]','GuidType') is not NULL

			UPDATE [VideoWallView]
			SET [definition_xml].modify('insert attribute smartClientId {sql:variable("@client_id")} into (/view)[1]')
			WHERE [pkid] = @destination_view_id AND [definition_xml].value('/view[not(@smartClientId)][1]','GuidType') is not NULL

			UPDATE [VideoWallViewItem]
			SET [definition_xml].modify('replace value of (/viewitem/@smartClientId)[1] with sql:variable("@client_id")')
			WHERE [view_id] = @destination_view_id AND [definition_xml].value('/viewitem[@smartClientId][1]','GuidType') is not NULL

			UPDATE [VideoWallViewItem]
			SET [definition_xml].modify('insert attribute smartClientId {sql:variable("@client_id")} into (/viewitem)[1]')
			WHERE [view_id] = @destination_view_id AND [definition_xml].value('/viewitem[not(@smartClientId)][1]','GuidType') is not NULL
		END


		-- Extract info for logging
		DECLARE @videoWallName [NameType]
		DECLARE @videowall_Id[GuidType]
		DECLARE @monitor_id [GuidType]
		DECLARE @monitorName [NameType]
		DECLARE @sourceViewName [NameType]
		DECLARE @sourceMonitorName [NameType]
	
		SELECT @monitor_id = vwm.[pkid], @monitorName = vwm.[Name], @videowall_Id = vw.[pkid], @videoWallName = vw.[name]
		FROM [VideoWall] vw WITH(UPDLOCK)
		INNER JOIN [VideoWallMonitor] vwm ON vwm.[videowall_id] = vw.[pkid]
		INNER JOIN [VideoWallView] vwv ON vwv.[pkid] = vwm.[view_id]
		WHERE vwv.[pkid] = @destination_view_id
	
		SELECT @sourceViewName = [Name]
		FROM [VideoWallView] WITH(UPDLOCK)
		WHERE [pkid] = @source_view_id

		SELECT @sourceMonitorName = vwm.[Name]
		FROM [VideoWallView] vwv WITH(UPDLOCK)
		INNER JOIN [VideoWallMonitor] vwm ON vwm.[view_id] = vwv.[pkid]
		WHERE vwv.[pkid] = @source_view_id
	
		COMMIT TRAN
		
		-- Should only log if destination is monitor
		IF(@monitor_id IS NOT NULL)
		BEGIN
			IF(@sourceMonitorName IS NOT NULL)
			BEGIN
				-- Apply monitor to monitor
				EXEC create_pending_log @session_id, 10114, @monitor_id, @monitorName, @videowall_id, @videoWallName, @sourceMonitorName
			END
			ELSE IF(@sourceViewName IS NOT NULL)
			BEGIN
				-- Apply normal view to monitor
				EXEC create_pending_log @session_id, 10115, @monitor_id, @monitorName, @videowall_id, @videoWallName, @sourceViewName
			END
		END
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
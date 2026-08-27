-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[install_client_profile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [install_client_profile]
GO

CREATE PROCEDURE [install_client_profile]
AS
BEGIN
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRANSACTION

		-- *** FOR ADD PROPERTIES ***
		--UPDATE ClientProfile
		--SET Settings.modify('insert 
		--	<Setting key="#KEY#">
		--		<Value>#DEFAULT VALUE#</Value>
		--		<Locked>#DEFAULT LOCKED (True/False)#</Locked>
		--	</Setting> 
		--	as last into (/Settings)[1]'),
		--  LastModified = @CurrentUtcTime
		--WHERE Settings.exist('/Settings/Setting[@key="#KEY#"]') = 0 -- NOT EXISTS
		--AND profile_type = 'SmartClientProfile'

		-- *** FOR REMOVE PROPERTIES ***
		--UPDATE ClientProfile
		--SET Settings.modify('delete 
		--/Settings/Setting[@key="#KEY#"][1]') 
		--WHERE Settings.exist('/Settings/Setting[@key="#KEY#"]') = 1 -- EXISTS
		--AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('delete 
		/Settings/Setting[@key="GeneralDefaultImageQuality"][1]') 
		WHERE Settings.exist('/Settings/Setting[@key="GeneralDefaultImageQuality"]') = 1 -- EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('delete 
		/Settings/Setting[@key="GeneralDefaultFrameRate"][1]') 
		WHERE Settings.exist('/Settings/Setting[@key="GeneralDefaultFrameRate"]') = 1 -- EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('delete 
		/Settings/Setting[@key="ApplicationSimpleMode"][1]') 
		WHERE Settings.exist('/Settings/Setting[@key="ApplicationSimpleMode"]') = 1 -- EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="PanesLiveMIPPlugin">
				<Value>Available</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="PanesLiveMIPPlugin"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="PanesPlaybackMIPPlugin">
				<Value>Available</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="PanesPlaybackMIPPlugin"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="PanesSetupMIPPlugin">
				<Value>Available</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="PanesSetupMIPPlugin"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="AdvancedMaxDecodingThreads">
				<Value>Auto</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="AdvancedMaxDecodingThreads"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="AdvancedDeinterlacing">
				<Value>Default</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="AdvancedDeinterlacing"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		-- Updates GENESIS
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="FunctionsLivePrintAvailability">
		    	<Value>Available</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="FunctionsLivePrintAvailability"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('
		replace value of (/Settings/Setting[@key="FunctionsLivePrintAvailability"]/Value[1]/text())[1]
		with "Available"'),
		LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="FunctionsLivePrintAvailability"]/Value[text()="Quick"]') = 1
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="AdvancedTimeZone">
		    	<Value>LocalMachine</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="AdvancedTimeZone"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="AdvancedTimeZoneCustom">
		    	<Value></Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="AdvancedTimeZoneCustom"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportDestinationType">
		    	<Value>Both</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportDestinationType"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportPrivacyMask">
		    	<Value>Available</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportPrivacyMask"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportVideoClipFormat">
		    	<Value>Unavailable</Value>
				<Locked>True</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportVideoClipFormat"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportVideoClipTimestamp">
		    	<Value>No</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportVideoClipTimestamp"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportVideoClipFrameRate">
		    	<Value>No</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportVideoClipFrameRate"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportVideoClipType">
		    	<Value>Both</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportVideoClipType"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
	
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportVideoClipTextsMode">
		    	<Value>Optional</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportVideoClipTextsMode"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportVideoClipTextsContent">
		    	<Value></Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportVideoClipTextsContent"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportVideoClipCodecProperties">
		    	<Value>Available</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportVideoClipCodecProperties"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportSmartClientPlayerFormat">
		    	<Value>Available</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportSmartClientPlayerFormat"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportSmartClientPlayerIncludePlayer">
		    	<Value>Yes</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportSmartClientPlayerIncludePlayer"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportSmartClientPlayerLockForReExport">
		    	<Value>Yes</Value>
				<Locked>True</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportSmartClientPlayerLockForReExport"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportSmartClientPlayerEncryptDatabases">
		    	<Value>Yes</Value>
				<Locked>True</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportSmartClientPlayerEncryptDatabases"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportSmartClientPlayerEncryptPassword">
		    	<Value></Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportSmartClientPlayerEncryptPassword"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportSmartClientPlayerEncryptionStrength">
		    	<Value>AES256</Value>
				<Locked>True</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportSmartClientPlayerEncryptionStrength"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportSmartClientPlayerGlobalCommentsMode">
		    	<Value>Optional</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportSmartClientPlayerGlobalCommentsMode"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportSmartClientPlayerGlobalCommentsContent">
		    	<Value></Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportSmartClientPlayerGlobalCommentsContent"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportSmartClientPlayerIndividualCommentsMode">
		    	<Value>Optional</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportSmartClientPlayerIndividualCommentsMode"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportSmartClientPlayerSignData">
		    	<Value>Yes</Value>
				<Locked>True</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportSmartClientPlayerSignData"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportStillImageFormat">
		    	<Value>Unavailable</Value>
				<Locked>True</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportStillImageFormat"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportStillImageTimestamp">
		    	<Value>No</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportStillImageTimestamp"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportVideoClipOutputType">
				<Value>MKV</Value>
				<Locked>True</Locked>
			 </Setting>
			 as last into (/Settings)[1]'), 
		LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportVideoClipOutputType"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		-- handling GDPR: by default ExportMergeMediaPlayerFormat should be disabled. However if the ExportVideoClipFormat was already enabled we'd like ExportMergeMediaPlayerFormat also to be enabled
		DECLARE @exportVideoClipFormatValue NVARCHAR(14)
		SELECT @exportVideoClipFormatValue = COALESCE(
			(SELECT TOP 1 Settings.value('(/Settings/Setting[@key="ExportVideoClipFormat"]/Value)[1]', 'NVARCHAR(14)')
			 FROM ClientProfile
			 WHERE profile_type = 'SmartClientProfile'
			   AND Settings.exist('/Settings/Setting[@key="ExportVideoClipFormat"]') = 1), 'Unavailable')
		IF @exportVideoClipFormatValue = 'Available'
		BEGIN
			UPDATE ClientProfile
			SET Settings.modify('insert 
				<Setting key="ExportMergeMediaPlayerFormat">
					<Value>Available</Value>
					<Locked>True</Locked>
				</Setting>
				as last into (/Settings)[1]'), 
			LastModified = @CurrentUtcTime
			WHERE Settings.exist('/Settings/Setting[@key="ExportMergeMediaPlayerFormat"]') = 0
			AND profile_type = 'SmartClientProfile'
		END
		ELSE
		BEGIN
			UPDATE ClientProfile
			SET Settings.modify('insert 
				<Setting key="ExportMergeMediaPlayerFormat">
					<Value>Unavailable</Value>
					<Locked>True</Locked>
				</Setting>
				as last into (/Settings)[1]'), 
			LastModified = @CurrentUtcTime
			WHERE Settings.exist('/Settings/Setting[@key="ExportMergeMediaPlayerFormat"]') = 0
			AND profile_type = 'SmartClientProfile'
		END

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportMergeVideoClipType">
				<Value>Both</Value>
				<Locked>False</Locked>
			 </Setting>
			 as last into (/Settings)[1]'), 
		LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportMergeVideoClipType"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportMergeQuality">
				<Value>Medium</Value>
				<Locked>False</Locked>
			 </Setting>
			 as last into (/Settings)[1]'), 
		LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportMergeQuality"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportMergeFramesPerSecond">
				<Value>Fps30</Value>
				<Locked>False</Locked>
			 </Setting>
			 as last into (/Settings)[1]'), 
		LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportMergeFramesPerSecond"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportMergeResolution">
				<Value>FullHD</Value>
				<Locked>False</Locked>
			 </Setting>
			 as last into (/Settings)[1]'), 
		LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportMergeResolution"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportMergeFormat">
				<Value>MP4</Value>
				<Locked>False</Locked>
			 </Setting>
			 as last into (/Settings)[1]'), 
		LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportMergeFormat"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportMergeSequenceType">
				<Value>Sequential</Value>
				<Locked>False</Locked>
			 </Setting>
			 as last into (/Settings)[1]'), 
		LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportMergeSequenceType"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportMergePreventUpscaling">
				<Value>No</Value>
				<Locked>False</Locked>
			 </Setting>
			 as last into (/Settings)[1]'), 
		LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportMergePreventUpscaling"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportMergeIncludeCameraNames">
				<Value>No</Value>
				<Locked>False</Locked>
			 </Setting>
			 as last into (/Settings)[1]'), 
		LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportMergeIncludeCameraNames"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportMergeIncludeTimestamps">
				<Value>No</Value>
				<Locked>False</Locked>
			 </Setting>
			 as last into (/Settings)[1]'), 
		LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportMergeIncludeTimestamps"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ExportMergeAddPadding">
				<Value>No</Value>
				<Locked>False</Locked>
			 </Setting>
			 as last into (/Settings)[1]'), 
		LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ExportMergeAddPadding"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="TimelineIncomingAudio">
		    	<Value>Hide</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="TimelineIncomingAudio"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="TimelineOutgoingAudio">
		    	<Value>Hide</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="TimelineOutgoingAudio"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="TimelineMipData">
		    	<Value>Hide</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="TimelineMipData"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="TimelineBookmarks">
		    	<Value>Show</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="TimelineBookmarks"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="TimelineMipMarker">
		    	<Value>Hide</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="TimelineMipMarker"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="TimelineMotion">
		    	<Value>Show</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="TimelineMotion"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="TimelineAllCamerasTimeline">
		    	<Value>Show</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="TimelineAllCamerasTimeline"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="TimelineSkipGaps">
		    	<Value>DoSkipGaps</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="TimelineSkipGaps"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="CentralizedSearchTab">
		    	<Value>Available</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="CentralizedSearchTab"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('
			replace value of (/Settings/Setting[@key="CentralizedSearchTab"]/Value[1]/text())[1]
			with "Unavailable"
		')
		WHERE (Settings.exist('/Settings/Setting[@key="CentralizedSearchTab"]/Value[text()="Unavailable"]') = 1
		OR Settings.exist('/Settings/Setting[@key="CentralizedSearchTab"]/Value[text()="Unavailable"]') = 1)
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="MapEditMaps">
		    	<Value>Available</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="MapEditMaps"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="GeneralDefaultPtzPointAndClickMode">
		    	<Value>VirtualJoystick</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="GeneralDefaultPtzPointAndClickMode"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert
			<Setting key="ApplicationHideMousePointerTimeout">
				<Value>Seconds_5</Value>
				<Locked>False</Locked>
			</Setting>
			as last into (/Settings)[1]'),
			LastModified= @CurrentUtcTime 
		WHERE Settings.exist('/Settings/Setting[@key="ApplicationHideMousePointerTimeout"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="AlarmManagerTab">
				<Value>Available</Value>
				<Locked>False</Locked>
			</Setting>
			as last into (/Settings)[1]'),
			LastModified = @CurrentUtcTime 
		WHERE Settings.exist('/Settings/Setting[@key="AlarmManagerTab"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ApplicationSnapshotAvailability">
				<Value>Available</Value>
				<Locked>False</Locked>
			</Setting>
			as last into (/Settings)[1]'),
			LastModified=@CurrentUtcTime 
		WHERE Settings.exist('/Settings/Setting[@key="ApplicationSnapshotAvailability"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ApplicationSnapshotCameraNameOverlay">
				<Value>Hide</Value>
				<Locked>False</Locked>
			</Setting>
			as last into (/Settings)[1]'),
			LastModified=@CurrentUtcTime 
		WHERE Settings.exist('/Settings/Setting[@key="ApplicationSnapshotCameraNameOverlay"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ApplicationSnapshotTimeStampOverlay">
				<Value>Hide</Value>
				<Locked>False</Locked>
			</Setting>
			as last into (/Settings)[1]'),
			LastModified=@CurrentUtcTime 
		WHERE Settings.exist('/Settings/Setting[@key="ApplicationSnapshotTimeStampOverlay"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ApplicationSnapshotPath">
				<Value>c:\Snapshots</Value>
				<Locked>False</Locked>
			</Setting>
			as last into (/Settings)[1]'),
			LastModified=@CurrentUtcTime 
		WHERE Settings.exist('/Settings/Setting[@key="ApplicationSnapshotPath"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="SystemMonitorTab">
		    	<Value>Available</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="SystemMonitorTab"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('delete 
		/Settings/Setting[@key="PanesLiveSequencesPreview"][1]') 
		WHERE Settings.exist('/Settings/Setting[@key="PanesLiveSequencesPreview"]') = 1 -- EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('delete 
		/Settings/Setting[@key="PanesLiveCreateBookmark"][1]') 
		WHERE Settings.exist('/Settings/Setting[@key="PanesLiveCreateBookmark"]') = 1 -- EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('delete 
		/Settings/Setting[@key="PanesPlaybackTimeNavigation"][1]') 
		WHERE Settings.exist('/Settings/Setting[@key="PanesPlaybackTimeNavigation"]') = 1 -- EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('delete 
		/Settings/Setting[@key="PanesPlaybackSequencesPreview"][1]') 
		WHERE Settings.exist('/Settings/Setting[@key="PanesPlaybackSequencesPreview"]') = 1 -- EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('delete 
		/Settings/Setting[@key="PanesPlaybackCreateBookmark"][1]') 
		WHERE Settings.exist('/Settings/Setting[@key="PanesPlaybackCreateBookmark"]') = 1 -- EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('delete 
		/Settings/Setting[@key="PanesPlaybackPTZ"][1]') 
		WHERE Settings.exist('/Settings/Setting[@key="PanesPlaybackPTZ"]') = 1 -- EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('delete 
		/Settings/Setting[@key="PanesLivePTZ"][1]') 
		WHERE Settings.exist('/Settings/Setting[@key="PanesLivePTZ"]') = 1 -- EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert <Setting key="FunctionsPlaybackBoundingBoxes"><Value>Available</Value><Locked>False</Locked></Setting> as last into (/Settings)[1]'),
		LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="FunctionsPlaybackBoundingBoxes"]') = 0
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert <Setting key="FunctionsLiveBoundingBoxes"><Value>Available</Value><Locked>False</Locked></Setting> as last into (/Settings)[1]'),
		LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="FunctionsLiveBoundingBoxes"]') = 0
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ApplicationEvidenceLock">
				<Value>Available</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ApplicationEvidenceLock"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ShowAccessRequestNotifications">
				<Value>Yes</Value>
				<Locked>False</Locked>
			</Setting>
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ShowAccessRequestNotifications"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="AlarmManagerShowDesktopNotifications">
				<Value>No</Value>
				<Locked>False</Locked>
			</Setting>
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="AlarmManagerShowDesktopNotifications"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="AlarmManagerPlaySoundNotifications">
				<Value>No</Value>
				<Locked>False</Locked>
			</Setting>
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="AlarmManagerPlaySoundNotifications"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
				
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="GeneralDefaultVideoBuffer">
				<Value>Standard</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="GeneralDefaultVideoBuffer"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="AdvancedVideoPlayerDiagnosticLevel">
				<Value>Level0</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="AdvancedVideoPlayerDiagnosticLevel"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="AdvancedVideoPlayerHardwareDecodingMode">
				<Value>Auto</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="AdvancedVideoPlayerHardwareDecodingMode"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="AdvancedAdaptiveStreaming">
				<Value>Disabled</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="AdvancedAdaptiveStreaming"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'


		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="AdvancedMulticast">
				<Value>Enabled</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="AdvancedMulticast"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="GeneralHtmlViewItemScripting">
				<Value>Disabled</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="GeneralHtmlViewItemScripting"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('delete 
		/Settings/Setting[@key="GeneralLiveIndicator"][1]') 
		WHERE Settings.exist('/Settings/Setting[@key="GeneralLiveIndicator"]') = 1 -- EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('delete 
		/Settings/Setting[@key="AdvancedGisMapCacheCleanUp"][1]') 
		WHERE Settings.exist('/Settings/Setting[@key="AdvancedGisMapCacheCleanUp"]') = 1 -- EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="GisMapBingMapKey">
				<Value></Value>
				<Locked>True</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="GisMapBingMapKey"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="GisMapGoogleMapClientId">
				<Value></Value>
				<Locked>True</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="GisMapGoogleMapClientId"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="GisMapGoogleMapPrivateKey">
				<Value></Value>
				<Locked>True</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="GisMapGoogleMapPrivateKey"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="GisMapCacheCleanUp">
				<Value>OnScClose_IfFileNotUsed</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="GisMapCacheCleanUp"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="GisMapOpenStreetMapGeographicLayer">
				<Value>Unavailable</Value>
				<Locked>True</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="GisMapOpenStreetMapGeographicLayer"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="GisMapCreateLayerLocation">
				<Value>No</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="GisMapCreateLayerLocation"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="GisMapOpenStreetMapAlternativeServer">
				<Value></Value>
				<Locked>True</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="GisMapOpenStreetMapAlternativeServer"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="GisMapGoogleMapSigningSecret">
				<Value></Value>
				<Locked>True</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="GisMapGoogleMapSigningSecret"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="MapEditGisMap">
				<Value>Available</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="MapEditGisMap"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ApplicationLiftPrivacyMaskTimeout">
				<Value>Minutes_30</Value>
				<Locked>True</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ApplicationLiftPrivacyMaskTimeout"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ApplicationOnlineHelp">
				<Value>Available</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ApplicationOnlineHelp"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ApplicationVideoTutorials">
				<Value>Available</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ApplicationVideoTutorials"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="ApplicationTransactTab">
				<Value>Available</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="ApplicationTransactTab"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('delete 
		/Settings/Setting[@key="PanesPlaybackSmartSearch"][1]') 
		WHERE Settings.exist('/Settings/Setting[@key="PanesPlaybackSmartSearch"]') = 1 -- EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="CentralizedSearchMaxDeviceCount">
				<Value>Count_100</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="CentralizedSearchMaxDeviceCount"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'
		
		UPDATE ClientProfile
		SET Settings.modify('insert 
			<Setting key="GeneralCameraStoppedMessage">
				<Value>BlackImageWithOverlay</Value>
				<Locked>False</Locked>
			</Setting> 
			as last into (/Settings)[1]'),
		  LastModified = @CurrentUtcTime
		WHERE Settings.exist('/Settings/Setting[@key="GeneralCameraStoppedMessage"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert
			<Setting key="TimelineHideForNormalViewsTimeout">
				<Value>Disabled</Value>
				<Locked>False</Locked>
			</Setting>
			as last into (/Settings)[1]'),
			LastModified= @CurrentUtcTime 
		WHERE Settings.exist('/Settings/Setting[@key="TimelineHideForNormalViewsTimeout"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

		UPDATE ClientProfile
		SET Settings.modify('insert
			<Setting key="TimelineHideForSmartWallViewsTimeout">
				<Value>Seconds_5</Value>
				<Locked>False</Locked>
			</Setting>
			as last into (/Settings)[1]'),
			LastModified= @CurrentUtcTime 
		WHERE Settings.exist('/Settings/Setting[@key="TimelineHideForSmartWallViewsTimeout"]') = 0 -- NOT EXISTS
		AND profile_type = 'SmartClientProfile'

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

EXEC [install_client_profile]
GO
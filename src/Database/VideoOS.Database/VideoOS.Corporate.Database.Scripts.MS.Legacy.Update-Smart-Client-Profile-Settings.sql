-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

  DECLARE @IDClientProfile UNIQUEIDENTIFIER

  SELECT @IDClientProfile = '62683B38-FD76-4AB8-9B18-71961E4CC2C3'

  IF EXISTS(SELECT * FROM ClientProfile WHERE IDClientProfile = @IDClientProfile)
	UPDATE ClientProfile SET Settings = 
	'<Settings>
		  <Setting key="GeneralTimeInTitlebar">
			<Value>Show</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="GeneralLiveIndicator">
			<Value>Hide</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="GeneralTitleBar">
			<Value>Show</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="GeneralEmptySpaces">
			<Value>CompanyLogo</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="ViewsCustomLogoInEmptySpaces">
			<Value></Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="GeneralCameraErrors">
			<Value>BlackImageWithOverlay</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="GeneralServerErrors">
			<Value>Hidden</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="GeneralViewGridSpacer">
			<Value>Pixel1</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="GeneralApplicationMaximization">
			<Value>FullScreen</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="ApplicationInactivityTimeout">
			<Value>0</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="GeneralDefaultImageQuality">
			<Value>Full</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="GeneralDefaultFrameRate">
			<Value>Unlimited</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="ApplicationMinimizeButton">
			<Value>Available</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="ApplicationMaximizeButton">
			<Value>Available</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="ApplicationLogoutButton">
			<Value>Available</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="ApplicationExitButton">
			<Value>Available</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="ApplicationOptionsDialogButton">
			<Value>Available</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="ApplicationKeyboardSetup">
			<Value>Available</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="ApplicationJoystickSetup">
			<Value>Available</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="ApplicationRememberPassword">
			<Value>Available</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="ApplicationAutoLogin">
			<Value>Available</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="ApplicationStartMode">
			<Value>Last</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="ApplicationStartView">
			<Value>Last</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="ApplicationNewSCVersionWindow">
			<Value>Show</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="ApplicationNewSCVersionText">
			<Value></Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="ApplicationOnlineHelp">
			<Value>Available</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="ApplicationVideoTutorials">
			<Value>Available</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="PanesLiveTab">
			<Value>Available</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="PanesLiveViews">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesLiveSystemOverview">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesLivePTZ">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesLiveAudio">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesLiveOutputs">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesLiveEvents">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesLiveSequencesPreview">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesLiveCreateBookmark">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="FunctionsLiveCameraPlayback">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="FunctionsLiveOverlayButtons">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="FunctionsLiveBookmarkMode">
			<Value>Quick</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesPlaybackTab">
			<Value>Available</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="PanesPlaybackViews">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesPlaybackSystemOverview">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesPlaybackPTZ">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesPlaybackRecordingSearch">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesPlaybackAudio">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesPlaybackSmartSearch">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesPlaybackExport">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="FunctionsPlaybackExportPath">
			<Value>Default</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="FunctionsPlaybackExportPathCustom">
			<Value>C:\Export</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesPlaybackTimeNavigation">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesPlaybackAlerts">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesPlaybackPrint">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="FunctionsPlaybackPrintReportHeadline">
			<Value>Default</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="FunctionsPlaybackPrintReportHeadlineText">
			<Value></Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="FunctionsPlaybackPrintReportUserInformation">
			<Value>Show</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="FunctionsPlaybackPrintReportCopyRights">
			<Value>Show</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesPlaybackSequencesPreview">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesPlaybackCreateBookmark">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="FunctionsPlaybackIndependentPlayback">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="FunctionsPlaybackBookmarkMode">
			<Value>Detail</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesSetupTab">
			<Value>Unavailable</Value>
			<Locked>TRUE</Locked>
		  </Setting>
		  <Setting key="PanesSetupViews">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesSetupSystemOverview">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesSetupOverlayButtons">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="PanesSetupProperties">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="FunctionsSetupOverlayButtons">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="FunctionsSetupEnableVideoBufferingOption">
			<Value>Available</Value>
			<Locked>FALSE</Locked>
		  </Setting>
		  <Setting key="ViewsLayoutsDisabled">
			<Value></Value>
			<Locked>False</Locked>
		  </Setting>
		  <Setting key="ExportDestinationType">
			<Value>Both</Value>
			<Locked>False</Locked>
		  </Setting>
		</Settings>
		'
		WHERE IDClientProfile = @IDClientProfile
GO
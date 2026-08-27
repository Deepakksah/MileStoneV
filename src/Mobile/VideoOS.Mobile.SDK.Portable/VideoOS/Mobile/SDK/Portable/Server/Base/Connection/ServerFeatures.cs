using System;
using System.Collections.Generic;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class ServerFeatures : IServerFeatures
{
	public bool VideoPushEnabled { get; private set; }

	public bool VideoPushBookmarksEnabled { get; private set; }

	public bool VideoPushLocationEnabled { get; private set; }

	public bool StartExportEnabled { get; private set; }

	public bool ViewOthersExportsEnabled { get; private set; }

	public bool AviExportEnabled { get; private set; }

	public bool DbExportEnabled { get; private set; }

	public bool MkvExportEnabled { get; private set; }

	public bool AllCamerasViewEnabled { get; private set; }

	public bool OutputsAndEventEnabled { get; private set; }

	public bool NativeStreamingAvailable { get; private set; }

	public bool NativeStreamingSugested { get; private set; }

	public bool NativeStreamingRequired { get; private set; }

	public bool TranscodedStreamingAvailable { get; private set; }

	public bool SupportsExtendedResampling { get; private set; }

	public bool MultiCameraPlayback { get; private set; }

	public bool SupportsAlarms { get; private set; }

	public bool SupportsBookmarks { get; private set; }

	public bool SupportsBookmarkExtendedSearchCameraIds { get; private set; }

	public bool AllViewsAndCamerasEnabled { get; private set; }

	public string ServerDescription { get; private set; }

	public bool BasicUserManagementEnabled { get; private set; }

	public bool SupportsInvestigations { get; private set; }

	public bool SupportsTwoStepsVerification { get; private set; }

	public bool SupportsPtzGestures { get; private set; }

	public bool SupportsFootages { get; private set; }

	public bool SupportsAccessControl { get; private set; }

	public bool SupportsIncomingAudio { get; private set; }

	public bool SupportsOutgoingAudio { get; private set; }

	public bool SupportsOutgoingSpeakerAudio { get; private set; }

	public bool SupportsAudioPush { get; private set; }

	public bool SupportsDirectStreamingLive { get; private set; }

	public bool SupportsDirectStreamingPlayback { get; private set; }

	public bool SupportsPlaybackInfoHeader { get; private set; }

	public bool SupportsAdaptiveStreaming { get; private set; }

	public bool SupportsPlaybackInTimeRange { get; private set; }

	public bool SupportsSmartMap { get; private set; }

	public bool SupportsShare { get; private set; }

	public Guid ConfigurationId { get; private set; }

	public bool SupportsPlaybackAutoLoop { get; private set; }

	public ServerFeatures()
	{
		InitDefaultValues();
	}

	private void InitDefaultValues()
	{
		ConfigurationId = Guid.Empty;
		VideoPushEnabled = false;
		VideoPushBookmarksEnabled = false;
		VideoPushLocationEnabled = false;
		StartExportEnabled = false;
		ViewOthersExportsEnabled = false;
		AviExportEnabled = false;
		DbExportEnabled = false;
		MkvExportEnabled = false;
		AllCamerasViewEnabled = false;
		OutputsAndEventEnabled = false;
		NativeStreamingAvailable = false;
		NativeStreamingSugested = false;
		NativeStreamingRequired = false;
		TranscodedStreamingAvailable = true;
		SupportsDirectStreamingLive = false;
		SupportsDirectStreamingPlayback = false;
		SupportsPlaybackInfoHeader = false;
		SupportsAdaptiveStreaming = false;
		SupportsExtendedResampling = false;
		MultiCameraPlayback = false;
		SupportsAlarms = false;
		SupportsBookmarks = false;
		SupportsBookmarkExtendedSearchCameraIds = false;
		AllViewsAndCamerasEnabled = false;
		SupportsInvestigations = false;
		ServerDescription = null;
		BasicUserManagementEnabled = false;
		SupportsTwoStepsVerification = false;
		SupportsFootages = false;
		SupportsAccessControl = false;
		SupportsPtzGestures = false;
		SupportsIncomingAudio = false;
		SupportsOutgoingAudio = false;
		SupportsOutgoingSpeakerAudio = false;
		SupportsAudioPush = false;
		SupportsPlaybackInTimeRange = false;
		SupportsSmartMap = false;
		SupportsShare = false;
		SupportsPlaybackAutoLoop = false;
	}

	public void InitFromCommand(Dictionary<string, string> serverParams)
	{
		ConfigurationId = CapabilitiesHelper.ExtractGuid(serverParams, "ConfigurationId", ConfigurationId);
		VideoPushEnabled = CapabilitiesHelper.ExtractBool(serverParams, "VideoPush", VideoPushEnabled);
		VideoPushBookmarksEnabled = CapabilitiesHelper.ExtractBool(serverParams, "Bookmark", VideoPushBookmarksEnabled);
		VideoPushLocationEnabled = CapabilitiesHelper.ExtractBool(serverParams, "VideoPushLocation", VideoPushLocationEnabled);
		StartExportEnabled = CapabilitiesHelper.ExtractBool(serverParams, "Exports", StartExportEnabled);
		ViewOthersExportsEnabled = CapabilitiesHelper.ExtractBool(serverParams, "ViewOthersExports", ViewOthersExportsEnabled);
		AviExportEnabled = CapabilitiesHelper.ExtractBool(serverParams, "ExportToAvi", AviExportEnabled);
		DbExportEnabled = CapabilitiesHelper.ExtractBool(serverParams, "ExportToDb", DbExportEnabled);
		MkvExportEnabled = CapabilitiesHelper.ExtractBool(serverParams, "ExportToMkv", MkvExportEnabled);
		AllCamerasViewEnabled = CapabilitiesHelper.ExtractBool(serverParams, "AllCamerasView", AllCamerasViewEnabled);
		OutputsAndEventEnabled = CapabilitiesHelper.ExtractBool(serverParams, "OutputsAndEvents", OutputsAndEventEnabled);
		NativeStreamingAvailable = CapabilitiesHelper.ExtractBool(serverParams, "NativeStreamingAvailable", NativeStreamingAvailable);
		NativeStreamingSugested = CapabilitiesHelper.ExtractBool(serverParams, "NativeStreamingSuggested", NativeStreamingSugested);
		NativeStreamingRequired = CapabilitiesHelper.ExtractBool(serverParams, "NativeStreamingRequired", NativeStreamingRequired);
		TranscodedStreamingAvailable = CapabilitiesHelper.ExtractBool(serverParams, "TranscodedStreamingAvailable", TranscodedStreamingAvailable);
		SupportsExtendedResampling = CapabilitiesHelper.ExtractBool(serverParams, "SupportsExtendedResamplingFactor", SupportsExtendedResampling);
		MultiCameraPlayback = CapabilitiesHelper.ExtractBool(serverParams, "MultiCameraPlayback", MultiCameraPlayback);
		SupportsAlarms = CapabilitiesHelper.ExtractBool(serverParams, "SupportsAlarms", SupportsAlarms);
		SupportsBookmarks = CapabilitiesHelper.ExtractBool(serverParams, "SupportsBookmarks", SupportsBookmarks);
		SupportsBookmarkExtendedSearchCameraIds = CapabilitiesHelper.ExtractBool(serverParams, "SupportsBookmarkExtendedSearchCameraIds", SupportsBookmarkExtendedSearchCameraIds);
		AllViewsAndCamerasEnabled = CapabilitiesHelper.ExtractBool(serverParams, "AllViewsAndCamerasEnabled", AllViewsAndCamerasEnabled);
		ServerDescription = CapabilitiesHelper.Extract(serverParams, "ServerDescription", (string value) => value, ServerDescription);
		BasicUserManagementEnabled = CapabilitiesHelper.ExtractBool(serverParams, "BasicUserManagementEnabled", BasicUserManagementEnabled);
		SupportsInvestigations = CapabilitiesHelper.ExtractBool(serverParams, "Investigations", SupportsInvestigations);
		SupportsTwoStepsVerification = CapabilitiesHelper.ExtractBool(serverParams, "SupportsTwoStepsVerification", SupportsTwoStepsVerification);
		SupportsFootages = CapabilitiesHelper.ExtractBool(serverParams, "Footages", SupportsFootages);
		SupportsAccessControl = CapabilitiesHelper.ExtractBool(serverParams, "SupportsAccessControl", SupportsAccessControl);
		SupportsPtzGestures = CapabilitiesHelper.ExtractBool(serverParams, "PtzGestures", SupportsPtzGestures);
		SupportsIncomingAudio = CapabilitiesHelper.ExtractBool(serverParams, "SupportsIncomingAudio", SupportsIncomingAudio);
		SupportsOutgoingAudio = CapabilitiesHelper.ExtractBool(serverParams, "SupportsOutgoingAudio", SupportsOutgoingAudio);
		SupportsOutgoingSpeakerAudio = CapabilitiesHelper.ExtractBool(serverParams, "SupportsOutgoingPTTAudio", SupportsOutgoingSpeakerAudio);
		SupportsAudioPush = CapabilitiesHelper.ExtractBool(serverParams, "VideoPushAudio", SupportsAudioPush);
		SupportsDirectStreamingLive = CapabilitiesHelper.ExtractBool(serverParams, "DirectStreamingLive", SupportsDirectStreamingLive);
		SupportsDirectStreamingPlayback = CapabilitiesHelper.ExtractBool(serverParams, "DirectStreamingPlayback", SupportsDirectStreamingPlayback);
		SupportsPlaybackInfoHeader = CapabilitiesHelper.ExtractBool(serverParams, "SupportsPlaybackInfoHeader", SupportsPlaybackInfoHeader);
		SupportsAdaptiveStreaming = CapabilitiesHelper.ExtractBool(serverParams, "SupportsAdaptiveStreaming", SupportsAdaptiveStreaming);
		SupportsPlaybackInTimeRange = CapabilitiesHelper.ExtractBool(serverParams, "SupportsPlaybackInTimeRange", SupportsPlaybackInTimeRange);
		SupportsSmartMap = CapabilitiesHelper.ExtractBool(serverParams, "SupportsSmartMap", SupportsSmartMap);
		SupportsShare = CapabilitiesHelper.ExtractBool(serverParams, "SupportsShare", SupportsShare);
		SupportsPlaybackAutoLoop = CapabilitiesHelper.ExtractBool(serverParams, "SupportsPlaybackAutoLoop", SupportsPlaybackAutoLoop);
	}
}

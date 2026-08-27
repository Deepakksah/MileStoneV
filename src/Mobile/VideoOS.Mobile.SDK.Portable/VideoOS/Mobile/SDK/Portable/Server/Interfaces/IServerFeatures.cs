using System;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IServerFeatures
{
	bool VideoPushEnabled { get; }

	bool VideoPushBookmarksEnabled { get; }

	bool VideoPushLocationEnabled { get; }

	bool StartExportEnabled { get; }

	bool ViewOthersExportsEnabled { get; }

	bool AviExportEnabled { get; }

	bool DbExportEnabled { get; }

	bool MkvExportEnabled { get; }

	bool AllCamerasViewEnabled { get; }

	bool OutputsAndEventEnabled { get; }

	bool NativeStreamingAvailable { get; }

	bool NativeStreamingSugested { get; }

	bool NativeStreamingRequired { get; }

	bool TranscodedStreamingAvailable { get; }

	bool SupportsExtendedResampling { get; }

	bool MultiCameraPlayback { get; }

	bool SupportsAlarms { get; }

	bool SupportsBookmarks { get; }

	bool SupportsBookmarkExtendedSearchCameraIds { get; }

	bool AllViewsAndCamerasEnabled { get; }

	string ServerDescription { get; }

	bool BasicUserManagementEnabled { get; }

	bool SupportsInvestigations { get; }

	bool SupportsTwoStepsVerification { get; }

	bool SupportsPtzGestures { get; }

	bool SupportsFootages { get; }

	bool SupportsAccessControl { get; }

	bool SupportsIncomingAudio { get; }

	bool SupportsOutgoingAudio { get; }

	bool SupportsOutgoingSpeakerAudio { get; }

	bool SupportsAudioPush { get; }

	bool SupportsDirectStreamingLive { get; }

	bool SupportsPlaybackInfoHeader { get; }

	bool SupportsDirectStreamingPlayback { get; }

	bool SupportsAdaptiveStreaming { get; }

	bool SupportsSmartMap { get; }

	bool SupportsShare { get; }

	Guid ConfigurationId { get; }
}

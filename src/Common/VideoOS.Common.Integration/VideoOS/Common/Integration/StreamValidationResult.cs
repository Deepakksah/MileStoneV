namespace VideoOS.Common.Integration;

public class StreamValidationResult
{
	public bool SameStreamsError { get; set; }

	public bool SameStreamNamesError { get; set; }

	public bool EmptyStreamNameError { get; set; }

	public bool EmptySelectionError { get; set; }

	public bool InvalidSelectionError { get; set; }

	public bool StreamCombinationError { get; set; }

	public bool OnlyOneStreamAllowedError { get; set; }

	public bool OnlyOneDefaultLiveStreamError { get; set; }

	public bool MustHaveOneDefaultLiveStreamError { get; set; }

	public bool OnlyOneDefaultPlaybackStreamError { get; set; }

	public bool MustHaveOneDefaultPlaybackStreamError { get; set; }

	public bool OnlyOnePrimaryTrackStreamError { get; set; }

	public bool MustHaveOnePrimaryTrackStreamError { get; set; }

	public bool TwoOrMoreStreamsWithSameTrackStreamError { get; set; }

	public bool UnknownTrackStreamError { get; set; }

	public bool UnknownStreamError { get; set; }

	public bool HasError
	{
		get
		{
			if (!SameStreamsError && !SameStreamNamesError && !EmptyStreamNameError && !EmptySelectionError && !InvalidSelectionError && !StreamCombinationError && !OnlyOneStreamAllowedError && !OnlyOneDefaultLiveStreamError && !MustHaveOneDefaultLiveStreamError && !OnlyOneDefaultPlaybackStreamError && !MustHaveOneDefaultPlaybackStreamError && !OnlyOnePrimaryTrackStreamError && !MustHaveOnePrimaryTrackStreamError && !TwoOrMoreStreamsWithSameTrackStreamError && !UnknownTrackStreamError)
			{
				return UnknownStreamError;
			}
			return true;
		}
	}
}

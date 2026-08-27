using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.MatchLists.Telemetry;

public class NoOpMatchListTelemetryClient : IMatchListTelemetryClient
{
	public void TrackLPRTabMatchListItemAdded(MatchListItemType matchListItemType)
	{
	}

	public void TrackLPRTabMatchListItemEdited(MatchListItemType matchListItemType)
	{
	}

	public void TrackLPRTabMatchListItemDeleted(MatchListItemType matchListItemType)
	{
	}

	public void TrackLPRTabMatchListsImported()
	{
	}

	public void TrackLPRTabMatchListsExported()
	{
	}
}

using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.MatchLists.Telemetry;

public interface IMatchListTelemetryClient
{
	void TrackLPRTabMatchListItemAdded(MatchListItemType matchListItemType);

	void TrackLPRTabMatchListItemEdited(MatchListItemType matchListItemType);

	void TrackLPRTabMatchListItemDeleted(MatchListItemType matchListItemType);

	void TrackLPRTabMatchListsImported();

	void TrackLPRTabMatchListsExported();
}

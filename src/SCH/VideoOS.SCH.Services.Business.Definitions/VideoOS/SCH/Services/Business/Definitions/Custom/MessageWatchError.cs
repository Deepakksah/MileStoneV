namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public enum MessageWatchError
{
	MessageWatchAlreadyExists = -203000,
	MessageWatchRemoveFailed = -203001,
	MatchOperatorCannotBeEmpty = -203002,
	InvalidMatchOperatorName = -203003,
	WatchDataKeyDidNotMatchMsgType = -204004,
	InvalidMessageWatchDataMsgTypeName = -204005,
	InvalidMsgWatchDataMsgTypeNameTooLong = -204006,
	MatchDataKeyCannotBeEmpty = -204007,
	MatchDataKeyTooLong = -204008,
	MatchDataValueCannotBeEmpty = -204009,
	MatchDataValueTooLong = -204010
}

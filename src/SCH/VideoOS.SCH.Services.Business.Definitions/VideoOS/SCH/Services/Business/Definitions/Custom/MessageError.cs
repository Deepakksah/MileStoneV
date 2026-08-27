namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public enum MessageError
{
	InvalidMessageDataMsgTypeName = -205000,
	InvalidMessageDataDataValue = -205001,
	MessageSizeTooBigToFitInOneResponse = -205002,
	InvalidMessageDataValueTooLong = -205003,
	InvalidMessageMsgTypeNameTooLong = -205004,
	InvalidMessageDataNullKey = -205005,
	InvalidMessageDataKeyTooLong = -205006
}

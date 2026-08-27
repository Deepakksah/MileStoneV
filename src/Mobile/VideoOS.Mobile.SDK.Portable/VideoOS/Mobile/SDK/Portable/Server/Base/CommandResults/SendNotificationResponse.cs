using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class SendNotificationResponse : BaseCommandResponse
{
	public NotificationParamsHelper.NotificationResult OperationResult { get; private set; }

	public SendNotificationResponse(Command command)
		: base(command)
	{
		OperationResult = NotificationParamsHelper.NotificationResult.UnknownError;
		if (base.OutputParams != null && base.OutputParams.Count >= 1 && base.OutputParams.ContainsKey("Result"))
		{
			OperationResult = EnumsHelper.EnumConverter<NotificationParamsHelper.NotificationResult>(base.OutputParams["Result"]);
		}
	}

	public SendNotificationResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}

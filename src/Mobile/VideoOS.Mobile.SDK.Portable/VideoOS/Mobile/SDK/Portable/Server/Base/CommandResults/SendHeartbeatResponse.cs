using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class SendHeartbeatResponse : BaseCommandResponse
{
	public HeartbeatResponse OperationResult { get; private set; }

	public SendHeartbeatResponse(Command command)
		: base(command)
	{
		try
		{
			OperationResult = SerializableHeartBeatResponse.GetHeartbeatResponse(command.Items[0]);
		}
		catch (Exception)
		{
			OperationResult = new HeartbeatResponse
			{
				HeartbeatResult = HeartbeatResult.InternalServiceError
			};
		}
		switch (base.ErrorCode)
		{
		case ErrorCodes.HttpResponseError:
		case ErrorCodes.HttpRequestError:
		case ErrorCodes.CommandTimedOut:
			OperationResult.HeartbeatResult = HeartbeatResult.OnlineServiceCommFailure;
			break;
		case ErrorCodes.LicenseNotValid:
			OperationResult.HeartbeatResult = HeartbeatResult.InvalidSLC;
			break;
		}
	}

	public SendHeartbeatResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}

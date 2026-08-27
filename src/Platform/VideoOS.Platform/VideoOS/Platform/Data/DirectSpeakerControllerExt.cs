using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Data;

public class DirectSpeakerControllerExt : DirectSpeakerController, IDirectSpeakerControllerExt, IDirectSpeakerController
{
	public int OutgoingAudioErrorCode { get; set; }

	public FQID DeviceId { get; set; }

	public override string OutgoingAudioErrorText
	{
		get
		{
			return base.OutgoingAudioErrorText;
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				OutgoingAudioErrorCode = 0;
			}
			base.OutgoingAudioErrorText = value;
		}
	}

	public DirectSpeakerControllerExt()
	{
		new TrackPageViewData(typeof(DirectSpeakerControllerExt)).Send();
	}

	protected override void ShowErrorInISClientCommunicationMessage(string errorMessage, int errorCode)
	{
		OutgoingAudioErrorCode = errorCode;
		base.ShowErrorInISClientCommunicationMessage(errorMessage, errorCode);
	}

	public override bool Connect(FQID speakerFQID, int timeoutMSec = 10000)
	{
		DeviceId = speakerFQID;
		return base.Connect(speakerFQID, timeoutMSec);
	}
}

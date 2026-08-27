namespace VideoOS.Platform.Data;

public interface IDirectSpeakerControllerExt : IDirectSpeakerController
{
	int OutgoingAudioErrorCode { get; set; }

	FQID DeviceId { get; set; }
}

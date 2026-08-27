namespace VideoOS.EventServer.Communication.Configuration;

public class UploadSoundRequestObject : RequestObject
{
	private SoundDataItem _soundData;

	public SoundDataItem SoundData => _soundData;

	public UploadSoundRequestObject(int requestId, SoundDataItem soundData)
		: base(requestId)
	{
		_soundData = soundData;
	}
}

namespace VideoOS.Server.RecorderCommunication;

public interface IRecorderEntryFactory
{
	IRecorderEntry CreateRecorder(IRecorderData recorderData);
}

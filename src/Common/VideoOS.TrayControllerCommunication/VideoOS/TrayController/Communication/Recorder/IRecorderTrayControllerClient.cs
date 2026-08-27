using System.Collections.Generic;

namespace VideoOS.TrayController.Communication.Recorder;

public interface IRecorderTrayControllerClient
{
	List<LogMessage> GetMessages(int sequenceNumber);

	RecorderState GetRecorderState();

	bool IsRecorderRunningAndConnected();

	void CloseChannel();
}

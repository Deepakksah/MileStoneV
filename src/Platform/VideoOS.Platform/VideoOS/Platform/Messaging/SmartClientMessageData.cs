namespace VideoOS.Platform.Messaging;

public class SmartClientMessageData
{
	public object MessageId { get; set; }

	public string Message { get; set; }

	public SmartClientMessageDataType MessageType { get; set; }

	public SmartClientMessageDataPriority Priority { get; set; }

	public bool IsClosable { get; set; }

	public string ButtonText { get; set; }

	public SmartClientMessageDataTaskState TaskState { get; set; }

	public double TaskProgress { get; set; }

	public string TaskText { get; set; }

	public SmartClientMessageData()
	{
		Message = string.Empty;
		MessageType = SmartClientMessageDataType.Error;
		Priority = SmartClientMessageDataPriority.Normal;
		IsClosable = false;
		ButtonText = string.Empty;
		TaskState = SmartClientMessageDataTaskState.Complete;
		TaskText = string.Empty;
		TaskProgress = 0.0;
	}
}

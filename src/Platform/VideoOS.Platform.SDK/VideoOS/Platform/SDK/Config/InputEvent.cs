using System;

namespace VideoOS.Platform.SDK.Config;

public class InputEvent
{
	public Guid Guid;

	public string Name;

	public Guid RecorderGuid;

	public InputEvent(Guid guid, string name, Guid recorderGuid)
	{
		Guid = guid;
		Name = name;
		RecorderGuid = recorderGuid;
	}
}

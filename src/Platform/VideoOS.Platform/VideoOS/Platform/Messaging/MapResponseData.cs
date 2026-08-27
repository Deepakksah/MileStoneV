using System;

namespace VideoOS.Platform.Messaging;

[Serializable]
public class MapResponseData
{
	public string SyncString;

	public string ErrorText;

	public string MapGuid;

	public MapResponseDataInstance[] MapCollection;
}

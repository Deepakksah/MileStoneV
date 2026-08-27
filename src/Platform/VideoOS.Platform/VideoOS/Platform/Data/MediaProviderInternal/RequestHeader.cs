using System.Collections.Generic;

namespace VideoOS.Platform.Data.MediaProviderInternal;

public class RequestHeader
{
	public string Command = "";

	public string Action;

	public int Channel = 1;

	public bool Error;

	public bool ResponseSent;

	public MediaProviderSession MediaProviderSession;

	public string[] ParameterList;

	public List<string> ParameterIds = new List<string>();

	public Dictionary<string, string> Parameters = new Dictionary<string, string>();

	public Dictionary<string, string> SetParameters = new Dictionary<string, string>();

	public string CommandMessageId;

	public object CommandData;
}

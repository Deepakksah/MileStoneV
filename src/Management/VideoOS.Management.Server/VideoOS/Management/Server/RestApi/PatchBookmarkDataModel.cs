using System;

namespace VideoOS.Management.Server.RestApi;

public class PatchBookmarkDataModel
{
	public string description { get; set; }

	public DateTime? timeBegin { get; set; }

	public DateTime? timeEnd { get; set; }

	public DateTime? timeTriggered { get; set; }

	public string header { get; set; }

	public string reference { get; set; }
}

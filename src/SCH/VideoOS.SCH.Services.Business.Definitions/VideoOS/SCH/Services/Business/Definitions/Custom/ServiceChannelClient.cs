using System.Collections.Generic;

namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public class ServiceChannelClient
{
	public string Name { get; set; }

	public List<string> MemberGroups { get; set; }

	public ServiceChannelClient(string name, List<string> memberGroups)
	{
		Name = name;
		MemberGroups = memberGroups;
	}
}

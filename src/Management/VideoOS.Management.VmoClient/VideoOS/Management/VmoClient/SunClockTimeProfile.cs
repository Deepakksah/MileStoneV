using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("SunClockTimeProfile")]
public class SunClockTimeProfile : TimeProfile, ICreatable, IUpdatable, IDeletable
{
	public SunClockTimeProfile(ManagementServer managementServer, Guid customId)
		: this(managementServer.Repositories, customId)
	{
	}

	public SunClockTimeProfile(IVmoRepositories repositories, Guid customId)
		: this(repositories)
	{
		if (customId == Guid.Empty)
		{
			throw new ArgumentException("custom id can not be empty", "customId");
		}
		SetPropertyValue("Id", customId.ToString());
	}

	public SunClockTimeProfile(ManagementServer managementServer)
		: this(managementServer.Repositories)
	{
	}

	public SunClockTimeProfile(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: false)
	{
	}

	public SunClockTimeProfile(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}
}

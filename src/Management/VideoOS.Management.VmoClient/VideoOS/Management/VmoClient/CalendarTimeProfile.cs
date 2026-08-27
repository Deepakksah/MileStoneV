using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("CalendarTimeProfile")]
public class CalendarTimeProfile : TimeProfile, ICreatable, IUpdatable, IDeletable
{
	public CalendarTimeProfile(ManagementServer managementServer, Guid customId)
		: this(managementServer.Repositories, customId)
	{
	}

	public CalendarTimeProfile(IVmoRepositories repositories, Guid customId)
		: this(repositories)
	{
		if (customId == Guid.Empty)
		{
			throw new ArgumentException("custom id can not be empty", "customId");
		}
		SetPropertyValue("Id", customId.ToString());
	}

	public CalendarTimeProfile(ManagementServer managementServer)
		: base(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), managementServer.Repositories, isInitialized: true)
	{
	}

	public CalendarTimeProfile(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: false)
	{
	}

	public CalendarTimeProfile(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}
}

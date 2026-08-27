using System;
using VideoOS.Management.Common;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class UserDefinedEvent
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public DateTime LastModified { get; set; }

	public EventDefinition Definition { get; set; }

	public static UserDefinedEvent QueryUserDefinedEvent(Guid userDefinedEventId)
	{
		using UserDefinedEventComponent userDefinedEventComponent = new UserDefinedEventComponent();
		return userDefinedEventComponent.QueryUserDefinedEvent(userDefinedEventId);
	}
}

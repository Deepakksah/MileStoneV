using System;

namespace VideoOS.Management.Server;

public interface IUserDefinedEventComponent : IDisposable
{
	void CreateUserDefinedEvent(UserDefinedEvent newUserDefinedEvent, string userSid);

	string[] QueryUserDefinedEventIds();

	UserDefinedEvent QueryUserDefinedEvent(string userDefinedEventId);

	UserDefinedEvent[] QueryUserDefinedEvents(string[] userDefinedEventIds);

	void UpdateUserDefinedEvent(UserDefinedEvent userDefinedEvent);

	void DeleteUserDefinedEvent(string userDefinedEventId);
}

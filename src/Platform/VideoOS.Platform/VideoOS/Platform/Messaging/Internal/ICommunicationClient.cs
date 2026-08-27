using System;

namespace VideoOS.Platform.Messaging.Internal;

public abstract class ICommunicationClient
{
	public abstract bool IsConnected { get; set; }

	public abstract Uri Uri { get; }

	public abstract void Init();

	public abstract void Close();

	public abstract void StoreForRegistration(MessageReceiver messageReceiver, CommunicationFilter communicationFilter);

	public abstract void WaitForServerRegistration(CommunicationIdFilter messageFilter, TimeSpan timeout);

	public abstract void ToBeRemoved(Guid id);

	public abstract void Transmit(Message message, FQID destinationEndPoint, FQID destinationObject, FQID sourceObject);
}

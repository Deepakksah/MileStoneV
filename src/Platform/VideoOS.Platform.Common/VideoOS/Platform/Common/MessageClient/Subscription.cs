using System;
using VideoOS.Platform.Messaging;

namespace VideoOS.Platform.Common.MessageClient;

public class Subscription
{
	public Guid Id { get; set; }

	public string Topic { get; set; }

	public MessageReceiverAsync ReceiverAsync { get; set; }

	public Type Type { get; set; }
}

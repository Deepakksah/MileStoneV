using System;

namespace VideoOS.Platform.Messaging;

[Serializable]
public class AnalyticsMessageData
{
	public string Name { get; set; }

	public Guid Id { get; set; }

	public string Description { get; set; }
}

using System;

namespace VideoOS.IM.Server.Service.Time;

public sealed class UtcLocalTimeProvider : ITimeProvider
{
	public DateTime LocalTime => DateTime.UtcNow;
}

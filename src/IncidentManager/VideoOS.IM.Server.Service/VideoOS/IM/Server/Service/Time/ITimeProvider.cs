using System;

namespace VideoOS.IM.Server.Service.Time;

public interface ITimeProvider
{
	DateTime LocalTime { get; }
}

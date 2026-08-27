using System;

namespace VideoOS.Common.Interfaces.Recorder;

public interface IDataSecurityExtension
{
	void ConnectionEstablished();

	void ConnectionTerminated();

	bool IsAllowedLive(Guid[] roleIds, DateTime time);

	bool IsAllowedBrowse(Guid[] roleIds, DateTime startTime, DateTime stopTime);
}

using System;

namespace VideoOS.Common.Integration;

public interface IInfoCollector
{
	Guid ManagementServerId();

	string ManagementServerName();
}

using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IFailoverServerInfo
{
	Guid Id { get; }

	string Name { get; }
}

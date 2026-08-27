using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IChange
{
	IChangeType ChangeType { get; set; }

	string Id { get; set; }

	DateTime Time { get; set; }
}

using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IPresetInfo
{
	string Name { get; set; }

	IPositionInfo Position { get; set; }

	string Shortcut { get; set; }

	Guid Id { get; set; }

	bool Locked { get; set; }
}

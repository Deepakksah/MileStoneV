using System;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public class Preset
{
	private readonly string _name;

	private readonly Guid _guid;

	public string Name => _name;

	public Guid Guid => _guid;

	public Preset(ICamera camera, IPresetInfo presetInfo)
	{
		_name = presetInfo.Name;
		_guid = presetInfo.Id;
	}
}

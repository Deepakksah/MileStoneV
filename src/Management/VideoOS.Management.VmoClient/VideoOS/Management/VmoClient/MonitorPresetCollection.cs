using System;
using System.Linq;

namespace VideoOS.Management.VmoClient;

public class MonitorPresetCollection : VmoObjectGuidBasedCollection<MonitorPreset>
{
	public MonitorPreset this[string presetName] => this.First((MonitorPreset m) => m.VideoWallPreset.Name == presetName);

	public MonitorPresetCollection(LoadMethod loadMonitorPresets, VmoObject parent, bool isInitialized)
		: base((Func<bool, MonitorPreset[]>)loadMonitorPresets.Invoke, parent, isInitialized)
	{
	}
}

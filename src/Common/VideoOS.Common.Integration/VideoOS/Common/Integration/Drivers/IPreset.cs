using System.Collections.Generic;

namespace VideoOS.Common.Integration.Drivers;

public interface IPreset
{
	bool IsPresetsEnabled { get; }

	string QueryPresetsFromDevice(IEnumerable<PtzPreset> existingPresets);

	void GotoPreset(PtzPreset preset);
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.Management.VmoClient;

public class DeviceStickerSizeCollection : Collection<DeviceStickerSize>
{
	public void AddRange(IEnumerable<DeviceStickerSize> range)
	{
		if (range == null)
		{
			throw new ArgumentNullException("range");
		}
		foreach (DeviceStickerSize item in range)
		{
			Add(item);
		}
	}
}

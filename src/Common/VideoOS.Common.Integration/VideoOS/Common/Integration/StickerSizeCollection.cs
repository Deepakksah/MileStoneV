using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration;

[Serializable]
[Obsolete("We need to keep class only because it is used in legacy interfaces.")]
public class StickerSizeCollection : Collection<StickerSize>
{
	public void AddRange(IEnumerable<StickerSize> elements)
	{
		if (elements == null)
		{
			throw new ArgumentNullException("elements");
		}
		foreach (StickerSize element in elements)
		{
			Add(element);
		}
	}
}

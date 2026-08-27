using System;

namespace VideoOS.GisMapSharedData.DataContracts.Utility;

public static class LayerModifiersUtil
{
	[Flags]
	public enum LayerModifierMasks
	{
		IsGeographicMask = 1
	}

	public static bool IsGeographic(int modifiers)
	{
		return (modifiers & 1) != 0;
	}

	public static LayerModifierMasks Set(int modifiers, LayerModifierMasks modifier)
	{
		return (LayerModifierMasks)(modifiers | (int)modifier);
	}

	public static LayerModifierMasks Unset(int modifiers, LayerModifierMasks modifier)
	{
		return (LayerModifierMasks)(modifiers & (int)(~modifier));
	}
}

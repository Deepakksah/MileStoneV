using System;
using VideoOS.Common.Integration.Drivers;

namespace VideoOS.Common.Integration.Plugins;

public static class WellKnownPluginIds
{
	public const string MotionPluginIdString = "F020C881-72BF-4014-8504-2B4A5A658F1A";

	public static Guid MotionPluginId { get; private set; }

	public static Guid ImmerVisionPlugin { get; private set; }

	public static Guid GenericDewarpPlugin { get; private set; }

	static WellKnownPluginIds()
	{
		MotionPluginId = new Guid("F020C881-72BF-4014-8504-2B4A5A658F1A");
		ImmerVisionPlugin = GlobalIDs.ImmerVisionPlugInGuid;
	}
}

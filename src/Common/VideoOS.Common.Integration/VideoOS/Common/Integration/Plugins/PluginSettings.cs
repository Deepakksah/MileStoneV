using System;
using System.IO;
using System.Reflection;
using VideoOS.Common.Integration.Definition.Plugin;

namespace VideoOS.Common.Integration.Plugins;

public class PluginSettings
{
	private MainPluginDefinition localDefinition;

	private static object motionDetectionPluginSettingsSync = new object();

	private static volatile PluginSettings motionDetectionPluginSettings;

	public Settings DefaultSettings { get; private set; }

	public string DefinitionAsXml => localDefinition.SaveAsXml();

	public PluginSettings(Guid pluginId, string pluginDefinitionXml)
	{
		localDefinition = MainPluginDefinition.LoadXml(pluginDefinitionXml);
		PluginDefinition pluginDefinition = localDefinition.GetPluginDefinition(pluginId);
		PluginSectionSettings pluginSectionSettings = new PluginSectionSettings();
		pluginSectionSettings.ApplyDefaultValues(pluginDefinition.Base.capabilities);
		DefaultSettings = pluginSectionSettings;
	}

	public static PluginSettings GetMotionDetectionPluginSettings()
	{
		if (motionDetectionPluginSettings != null)
		{
			return motionDetectionPluginSettings;
		}
		lock (motionDetectionPluginSettingsSync)
		{
			if (motionDetectionPluginSettings != null)
			{
				return motionDetectionPluginSettings;
			}
			string name = "VideoOS.Common.Integration.Plugins.VideoOS.Plugin.MotionDetection.xml";
			using (StreamReader streamReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(name)))
			{
				string pluginDefinitionXml = streamReader.ReadToEnd();
				motionDetectionPluginSettings = new PluginSettings(WellKnownPluginIds.MotionPluginId, pluginDefinitionXml);
			}
			return motionDetectionPluginSettings;
		}
	}
}

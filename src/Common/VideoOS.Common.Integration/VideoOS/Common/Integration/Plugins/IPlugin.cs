using System;
using VideoOS.Common.Integration.Media;

namespace VideoOS.Common.Integration.Plugins;

public interface IPlugin
{
	bool IsEnabled { get; set; }

	event EventHandler<PluginEventArgs> PluginEvent;

	void SetConfiguration(string xmlConfig);

	void ProcessFrame(IFrame frame);

	void AddMetadataDelayed(IFrame[] frames);
}

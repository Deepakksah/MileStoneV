using System;
using System.Collections.Generic;
using VideoOS.Mobile.Configuration.Section;

namespace VideoOS.Mobile.Configuration.Group;

public class ServerSettingsConfigGroup : IConfigGroup
{
	public IdentificationConfig Identification { get; private set; }

	public MediaProcessingConfig MediaProcessing { get; private set; }

	public TimeoutConfig ToolkitConnectionTimeout { get; private set; }

	public List<IConfigSection> Items { get; private set; }

	public string GroupName => "ServerSetings";

	public ServerSettingsConfigGroup()
	{
		Identification = new IdentificationConfig(Guid.Empty);
		MediaProcessing = new MediaProcessingConfig("Auto");
		ToolkitConnectionTimeout = new TimeoutConfig(TimeSpan.FromSeconds(5.0), "ToolkitConnectionTimeout");
		Items = new List<IConfigSection> { Identification, MediaProcessing, ToolkitConnectionTimeout };
	}
}

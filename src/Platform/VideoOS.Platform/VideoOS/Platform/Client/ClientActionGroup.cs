using System;
using System.Collections.Generic;
using VideoOS.Platform.UI.Controls;

namespace VideoOS.Platform.Client;

public sealed class ClientActionGroup
{
	public Guid Id { get; }

	public string Name { get; }

	public VideoOSIconSourceBase Icon { get; }

	public List<ClientActionGroup> Groups { get; set; } = new List<ClientActionGroup>();

	public List<ClientAction> Actions { get; set; } = new List<ClientAction>();

	public ClientActionGroup(Guid id, string name)
		: this(id, name, null)
	{
	}

	public ClientActionGroup(Guid id, string name, VideoOSIconSourceBase icon)
	{
		Id = id;
		Name = name;
		Icon = icon;
	}
}

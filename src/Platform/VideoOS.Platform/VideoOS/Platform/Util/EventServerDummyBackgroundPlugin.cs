using System;
using System.Collections.Generic;
using VideoOS.Platform.Background;

namespace VideoOS.Platform.Util;

public class EventServerDummyBackgroundPlugin : BackgroundPlugin
{
	private Guid _id;

	private string _name;

	public override Guid Id => _id;

	public override string Name => _name;

	public override List<EnvironmentType> TargetEnvironments => new List<EnvironmentType> { EnvironmentType.Service };

	public EventServerDummyBackgroundPlugin(Guid id, string name)
	{
		_id = id;
		_name = name;
	}

	public override void Close()
	{
	}

	public override void Init()
	{
	}
}

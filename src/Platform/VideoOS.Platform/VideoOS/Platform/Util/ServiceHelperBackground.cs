using System;
using System.Collections.Generic;
using System.Threading;
using VideoOS.Platform.Background;
using VideoOS.Platform.Messaging;

namespace VideoOS.Platform.Util;

public class ServiceHelperBackground : BackgroundPlugin
{
	private List<EnvironmentType> _envTypes = new List<EnvironmentType>
	{
		EnvironmentType.SmartClient,
		EnvironmentType.Administration,
		EnvironmentType.Standalone,
		EnvironmentType.Service
	};

	private Guid _id = new Guid("CE400F76-4416-4574-8F48-4591EED5A35B");

	public override Guid Id => _id;

	public override string Name => "ServiceHelperBackground";

	public override List<EnvironmentType> TargetEnvironments => _envTypes;

	public override void Init()
	{
		MessageCommunicationManager.Init();
		MessageClientManager.Instance.StartAllUnStartedAsync(CancellationToken.None).GetAwaiter().GetResult();
	}

	public override void Close()
	{
		MessageCommunicationManager.Close();
		MessageClientManager.Instance.CloseAllUnclosedAsync().GetAwaiter().GetResult();
	}
}

using System;
using System.Xml;
using VideoOS.Management.VmoClient;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.ChangeHardwarePassword", "34730CB9-C464-4732-8214-DE28B08ECD0B", ActionTargetType.Unspecified)]
public sealed class ChangeHardwarePasswordRuleActionFactory : RuleActionFactoryBase
{
	private class ChangeHardwarePasswordTaskBuilderFactory : IChangeHardwarePasswordTaskBuilderFactory
	{
		private readonly IServerEnvironment _serverEnvironment;

		public ChangeHardwarePasswordTaskBuilderFactory(IServerEnvironment serverEnvironment)
		{
			_serverEnvironment = serverEnvironment;
		}

		public IChangeHardwarePasswordTaskBuilder CreateChangeHardwarePasswordTaskBuilder()
		{
			return new ChangeHardwarePasswordTaskBuilder(_serverEnvironment);
		}
	}

	private class AsyncTaskMonitorFactory : IAsyncTaskMonitorFactory
	{
		private IServerEnvironment _serverEnvironment;

		public AsyncTaskMonitorFactory(IServerEnvironment serverEnvironment)
		{
			_serverEnvironment = serverEnvironment;
		}

		public IAsyncTaskMonitor CreateAsyncTaskMonitor()
		{
			return new AsyncTaskMonitor(_serverEnvironment.Server);
		}
	}

	private class ChangeHardwarePasswordTaskBuilder : IChangeHardwarePasswordTaskBuilder, IDisposable
	{
		private ServerConnection _serverConnection;

		private VmoClient _vmoClient;

		public ChangeHardwarePasswordTaskBuilder(IServerEnvironment serverEnvironment)
		{
			_serverConnection = new ServerConnection(serverEnvironment.ServerConfig.VmoCommunicationConfiguration.ServerName);
			_serverConnection.ServerPort = serverEnvironment.ServerConfig.VmoCommunicationConfiguration.Port;
			_vmoClient = new VmoClient(_serverConnection);
		}

		public IAsyncTask CreateChangeHardwarePasswordTask(Guid hardwareId, string username, string password)
		{
			Hardware objectById = _vmoClient.Repositories.GetObjectById<Hardware>(hardwareId, refreshCachedObjects: false);
			return objectById.HardwarePasswordChange(password);
		}

		public void Dispose()
		{
			_vmoClient.Dispose();
			_serverConnection.Dispose();
		}
	}

	private readonly IServerEnvironment _serverEnvironment;

	public ChangeHardwarePasswordRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
		_serverEnvironment = new ServerEnvironment();
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		if (subRule == null)
		{
			throw new ArgumentNullException("subRule");
		}
		if (actionNode == null)
		{
			throw new ArgumentNullException("actionNode");
		}
		Guid[] hardwareIds = RuleActionFactoryBase.BuildGuidArray(RuleActionFactoryBase.GetActionParameterByName(actionNode, "HardwareIds"));
		AsyncTaskMonitorFactory taskMonitorFactory = new AsyncTaskMonitorFactory(_serverEnvironment);
		ChangeHardwarePasswordTaskBuilderFactory taskBuilderFactory = new ChangeHardwarePasswordTaskBuilderFactory(_serverEnvironment);
		return new ChangeHardwarePasswordRuleAction(_serverEnvironment, taskMonitorFactory, taskBuilderFactory, hardwareIds);
	}
}

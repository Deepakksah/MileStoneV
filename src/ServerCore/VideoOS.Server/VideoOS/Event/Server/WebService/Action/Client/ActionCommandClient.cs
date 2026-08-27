using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.Event.Server.WebService.Action.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ActionCommandClient : ClientBase<IActionCommand>, IActionCommand
{
	public ActionCommandClient()
	{
	}

	public ActionCommandClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ActionCommandClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ActionCommandClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ActionCommandClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public void ExecuteAction(ActionExecutionParams actionParams)
	{
		base.Channel.ExecuteAction(actionParams);
	}

	public void RuleConfigurationChanged()
	{
		base.Channel.RuleConfigurationChanged();
	}
}

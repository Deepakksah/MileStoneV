using System.CodeDom.Compiler;
using System.ServiceModel;

namespace VideoOS.Event.Server.WebService.Action.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/CentralServerActionCommand", ConfigurationName = "VideoOS.Event.Server.WebService.Action.Client.IActionCommand", SessionMode = SessionMode.NotAllowed)]
public interface IActionCommand
{
	[OperationContract(Action = "http://videoos.net/2/CentralServerActionCommand/IActionCommand/ExecuteAction", ReplyAction = "http://videoos.net/2/CentralServerActionCommand/IActionCommand/ExecuteActionResponse")]
	[FaultContract(typeof(ActionServiceFault), Action = "http://videoos.net/2/CentralServerActionCommand/IActionCommand/ExecuteActionActionServiceFaultFault", Name = "ActionServiceFault")]
	void ExecuteAction(ActionExecutionParams actionParams);

	[OperationContract(Action = "http://videoos.net/2/CentralServerActionCommand/IActionCommand/RuleConfigurationChanged", ReplyAction = "http://videoos.net/2/CentralServerActionCommand/IActionCommand/RuleConfigurationChangedResponse")]
	[FaultContract(typeof(ActionServiceFault), Action = "http://videoos.net/2/CentralServerActionCommand/IActionCommand/RuleConfigurationChangedActionServiceFaultFault", Name = "ActionServiceFault")]
	void RuleConfigurationChanged();
}

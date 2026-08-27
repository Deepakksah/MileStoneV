using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Notification.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/GisNotificationCommand", ConfigurationName = "VideoOS.GisMapCommunication.Notification.Client.IGisNotificationCommand", SessionMode = SessionMode.NotAllowed)]
public interface IGisNotificationCommand
{
	[OperationContract(Action = "http://videoos.net/2/GisNotificationCommand/IGisNotificationCommand/Dummy", ReplyAction = "http://videoos.net/2/GisNotificationCommand/IGisNotificationCommand/DummyResponse")]
	[FaultContract(typeof(GisMapServiceFault), Action = "http://videoos.net/2/GisNotificationCommand/IGisNotificationCommand/DummyGisMapServiceFaultFault", Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
	void Dummy();

	[OperationContract(Action = "http://videoos.net/2/GisNotificationCommand/IGisNotificationCommand/Dummy", ReplyAction = "http://videoos.net/2/GisNotificationCommand/IGisNotificationCommand/DummyResponse")]
	Task DummyAsync();
}

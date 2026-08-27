using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;
using videoos.net._2.XProtectCSServiceRegistration;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01", ConfigurationName = "IServiceRegistrationService")]
public interface IServiceRegistrationService
{
	[OperationContract(Action = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/GetServices", ReplyAction = "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01/IServiceRegistrationService/GetServicesResponse")]
	Task<ServiceInfo[]> GetServicesAsync();
}

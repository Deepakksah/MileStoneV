using System.CodeDom.Compiler;
using System.ServiceModel;

namespace VideoOS.Management.Common;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://schemas.videoos/ServerService/2008/05/Vmo/01")]
public interface IFakeService
{
	[OperationContract]
	void TestConnection();
}

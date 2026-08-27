using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://schemas.videoos/SecurityService/2008/05/Vmo/01", ConfigurationName = "ISecurityService")]
public interface ISecurityService
{
	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByActionList", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByActionListResponse")]
	Task<bool[]> HasPermissionByActionListAsync(string namespaceId, string identity, string[] actionList, string objectId);
}

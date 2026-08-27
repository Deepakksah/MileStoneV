using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskDeleteIdpAdmin")]
public class DeleteIdpAdminTask : AsyncTask
{
	public override string Name => TaskResources.TaskDeleteIdpAdmin;

	public override string EntityName => TaskResources.EntityNameManagementServer;

	public TaskError ErrorResult => base.ErrorResultInstance;

	public string AdminId => GetNestedPropertyValue<string>("InputParameters", "adminId");

	public DeleteIdpAdminTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static DeleteIdpAdminTask StartNew(IVmoRepositories repositories, string id)
	{
		DeleteIdpAdminTask deleteIdpAdminTask = new DeleteIdpAdminTask(string.Empty, new VmoPropertyCollection(), repositories);
		deleteIdpAdminTask.SetNestedPropertyValue("InputParameters", "adminId", id);
		deleteIdpAdminTask.CreateObject(deleteIdpAdminTask);
		return deleteIdpAdminTask;
	}
}

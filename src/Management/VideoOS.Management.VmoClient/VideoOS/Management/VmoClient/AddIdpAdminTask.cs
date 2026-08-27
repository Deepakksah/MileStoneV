using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskAddIdpAdmin")]
public class AddIdpAdminTask : AsyncTask
{
	public override string Name => TaskResources.TaskAddIdpAdmin;

	public override string EntityName => TaskResources.EntityNameManagementServer;

	public TaskError ErrorResult => base.ErrorResultInstance;

	public string AdminId => GetNestedPropertyValue<string>("InputParameters", "adminId");

	public AddIdpAdminTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static AddIdpAdminTask StartNew(IVmoRepositories repositories, string id)
	{
		AddIdpAdminTask addIdpAdminTask = new AddIdpAdminTask(string.Empty, new VmoPropertyCollection(), repositories);
		addIdpAdminTask.SetNestedPropertyValue("InputParameters", "adminId", id);
		addIdpAdminTask.CreateObject(addIdpAdminTask);
		return addIdpAdminTask;
	}
}

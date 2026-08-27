using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskDeleteIdpUser")]
public class DeleteIdpUserTask : AsyncTask
{
	public override string Name => TaskResources.TaskDeleteIdpUser;

	public override string EntityName => TaskResources.EntityNameManagementServer;

	public TaskError ErrorResult => base.ErrorResultInstance;

	public string UserId => GetNestedPropertyValue<string>("InputParameters", "id");

	public DeleteIdpUserTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static DeleteIdpUserTask StartNew(IVmoRepositories repositories, string id)
	{
		DeleteIdpUserTask deleteIdpUserTask = new DeleteIdpUserTask(string.Empty, new VmoPropertyCollection(), repositories);
		deleteIdpUserTask.SetNestedPropertyValue("InputParameters", "id", id);
		deleteIdpUserTask.CreateObject(deleteIdpUserTask);
		return deleteIdpUserTask;
	}
}

using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskActivateLicense")]
public class ActivateLicenseTask : AsyncTask
{
	public override string Name => TaskResources.TaskActivateLicense;

	public override string EntityName => TaskResources.EntityNameManagementServer;

	public TaskError ErrorResult => base.ErrorResultInstance;

	public ActivateLicenseTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static ActivateLicenseTask StartNew(IVmoRepositories repositories, string userName, string password)
	{
		ActivateLicenseTask activateLicenseTask = new ActivateLicenseTask(string.Empty, new VmoPropertyCollection(), repositories);
		activateLicenseTask.SetNestedPropertyValue("InputParameters", "userName", userName);
		activateLicenseTask.SetNestedPropertyValue("InputParameters", "password", password);
		activateLicenseTask.CreateObject(activateLicenseTask);
		return activateLicenseTask;
	}
}

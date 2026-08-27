using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskGetDevicePresets")]
public class GetDevicePresetsTask : AsyncTask
{
	public override string Name => TaskResources.TaskGetDevicePresets;

	public override string EntityName => TaskResources.EntityNameManagementServer;

	public TaskError ErrorResult => base.ErrorResultInstance;

	public GetDevicePresetsTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}
}

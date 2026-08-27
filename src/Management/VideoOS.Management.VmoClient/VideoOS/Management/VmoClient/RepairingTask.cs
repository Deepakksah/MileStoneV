using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskRepairing")]
public class RepairingTask : AsyncTask
{
	public RecordingServer RecordingServer => GetVMOObjectById<RecordingServer>(GetNestedPropertyValue<string>("InputParameters", "idRecorder"));

	public string RecordingServerName => GetNestedPropertyValue<string>("InputParameters", "recordingServerName");

	public override string Name => TaskResources.TaskRepairing;

	public override string EntityName => RecordingServerName;

	public RepairingTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}
}

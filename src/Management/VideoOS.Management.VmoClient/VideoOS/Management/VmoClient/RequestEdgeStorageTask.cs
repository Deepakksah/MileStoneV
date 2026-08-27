using System;
using System.Globalization;
using System.Security;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskRequestEdgeStorage")]
public class RequestEdgeStorageTask : AsyncTask
{
	public Guid RecordingServerId => GetNestedPropertyValue<Guid>("InputParameters", "idRecorder");

	public Guid DeviceId => GetNestedPropertyValue<Guid>("InputParameters", "idDevice");

	public string DeviceName => GetNestedPropertyValue<string>("InputParameters", "deviceName");

	public DateTime EdgeStartTime => DateTime.ParseExact(GetNestedPropertyValue<string>("InputParameters", "startTime"), "O", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);

	public DateTime EdgeEndTime => DateTime.ParseExact(GetNestedPropertyValue<string>("InputParameters", "endTime"), "O", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);

	public string Metadata
	{
		get
		{
			string text = GetNestedPropertyValue<string>("InputParameters", "metadata");
			if (!string.IsNullOrWhiteSpace(text))
			{
				SecurityElement securityElement = new SecurityElement("Metadata", text);
				if (securityElement != null)
				{
					text = securityElement.Text;
				}
			}
			return text;
		}
	}

	public override string Name => TaskResources.TaskRetrievingEdgeRecordings;

	public override string EntityName => DeviceName;

	public RequestEdgeStorageTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static RequestEdgeStorageTask StartNew(IVmoRepositories repositories, Guid deviceId, DateTime startTime, DateTime endTime, string metadata)
	{
		RequestEdgeStorageTask requestEdgeStorageTask = new RequestEdgeStorageTask(string.Empty, new VmoPropertyCollection(), repositories);
		requestEdgeStorageTask.SetNestedPropertyValue("InputParameters", "idDevice", deviceId.ToString().ToUpperInvariant());
		requestEdgeStorageTask.SetNestedPropertyValue("InputParameters", "startTime", startTime.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture));
		requestEdgeStorageTask.SetNestedPropertyValue("InputParameters", "endTime", endTime.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture));
		requestEdgeStorageTask.SetNestedPropertyValue("InputParameters", "metadata", metadata);
		requestEdgeStorageTask.CreateObject(requestEdgeStorageTask);
		return requestEdgeStorageTask;
	}

	public void Stop()
	{
		base.Repositories.ServerCommunication.StopEdgeStorageRetrieval(base.InternalId);
	}
}

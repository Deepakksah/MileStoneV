using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskHardwareScan")]
public class HardwareScanTask : AsyncTask
{
	public override string Name => TaskResources.TaskHardwareScan;

	public Uri Address => new Uri(GetNestedPropertyValue<string>("InputParameters", "address"));

	public IEnumerable<int> Drivers
	{
		get
		{
			IEnumerable<int> result = null;
			string nestedPropertyValue = GetNestedPropertyValue<string>("InputParameters", "drivers");
			if (!string.IsNullOrEmpty(nestedPropertyValue))
			{
				result = (from f in nestedPropertyValue.Trim().Split(';')
					select int.Parse(f, CultureInfo.InvariantCulture)).ToArray();
			}
			return result;
		}
	}

	public string UserName => GetNestedPropertyValue<string>("InputParameters", "userName");

	public string Password => GetNestedPropertyValue<string>("InputParameters", "password");

	public bool UseDefaultCredentials => GetNestedPropertyValue<int>("InputParameters", "useDefaultCredentials") != 0;

	public Guid RecordingServerId => GetNestedPropertyValue<Guid>("InputParameters", "idRecorder");

	public string RecordingServerName => GetNestedPropertyValue<string>("InputParameters", "recordingServerName");

	public override string EntityName => RecordingServerName;

	public TaskError ErrorResult => base.ErrorResultInstance;

	public HardwareScanTaskResult SuccessResult
	{
		get
		{
			if (base.Status != AsyncTaskStatus.Succeeded)
			{
				throw new InvalidOperationException("Not possible to get success result if task status is not 'Succeeded'");
			}
			try
			{
				return HardwareScanTaskResult.FromXml(base.StatusDetails);
			}
			catch (InvalidOperationException)
			{
				return null;
			}
		}
	}

	public HardwareScanTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static HardwareScanTask StartNew(IVmoRepositories repositories, Guid recordingServerId, Uri address, int[] drivers, string userName, string password, bool useDefaultCredentials)
	{
		string value = null;
		if (drivers != null)
		{
			value = string.Join(";", drivers.Select((int i) => i.ToString(CultureInfo.InvariantCulture)).ToArray());
		}
		HardwareScanTask hardwareScanTask = new HardwareScanTask(string.Empty, new VmoPropertyCollection(), repositories);
		hardwareScanTask.SetNestedPropertyValue("InputParameters", "idRecorder", recordingServerId.ToString().ToUpperInvariant());
		hardwareScanTask.SetNestedPropertyValue("InputParameters", "address", address.AbsoluteUri);
		hardwareScanTask.SetNestedPropertyValue("InputParameters", "drivers", value);
		hardwareScanTask.SetNestedPropertyValue("InputParameters", "userName", userName);
		hardwareScanTask.SetNestedPropertyValue("InputParameters", "password", password);
		hardwareScanTask.SetNestedPropertyValue("InputParameters", "useDefaultCredentials", useDefaultCredentials);
		hardwareScanTask.CreateObject();
		return hardwareScanTask;
	}
}

using System;
using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskHardwareScanExpress")]
public class HardwareScanExpressTask : AsyncTask
{
	private CameraCredentialCollection _cameraCredentials;

	public override string Name => TaskResources.TaskHardwareScanExpress;

	public bool UseDefaultCredentials => GetNestedPropertyValue<int>("InputParameters", "useDefaultCredentials") != 0;

	public bool UseSecurePort => GetNestedPropertyValue<int>("InputParameters", "useSecurePort") != 0;

	public Guid RecordingServerId => GetNestedPropertyValue<Guid>("InputParameters", "idRecorder");

	public string RecordingServerName => GetNestedPropertyValue<string>("InputParameters", "recordingServerName");

	public CameraCredentialCollection CameraCredentials
	{
		get
		{
			if (_cameraCredentials == null)
			{
				_cameraCredentials = new CameraCredentialCollection();
				string nestedPropertyValue = GetNestedPropertyValue<string>("InputParameters", "credentialsListXml");
				if (!string.IsNullOrEmpty(nestedPropertyValue))
				{
					foreach (ServerCameraCredential item in ServerCameraCredentials.GetFromXml(nestedPropertyValue))
					{
						CameraCredential cameraCredential = new CameraCredential(item.UserName, item.Password);
						_cameraCredentials.Add(cameraCredential);
					}
				}
				_cameraCredentials.CollectionChangedEvent += CredentialsChanged;
			}
			return _cameraCredentials;
		}
	}

	public override string EntityName => RecordingServerName;

	public TaskError ErrorResult => base.ErrorResultInstance;

	public HardwareScanExpressTaskResult SuccessResult
	{
		get
		{
			if (base.Status != AsyncTaskStatus.Succeeded)
			{
				throw new InvalidOperationException("Not possible to get success result if task status is not 'Succeeded'");
			}
			try
			{
				return HardwareScanExpressTaskResult.FromXml(base.StatusDetails);
			}
			catch (InvalidOperationException)
			{
				return null;
			}
		}
	}

	public HardwareScanExpressTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	protected override void PropertiesLoaded()
	{
		if (_cameraCredentials != null)
		{
			_cameraCredentials.CollectionChangedEvent -= CredentialsChanged;
			_cameraCredentials = null;
		}
	}

	private void CredentialsChanged(object sender, EventArgs e)
	{
		SetPropertyValue("credentialsListXml", _cameraCredentials.ToServerCameraCredentialXml());
	}

	public static HardwareScanExpressTask StartNew(IVmoRepositories repositories, Guid recordingServerId, CameraCredentialCollection cameraCredentials, bool useDefaultCredentials, bool useSecurePort)
	{
		HardwareScanExpressTask hardwareScanExpressTask = new HardwareScanExpressTask(string.Empty, new VmoPropertyCollection(), repositories);
		hardwareScanExpressTask.SetNestedPropertyValue("InputParameters", "idRecorder", recordingServerId.ToString().ToUpperInvariant());
		hardwareScanExpressTask.SetNestedPropertyValue("InputParameters", "credentialsListXml", cameraCredentials.ToServerCameraCredentialXml());
		hardwareScanExpressTask.SetNestedPropertyValue("InputParameters", "useDefaultCredentials", useDefaultCredentials);
		hardwareScanExpressTask.SetNestedPropertyValue("InputParameters", "useSecurePort", useSecurePort);
		hardwareScanExpressTask.CreateObject();
		return hardwareScanExpressTask;
	}
}

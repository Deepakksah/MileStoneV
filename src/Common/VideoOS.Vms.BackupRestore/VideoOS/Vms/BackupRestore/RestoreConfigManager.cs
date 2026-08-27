using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Utilities.Database;

namespace VideoOS.Vms.BackupRestore;

public class RestoreConfigManager
{
	private delegate EventArgs PrepareForRestoreWorkerDel(string backupFileName, string localTargetFolderName, string shareName);

	private delegate EventArgs ValidateForRestoreWorkerDel(string backupFolderName, IList<RegisteredService> registeredServices);

	private delegate EventArgs RestoreWorkerDel(string backupFolderName);

	private delegate EventArgs WaitForZeroActiveActionsWorkerDel(int timeoutInMs);

	private delegate EventArgs CleanUpWorkerDel(string fullSharedFolderPath, string sharedFolderName);

	private delegate EventArgs StartServicesWorkerDel();

	private delegate EventArgs StopServicesWorkerDel();

	private class ComponentListProgress
	{
		public int NoofComponents { get; set; }

		public int NoofComponentsDone { get; set; }

		public int PercentPerComponent { get; set; }

		public int PercentInTotal { get; set; }

		public ComponentListProgress(int noofComponents, int percentInTotal)
		{
			if (noofComponents <= 0)
			{
				throw new ArgumentException("noofComponents must be positive");
			}
			if (percentInTotal <= 0)
			{
				throw new ArgumentException("percentInTotal must be positive");
			}
			NoofComponents = noofComponents;
			NoofComponentsDone = 0;
			PercentInTotal = percentInTotal;
			PercentPerComponent = PercentInTotal / NoofComponents;
		}
	}

	private IRestoreConfigManagerExternalCommunication _restoreConfigManagerExternalCommunication;

	private RestoreConfigActionStatus _currentActionStatus;

	private int _currentProgress;

	private ComponentListProgress _componentProgress;

	private bool _abortRequested;

	private int _noofActiveActions;

	private List<IConfigRestore> _restoreComponents = new List<IConfigRestore>();

	private int NoofActiveActions
	{
		get
		{
			lock (this)
			{
				return _noofActiveActions;
			}
		}
	}

	public bool AbortRequested
	{
		get
		{
			lock (this)
			{
				return _abortRequested;
			}
		}
		set
		{
			lock (this)
			{
				_abortRequested = value;
			}
		}
	}

	public event EventHandler<RestoreActionProgressEventArgs> RestoreActionProgress;

	public event EventHandler<RestoreActionSucceededEventArgs> RestoreActionSucceeded;

	public event EventHandler<RestoreActionFailedEventArgs> RestoreActionFailed;

	public event EventHandler<RestoreActionAbortedEventArgs> RestoreActionAborted;

	public RestoreConfigManager(IRestoreConfigManagerExternalCommunication restoreConfigManagerExternalCommunication)
	{
		_restoreConfigManagerExternalCommunication = restoreConfigManagerExternalCommunication;
		_currentActionStatus = RestoreConfigActionStatus.Undefined;
		_currentProgress = 0;
		_abortRequested = false;
		_noofActiveActions = 0;
	}

	public void AddComponent(IConfigRestore restoreComponent)
	{
		_restoreComponents.Add(restoreComponent);
	}

	public void BeginPrepareForRestore(string backupFileName, string localTargetFolderName, string shareName)
	{
		new PrepareForRestoreWorkerDel(PrepareForRestoreWorker).BeginInvoke(backupFileName, localTargetFolderName, shareName, ActionCompleted, RestoreConfigActionStatus.PreparingForRestore);
	}

	public EventArgs PrepareForRestore(string backupFileName, string localTargetFolderName, string shareName)
	{
		try
		{
			return PrepareForRestoreWorker(backupFileName, localTargetFolderName, shareName);
		}
		finally
		{
			DecreaseNoofActiveActions();
		}
	}

	private EventArgs PrepareForRestoreWorker(string backupFileName, string localTargetFolderName, string shareName)
	{
		IncreaseNoofActiveActions();
		if (string.IsNullOrEmpty(backupFileName) || string.IsNullOrEmpty(localTargetFolderName))
		{
			return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.PreparingForRestore, RestoreConfigError.InvalidStartParameters, "Invalid start parameters to PrepareForRestore()", null, null);
		}
		ReportProgress(RestoreConfigActionStatus.PreparingForRestore, 0);
		_restoreConfigManagerExternalCommunication.DeleteFiles(localTargetFolderName);
		CorporateSqlData corporateSqlData = new CorporateSqlData(_restoreConfigManagerExternalCommunication.SqlConnectionManager);
		try
		{
			_restoreConfigManagerExternalCommunication.CreateSharedFolderSqlServerAccount(localTargetFolderName, shareName, corporateSqlData);
		}
		catch (SqlServerUseLocalUserAccountException ex)
		{
			return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.PreparingForRestore, RestoreConfigError.SqlServerRunningAsLocal, ex.Message, null, ex);
		}
		catch (DatabaseException ex2)
		{
			DatabaseError[] errors = ex2.Errors;
			for (int i = 0; i < errors.Length; i++)
			{
				switch (errors[i].Number)
				{
				case 4060:
					return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.PreparingForRestore, RestoreConfigError.SqlAccessDatabaseError, ex2.Message, new string[1] { _restoreConfigManagerExternalCommunication.SqlConnectionManager.DatabaseName }, ex2);
				case 18456:
					return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.PreparingForRestore, RestoreConfigError.SqlAuthenticationError, ex2.Message, null, ex2);
				}
			}
			return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.PreparingForRestore, RestoreConfigError.SqlRestoreError, ex2.Message, null, ex2);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex4)
		{
			return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.PreparingForRestore, RestoreConfigError.CreateShareFailed, ex4.Message, null, ex4);
		}
		_componentProgress = new ComponentListProgress(2, 100);
		try
		{
			_restoreConfigManagerExternalCommunication.CopyFileWithProgress(backupFileName, localTargetFolderName, ReportComponentProgress);
		}
		catch (FileNotFoundException ex5)
		{
			return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.PreparingForRestore, RestoreConfigError.FileNotFound, ex5.Message, null, ex5);
		}
		catch (IOException ex6)
		{
			return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.PreparingForRestore, RestoreConfigError.FileNotFound, ex6.Message, null, ex6);
		}
		if (CheckAndResetAbortRequest())
		{
			return new RestoreActionAbortedEventArgs(RestoreConfigActionStatus.PreparingForRestore);
		}
		_componentProgress.NoofComponentsDone++;
		try
		{
			_restoreConfigManagerExternalCommunication.DecompressFileWithProgress(Path.Combine(localTargetFolderName, Path.GetFileName(backupFileName)), localTargetFolderName, ReportComponentProgress);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex8)
		{
			return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.PreparingForRestore, RestoreConfigError.ExceptionOccurred, ex8.Message, null, ex8);
		}
		return new RestoreActionSucceededEventArgs(RestoreConfigActionStatus.PreparingForRestore);
	}

	public void BeginValidateForRestore(string backupFolderName, IList<RegisteredService> registeredServices)
	{
		new ValidateForRestoreWorkerDel(ValidateForRestoreWorker).BeginInvoke(backupFolderName, registeredServices, ActionCompleted, RestoreConfigActionStatus.Validating);
	}

	public EventArgs ValidateForRestore(string backupFolderName, IList<RegisteredService> registeredServices)
	{
		try
		{
			return ValidateForRestoreWorker(backupFolderName, registeredServices);
		}
		finally
		{
			DecreaseNoofActiveActions();
		}
	}

	private EventArgs ValidateForRestoreWorker(string fullBackupFolderPath, IList<RegisteredService> registeredServices)
	{
		IncreaseNoofActiveActions();
		if (string.IsNullOrEmpty(fullBackupFolderPath))
		{
			return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.Validating, RestoreConfigError.InvalidStartParameters, "Invalid start parameters to ValidateForRestore()", null, null);
		}
		ReportProgress(RestoreConfigActionStatus.Validating, 0);
		BackupConfigSettings backupConfigSettings = null;
		try
		{
			backupConfigSettings = _restoreConfigManagerExternalCommunication.ReadConfigFromFile(fullBackupFolderPath);
		}
		catch (XmlException ex)
		{
			return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.Validating, RestoreConfigError.InvalidConfigurationFile, ex.Message, null, ex);
		}
		_componentProgress = new ComponentListProgress(_restoreComponents.Count, 100);
		foreach (IConfigRestore restoreComponent in _restoreComponents)
		{
			if (backupConfigSettings.ComponentSettings.TryGetValue(restoreComponent.ComponentName, out var value))
			{
				RestoreValidationResult restoreValidationResult = restoreComponent.ValidateForRestore(value.ConfigData, registeredServices, ReportComponentProgress, IsAbortRequested);
				if (CheckAndResetAbortRequest())
				{
					return new RestoreActionAbortedEventArgs(RestoreConfigActionStatus.Validating);
				}
				if (!restoreValidationResult.Success)
				{
					return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.Validating, restoreValidationResult.ValidationError, restoreValidationResult.EnglishErrorDescription, restoreValidationResult.ErrorParameters, null);
				}
				if (CheckAndResetAbortRequest())
				{
					return new RestoreActionAbortedEventArgs(RestoreConfigActionStatus.Validating);
				}
				_componentProgress.NoofComponentsDone++;
				continue;
			}
			return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.Validating, RestoreConfigError.NoComponentSettingsFound, "No component settings could be found for the restore component '{0}'.", new string[1] { restoreComponent.ComponentName }, null);
		}
		List<string> list = new List<string>();
		foreach (IConfigRestore restoreComponent2 in _restoreComponents)
		{
			string[] serviceAccounts = restoreComponent2.GetServiceAccounts(registeredServices);
			if (serviceAccounts != null)
			{
				list.AddRange(serviceAccounts);
			}
		}
		ReportProgress(RestoreConfigActionStatus.Validating, 100);
		return new ValidateSucceededEventArgs(list);
	}

	public void BeginRestore(string backupFolderName)
	{
		new RestoreWorkerDel(RestoreWorker).BeginInvoke(backupFolderName, ActionCompleted, RestoreConfigActionStatus.Restoring);
	}

	public EventArgs Restore(string backupFolderName)
	{
		try
		{
			return RestoreWorker(backupFolderName);
		}
		finally
		{
			DecreaseNoofActiveActions();
		}
	}

	private EventArgs RestoreWorker(string backupFolderName)
	{
		IncreaseNoofActiveActions();
		if (string.IsNullOrEmpty(backupFolderName))
		{
			return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.Restoring, RestoreConfigError.InvalidStartParameters, "Invalid start parameters to Restore()", null, null);
		}
		ReportProgress(RestoreConfigActionStatus.Restoring, 0);
		_componentProgress = new ComponentListProgress(_restoreComponents.Count, 100);
		foreach (IConfigRestore restoreComponent in _restoreComponents)
		{
			RestoreComponentResult restoreComponentResult = restoreComponent.Restore(backupFolderName, ReportComponentProgress, IsAbortRequested);
			if (!restoreComponentResult.Success)
			{
				return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.Restoring, restoreComponentResult.ComponentError, restoreComponentResult.EnglishErrorDescription, restoreComponentResult.ErrorParameters, restoreComponentResult.Exception);
			}
			_componentProgress.NoofComponentsDone++;
		}
		ReportProgress(RestoreConfigActionStatus.Restoring, 100);
		return new RestoreActionSucceededEventArgs(RestoreConfigActionStatus.Restoring);
	}

	public void BeginWaitForZeroActiveActions(int timeoutInMS)
	{
		new WaitForZeroActiveActionsWorkerDel(WaitForZeroActiveActionsWorker).BeginInvoke(timeoutInMS, ActionCompleted, RestoreConfigActionStatus.WaitingForZeroActiveActions);
	}

	public EventArgs WaitForZeroActiveActions(int timeoutInMS)
	{
		return WaitForZeroActiveActionsWorker(timeoutInMS);
	}

	private EventArgs WaitForZeroActiveActionsWorker(int timeoutInMS)
	{
		DateTime dateTime = ((timeoutInMS >= 0) ? DateTime.UtcNow.AddMilliseconds(timeoutInMS) : DateTime.MaxValue);
		do
		{
			if (NoofActiveActions == 0)
			{
				return new WaitForZeroActiveActionsSucceededEventArgs(NoofActiveActions, timeoutElapsed: false);
			}
			Application.DoEvents();
			Thread.Sleep(100);
		}
		while (DateTime.UtcNow < dateTime);
		return new WaitForZeroActiveActionsSucceededEventArgs(NoofActiveActions, timeoutElapsed: true);
	}

	public void BeginCleanUp(string fullSharedFolderPath, string sharedFolderName)
	{
		new CleanUpWorkerDel(CleanUpWorker).BeginInvoke(fullSharedFolderPath, sharedFolderName, ActionCompleted, RestoreConfigActionStatus.CleaningUp);
	}

	public EventArgs CleanUp(string fullSharedFolderPath, string sharedFolderName)
	{
		return CleanUpWorker(fullSharedFolderPath, sharedFolderName);
	}

	private EventArgs CleanUpWorker(string fullSharedFolderPath, string sharedFolderName)
	{
		if (string.IsNullOrEmpty(fullSharedFolderPath))
		{
			return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.CleaningUp, RestoreConfigError.InvalidStartParameters, "Invalid start parameters to CleanUp()", null, null);
		}
		ReportProgress(RestoreConfigActionStatus.CleaningUp, 0);
		ClearBackupFolder(sharedFolderName, sharedFolderName);
		ReportProgress(100);
		return new RestoreActionSucceededEventArgs(RestoreConfigActionStatus.CleaningUp);
	}

	private void ClearBackupFolder(string fullSharedFolderPath, string sharedFolderName)
	{
		try
		{
			_restoreConfigManagerExternalCommunication.RemoveShare(sharedFolderName);
			if (_restoreConfigManagerExternalCommunication.DirectoryExists(fullSharedFolderPath))
			{
				_restoreConfigManagerExternalCommunication.DeleteFiles(fullSharedFolderPath);
				_restoreConfigManagerExternalCommunication.DeleteFolder(fullSharedFolderPath);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	public void BeginStartServices()
	{
		new StartServicesWorkerDel(StartServicesWorker).BeginInvoke(ActionCompleted, RestoreConfigActionStatus.StartingServices);
	}

	public EventArgs StartServices()
	{
		return StartServicesWorker();
	}

	private EventArgs StartServicesWorker()
	{
		ReportProgress(RestoreConfigActionStatus.StartingServices, 0);
		_componentProgress = new ComponentListProgress(_restoreComponents.Count, 100);
		foreach (IConfigRestore restoreComponent in _restoreComponents)
		{
			if (!restoreComponent.StartServices(ReportComponentProgress))
			{
				return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.StartingServices, RestoreConfigError.ServicesFailedToStart, "Component {0} failed to start services.", new string[1] { restoreComponent.ComponentName }, null);
			}
			_componentProgress.NoofComponentsDone++;
		}
		ReportProgress(100);
		return new RestoreActionSucceededEventArgs(RestoreConfigActionStatus.StartingServices);
	}

	public void BeginStopServices()
	{
		new StopServicesWorkerDel(StopServicesWorker).BeginInvoke(ActionCompleted, RestoreConfigActionStatus.StoppingServices);
	}

	public EventArgs StopServices()
	{
		return StopServicesWorker();
	}

	private EventArgs StopServicesWorker()
	{
		ReportProgress(RestoreConfigActionStatus.StoppingServices, 0);
		_componentProgress = new ComponentListProgress(_restoreComponents.Count, 100);
		foreach (IConfigRestore restoreComponent in _restoreComponents)
		{
			if (!restoreComponent.StopServices(ReportComponentProgress))
			{
				return new RestoreActionFailedEventArgs(RestoreConfigActionStatus.StoppingServices, RestoreConfigError.ServicesFailedToStop, "Component {0} failed to stop services.", new string[1] { restoreComponent.ComponentName }, null);
			}
			_componentProgress.NoofComponentsDone++;
		}
		ReportProgress(100);
		return new RestoreActionSucceededEventArgs(RestoreConfigActionStatus.StoppingServices);
	}

	private void IncreaseNoofActiveActions()
	{
		lock (this)
		{
			_noofActiveActions++;
		}
	}

	private void DecreaseNoofActiveActions()
	{
		lock (this)
		{
			_noofActiveActions--;
		}
	}

	private bool CheckAndResetAbortRequest()
	{
		lock (this)
		{
			if (_abortRequested)
			{
				_abortRequested = false;
				return true;
			}
			return false;
		}
	}

	private bool IsAbortRequested()
	{
		return AbortRequested;
	}

	private void ActionCompleted(IAsyncResult result)
	{
		AsyncResult asyncResult = (AsyncResult)result;
		EventArgs e = null;
		RestoreConfigActionStatus restoreConfigActionStatus = (RestoreConfigActionStatus)result.AsyncState;
		try
		{
			switch (restoreConfigActionStatus)
			{
			case RestoreConfigActionStatus.PreparingForRestore:
				DecreaseNoofActiveActions();
				e = ((PrepareForRestoreWorkerDel)asyncResult.AsyncDelegate).EndInvoke(result);
				break;
			case RestoreConfigActionStatus.Validating:
				DecreaseNoofActiveActions();
				e = ((ValidateForRestoreWorkerDel)asyncResult.AsyncDelegate).EndInvoke(result);
				break;
			case RestoreConfigActionStatus.Restoring:
				DecreaseNoofActiveActions();
				e = ((RestoreWorkerDel)asyncResult.AsyncDelegate).EndInvoke(result);
				break;
			case RestoreConfigActionStatus.WaitingForZeroActiveActions:
				e = ((WaitForZeroActiveActionsWorkerDel)asyncResult.AsyncDelegate).EndInvoke(result);
				break;
			case RestoreConfigActionStatus.StartingServices:
				e = ((StartServicesWorkerDel)asyncResult.AsyncDelegate).EndInvoke(result);
				break;
			case RestoreConfigActionStatus.StoppingServices:
				e = ((StopServicesWorkerDel)asyncResult.AsyncDelegate).EndInvoke(result);
				break;
			case RestoreConfigActionStatus.CleaningUp:
				e = ((CleanUpWorkerDel)asyncResult.AsyncDelegate).EndInvoke(result);
				break;
			default:
				e = new RestoreActionFailedEventArgs(RestoreConfigActionStatus.Undefined, RestoreConfigError.Undefined, "Unknown ActionStatus in ActionCompleted()", null, null);
				break;
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception actionException)
		{
			e = new RestoreActionFailedEventArgs(restoreConfigActionStatus, RestoreConfigError.ExceptionOccurred, "Exception occurred during " + restoreConfigActionStatus, null, actionException);
		}
		if (e is RestoreActionFailedEventArgs e2)
		{
			OnRestoreActionFailed(e2);
		}
		else if (e is RestoreActionSucceededEventArgs e3)
		{
			OnRestoreActionSucceeded(e3);
		}
		else if (e is RestoreActionAbortedEventArgs e4)
		{
			OnRestoreActionAborted(e4);
		}
	}

	private void ReportComponentProgress(string componentName, int percentDone)
	{
		int currentActionProgress = _currentProgress + _componentProgress.NoofComponentsDone * _componentProgress.PercentPerComponent + Convert.ToInt32(_componentProgress.PercentPerComponent * percentDone / 100);
		RestoreActionProgressEventArgs e = new RestoreActionProgressEventArgs(_currentActionStatus, componentName, currentActionProgress);
		OnRestoreActionProgress(e);
	}

	private void ReportProgress(int currentProgress)
	{
		_currentProgress = currentProgress;
		RestoreActionProgressEventArgs e = new RestoreActionProgressEventArgs(_currentActionStatus, null, _currentProgress);
		OnRestoreActionProgress(e);
	}

	private void ReportProgress(RestoreConfigActionStatus currentActionStatus, int currentProgress)
	{
		_currentActionStatus = currentActionStatus;
		_currentProgress = currentProgress;
		RestoreActionProgressEventArgs e = new RestoreActionProgressEventArgs(_currentActionStatus, null, _currentProgress);
		OnRestoreActionProgress(e);
	}

	private void OnRestoreActionProgress(RestoreActionProgressEventArgs e)
	{
		EventHandler<RestoreActionProgressEventArgs> eventHandler = this.RestoreActionProgress;
		if (eventHandler != null)
		{
			try
			{
				eventHandler(this, e);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}

	private void OnRestoreActionSucceeded(RestoreActionSucceededEventArgs e)
	{
		EventHandler<RestoreActionSucceededEventArgs> eventHandler = this.RestoreActionSucceeded;
		if (eventHandler != null)
		{
			try
			{
				eventHandler(this, e);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}

	private void OnRestoreActionFailed(RestoreActionFailedEventArgs e)
	{
		EventHandler<RestoreActionFailedEventArgs> eventHandler = this.RestoreActionFailed;
		if (eventHandler != null)
		{
			try
			{
				eventHandler(this, e);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}

	private void OnRestoreActionAborted(RestoreActionAbortedEventArgs e)
	{
		EventHandler<RestoreActionAbortedEventArgs> eventHandler = this.RestoreActionAborted;
		if (eventHandler != null)
		{
			try
			{
				eventHandler(this, e);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}

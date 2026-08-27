using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace VideoOS.Vms.BackupRestore;

public class BackupConfigManager
{
	public enum BackupConfigAction
	{
		StartBackup,
		AbortBackup
	}

	public enum BackupConfigResult
	{
		Success,
		Failed,
		Aborted
	}

	public enum BackupConfigError
	{
		Undefined = 0,
		BackupAlreadyActive = -100001,
		BackupAlreadyInactive = -100002,
		ExceptionOccurred = -100003,
		SqlServerRunningAsLocal = -100004,
		NoBackupFilesWereMade = -100005,
		MoveFilesFailed = -100006,
		MissingBackupFolder = -100007,
		LowOnDiskSpace = -100008,
		SqlBackupError = -100009,
		SqlAuthenticationError = -100010,
		FileIOError = -100011,
		SqlNetworkError = -100012
	}

	public enum BackupConfigActionStatus
	{
		Undefined,
		NotStarted,
		Validating,
		CreatingBackup,
		CompressingBackupFiles
	}

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

	private BackupConfigActionStatus _currentActionStatus;

	private int _currentProgress;

	private ComponentListProgress _componentProgress;

	private bool _backupIsActive;

	private bool _abortRequested;

	private IBackupConfigManagerExternalCommunication _backupConfigManagerExternalCommunication;

	private List<IConfigBackup> _backupComponents = new List<IConfigBackup>();

	public ILogger Logger { get; private set; }

	public event EventHandler<BackupProgressEventArgs> BackupProgress;

	public BackupConfigManager(IBackupConfigManagerExternalCommunication backupConfigManagerExternalCommunication)
	{
		if (backupConfigManagerExternalCommunication == null)
		{
			throw new ArgumentNullException("backupConfigManagerExternalCommunication");
		}
		_backupIsActive = false;
		_abortRequested = false;
		_currentActionStatus = BackupConfigActionStatus.NotStarted;
		_currentProgress = 0;
		Logger = backupConfigManagerExternalCommunication.Logger;
		_backupConfigManagerExternalCommunication = backupConfigManagerExternalCommunication;
	}

	public void AddComponent(IConfigBackup backupComponent)
	{
		if (backupComponent == null)
		{
			throw new ArgumentNullException("backupComponent");
		}
		_backupComponents.Add(backupComponent);
	}

	public BackupConfigResultData Backup(string backupPath)
	{
		_currentActionStatus = BackupConfigActionStatus.NotStarted;
		List<string> list = new List<string>();
		IList<RegisteredService> getRegisteredServices = _backupConfigManagerExternalCommunication.GetRegisteredServices;
		BackupConfigSettings backupConfigSettings = new BackupConfigSettings(Environment.MachineName, getRegisteredServices, new Dictionary<string, BackupConfigComponentSettings>());
		lock (this)
		{
			if (_backupIsActive)
			{
				return BackupConfigResultData.CreateFailedResult(BackupConfigError.BackupAlreadyActive);
			}
			_backupIsActive = true;
			_abortRequested = false;
		}
		try
		{
			ReportProgress(BackupConfigActionStatus.Validating, 0);
			if (!_backupConfigManagerExternalCommunication.DirectoryExists(backupPath))
			{
				return BackupConfigResultData.CreateFailedResult(BackupConfigError.MissingBackupFolder, "Backup folder doesn't exists", new string[1] { backupPath }, null);
			}
			lock (this)
			{
				if (_abortRequested)
				{
					return BackupConfigResultData.CreateAbortedResult();
				}
			}
			_backupConfigManagerExternalCommunication.DeleteFiles(backupPath);
			foreach (IConfigBackup backupComponent in _backupComponents)
			{
				backupConfigSettings.ComponentSettings.Add(backupComponent.ComponentName, new BackupConfigComponentSettings(backupComponent.ComponentName, backupComponent.ConfigData()));
			}
			_backupConfigManagerExternalCommunication.WriteConfigToFile(backupConfigSettings, Path.Combine(backupPath, BackupResources.BackupSettingsFileName));
			list.Add(BackupResources.BackupSettingsFileName);
			ReportProgress(20);
			_componentProgress = new ComponentListProgress(_backupComponents.Count(), 20);
			foreach (IConfigBackup backupComponent2 in _backupComponents)
			{
				BackupValidationResult backupValidationResult = backupComponent2.ValidateForBackup(ReportComponentProgress, AbortRequested);
				lock (this)
				{
					if (_abortRequested)
					{
						return BackupConfigResultData.CreateAbortedResult();
					}
				}
				if (!backupValidationResult.Success)
				{
					return BackupConfigResultData.CreateFailedResult(backupValidationResult.ValidationError, backupValidationResult.EnglishErrorDescription, backupValidationResult.ErrorParameters, null);
				}
				_componentProgress.NoofComponentsDone++;
			}
			ReportProgress(BackupConfigActionStatus.CreatingBackup, 40);
			_componentProgress = new ComponentListProgress(_backupComponents.Count(), 40);
			foreach (IConfigBackup backupComponent3 in _backupComponents)
			{
				BackupComponentResult backupComponentResult = backupComponent3.Backup(backupPath, ReportComponentProgress, AbortRequested);
				lock (this)
				{
					if (_abortRequested)
					{
						return BackupConfigResultData.CreateAbortedResult();
					}
				}
				if (!backupComponentResult.Success)
				{
					return BackupConfigResultData.CreateFailedResult(backupComponentResult.ComponentError, backupComponentResult.EnglishErrorDescription, backupComponentResult.ErrorParameters, backupComponentResult.Exception);
				}
				list.AddRange(backupComponentResult.FilesHandled);
				_componentProgress.NoofComponentsDone++;
			}
			ReportProgress(BackupConfigActionStatus.CompressingBackupFiles, 80);
			_componentProgress = new ComponentListProgress(1, 20);
			if (list.Count > 0)
			{
				string text = _backupConfigManagerExternalCommunication.CompressFiles(backupPath, list, ReportProgress, AbortRequested);
				_backupConfigManagerExternalCommunication.DeleteFiles(backupPath, list);
				list.Clear();
				if (_abortRequested)
				{
					string path = Path.Combine(backupPath, text);
					if (File.Exists(path))
					{
						File.Delete(path);
					}
					return BackupConfigResultData.CreateAbortedResult();
				}
				list.Add(text);
				ReportProgress(100);
				return BackupConfigResultData.CreateSuccessResult(list);
			}
			return BackupConfigResultData.CreateFailedResult(BackupConfigError.NoBackupFilesWereMade);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (IOException ex2)
		{
			return BackupConfigResultData.CreateFailedResult(BackupConfigError.FileIOError, ex2.Message, null, ex2);
		}
		catch (Exception serverException)
		{
			return BackupConfigResultData.CreateFailedResult(BackupConfigError.ExceptionOccurred, "Unexpected exception occurred during backup command.", null, serverException);
		}
		finally
		{
			lock (this)
			{
				_backupIsActive = false;
			}
		}
	}

	public bool AbortBackup()
	{
		lock (this)
		{
			if (_backupIsActive)
			{
				_abortRequested = true;
				return true;
			}
			return false;
		}
	}

	private bool AbortRequested()
	{
		lock (this)
		{
			return _abortRequested;
		}
	}

	private void ReportComponentProgress(string componentName, int percentDone)
	{
		int currentActionProgress = _currentProgress + _componentProgress.NoofComponentsDone * _componentProgress.PercentPerComponent + Convert.ToInt32(_componentProgress.PercentPerComponent * percentDone / 100);
		BackupProgressEventArgs e = new BackupProgressEventArgs(_currentActionStatus, componentName, currentActionProgress);
		OnBackupProgress(e);
	}

	private void ReportProgress(int currentProgress)
	{
		_currentProgress = currentProgress;
		BackupProgressEventArgs e = new BackupProgressEventArgs(_currentActionStatus, null, _currentProgress);
		OnBackupProgress(e);
	}

	private void ReportProgress(BackupConfigActionStatus currentActionStatus, int currentProgress)
	{
		_currentActionStatus = currentActionStatus;
		_currentProgress = currentProgress;
		BackupProgressEventArgs e = new BackupProgressEventArgs(_currentActionStatus, null, _currentProgress);
		OnBackupProgress(e);
	}

	private void OnBackupProgress(BackupProgressEventArgs e)
	{
		EventHandler<BackupProgressEventArgs> eventHandler = this.BackupProgress;
		if (eventHandler != null)
		{
			try
			{
				eventHandler(this, e);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				LoggerExtensions.LogError(Logger, "Exception occurred during the BackupProgress event: {ExceptionMessage}", new object[1] { ex2.Message });
				throw;
			}
		}
	}
}

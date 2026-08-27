using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using VideoOS.Installer.Core.Cab;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Prerequisites.Properties;

namespace VideoOS.Installer.Prerequisites.PrepareTasks;

public class UnpackTask : InstallerTask
{
	protected CabHandler _cabHandler;

	protected string _targetDir;

	public override bool RunInHardwareManufacturerMode => true;

	public override bool RunInFinalizeMode => false;

	public UnpackTask()
	{
		_cabHandler = new CabHandler();
	}

	public UnpackTask(string targetDir)
	{
		_cabHandler = new CabHandler();
		base.MaximumProgress = 0;
		_targetDir = targetDir;
	}

	protected override void DoRun()
	{
		string text = "media.cab";
		base.Message = string.Format(Resources.Message_Unpacking);
		_cabHandler.SetProgressCallbacks(GetProgress, GetMessage);
		base.MaximumProgress = 1;
		NotifyObservers();
		base.MaximumProgress = 0;
		try
		{
			string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			LogManager.Write(string.Format("{0} {1}", "Unpacking...", text + "into " + directoryName));
			_cabHandler.ExtractAll(Path.Combine(directoryName, text), _targetDir);
		}
		catch (CabException ex)
		{
			LogManager.Write(ex.TypeAndMessage(), ex.StackTrace, TraceEventType.Error);
		}
		base.MaximumProgress = 0;
		Status = TaskStatus.Completed;
		_result = TaskResult.Success;
		NotifyObservers();
	}

	protected override void PreRun()
	{
		base.CurrentProgress = 0;
		base.MaximumProgress = 0;
		NotifyObservers();
	}

	protected void GetMessage(string message)
	{
		base.Message = string.Format(Resources.Message_Unpacking, message);
		NotifyObservers();
	}

	protected void GetProgress(int fileCount, int maximum)
	{
		base.CurrentProgress = fileCount;
		base.MaximumProgress = maximum;
		NotifyObservers();
	}

	protected override void PostRun()
	{
		try
		{
			File.Delete(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "media.cab"));
		}
		catch (Exception)
		{
		}
		Status = TaskStatus.NotStarted;
	}
}

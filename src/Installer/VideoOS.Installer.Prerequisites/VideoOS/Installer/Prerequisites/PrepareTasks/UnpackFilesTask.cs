using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using VideoOS.Installer.Core.Cab;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Prerequisites.Properties;

namespace VideoOS.Installer.Prerequisites.PrepareTasks;

public class UnpackFilesTask : UnpackTask
{
	private List<string> _filesToExtract;

	public UnpackFilesTask(string targetDir, List<string> files)
	{
		_cabHandler = new CabHandler();
		base.MaximumProgress = 0;
		_targetDir = targetDir;
		_filesToExtract = files;
	}

	protected override void DoRun()
	{
		string text = "media.cab";
		base.Message = string.Format(Resources.Message_Unpacking);
		_cabHandler.SetProgressCallbacks(base.GetProgress, base.GetMessage);
		base.MaximumProgress = 1;
		NotifyObservers();
		base.MaximumProgress = 0;
		try
		{
			string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			LogManager.Write(string.Format("{0} {1}", "Unpacking...", text + " into " + directoryName));
			_cabHandler.ExtractAll(text, _targetDir);
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

	protected override void PostRun()
	{
	}
}

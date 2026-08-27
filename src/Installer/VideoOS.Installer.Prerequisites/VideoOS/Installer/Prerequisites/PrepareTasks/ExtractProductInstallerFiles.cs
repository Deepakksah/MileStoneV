using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using VideoOS.Installer.Common.Exceptions;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Prerequisites.Properties;

namespace VideoOS.Installer.Prerequisites.PrepareTasks;

public class ExtractProductInstallerFiles : InstallerTask
{
	private class ProgressProxy : MarshalByRefObject, IProgress<int>
	{
		private readonly Action<int> _reportAction;

		public ProgressProxy(Action<int> reportAction)
		{
			_reportAction = reportAction;
		}

		public void Report(int value)
		{
			_reportAction(value);
		}
	}

	private class MethodInvoker : MarshalByRefObject
	{
		public string InvokeMethod(string assemblyPath, IEnumerable<string> filesToExtract, string targetDir, IProgress<int> progress)
		{
			return (string)((Assembly.LoadFrom(assemblyPath).GetType("VideoOS.Installer.Setup.Extract") ?? throw new ArgumentException("Could not find type 'VideoOS.Installer.Setup.Extract'.")).GetMethod("ExtractEmbeddedFiles", BindingFlags.Static | BindingFlags.Public) ?? throw new ArgumentException("Could not find method 'ExtractEmbeddedFiles' in class 'VideoOS.Installer.Setup.Extract'.")).Invoke(null, new object[3] { filesToExtract, targetDir, progress });
		}
	}

	private readonly string _installerFileFullPath;

	private readonly IEnumerable<string> _filesToExtract;

	public override bool RunInHardwareManufacturerMode => true;

	public override bool RunInFinalizeMode => false;

	public bool IsSupportedInstaller
	{
		get
		{
			try
			{
				AssemblyName.GetAssemblyName(_installerFileFullPath);
				return true;
			}
			catch (BadImageFormatException)
			{
				return false;
			}
		}
	}

	public ExtractProductInstallerFiles(string targetDir, string displayName, string installerFileFullPath, IEnumerable<string> filesToExtract)
	{
		base.TargetDir = targetDir;
		base.DisplayName = displayName;
		_installerFileFullPath = installerFileFullPath;
		_filesToExtract = filesToExtract;
	}

	protected override void DoRun()
	{
		IProgress<int> progress = new Progress<int>(delegate(int pct)
		{
			base.CurrentProgress = pct;
			NotifyObservers();
		});
		base.Message = Resources.Message_Unpacking;
		base.MaximumProgress = 100;
		progress.Report(0);
		Extract(progress);
		Status = TaskStatus.Completed;
		_result = TaskResult.Success;
		progress.Report(100);
		NotifyObservers();
	}

	protected override void PostRun()
	{
	}

	protected override void PreRun()
	{
		base.CurrentProgress = 0;
		NotifyObservers();
	}

	private void Extract(IProgress<int> progress)
	{
		string text = "";
		try
		{
			AppDomain appDomain = AppDomain.CreateDomain("Extract installer domain", null, new AppDomainSetup
			{
				ApplicationBase = AppDomain.CurrentDomain.BaseDirectory
			});
			try
			{
				text = ((MethodInvoker)appDomain.CreateInstanceAndUnwrap(typeof(MethodInvoker).Assembly.FullName, typeof(MethodInvoker).FullName)).InvokeMethod(progress: new ProgressProxy(delegate(int x)
				{
					progress.Report(x);
				}), assemblyPath: _installerFileFullPath, filesToExtract: _filesToExtract, targetDir: base.TargetDir);
			}
			finally
			{
				AppDomain.Unload(appDomain);
			}
		}
		catch (Exception ex)
		{
			LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("Failed to extract {0} from {1}.\r\n{2}", string.Join(", ", _filesToExtract), _installerFileFullPath, ex)), TraceEventType.Error);
			throw new FileExtractException(ex.Message, () => Resources.ErrorExtractingInstallerFiles, ex);
		}
		if (!string.IsNullOrEmpty(text))
		{
			LogManager.Write(FormattableString.Invariant(FormattableStringFactory.Create("Failed to extract {0} from {1}. {2}", string.Join(", ", _filesToExtract), _installerFileFullPath, text)), TraceEventType.Error);
			throw new FileExtractException(text, () => Resources.ErrorExtractingInstallerFiles, null);
		}
	}
}

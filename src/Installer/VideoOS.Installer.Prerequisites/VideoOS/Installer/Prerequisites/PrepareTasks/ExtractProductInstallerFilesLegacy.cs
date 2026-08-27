using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VideoOS.Installer.Common.Exceptions;
using VideoOS.Installer.Core.Cab;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Prerequisites.Properties;

namespace VideoOS.Installer.Prerequisites.PrepareTasks;

public class ExtractProductInstallerFilesLegacy : InstallerTask
{
	private readonly string _productInstallerFile;

	private readonly CabHandler _cabHandler;

	private readonly List<string> _fileNamesToExtract;

	public override bool RunInHardwareManufacturerMode => true;

	public override bool RunInFinalizeMode => false;

	public ExtractProductInstallerFilesLegacy(string targetDir, string productInstallerFile, List<string> fileNames, string displayName)
	{
		_cabHandler = new CabHandler();
		base.TargetDir = targetDir;
		base.DisplayName = displayName;
		_fileNamesToExtract = fileNames;
		_productInstallerFile = productInstallerFile;
	}

	protected override void DoRun()
	{
		base.Message = string.Format(Resources.Message_Unpacking);
		base.MaximumProgress = 1;
		NotifyObservers();
		base.MaximumProgress = 0;
		try
		{
			string temporaryDirectory = GetTemporaryDirectory();
			LogManager.Write($"Extracting product installer {_productInstallerFile} into {base.TargetDir}");
			ExtractFiles(temporaryDirectory);
			foreach (string item in _fileNamesToExtract)
			{
				string path = Path.Combine(base.TargetDir, Path.GetDirectoryName(item));
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				File.Copy(Path.Combine(temporaryDirectory, item), Path.Combine(base.TargetDir, item), overwrite: true);
			}
			try
			{
				SetNormalAttributes(temporaryDirectory);
				Directory.Delete(temporaryDirectory, recursive: true);
			}
			catch (Exception ex)
			{
				LogManager.Write(ex.TypeAndMessage(), TraceEventType.Warning);
			}
		}
		catch (ArithmeticException innerException)
		{
			base.MaximumProgress = 1;
			throw new FileExtractException("Error extracting product installer files. See inner exception.", () => Resources.ErrorExtractingInstallerFiles, innerException);
		}
		catch (Exception ex2)
		{
			LogManager.Write($"CabHandler warning: {ex2.TypeAndMessage()}", TraceEventType.Information);
		}
		base.MaximumProgress = 1;
		_result = TaskResult.Success;
		Status = TaskStatus.Completed;
	}

	private void SetNormalAttributes(string currentDir)
	{
		if (Directory.Exists(currentDir))
		{
			string[] directories = Directory.GetDirectories(currentDir);
			foreach (string normalAttributes in directories)
			{
				SetNormalAttributes(normalAttributes);
			}
			directories = Directory.GetFiles(currentDir);
			for (int i = 0; i < directories.Length; i++)
			{
				File.SetAttributes(directories[i], FileAttributes.Normal);
			}
		}
	}

	private string GetTemporaryDirectory()
	{
		string text = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
		Directory.CreateDirectory(text);
		return text;
	}

	private void ExtractFiles(string tempDir)
	{
		try
		{
			_cabHandler.ExtractInstallerFiles(Path.Combine(base.TargetDir, _productInstallerFile), tempDir);
		}
		catch (CabException)
		{
			LogManager.Write($"Extracting of the product installer files failed for {base.DisplayName} package.");
		}
	}

	protected override void PostRun()
	{
	}

	protected override void PreRun()
	{
		base.CurrentProgress = 0;
		base.MaximumProgress = 0;
		NotifyObservers();
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Utilities.Database;
using VideoOS.Vms.BackupRestore;

namespace VideoOS.Server;

public sealed class BackupConfigManagerExternalCommunication : IBackupConfigManagerExternalCommunication
{
	private ISqlConnectionManager _sqlConnectionManager;

	IList<RegisteredService> IBackupConfigManagerExternalCommunication.GetRegisteredServices
	{
		get
		{
			CorporateSqlData corporateSqlData = new CorporateSqlData(_sqlConnectionManager);
			return corporateSqlData.GetRegisteredServices();
		}
	}

	ILogger IBackupConfigManagerExternalCommunication.Logger => Server.Instance.Logger;

	public BackupConfigManagerExternalCommunication(ISqlConnectionManager sqlConnectionManager)
	{
		_sqlConnectionManager = sqlConnectionManager;
	}

	bool IBackupConfigManagerExternalCommunication.DirectoryExists(string directoryPath)
	{
		return Directory.Exists(directoryPath);
	}

	void IBackupConfigManagerExternalCommunication.DeleteFiles(string directoryPath)
	{
		DirectoryUtilities.DeleteFiles(directoryPath);
	}

	void IBackupConfigManagerExternalCommunication.DeleteFiles(string directoryPath, IList<string> fileNames)
	{
		if (fileNames == null)
		{
			throw new ArgumentNullException("fileNames");
		}
		foreach (string fileName in fileNames)
		{
			File.Delete(Path.Combine(directoryPath, fileName));
		}
	}

	void IBackupConfigManagerExternalCommunication.WriteConfigToFile(BackupConfigSettings backupConfigSettings, string fileName)
	{
		BackupConfigSettings.WriteToFile(backupConfigSettings, fileName);
	}

	string IBackupConfigManagerExternalCommunication.CompressFiles(string destinationPath, IList<string> sourceFiles, ReportProgress reportStatus, AbortRequested abortRequested)
	{
		return CompressBackupFiles(destinationPath, sourceFiles, reportStatus, abortRequested);
	}

	private static string CompressBackupFiles(string folderName, IList<string> fileNames, ReportProgress reportStatus, AbortRequested abortRequested)
	{
		try
		{
			string text = "Configuration_Backup_" + DateTime.UtcNow.ToString("yyyyMMdd.HHmmss", CultureInfo.InvariantCulture) + ".cnf";
			ZipArchive val = ZipFile.Open(Path.Combine(folderName, text), (ZipArchiveMode)1);
			try
			{
				foreach (string fileName in fileNames)
				{
					AddFileToZip(val, Path.Combine(folderName, fileName), reportStatus, abortRequested);
					if (abortRequested())
					{
						break;
					}
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			return text;
		}
		catch (IOException ex)
		{
			throw new ServerException(ex.Message, ex);
		}
	}

	private static bool AddFileToZip(ZipArchive zs, string fullPath, ReportProgress reportStatus, AbortRequested abortRequested)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
		try
		{
			using FileStream fileStream = File.OpenRead(fullPath);
			string fileName = Path.GetFileName(fullPath);
			ZipArchiveEntry val = zs.CreateEntry(fileName);
			byte[] array = new byte[65536];
			if (fileStream.Length > 65536)
			{
				reportStatus(0);
			}
			long num = 0L;
			using (Stream stream = val.Open())
			{
				for (int num2 = fileStream.Read(array, 0, array.Length); num2 > 0; num2 = fileStream.Read(array, 0, array.Length))
				{
					if (fileStream.Length > 65536)
					{
						reportStatus(Convert.ToInt32(fileStream.Position * 100 / fileStream.Length));
					}
					stream.Write(array, 0, num2);
					num += num2;
					if (abortRequested())
					{
						break;
					}
				}
			}
			if (!abortRequested() && num != fileStream.Length)
			{
				throw new IOException(string.Format(CultureInfo.CurrentCulture, "Mismatch of read bytes and filesize of backup file {0} - totalBytesRead {1}, file size {2}", fullPath, num, fileStream.Length));
			}
			fileStream.Close();
			return true;
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}
}

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using VideoOS.Installer.CoSQLExprAdapter.Properties;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;

namespace VideoOS.Installer.CoSQLExprAdapter;

public class SqlServerInstaller
{
	private string _AssemblyPath;

	public string NTAuthorityName { get; set; }

	public string NTAuthoritySid { get; set; }

	private string SqlNativeClientMsiName => Path.Combine(_AssemblyPath, "SQLClientX64\\sqlncli.msi");

	public SqlServerInstaller()
	{
		string location = Assembly.GetExecutingAssembly().Location;
		_AssemblyPath = Path.GetDirectoryName(location);
	}

	public static bool Is64BitOS()
	{
		bool Wow64Process = false;
		try
		{
			NativeMethods.IsWow64Process(Process.GetCurrentProcess().Handle, out Wow64Process);
		}
		catch
		{
		}
		if (Wow64Process)
		{
			return true;
		}
		return IntPtr.Size == 8;
	}

	private static int RunProcess(string fileName, string arguments)
	{
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "RunProcess: '{0}' {1}", fileName, arguments), TraceEventType.Information);
		using Process process = new Process();
		process.StartInfo.FileName = fileName;
		process.StartInfo.Arguments = arguments;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.CreateNoWindow = true;
		process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		process.Start();
		process.WaitForExit();
		return process.ExitCode;
	}

	public int Unpack(string fileName, string destinationFolder)
	{
		try
		{
			string arguments = string.Format(CultureInfo.InvariantCulture, "/X:\"{0}\" /INDICATEPROGRESS=\"yes\" /Q", destinationFolder);
			return RunProcess(fileName, arguments);
		}
		catch (Exception ex)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, Resources.SQL_InstallException, ex.Message), TraceEventType.Error);
		}
		return -1;
	}

	public int Install(string configFileName, string destinationFolder)
	{
		try
		{
			string text = Path.Combine(destinationFolder, "sqlsetup.ini");
			File.Copy(configFileName, text, overwrite: true);
			string text2 = Path.Combine(destinationFolder, "patch");
			Directory.CreateDirectory(text2);
			string path = Path.Combine(Path.GetDirectoryName(configFileName), "patch");
			if (Directory.Exists(path))
			{
				string[] files = Directory.GetFiles(path, "*.exe");
				if (files.Length != 0)
				{
					string text3 = files[0];
					File.Copy(text3, Path.Combine(text2, Path.GetFileName(text3)), overwrite: true);
				}
			}
			if (SubstituteWindowsUser(text))
			{
				string fileName = Path.Combine(destinationFolder, "setup.exe");
				string arguments = string.Format(CultureInfo.InvariantCulture, "/CONFIGURATIONFILE=\"{0}\"", text);
				return RunProcess(fileName, arguments);
			}
			try
			{
				File.Delete(text);
			}
			catch
			{
			}
		}
		catch (Exception ex)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, Resources.SQL_InstallException, ex.Message), TraceEventType.Error);
		}
		return -1;
	}

	public void InstallNativeClient()
	{
		try
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo("msiexec.exe");
			processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			processStartInfo.UseShellExecute = false;
			processStartInfo.CreateNoWindow = true;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.Arguments = string.Format(CultureInfo.InvariantCulture, "/i \"" + SqlNativeClientMsiName + "\" /quiet IACCEPTSQLNCLILICENSETERMS=YES /L*v {0}", Path.Combine(Environment.GetEnvironmentVariable("windir"), "Temp\\SqlNativeClient.log"));
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "SQL Server Native Client Arguments: {0}", processStartInfo.Arguments));
			Process process = Process.Start(processStartInfo);
			if (process != null)
			{
				process.WaitForExit();
				if (process.ExitCode != 0)
				{
					LogManager.Write(string.Format(CultureInfo.CurrentCulture, "Installing SQL Server Native Client returend Exit code: {0} output = {1}", process.ExitCode, process.StandardOutput.ReadToEnd()), TraceEventType.Error);
					throw new InstallationAdapterException(string.Format(CultureInfo.CurrentCulture, "Installing SQL Server Native Client returned Exit code: {0} output = {1}", process.ExitCode, process.StandardOutput.ReadToEnd()));
				}
				LogManager.Write("SQL Server Native Client installation done!");
			}
		}
		catch (Exception)
		{
			LogManager.Write("SQL Server Native Client installation failed.");
			throw;
		}
	}

	private bool SubstituteWindowsUser(string iniPath)
	{
		try
		{
			string standardWindowsUserName = GetStandardWindowsUserName(NTAuthoritySid);
			if (standardWindowsUserName != NTAuthorityName)
			{
				standardWindowsUserName = string.Format(CultureInfo.InvariantCulture, "\"{0}\"", standardWindowsUserName);
				FileInfo fileInfo = new FileInfo(iniPath);
				if ((fileInfo.Attributes & FileAttributes.ReadOnly) > (FileAttributes)0)
				{
					fileInfo.Attributes--;
				}
				string[] array = File.ReadAllLines(iniPath);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].StartsWith("SQLSVCACCOUNT", StringComparison.OrdinalIgnoreCase))
					{
						array[i] = "SQLSVCACCOUNT=" + standardWindowsUserName;
					}
					if (array[i].StartsWith("ISSVCACCOUNT", StringComparison.OrdinalIgnoreCase))
					{
						array[i] = "ISSVCACCOUNT=" + standardWindowsUserName;
					}
				}
				File.WriteAllLines(iniPath, array, Encoding.Unicode);
			}
		}
		catch (Exception ex)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, Resources.SQL_InstallException, ex.Message), TraceEventType.Error);
			return false;
		}
		return true;
	}

	private static string GetStandardWindowsUserName(string sid)
	{
		IntPtr sid2 = IntPtr.Zero;
		int cbName = 0;
		int cbReferencedDomainName = 0;
		StringBuilder referencedDomainName = new StringBuilder();
		StringBuilder name = new StringBuilder();
		if (!NativeMethods.ConvertStringSidToSidW(sid, ref sid2))
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			Marshal.FreeHGlobal(sid2);
			throw new Exception(new Win32Exception(lastWin32Error).Message);
		}
		NativeMethods.LookupAccountSidW(null, sid2, name, ref cbName, referencedDomainName, ref cbReferencedDomainName, out var use);
		referencedDomainName = new StringBuilder(cbReferencedDomainName);
		name = new StringBuilder(cbName);
		if (!NativeMethods.LookupAccountSidW(null, sid2, name, ref cbName, referencedDomainName, ref cbReferencedDomainName, out use))
		{
			int lastWin32Error2 = Marshal.GetLastWin32Error();
			Marshal.FreeHGlobal(sid2);
			throw new Exception(new Win32Exception(lastWin32Error2).Message);
		}
		Marshal.FreeHGlobal(sid2);
		return referencedDomainName.ToString() + "\\" + name.ToString();
	}

	public bool IsSqlServerNativeClientInstalled()
	{
		int num = 1024;
		StringBuilder stringBuilder = new StringBuilder(num);
		int num2 = 1024;
		StringBuilder pProductId = new StringBuilder(num2);
		ClMsiHandler.Instance.FindProductAndUpgradeCode(SqlNativeClientMsiName, pProductId, num2, stringBuilder, num);
		if (ClMsiHandler.Instance.EnumRelatedProducts(stringBuilder, 0u).Length != 0)
		{
			LogManager.Write("SQL Server Native Client was detected on the machine.");
			return true;
		}
		return false;
	}
}

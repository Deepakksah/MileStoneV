using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.SQLExpressAdapter;

public class SqlServerInstaller
{
	private string _AssemblyPath;

	public StringBuilder StdOut;

	public string NtAuthorityName { get; set; }

	public string NtAuthoritySid { get; set; }

	public event ProgressDelegate OnProgress;

	public SqlServerInstaller()
	{
		string location = Assembly.GetExecutingAssembly().Location;
		_AssemblyPath = Path.GetDirectoryName(location);
	}

	public bool Is64BitOS()
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

	private void onDataReceived(object caller, DataReceivedEventArgs outLine)
	{
		if (!string.IsNullOrEmpty(outLine.Data) && this.OnProgress != null)
		{
			this.OnProgress(outLine.Data);
		}
	}

	private int RunProcess(string fileName, string arguments, out StringBuilder stdOut)
	{
		LogManager.Write($"RunProcess: '{fileName}' {arguments}", TraceEventType.Information);
		new ProcessStartInfo(fileName, arguments);
		Process process = new Process();
		process.StartInfo.FileName = fileName;
		process.StartInfo.Arguments = arguments;
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.CreateNoWindow = true;
		process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		process.OutputDataReceived += onDataReceived;
		stdOut = new StringBuilder();
		StdOut = stdOut;
		process.Start();
		process.BeginOutputReadLine();
		process.WaitForExit();
		return process.ExitCode;
	}

	public int Unpack(string fileName, string destinationFolder)
	{
		try
		{
			StringBuilder stdOut = null;
			string arguments = $"/X:\"{destinationFolder}\" /INDICATEPROGRESS=\"yes\" /Q";
			return RunProcess(fileName, arguments, out stdOut);
		}
		catch (Exception ex)
		{
			LogManager.Write($"SQL Express installation exception '{ex.TypeAndMessage()}'", TraceEventType.Error);
		}
		return -1;
	}

	public int Install(string configFileName, string destinationFolder)
	{
		try
		{
			string text = Path.Combine(destinationFolder, "sqlsetup.ini");
			File.Copy(configFileName, text, overwrite: true);
			if (SubstituteWindowsUser(text))
			{
				string fileName = Path.Combine(destinationFolder, "setup.exe");
				string arguments = $"/CONFIGURATIONFILE=\"{text}\"";
				StringBuilder stdOut = null;
				return RunProcess(fileName, arguments, out stdOut);
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
			LogManager.Write($"SQL Express installation exception '{ex.TypeAndMessage()}'", TraceEventType.Error);
		}
		return -1;
	}

	private bool SubstituteWindowsUser(string iniPath)
	{
		try
		{
			string standardWindowsUserName = GetStandardWindowsUserName(NtAuthoritySid);
			if (standardWindowsUserName != NtAuthorityName)
			{
				standardWindowsUserName = $"\"{standardWindowsUserName}\"";
				FileInfo fileInfo = new FileInfo(iniPath);
				if ((fileInfo.Attributes & FileAttributes.ReadOnly) > (FileAttributes)0)
				{
					fileInfo.Attributes--;
				}
				string[] array = File.ReadAllLines(iniPath);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].StartsWith("SQLSVCACCOUNT"))
					{
						array[i] = "SQLSVCACCOUNT=" + standardWindowsUserName;
					}
					if (array[i].StartsWith("ISSVCACCOUNT"))
					{
						array[i] = "ISSVCACCOUNT=" + standardWindowsUserName;
					}
				}
				File.WriteAllLines(iniPath, array, Encoding.Unicode);
			}
		}
		catch (Exception ex)
		{
			LogManager.Write($"SQL Express installation exception '{ex.TypeAndMessage()}'", TraceEventType.Error);
			return false;
		}
		return true;
	}

	private static string GetStandardWindowsUserSid(string name)
	{
		IntPtr zero = IntPtr.Zero;
		int cbSid = 0;
		int cbReferencedDomainName = 0;
		StringBuilder referencedDomainName = new StringBuilder();
		int num = 0;
		string pStringSid = "";
		NativeMethods.LookupAccountNameW(null, name, zero, ref cbSid, referencedDomainName, ref cbReferencedDomainName, out var use);
		num = Marshal.GetLastWin32Error();
		if (num != 122)
		{
			throw new Exception(new Win32Exception(num).Message);
		}
		referencedDomainName = new StringBuilder(cbReferencedDomainName);
		zero = Marshal.AllocHGlobal(cbSid);
		if (!NativeMethods.LookupAccountNameW(null, name, zero, ref cbSid, referencedDomainName, ref cbReferencedDomainName, out use))
		{
			num = Marshal.GetLastWin32Error();
			Marshal.FreeHGlobal(zero);
			throw new Exception(new Win32Exception(num).Message);
		}
		if (!NativeMethods.ConvertSidToStringSidW(zero, ref pStringSid))
		{
			num = Marshal.GetLastWin32Error();
			Marshal.FreeHGlobal(zero);
			throw new Exception(new Win32Exception(num).Message);
		}
		Marshal.FreeHGlobal(zero);
		return pStringSid;
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
}

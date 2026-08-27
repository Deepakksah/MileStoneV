using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.Common;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class CleanUpWrapper : MarshalByRefObject, IWrapperCleanup, IWrapperLogger
{
	private StringBuilder _resultBuffer = new StringBuilder();

	private string _certificateRegistrationCommonFile;

	private string _serverConfiguratorCertificateRegistrationFile;

	private const string _delimiter = "#2?";

	private const string _logSubDelimiter = "#1?";

	protected object InnerImplementation { get; private set; }

	public string InterfaceName { get; set; }

	public string InitializeName { get; set; }

	public string CleanupName { get; set; }

	public string StatusCodeName { get; set; }

	public string LogFilePath => null;

	public string CleanUp()
	{
		object obj = null;
		try
		{
			obj = InnerImplementation.GetType().GetInterface(InterfaceName).GetMethod(CleanupName)
				.Invoke(InnerImplementation, null);
		}
		catch
		{
			return WrapStatusCode("UnspecifiedErrorEncryption");
		}
		return WrapStatusCode(obj);
	}

	public string Initialize(IWrapperLogger logger)
	{
		object obj = null;
		try
		{
			MethodInfo method = InnerImplementation.GetType().GetInterface(InterfaceName).GetMethod(InitializeName);
			try
			{
				obj = method.Invoke(InnerImplementation, new object[1]
				{
					new RegistrationLoggerWrapper(this)
				});
			}
			catch
			{
				obj = method.Invoke(InnerImplementation, new object[1]);
				LogInfo(FormattableString.Invariant($"Cleanup of {InnerImplementation.GetType()} executed without logging"));
			}
		}
		catch
		{
			LogInfo(FormattableString.Invariant($"Failed to initialize {InnerImplementation.GetType()}"));
			return WrapStatusCode("UnspecifiedErrorEncryption");
		}
		return WrapStatus(obj);
	}

	private string WrapStatus(object status)
	{
		try
		{
			object value = status.GetType().GetProperty(StatusCodeName).GetValue(status);
			return WrapStatusCode(value);
		}
		catch
		{
			return WrapStatusCode("UnspecifiedErrorEncryption");
		}
	}

	private string WrapStatusCode(object statusCode)
	{
		_resultBuffer.Append(statusCode);
		string result = _resultBuffer.ToString();
		_resultBuffer.Clear();
		return result;
	}

	public void CreateInnerImplementation(string assemblyQualifiedName, string fileLocation)
	{
		HackToCompensateForMissingFilesIn2020R2Installations(assemblyQualifiedName, fileLocation);
		InnerImplementation = VideoOS.ServerConfigurator.Common.Activator.Instance.CreateInstance<object>(assemblyQualifiedName, fileLocation, new LoggerWrapperToLogger(this), useInstanceCache: false, out var _, null, createLocal: true, DefaultCompatibilityProvider.Instance);
	}

	private void HackToCompensateForMissingFilesIn2020R2Installations(string assemblyQualifiedName, string fileLocation)
	{
		if (IsFile2020R2(GetFilePath(assemblyQualifiedName, fileLocation)))
		{
			string fullPath = Path.GetFullPath(Path.Combine(fileLocation, "..\\"));
			_certificateRegistrationCommonFile = Find(fullPath, "VideoOS.CertificateRegistration.Common.dll");
			_serverConfiguratorCertificateRegistrationFile = Find(fullPath, "VideoOS.ServerConfigurator.CertificateRegistration.dll");
			if (_certificateRegistrationCommonFile != null && _serverConfiguratorCertificateRegistrationFile != null)
			{
				AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;
				return;
			}
			LogWarning(FormattableString.Invariant(FormattableStringFactory.Create("Cleanup of {0} not possible. The file {1} was not found in an 2020R2 version", assemblyQualifiedName, (_certificateRegistrationCommonFile == null) ? "VideoOS.CertificateRegistration.Common.dll" : "VideoOS.ServerConfigurator.CertificateRegistration.dll")));
		}
	}

	private Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
	{
		try
		{
			string name = new AssemblyName(args?.Name).Name;
			if (name != null)
			{
				if (name.Equals("VideoOS.ServerConfigurator.CertificateRegistration", StringComparison.OrdinalIgnoreCase))
				{
					return Assembly.LoadFile(_serverConfiguratorCertificateRegistrationFile);
				}
				if (name.Equals("VideoOS.CertificateRegistration.Common", StringComparison.OrdinalIgnoreCase))
				{
					return Assembly.LoadFile(_certificateRegistrationCommonFile);
				}
			}
			return null;
		}
		catch (Exception)
		{
			return null;
		}
	}

	private static string Find(string toplevelPath, string fileToFind)
	{
		return Directory.EnumerateFiles(toplevelPath, fileToFind, SearchOption.AllDirectories).FirstOrDefault((string x) => IsFile2020R2(x));
	}

	private static bool IsFile2020R2(string filePath)
	{
		FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(filePath);
		if (versionInfo.FileMajorPart == 20)
		{
			return versionInfo.FileMinorPart == 2;
		}
		return false;
	}

	private static string GetFilePath(string assemblyQualifiedName, string fileLocation)
	{
		return Path.Combine(fileLocation, assemblyQualifiedName.Split(',').ElementAt(1).Trim() + ".dll");
	}

	public void LogDebug(string entry)
	{
		Log("LogDebug", entry);
	}

	public void LogError(string entry)
	{
		Log("LogError", entry);
	}

	public void LogInfo(string entry)
	{
		Log("LogInfo", entry);
	}

	public void LogWarning(string entry)
	{
		Log("LogWarning", entry);
	}

	private void Log(string type, string logEntry)
	{
		_resultBuffer.Append(type);
		_resultBuffer.Append("#1?");
		_resultBuffer.Append(logEntry);
		_resultBuffer.Append("#2?");
	}

	private static string WrapLogEntry(string type, string logEntry)
	{
		return FormattableString.Invariant(FormattableStringFactory.Create("{0}{1}{2}", type, "#2?", logEntry));
	}

	public static (StatusCode, string[] logEntries) UnwrapResult(string wrappedResult)
	{
		if (wrappedResult == null)
		{
			throw new ArgumentNullException("wrappedResult");
		}
		string[] array = wrappedResult.Split(new string[1] { "#2?" }, StringSplitOptions.None);
		StatusCode item = Unwrap(array.Last());
		Array.Resize(ref array, array.Length - 1);
		return (item, logEntries: array);
	}

	private static StatusCode Unwrap(string statusCodeString)
	{
		if (Enum.TryParse<StatusCode>(statusCodeString, out var result))
		{
			return result;
		}
		return StatusCode.UnspecifiedErrorEncryption;
	}

	public static void LogFromBuffer(ILogger logger, string[] logEntries)
	{
		if (logEntries == null)
		{
			throw new ArgumentNullException("logEntries");
		}
		for (int i = 0; i < logEntries.Length; i++)
		{
			var (text, entry) = UnwrapLogEntry(logEntries[i]);
			switch (text)
			{
			case "LogDebug":
				logger?.LogDebug(entry);
				break;
			case "LogInfo":
				logger?.LogInfo(entry);
				break;
			case "LogWarning":
				logger?.LogWarning(entry);
				break;
			case "LogError":
				logger?.LogError(entry);
				break;
			}
		}
	}

	private static (string logType, string LogString) UnwrapLogEntry(string logEntry)
	{
		string[] array = logEntry.Split(new string[1] { "#1?" }, StringSplitOptions.None);
		return (logType: array[0], LogString: array[1]);
	}
}

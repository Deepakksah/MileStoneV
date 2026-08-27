using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace VideoOS.Installer.Common.Dism;

public class DismSession : IDismSession
{
	private static class NativeMethods
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct DismFeature
		{
			[MarshalAs(UnmanagedType.LPWStr)]
			public string FeatureName;

			public PackageFeatureState State;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct DismFeatureInfo
		{
			[MarshalAs(UnmanagedType.LPWStr)]
			public string FeatureName;

			public PackageFeatureState FeatureState;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string DisplayName;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string Description;

			public RestartType RestartRequired;

			public IntPtr CustomProperty;

			public uint CustomPropertyCount;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
		public struct DismString
		{
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Value;
		}

		public const string DISM_ONLINE_IMAGE = "DISM_{53BFAE52-B167-4E2F-A258-0A37B57FF845}";

		public const int S_OK = 0;

		public const int SUCCESS_RELOAD_IMAGE_SESSION_REQUIRED = 1;

		public const int SUCCESS_REBOOT_REQUIRED = 3010;
	}

	public enum LogLevel
	{
		Errors,
		Warnings,
		Info
	}

	public enum PackageIdentifier
	{
		PackageNone,
		PackageName,
		PackagePath
	}

	private static bool _dismInitialized;

	private readonly ILogger _logger;

	private INativeDismWrapper _nativeMethods;

	private uint? _session;

	public event EventHandler<ProgressInfoEventArgs> Progress;

	public DismSession(ILogger logger)
		: this(new NativeDismWrapper(), logger)
	{
	}

	public DismSession(INativeDismWrapper injectedDismWrapper, ILogger logger)
	{
		_nativeMethods = injectedDismWrapper;
		_logger = logger;
	}

	public void Initialize(string logFile)
	{
		ILogger logger = _logger;
		if (logger != null)
		{
			LoggerExtensions.LogInformation(logger, "DismSession initialize", Array.Empty<object>());
		}
		if (_dismInitialized)
		{
			throw new InvalidOperationException("Dism is already initialized.");
		}
		try
		{
			int num = _nativeMethods.DismInitialize(LogLevel.Info, logFile, null);
			if (!Success(num))
			{
				ThrowDismApiExceptionWithLastError("DismInitialize", num);
			}
			_dismInitialized = true;
		}
		catch (DllNotFoundException innerException)
		{
			throw new NotSupportedException("Dism is unavailable", innerException);
		}
		ILogger logger2 = _logger;
		if (logger2 != null)
		{
			LoggerExtensions.LogInformation(logger2, "DismSession initialized", Array.Empty<object>());
		}
	}

	public DismStatus Shutdown()
	{
		ILogger logger = _logger;
		if (logger != null)
		{
			LoggerExtensions.LogInformation(logger, "DismSession Shutdown", Array.Empty<object>());
		}
		if (!_dismInitialized)
		{
			throw new InvalidOperationException("Dism is not initialized.");
		}
		int num = _nativeMethods.DismShutdown();
		if (num == 3010)
		{
			_dismInitialized = false;
			ILogger logger2 = _logger;
			if (logger2 != null)
			{
				LoggerExtensions.LogInformation(logger2, "DismSession Shut down", Array.Empty<object>());
			}
			return DismStatus.RebootRequired;
		}
		if (!Success(num))
		{
			throw new DismException(string.Format(CultureInfo.InvariantCulture, "Call to 'DismShutdown' failed. Result: {0}", num));
		}
		_dismInitialized = false;
		ILogger logger3 = _logger;
		if (logger3 != null)
		{
			LoggerExtensions.LogInformation(logger3, "DismSession Shut down", Array.Empty<object>());
		}
		return DismStatus.Success;
	}

	private void ProgressCallback(uint current, uint total, IntPtr userData)
	{
		this.Progress?.Invoke(this, new ProgressInfoEventArgs
		{
			Total = total,
			Current = current
		});
	}

	public void Open()
	{
		if (!_dismInitialized)
		{
			throw new InvalidOperationException("Dism not initialized.");
		}
		if (_session.HasValue)
		{
			throw new InvalidOperationException("The session is already open.");
		}
		uint session;
		int num = _nativeMethods.DismOpenSession("DISM_{53BFAE52-B167-4E2F-A258-0A37B57FF845}", null, null, out session);
		if (!Success(num))
		{
			ThrowDismApiExceptionWithLastError("DismOpenSession", num);
		}
		_session = session;
	}

	public DismStatus Close()
	{
		ILogger logger = _logger;
		if (logger != null)
		{
			LoggerExtensions.LogInformation(logger, "DismSession Close", Array.Empty<object>());
		}
		if (!_session.HasValue)
		{
			throw new InvalidOperationException("The session is not open.");
		}
		int num = _nativeMethods.DismCloseSession(_session.Value);
		if (num == 3010)
		{
			_session = null;
			ILogger logger2 = _logger;
			if (logger2 != null)
			{
				LoggerExtensions.LogInformation(logger2, "DismSession Closed", Array.Empty<object>());
			}
			return DismStatus.RebootRequired;
		}
		if (!Success(num))
		{
			ThrowDismApiExceptionWithLastError("DismCloseSession", num);
		}
		_session = null;
		ILogger logger3 = _logger;
		if (logger3 != null)
		{
			LoggerExtensions.LogInformation(logger3, "DismSession Closed", Array.Empty<object>());
		}
		return DismStatus.Success;
	}

	public IEnumerable<Feature> QueryFeatures()
	{
		if (!_session.HasValue)
		{
			throw new InvalidOperationException("The session is not open.");
		}
		IntPtr feature;
		uint count;
		int num = _nativeMethods.DismGetFeatures(_session.Value, null, PackageIdentifier.PackageNone, out feature, out count);
		if (!Success(num))
		{
			ThrowDismApiExceptionWithLastError("DismGetFeatures", num);
		}
		IntPtr ptr = feature;
		NativeMethods.DismFeature[] array = new NativeMethods.DismFeature[count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (NativeMethods.DismFeature)Marshal.PtrToStructure(ptr, typeof(NativeMethods.DismFeature));
			ptr += Marshal.SizeOf(typeof(NativeMethods.DismFeature));
		}
		_nativeMethods.DismDelete(feature);
		return array.Select((NativeMethods.DismFeature f) => new Feature(f.FeatureName, f.State)).ToList();
	}

	public FeatureInfo QueryFeatureInfo(string featureName)
	{
		if (!_session.HasValue)
		{
			throw new InvalidOperationException("The session is not open.");
		}
		if (featureName == null)
		{
			throw new ArgumentNullException("featureName");
		}
		IntPtr featureInfo;
		int num = _nativeMethods.DismGetFeatureInfo(_session.Value, featureName, null, PackageIdentifier.PackageNone, out featureInfo);
		if (!Success(num))
		{
			ThrowDismApiExceptionWithLastError("DismGetFeatureInfo", num);
		}
		NativeMethods.DismFeatureInfo dismFeatureInfo = (NativeMethods.DismFeatureInfo)Marshal.PtrToStructure(featureInfo, typeof(NativeMethods.DismFeatureInfo));
		_nativeMethods.DismDelete(featureInfo);
		return new FeatureInfo(dismFeatureInfo.FeatureName, dismFeatureInfo.FeatureState, dismFeatureInfo.DisplayName, dismFeatureInfo.Description, dismFeatureInfo.RestartRequired);
	}

	public DismStatus EnableFeature(string featureName, bool enableAll)
	{
		if (!_session.HasValue)
		{
			throw new InvalidOperationException("The session is not open.");
		}
		if (featureName == null)
		{
			throw new ArgumentNullException("featureName");
		}
		ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
		try
		{
			int num = _nativeMethods.DismEnableFeature(_session.Value, featureName, null, PackageIdentifier.PackageNone, limitAccess: true, null, 0u, enableAll, manualResetEvent.SafeWaitHandle, ProgressCallback, IntPtr.Zero);
			if (num == 3010)
			{
				return DismStatus.RebootRequired;
			}
			if (!Success(num))
			{
				ThrowDismApiExceptionWithLastError(string.Format(CultureInfo.InvariantCulture, "DismEnableFeature - Feature(s) = {0}", featureName), num);
			}
			return DismStatus.Success;
		}
		finally
		{
			manualResetEvent.Dispose();
		}
	}

	public DismStatus EnableFeatures(bool enableAll, bool bulk, params string[] featureNames)
	{
		if (featureNames == null)
		{
			throw new ArgumentNullException("featureNames");
		}
		if (bulk)
		{
			string featureName = string.Join(";", featureNames);
			return EnableFeature(featureName, enableAll);
		}
		DismStatus result = DismStatus.Success;
		foreach (string featureName2 in featureNames)
		{
			if (EnableFeature(featureName2, enableAll) == DismStatus.RebootRequired)
			{
				result = DismStatus.RebootRequired;
			}
		}
		return result;
	}

	public DismStatus DisableFeature(string featureName)
	{
		if (!_session.HasValue)
		{
			throw new InvalidOperationException("The session is not open.");
		}
		if (featureName == null)
		{
			throw new ArgumentNullException("featureName");
		}
		ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
		try
		{
			int num = _nativeMethods.DismDisableFeature(_session.Value, featureName, null, removePayload: true, manualResetEvent.SafeWaitHandle, ProgressCallback, IntPtr.Zero);
			if (num == 3010)
			{
				return DismStatus.RebootRequired;
			}
			if (!Success(num))
			{
				ThrowDismApiExceptionWithLastError("DismDisableFeature", num);
			}
			return DismStatus.Success;
		}
		finally
		{
			manualResetEvent.Dispose();
		}
	}

	public FeatureInstallResult EnsureFeaturesAreInstalled(params string[] featureNamesToEnsure)
	{
		Open();
		try
		{
			IEnumerable<Feature> source = from f1 in QueryFeatures()
				where featureNamesToEnsure.Any((string b) => string.Equals(f1.Name, b, StringComparison.OrdinalIgnoreCase))
				select f1;
			IEnumerable<string> source2 = featureNamesToEnsure.Except(source.Select((Feature f) => f.Name));
			if (source2.Any())
			{
				return new FeatureInstallResult(InstallStatus.FeatureNotAvailable, source2.First(), null);
			}
			Dictionary<string, Feature> dictionary = source.ToDictionary((Feature f) => f.Name);
			Feature[] array = dictionary.Values.ToArray();
			foreach (Feature feature in array)
			{
				switch (feature.State)
				{
				case PackageFeatureState.Installed:
				case PackageFeatureState.Superseded:
					dictionary.Remove(feature.Name);
					break;
				case PackageFeatureState.NotPresent:
				case PackageFeatureState.Removed:
					return new FeatureInstallResult(InstallStatus.FeatureNotAvailable, feature.Name, null);
				case PackageFeatureState.UninstallPending:
				case PackageFeatureState.InstallPending:
					return new FeatureInstallResult(InstallStatus.NeedsReboot, feature.Name, null);
				default:
					return new FeatureInstallResult(InstallStatus.InstallationFailure, feature.Name, string.Format(CultureInfo.InvariantCulture, "The feature is in an unexpected state: {0}", feature.State));
				case PackageFeatureState.Staged:
				case PackageFeatureState.PartiallyInstalled:
					break;
				}
			}
			if (!dictionary.Any())
			{
				return new FeatureInstallResult(InstallStatus.Installed, null, null);
			}
			try
			{
				if (EnableFeatures(enableAll: true, bulk: true, dictionary.Values.Select((Feature f) => f.Name).ToArray()) == DismStatus.RebootRequired)
				{
					return new FeatureInstallResult(InstallStatus.NeedsReboot, null, null);
				}
				return new FeatureInstallResult(InstallStatus.Installed, null, null);
			}
			catch (DismException)
			{
				bool flag = false;
				foreach (Feature value in dictionary.Values)
				{
					if (QueryFeatureInfo(value.Name).FeatureState == PackageFeatureState.Installed)
					{
						continue;
					}
					try
					{
						if (EnableFeatures(true, false, value.Name) == DismStatus.RebootRequired)
						{
							flag = true;
						}
					}
					catch (DismException ex2)
					{
						return new FeatureInstallResult(InstallStatus.InstallationFailure, value.Name, ex2.Message);
					}
				}
				if (flag)
				{
					return new FeatureInstallResult(InstallStatus.NeedsReboot, null, null);
				}
				return new FeatureInstallResult(InstallStatus.Installed, null, null);
			}
		}
		finally
		{
			Close();
		}
	}

	private static bool Success(int hresult)
	{
		if (hresult != 0 && hresult != 1)
		{
			return hresult == 3010;
		}
		return true;
	}

	private void ThrowDismApiExceptionWithLastError(string failingFunction, int result)
	{
		if (_nativeMethods.DismGetLastErrorMessage(out var errorMessage) == 0)
		{
			NativeMethods.DismString obj = (NativeMethods.DismString)Marshal.PtrToStructure(errorMessage, typeof(NativeMethods.DismString));
			_nativeMethods.DismDelete(errorMessage);
			throw new DismException(obj.Value.Trim());
		}
		throw new DismException(string.Format(CultureInfo.InvariantCulture, "Call to '{0}' failed. Result: {1}", failingFunction, result));
	}
}

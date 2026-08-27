using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.IM.Common.GlobalDefinitions.Utilities;
using VideoOS.IM.Server.Logger;
using VideoOS.IM.Server.Service.Licenses.DTOs;
using VideoOS.IM.Server.Service.Licenses.Providers;
using VideoOS.IM.Server.Service.OEM;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Service.Licenses.Validators;

public sealed class LicenseValidationStatusService : ILicenseValidationStatusService, IDisposable
{
	[NotNull]
	private readonly ReaderWriterLockSlim _licenseValidLock = new ReaderWriterLockSlim();

	[NotNull]
	private readonly ReaderWriterLockSlim _initializedLock = new ReaderWriterLockSlim();

	[NotNull]
	private readonly ReaderWriterLockSlim _licenseValidationRunningLock = new ReaderWriterLockSlim();

	[NotNull]
	private readonly CancellationTokenSource _isStopped = new CancellationTokenSource();

	[NotNull]
	private ILicenseProvider _licenseProvider;

	[NotNull]
	private VideoOS.IM.Server.Logger.ILoggerProvider _loggerProvider;

	[NotNull]
	private IOemResourceService _oemResourceService;

	[NotNull]
	private TimeSpan _interval;

	private bool _isInitialized;

	private bool _isLicenseValidationRunning;

	private bool _isLicenseValid;

	private static readonly Lazy<ILicenseValidationStatusService> LazyInstance = new Lazy<ILicenseValidationStatusService>(() => new LicenseValidationStatusService());

	public static ILicenseValidationStatusService Instance => LazyInstance.Value;

	public bool IsLicenseValid
	{
		get
		{
			if (!IsInitialized)
			{
				throw new InvalidOperationException("StartVerifyLicenseValidationStatusAsync should be called first.");
			}
			_licenseValidLock.EnterReadLock();
			try
			{
				return _isLicenseValid;
			}
			finally
			{
				_licenseValidLock.ExitReadLock();
			}
		}
	}

	public bool IsLicenseValidationRunning
	{
		get
		{
			_licenseValidationRunningLock.EnterReadLock();
			try
			{
				if (!IsInitialized)
				{
					throw new InvalidOperationException("StartVerifyLicenseValidationStatusAsync should be called first.");
				}
				return _isLicenseValidationRunning;
			}
			finally
			{
				_licenseValidationRunningLock.ExitReadLock();
			}
		}
		private set
		{
			_licenseValidationRunningLock.EnterWriteLock();
			try
			{
				_isLicenseValidationRunning = value;
			}
			finally
			{
				_licenseValidationRunningLock.ExitWriteLock();
			}
		}
	}

	private bool IsInitialized
	{
		get
		{
			_initializedLock.EnterReadLock();
			try
			{
				return _isInitialized;
			}
			finally
			{
				_initializedLock.ExitReadLock();
			}
		}
		set
		{
			_initializedLock.EnterWriteLock();
			try
			{
				_isInitialized = value;
			}
			finally
			{
				_initializedLock.ExitWriteLock();
			}
		}
	}

	public LicenseValidationStatusService()
	{
	}

	public void Dispose()
	{
		_licenseValidLock.Dispose();
		_initializedLock.Dispose();
		_licenseValidationRunningLock.Dispose();
		_isStopped.Dispose();
	}

	public void InitVerifyLicenseValidationStatus([NotNull] ILicenseProvider licenseProvider, [NotNull] VideoOS.IM.Server.Logger.ILoggerProvider loggerProvider, [NotNull] IOemResourceService oemResourceService)
	{
		if (IsInitialized)
		{
			throw new InvalidOperationException("InitVerifyLicenseValidationStatus has already been called..");
		}
		IsInitialized = true;
		licenseProvider.NullCheck("", "InitVerifyLicenseValidationStatus", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\Licenses\\Validators\\LicenseValidationStatusService.cs", 63);
		loggerProvider.NullCheck("", "InitVerifyLicenseValidationStatus", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\Licenses\\Validators\\LicenseValidationStatusService.cs", 64);
		oemResourceService.NullCheck("", "InitVerifyLicenseValidationStatus", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\Licenses\\Validators\\LicenseValidationStatusService.cs", 65);
		_licenseProvider = licenseProvider;
		_loggerProvider = loggerProvider;
		_oemResourceService = oemResourceService;
		UpdateIsCurrentLicenseValidAsync().GetAwaiter().GetResult();
	}

	public Task StartVerifyLicenseValidationStatusAsync(TimeSpan? interval)
	{
		if (!IsInitialized)
		{
			throw new InvalidOperationException("InitVerifyLicenseValidationStatus should be called first.");
		}
		if (IsLicenseValidationRunning)
		{
			throw new InvalidOperationException("StartVerifyLicenseValidationStatusAsync is currently running. 'StopVerifyLicenseValidationStatus' must be called before trying to starting the process again");
		}
		IsLicenseValidationRunning = true;
		_interval = interval ?? new TimeSpan(24, 0, 0);
		return Task.Run((Func<Task>)StartVerifyLicenseValidationStatusBackgroundAsync);
	}

	public void StopVerifyLicenseValidationStatus()
	{
		_isStopped.Cancel();
	}

	private async Task StartVerifyLicenseValidationStatusBackgroundAsync()
	{
		while (!_isStopped.IsCancellationRequested)
		{
			await UpdateIsCurrentLicenseValidAsync().ConfigureAwait(continueOnCapturedContext: false);
			await Task.Delay(_interval).ConfigureAwait(continueOnCapturedContext: false);
		}
		IsLicenseValidationRunning = false;
	}

	private async Task UpdateIsCurrentLicenseValidAsync()
	{
		bool isLicenseValid = await IsCurrentLicenseValidAsync().ConfigureAwait(continueOnCapturedContext: false);
		_licenseValidLock.EnterWriteLock();
		try
		{
			_isLicenseValid = isLicenseValid;
		}
		finally
		{
			_licenseValidLock.ExitWriteLock();
		}
	}

	private async Task<bool> IsCurrentLicenseValidAsync()
	{
		try
		{
			LicenseInstalledProduct licenseInstalledProduct = (await _licenseProvider.GetLicenseAsync().ConfigureAwait(continueOnCapturedContext: false)).LicenseInstalledProducts.FirstOrDefault((LicenseInstalledProduct x) => x.DisplayName == Constants.IncidentManagerLicenseId || x.DisplayName == _oemResourceService.IncidentManagerApplicationNameLong);
			if (licenseInstalledProduct == null)
			{
				throw new InvalidOperationException("Current license does not support Incident Manager.");
			}
			if (string.Equals(licenseInstalledProduct.ExpirationDate, "unrestricted", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			return DateTime.Parse(licenseInstalledProduct.ExpirationDate, CultureInfo.InvariantCulture).ToUniversalTime() > DateTime.UtcNow;
		}
		catch (Exception ex)
		{
			_loggerProvider.Logger.LogError("Error at {Function}. ex:{Ex}", "IsCurrentLicenseValidAsync", ex);
			return false;
		}
	}
}

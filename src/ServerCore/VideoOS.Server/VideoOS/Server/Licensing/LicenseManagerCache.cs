using System;
using System.Collections.Generic;
using System.Threading;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Utilities.Threading;

namespace VideoOS.Server.Licensing;

public sealed class LicenseManagerCache : BaseTimeDelayedThread, ILicenseManagerCache, ICachedLicenseInfo
{
	private const int ResetCacheMilliseconds = 5000;

	private const int ThreadExceptionSleepDelay = 30;

	private object _lockObject = new object();

	private readonly IServerManagement _serverManagement;

	private readonly IServer _server;

	private LicenseInformation _cachedLicenseInfo;

	private ILicenseSnapshot _licenseSnapshotCache;

	private readonly Dictionary<Guid, byte[]> _recordingServerCache;

	public LicenseInformation CachedLicenseInfo
	{
		get
		{
			if (_cachedLicenseInfo == null || _cachedLicenseInfo.LicenseExpirationDate < DateTime.UtcNow)
			{
				return null;
			}
			return _cachedLicenseInfo;
		}
	}

	public bool HasCachedLicenseInfo => CachedLicenseInfo != null;

	public new bool IsRunning => IsRunning();

	public LicenseManagerCache(IServerManagement serverManagement, IServer server)
		: base(5000, "LicenseManagerCache")
	{
		_serverManagement = serverManagement ?? throw new ArgumentNullException("serverManagement");
		_server = server ?? throw new ArgumentNullException("server");
		_recordingServerCache = new Dictionary<Guid, byte[]>();
	}

	public void ClearCachedLicenseInfo()
	{
		_cachedLicenseInfo = null;
		if (_server.IsInitialized)
		{
			ScheduleNextRun();
		}
	}

	public void UpdateCachedLicenseInfo(LicenseInformation value)
	{
		_cachedLicenseInfo = value ?? throw new ArgumentNullException("value");
	}

	public void ClearAllCaches()
	{
		lock (_lockObject)
		{
			LicensingEventTracer.Logger.TraceMethodStart("ClearAllCaches");
			_licenseSnapshotCache = null;
			ClearCachedLicenseInfo();
			_recordingServerCache.Clear();
			LicensingEventTracer.Logger.TraceMethodStop("ClearAllCaches");
		}
	}

	public ILicenseSnapshot RetrieveLicenseSnapshot()
	{
		lock (_lockObject)
		{
			return _licenseSnapshotCache;
		}
	}

	public void AddLicenseSnapshot(ILicenseSnapshot licenseSnapshot)
	{
		lock (_lockObject)
		{
			LicensingEventTracer.Logger.TraceMethodStart("AddLicenseSnapshot");
			_licenseSnapshotCache = licenseSnapshot;
			LicensingEventTracer.Logger.TraceMethodStop("AddLicenseSnapshot");
		}
	}

	public void AddRecordingServerLicense(Guid recordingServerId, byte[] license)
	{
		_recordingServerCache[recordingServerId] = license;
	}

	public bool RetrieveRecordingServerLicense(Guid recordingServerId, out byte[] license)
	{
		lock (_lockObject)
		{
			if (_recordingServerCache.TryGetValue(recordingServerId, out license))
			{
				return true;
			}
		}
		license = Array.Empty<byte>();
		return false;
	}

	protected override void OnTimeElapsed()
	{
		_cachedLicenseInfo = _serverManagement.GetLicenseInformationCache();
	}

	protected override bool OnHandleException(Exception err)
	{
		Thread.Sleep(30);
		return true;
	}

	protected override bool CanContinueWaiting()
	{
		return !_server.IsClosing;
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Common.Integration.AviConfig;
using VideoOS.Server.Webservice.Shared;

namespace VideoOS.Management.Server;

public static class InstalledCompressorCache
{
	private static DateTime _lastLoad = DateTime.MinValue;

	private static TimeSpan _cacheValidTimeSpan = TimeSpan.FromMinutes(5.0);

	private static List<Compressors> _compressorCache = null;

	private static object _lockObject = new object();

	public static List<Compressors> QueryCompressors()
	{
		lock (_lockObject)
		{
			if (DateTime.UtcNow - _cacheValidTimeSpan > _lastLoad || _compressorCache == null)
			{
				_compressorCache = GetAviCodecs();
				_lastLoad = DateTime.UtcNow;
			}
			return _compressorCache;
		}
	}

	public static void Flush()
	{
		lock (_lockObject)
		{
			_compressorCache.Clear();
			_lastLoad = DateTime.MinValue;
		}
	}

	private static List<Compressors> GetAviCodecs()
	{
		using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
		AviCodec64[] array = serverWebServiceManager.CallWithRetry((ServerWebServiceClient client) => client.GetInstalledCodecs(), null, delegate
		{
			GeminiTrace.Error(TraceType.Configuration, "Getting AviCodecs failed");
			return false;
		});
		List<Compressors> list = new List<Compressors>();
		if (array != null)
		{
			AviCodec64[] array2 = array;
			foreach (AviCodec64 aviCodec in array2)
			{
				list.Add(new Compressors
				{
					Handler = aviCodec.Handler,
					IdCodec = Guid.Empty,
					Quality = aviCodec.QualityValue,
					QualityEnabled = aviCodec.QualityOption,
					KeyFrames = aviCodec.KeyFrameValue,
					KeyFramesEnabled = aviCodec.KeyFrameOption,
					DataRate = aviCodec.DataRateValue,
					DataRateEnabled = aviCodec.DataRateOption,
					ConfigData = aviCodec.ConfigData,
					Flags = 0,
					Description = aviCodec.Description
				});
			}
		}
		return list;
	}
}

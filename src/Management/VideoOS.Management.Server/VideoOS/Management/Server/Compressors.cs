using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Management.Server;

public class Compressors
{
	public DateTime LastModified { get; set; }

	public long Handler { get; set; }

	public Guid IdCodec { get; set; }

	public int Flags { get; set; }

	public int Quality { get; set; }

	public int KeyFrames { get; set; }

	public int DataRate { get; set; }

	public byte[] ConfigData { get; set; }

	public bool QualityEnabled { get; set; }

	public bool KeyFramesEnabled { get; set; }

	public bool DataRateEnabled { get; set; }

	public string Description { get; set; }

	public static Compressors[] QueryCompressors(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		return LoadAndSyncWithInstalledCompressors(requestContext);
	}

	public static Compressors QueryCompressor(IGeminiRequestContext requestContext, long handler)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		return LoadAndSyncWithInstalledCompressors(requestContext).FirstOrDefault((Compressors c) => c.Handler == handler);
	}

	public static void DeleteCompressor(IGeminiRequestContext requestContext, long handler)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		using ICompressorComponent compressorComponent = requestContext.CreateCompressorComponent();
		compressorComponent.DeleteCompressor(handler);
	}

	public static void UpdateCompressor(IGeminiRequestContext requestContext, long handler, Guid idCodec, int flags, int quality, int keyFrames, int dataRate, byte[] configData)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		using ICompressorComponent compressorComponent = requestContext.CreateCompressorComponent();
		compressorComponent.UpdateCompressor(handler, idCodec, flags, quality, keyFrames, dataRate, configData);
	}

	private static Compressors[] LoadAndSyncWithInstalledCompressors(IGeminiRequestContext requestContext)
	{
		List<Compressors> list = InstalledCompressorCache.QueryCompressors();
		using ICompressorComponent compressorComponent = requestContext.CreateCompressorComponent();
		Compressors[] array = compressorComponent.QueryCompressors();
		bool flag = false;
		foreach (Compressors installed in list)
		{
			if (array.FirstOrDefault((Compressors c) => c.Handler == installed.Handler) == null)
			{
				compressorComponent.UpdateCompressor(installed.Handler, Guid.NewGuid(), installed.Flags, installed.Quality, installed.KeyFrames, installed.DataRate, installed.ConfigData);
				flag = true;
			}
		}
		if (flag)
		{
			array = compressorComponent.QueryCompressors();
		}
		Compressors[] array2 = array;
		foreach (Compressors configuredCompressor in array2)
		{
			Compressors compressors = list.FirstOrDefault((Compressors c) => c.Handler == configuredCompressor.Handler);
			if (compressors != null)
			{
				configuredCompressor.Description = compressors.Description;
				configuredCompressor.QualityEnabled = compressors.QualityEnabled;
				configuredCompressor.KeyFramesEnabled = compressors.KeyFramesEnabled;
				configuredCompressor.DataRateEnabled = compressors.DataRateEnabled;
			}
		}
		return array;
	}
}

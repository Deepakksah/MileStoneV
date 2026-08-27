using System;

namespace VideoOS.Management.Common;

public static class DetectionMethod
{
	public enum DetectionMethodType
	{
		Normal = 1,
		Optimized,
		Fast
	}

	public static int ConvertDetectionMethodToIn32(DetectionMethodType detectionMethod)
	{
		return ConvertDetectionMethodToIn32(detectionMethod, 1);
	}

	public static int ConvertDetectionMethodToIn32(DetectionMethodType detectionMethod, int defaultValue)
	{
		if (Enum.IsDefined(typeof(DetectionMethodType), detectionMethod))
		{
			return (int)detectionMethod;
		}
		return defaultValue;
	}

	public static DetectionMethodType ConvertInt32ToDetectionMethod(int detectionMethod)
	{
		return ConvertDetectionMethodToIn32(detectionMethod, DetectionMethodType.Normal);
	}

	public static DetectionMethodType ConvertDetectionMethodToIn32(int detectionMethod, DetectionMethodType defaultValue)
	{
		if (Enum.IsDefined(typeof(DetectionMethodType), detectionMethod))
		{
			return (DetectionMethodType)detectionMethod;
		}
		return defaultValue;
	}
}

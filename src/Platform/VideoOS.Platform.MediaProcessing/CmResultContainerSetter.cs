using System;
using NmMediaProcessing;
using VideoOS.Platform.MediaProcessing;

public unsafe class CmResultContainerSetter(CmMotionDetectionResult* pResultContainer)
{
	private unsafe CmMotionDetectionResult* m_pResultContainer = pResultContainer;

	public unsafe void SetResultContainer(RegionMotionDetection region)
	{
		if (region == null)
		{
			throw new ArgumentNullException();
		}
		global::_003CModule_003E.NmMediaProcessing_002ECmMotionChecker_002ESetResultContainer(region.GetChecker(), m_pResultContainer);
		GC.KeepAlive(this);
	}
}

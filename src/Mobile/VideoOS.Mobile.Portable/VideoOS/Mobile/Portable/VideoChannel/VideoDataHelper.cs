using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.VideoChannel;

public class VideoDataHelper
{
	public static void DisposeFrame(IEnumerable<StreamData> videoDatas)
	{
		if (videoDatas == null)
		{
			return;
		}
		foreach (StreamData videoData in videoDatas)
		{
			try
			{
				if (videoData.Disposer != null)
				{
					videoData.Disposer();
				}
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, "VideoDataHelper.DisposeFrame", ex.Message);
			}
		}
	}
}

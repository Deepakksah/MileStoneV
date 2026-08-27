using System;
using System.Runtime.InteropServices;

namespace VideoOS.Platform.MediaProcessing;

public interface IVideoDataAccess
{
	public enum matchCriterion
	{
		after,
		before,
		atOrAfter,
		atOrBefore,
		nearest,
		exact
	}

	[return: MarshalAs(UnmanagedType.U1)]
	bool GetVideoData(DateTime timeStamp, matchCriterion criterion, [MarshalAs(UnmanagedType.U1)] bool bSyncPointOnly, ref GenericByteData rByteData);

	[return: MarshalAs(UnmanagedType.U1)]
	bool HasVideoData(DateTime timeStamp, matchCriterion criterion, [MarshalAs(UnmanagedType.U1)] bool bSyncPointOnly);

	[return: MarshalAs(UnmanagedType.U1)]
	bool GetVideoDataTime(DateTime timeStamp, matchCriterion criterion, [MarshalAs(UnmanagedType.U1)] bool bSyncPointOnly, ref DateTime rTimeStamp);
}

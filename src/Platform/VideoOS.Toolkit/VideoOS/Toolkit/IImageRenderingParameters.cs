using System;
using System.Runtime.InteropServices;

namespace VideoOS.Toolkit;

public interface IImageRenderingParameters : IRenderingParameters
{
	public enum DeinterlaceMode
	{
		NoFilter,
		StretchTopField,
		StretchBottomField,
		ContentAdaptive
	}

	public enum DecodingHardwareSelection
	{
		OnlyIPP = 0,
		OnlyIntel = 1,
		OnlyNvidia = 2,
		AutoNvidia = 4
	}

	bool AdaptivePlayback
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get;
		[param: MarshalAs(UnmanagedType.U1)]
		set;
	}

	bool AdaptiveStreaming
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get;
		[param: MarshalAs(UnmanagedType.U1)]
		set;
	}

	TimeSpan BufferTimeSpan { get; set; }

	int WindowHeight { get; set; }

	int WindowWidth { get; set; }

	bool KeepAspectRatio
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get;
		[param: MarshalAs(UnmanagedType.U1)]
		set;
	}

	int NumberOfDecodingThreads { get; set; }

	bool RemovableMasksLifted
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get;
		[param: MarshalAs(UnmanagedType.U1)]
		set;
	}

	DecodingHardwareSelection HardwareAcceleration { get; set; }

	DeinterlaceMode PreferredDeinterlaceMode { get; set; }
}

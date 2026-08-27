using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

public static class MFHelper
{
	private const ulong mediaFoundationVersion = 624uL;

	[DllImport("mfplat.dll", EntryPoint = "MFStartup")]
	private static extern int ExternMFStartup([In] ulong IVersion, [In] uint dwFlags);

	[DllImport("mfplat.dll", EntryPoint = "MFShutdown")]
	private static extern int ExternMFShutdown();

	[DllImport("mfplat.dll", EntryPoint = "MFCreateMediaType")]
	private static extern int ExternMFCreateMediaType([MarshalAs(UnmanagedType.Interface)] out IMFMediaType ppMFType);

	[DllImport("mfplat.dll", EntryPoint = "MFCreateSourceResolver")]
	private static extern int ExternMFCreateSourceResolver([MarshalAs(UnmanagedType.Interface)] out IMFSourceResolver ppISourceResolver);

	[DllImport("mfplat.dll", EntryPoint = "MFCreateAttributes")]
	private static extern int ExternMFCreateAttributes([MarshalAs(UnmanagedType.Interface)] out IMFAttributes ppMFAttributes, [In] uint cInitialSize);

	[DllImport("mfplat.dll", EntryPoint = "MFCreateAsyncResult")]
	private static extern int ExternMFCreateAsyncResult([In][MarshalAs(UnmanagedType.IUnknown)] object punkObject, [In] IMFAsyncCallback pCallback, [In][MarshalAs(UnmanagedType.IUnknown)] object punkState, [MarshalAs(UnmanagedType.Interface)] out IMFAsyncResult ppAsyncResult);

	[DllImport("mfplat.dll", EntryPoint = "MFCreateMFByteStreamOnStream")]
	private static extern int ExternMFCreateMFByteStreamOnStream([In] IStream punkStream, [MarshalAs(UnmanagedType.Interface)] out IMFByteStream ppMFByteStream);

	[DllImport("Mf.dll", EntryPoint = "MFCreateMediaSession")]
	private static extern int ExternMFCreateMediaSession([In][MarshalAs(UnmanagedType.Interface)] IMFAttributes pConfiguration, [MarshalAs(UnmanagedType.Interface)] out IMFMediaSession ppMS);

	[DllImport("Mf.dll", EntryPoint = "MFCreateTranscodeProfile")]
	private static extern int ExternMFCreateTranscodeProfile([MarshalAs(UnmanagedType.Interface)] out IMFTranscodeProfile ppTranscodeProfile);

	[DllImport("Mf.dll", EntryPoint = "MFCreateTranscodeTopology")]
	private static extern int ExternMFCreateTranscodeTopology([In][MarshalAs(UnmanagedType.Interface)] IMFMediaSource pSrc, [In][MarshalAs(UnmanagedType.LPWStr)] string pwszOutputFilePath, [In][MarshalAs(UnmanagedType.Interface)] IMFTranscodeProfile pProfile, [MarshalAs(UnmanagedType.Interface)] out IMFTopology ppTranscodeTopo);

	[DllImport("Mf.dll", EntryPoint = "MFCreateTopology")]
	private static extern int ExternMFCreateTopology([MarshalAs(UnmanagedType.Interface)] out IMFTopology ppTopo);

	[DllImport("Mf.dll", EntryPoint = "MFCreateTopologyNode")]
	private static extern int ExternMFCreateTopologyNode([In] uint NodeType, [MarshalAs(UnmanagedType.Interface)] out IMFTopologyNode ppNode);

	[DllImport("Mfplat.dll", EntryPoint = "MFCreateFile")]
	private static extern int ExternMFCreateFile([In] uint accessMode, [In] uint openMode, [In] uint flags, [In][MarshalAs(UnmanagedType.LPWStr)] string fileName, [MarshalAs(UnmanagedType.Interface)] out IMFByteStream stream);

	[DllImport("Mf.dll", EntryPoint = "MFCreateMPEG4MediaSink")]
	private static extern int ExternMFCreateMPEG4MediaSink([In][MarshalAs(UnmanagedType.Interface)] IMFByteStream pIByteStream, [In][MarshalAs(UnmanagedType.Interface)] IMFMediaType pVideoMediaType, [In][MarshalAs(UnmanagedType.Interface)] IMFMediaType pAudioMediaType, [MarshalAs(UnmanagedType.Interface)] out IMFMediaSink ppIMediaSink);

	[DllImport("Mf.dll", EntryPoint = "MFCreateFMPEG4MediaSink")]
	private static extern int ExternMFCreateFragmentedMPEG4MediaSink([In][MarshalAs(UnmanagedType.Interface)] IMFByteStream pIByteStream, [In][MarshalAs(UnmanagedType.Interface)] IMFMediaType pVideoMediaType, [In][MarshalAs(UnmanagedType.Interface)] IMFMediaType pAudioMediaType, [MarshalAs(UnmanagedType.Interface)] out IMFMediaSink ppIMediaSink);

	[DllImport("Mfreadwrite.dll", EntryPoint = "MFCreateSinkWriterFromMediaSink")]
	private static extern int ExternMFCreateSinkWriterFromMediaSink([In][MarshalAs(UnmanagedType.Interface)] IMFMediaSink pMediaSink, [In][MarshalAs(UnmanagedType.Interface)] IMFAttributes pAttributes, [MarshalAs(UnmanagedType.Interface)] out IMFSinkWriter ppSinkWriter);

	[DllImport("Mfplat.dll", EntryPoint = "MFCreateMemoryBuffer")]
	private static extern int ExternMFCreateMemoryBuffer([In] uint cbMaxLength, [MarshalAs(UnmanagedType.Interface)] out IMFMediaBuffer ppBuffer);

	[DllImport("Mfplat.dll", EntryPoint = "MFAverageTimePerFrameToFrameRate")]
	private static extern int ExternMFAverageTimePerFrameToFrameRate([In] ulong unAverageTimePerFrame, out uint punNumerator, out uint punDenominator);

	[DllImport("Mfplat.dll", EntryPoint = "MFCreateSample")]
	private static extern int ExternMFCreateSample([MarshalAs(UnmanagedType.Interface)] out IMFSample ppIMFSample);

	[DllImport("Mfplat.dll", EntryPoint = "MFCreateCollection")]
	private static extern int ExternMFCreateCollection([MarshalAs(UnmanagedType.Interface)] out IMFCollection collection);

	public static void MFStartup(uint dwFlags = 0u)
	{
		int num = ExternMFStartup(624uL, dwFlags);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + "(MFStartup)", num);
		}
	}

	public static void MFShutdown()
	{
		int num = ExternMFShutdown();
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFShutdown)", num);
		}
	}

	public static IMFMediaType MFCreateMediaType()
	{
		IMFMediaType ppMFType;
		int num = ExternMFCreateMediaType(out ppMFType);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFCreateMediaType)", num);
		}
		return ppMFType;
	}

	public static IMFSourceResolver MFCreateSourceResolver()
	{
		IMFSourceResolver ppISourceResolver;
		int num = ExternMFCreateSourceResolver(out ppISourceResolver);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFCreateSourceResolver)", num);
		}
		return ppISourceResolver;
	}

	public static void MFCreateAttributes(out IMFAttributes attributes, uint initialSize)
	{
		int num = ExternMFCreateAttributes(out attributes, initialSize);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFCreateAttributes)", num);
		}
	}

	public static void MFCreateAsyncResult(object punkObject, IMFAsyncCallback pCallback, object punkState, out IMFAsyncResult ppAsyncResult)
	{
		int num = ExternMFCreateAsyncResult(punkObject, pCallback, punkState, out ppAsyncResult);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFCreateAsyncResult)", num);
		}
	}

	public static void MFCreateMFByteStreamOnStream(IStream punkObject, out IMFByteStream ppMFByteStream)
	{
		int num = ExternMFCreateMFByteStreamOnStream(punkObject, out ppMFByteStream);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFCreateMFByteStreamOnStream)", num);
		}
	}

	public static void MFCreateMediaSession(IMFAttributes configuration, out IMFMediaSession mediaSession)
	{
		int num = ExternMFCreateMediaSession(configuration, out mediaSession);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFCreateMediaSession)", num);
		}
	}

	public static void MFCreateTranscodeProfile(out IMFTranscodeProfile transcodeProfile)
	{
		int num = ExternMFCreateTranscodeProfile(out transcodeProfile);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFCreateTranscodeProfile)", num);
		}
	}

	public static void MFCreateTranscodeTopology(IMFMediaSource mediaSource, string outputFilePath, IMFTranscodeProfile transcodeProfile, out IMFTopology transcodeTopology)
	{
		int num = ExternMFCreateTranscodeTopology(mediaSource, outputFilePath, transcodeProfile, out transcodeTopology);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFCreateTranscodeTopology failed)", num);
		}
	}

	public static IMFTopology MFCreateTopology()
	{
		IMFTopology ppTopo;
		int num = ExternMFCreateTopology(out ppTopo);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFCreateTopology failed)", num);
		}
		return ppTopo;
	}

	public static IMFTopologyNode MFCreateTopologyNode(NodeType type)
	{
		IMFTopologyNode ppNode;
		int num = ExternMFCreateTopologyNode((uint)type, out ppNode);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFCreateTopologyNode failed)", num);
		}
		return ppNode;
	}

	public static IMFSourceResolver MFCreateDShowSourceResolver()
	{
		return (IMFSourceResolver)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("14D7A407-396B-44B3-BE85-5199A0F0F80A"), throwOnError: true));
	}

	public static IMFByteStream MFCreateFile(MFFileAccessMode accessMode, MFFileOpenMode openMode, MFFileFlags flags, string fileName)
	{
		IMFByteStream stream;
		int num = ExternMFCreateFile((uint)accessMode, (uint)openMode, (uint)flags, fileName, out stream);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFCreateFile failed)", num);
		}
		return stream;
	}

	public static IMFMediaSink MFCreateMPEG4MediaSink(IMFByteStream byteStream, IMFMediaType videoMediaType, IMFMediaType audioMediaType, bool fragmented)
	{
		int num = 0;
		num = ((!fragmented) ? ExternMFCreateMPEG4MediaSink(byteStream, videoMediaType, audioMediaType, out var ppIMediaSink) : ExternMFCreateFragmentedMPEG4MediaSink(byteStream, videoMediaType, audioMediaType, out ppIMediaSink));
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFCreateMPEG4MediaSink failed)", num);
		}
		return ppIMediaSink;
	}

	public static IMFSinkWriter MFCreateSinkWriterFromMediaSink(IMFMediaSink mediaSink, IMFAttributes configuration)
	{
		IMFSinkWriter ppSinkWriter;
		int num = ExternMFCreateSinkWriterFromMediaSink(mediaSink, configuration, out ppSinkWriter);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFCreateSinkWriterFromMediaSink failed)", num);
		}
		return ppSinkWriter;
	}

	public static IMFMediaBuffer MFCreateMemoryBuffer(uint cbMaxLength)
	{
		IMFMediaBuffer ppBuffer;
		int num = ExternMFCreateMemoryBuffer(cbMaxLength, out ppBuffer);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFCreateMemoryBuffer failed)", num);
		}
		return ppBuffer;
	}

	public static void MFAverageTimePerFrameToFrameRate(ulong unAverageTimePerFrame, out uint punNumerator, out uint punDenominator)
	{
		uint punNumerator2;
		uint punDenominator2;
		int num = ExternMFAverageTimePerFrameToFrameRate(unAverageTimePerFrame, out punNumerator2, out punDenominator2);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFAverageTimePerFrameToFrameRate failed)", num);
		}
		punNumerator = punNumerator2;
		punDenominator = punDenominator2;
	}

	public static IMFSample MFCreateSample()
	{
		IMFSample ppIMFSample;
		int num = ExternMFCreateSample(out ppIMFSample);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFCreateSample failed)", num);
		}
		return ppIMFSample;
	}

	public static ulong Pack(uint left, uint right)
	{
		return ((ulong)left << 32) | right;
	}

	public static IMFCollection MFCreateCollection()
	{
		IMFCollection collection;
		int num = ExternMFCreateCollection(out collection);
		if (num < 0)
		{
			throw new COMException("Exception from HRESULT: 0x" + num.ToString("X", NumberFormatInfo.InvariantInfo) + " (MFCreateCollection failed)", num);
		}
		return collection;
	}
}

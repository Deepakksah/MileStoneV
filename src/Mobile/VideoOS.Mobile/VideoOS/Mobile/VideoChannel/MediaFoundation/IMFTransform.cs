using System;
using System.Runtime.InteropServices;
using System.Security;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[ComImport]
[SuppressUnmanagedCodeSecurity]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("BF94C121-5B05-4E6F-8000-BA598961414D")]
public interface IMFTransform
{
	void GetStreamLimits([Out] int pdwInputMinimum, [Out] int pdwInputMaximum, [Out] int pdwOutputMinimum, [Out] int pdwOutputMaximum);

	void GetStreamCount(out int pcInputStreams, out int pcOutputStreams);

	void GetStreamIDs(int dwInputIDArraySize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] out int[] pdwInputIDs, int dwOutputIDArraySize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] out int[] pdwOutputIDs);

	void GetInputStreamInfo(int dwInputStreamID, out MFTInputStreamInfo pStreamInfo);

	void GetOutputStreamInfo(int dwOutputStreamID, out MFTOutputStreamInfo pStreamInfo);

	void GetAttributes([MarshalAs(UnmanagedType.Interface)] out IMFAttributes pAttributes);

	void GetInputStreamAttributes(int dwInputStreamID, [MarshalAs(UnmanagedType.Interface)] out IMFAttributes pAttributes);

	void GetOutputStreamAttributes(int dwOutputStreamID, [MarshalAs(UnmanagedType.Interface)] out IMFAttributes pAttributes);

	void DeleteInputStream(int dwStreamID);

	void AddInputStreams(int cStreams, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] adwStreamIDs);

	void GetInputAvailableType(int dwInputStreamID, int dwTypeIndex, [MarshalAs(UnmanagedType.Interface)] out IMFMediaType ppType);

	int GetOutputAvailableType(int dwOutputStreamID, int dwTypeIndex, [MarshalAs(UnmanagedType.Interface)] out IMFMediaType ppType);

	void SetInputType(int dwInputStreamID, [In][MarshalAs(UnmanagedType.Interface)] IMFMediaType pType, MFTSetTypeFlags dwFlags);

	void SetOutputType(int dwOutputStreamID, [In][MarshalAs(UnmanagedType.Interface)] IMFMediaType pType, MFTSetTypeFlags dwFlags);

	void GetInputCurrentType(int dwInputStreamID, [MarshalAs(UnmanagedType.Interface)] out IMFMediaType ppType);

	void GetOutputCurrentType(int dwOutputStreamID, [MarshalAs(UnmanagedType.Interface)] out IMFMediaType ppType);

	void GetInputStatus(int dwInputStreamID, out MFTInputStatusFlags pdwFlags);

	void GetOutputStatus(out MFTOutputStatusFlags pdwFlags);

	void SetOutputBounds(long hnsLowerBound, long hnsUpperBound);

	void ProcessEvent(int dwInputStreamID, [In][MarshalAs(UnmanagedType.Interface)] IMFMediaEvent pEvent);

	void ProcessMessage(MFTMessageType eMessage, IntPtr ulParam);

	void ProcessInput(int dwInputStreamID, [MarshalAs(UnmanagedType.Interface)] IMFSample pSample, int dwFlags);

	void ProcessOutput(MFTProcessOutputFlags dwFlags, int cOutputBufferCount, [In][Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ref MFTOutputDataBuffer[] pOutputSamples, out ProcessOutputStatus pdwStatus);
}

using System.Runtime.InteropServices;
using System.Security;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[ComImport]
[SuppressUnmanagedCodeSecurity]
[Guid("6EF2A660-47C0-4666-B13D-CBB717F2FA2C")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMFMediaSink
{
	void GetCharacteristics(out MFMediaSinkCharacteristics pdwCharacteristics);

	void AddStreamSink([In] int dwStreamSinkIdentifier, [In][MarshalAs(UnmanagedType.Interface)] IMFMediaType pMediaType, [MarshalAs(UnmanagedType.Interface)] out IMFStreamSink ppStreamSink);

	void RemoveStreamSink([In] int dwStreamSinkIdentifier);

	void GetStreamSinkCount(out int pcStreamSinkCount);

	void GetStreamSinkByIndex([In] int dwIndex, [MarshalAs(UnmanagedType.Interface)] out IMFStreamSink ppStreamSink);

	void GetStreamSinkById([In] int dwStreamSinkIdentifier, [MarshalAs(UnmanagedType.Interface)] out IMFStreamSink ppStreamSink);

	void SetPresentationClock([In][MarshalAs(UnmanagedType.Interface)] IMFPresentationClock pPresentationClock);

	void GetPresentationClock([MarshalAs(UnmanagedType.Interface)] out IMFPresentationClock ppPresentationClock);

	void Shutdown();
}

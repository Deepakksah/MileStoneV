using VideoOS.Platform.SDK.Proxy.Status2.Net;

namespace VideoOS.Platform.SDK.StatusClient.StatusEventArgs;

public class EventArgsMapper
{
	public static CameraStateChangedEventArgs MapToCameraStateEventArgs(CameraDeviceStatus cameraDeviceStatus)
	{
		return new CameraStateChangedEventArgs(cameraDeviceStatus.Time, cameraDeviceStatus.DeviceId, cameraDeviceStatus.Enabled, cameraDeviceStatus.Started, cameraDeviceStatus.IsChange, cameraDeviceStatus.Error, cameraDeviceStatus.ErrorNoConnection, cameraDeviceStatus.ErrorNotLicensed, cameraDeviceStatus.Recording, cameraDeviceStatus.DbMoveInProgress, cameraDeviceStatus.ErrorOverflow, cameraDeviceStatus.ErrorWritingGop, cameraDeviceStatus.DbRepairInProgress, cameraDeviceStatus.Motion);
	}

	public static EventFiredEventArgs MapToEventFiredEventArgs(EventStatus eventStatus)
	{
		return new EventFiredEventArgs(eventStatus.Time, eventStatus.SourceId, eventStatus.EventId, eventStatus.Metadata);
	}

	public static IODeviceStateChangedEventArgs MapToIODeviceStateChangedEventArgs(IODeviceStatusBase ioDeviceStatus)
	{
		return new IODeviceStateChangedEventArgs(ioDeviceStatus.Time, ioDeviceStatus.DeviceId, ioDeviceStatus.Enabled, ioDeviceStatus.Started, ioDeviceStatus.IsChange, ioDeviceStatus.Error, ioDeviceStatus.ErrorNoConnection, ioDeviceStatus.ErrorNotLicensed, MapIOState(ioDeviceStatus.State));
	}

	public static MediaStreamDeviceStateChangedEventArgs ToMediaStreamDeviceStateChangedEventArgs(MediaStreamDeviceStatusBase mediaStreamDeviceStatus)
	{
		return new MediaStreamDeviceStateChangedEventArgs(mediaStreamDeviceStatus.Time, mediaStreamDeviceStatus.DeviceId, mediaStreamDeviceStatus.Enabled, mediaStreamDeviceStatus.Started, mediaStreamDeviceStatus.IsChange, mediaStreamDeviceStatus.Error, mediaStreamDeviceStatus.ErrorNoConnection, mediaStreamDeviceStatus.ErrorNotLicensed, mediaStreamDeviceStatus.Recording, mediaStreamDeviceStatus.DbMoveInProgress, mediaStreamDeviceStatus.ErrorOverflow, mediaStreamDeviceStatus.ErrorWritingGop, mediaStreamDeviceStatus.DbRepairInProgress);
	}

	public static StatusApiEventArgs MapToStatusApiEventArgs(StatusBase configurationChangedStatus)
	{
		return new StatusApiEventArgs(configurationChangedStatus.Time);
	}

	private static string MapIOState(IOState state)
	{
		return state.ToString();
	}

	public static HardwareStateChangedEventArgs ToHardwareStateChangedEventArgs(HardwareStatus hardwareStatus)
	{
		return new HardwareStateChangedEventArgs(hardwareStatus.Time, hardwareStatus.HardwareId, hardwareStatus.Enabled, hardwareStatus.Started, hardwareStatus.IsChange, hardwareStatus.Error, hardwareStatus.ErrorNoConnection, hardwareStatus.ErrorNotLicensed);
	}
}

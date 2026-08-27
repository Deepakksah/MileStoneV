using System;
using VideoOS.Platform.Common;
using VideoOS.Platform.Common.Util;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Multicast;
using VideoOS.Platform.SDK.Platform;

namespace VideoOS.Platform.SDK.Media;

public class SDKInternalMediaService : InternalMediaServiceCommon
{
	public override string GetDecoderImplementation()
	{
		return MediaCommonUtility.GetDecoderImplementation();
	}

	public override string GetDecoderThreads(int activeStreams)
	{
		return MediaCommonUtility.GetDecoderThreads(activeStreams);
	}

	public override string IsRemovableMasksLifted(FQID fqid)
	{
		if (!fqid.ServerId.UserContext.PrivacyMaskLifted)
		{
			return "false";
		}
		return "true";
	}

	public override Guid OnLiveDisconnectMulticast(FQID fqid)
	{
		Guid result = Guid.Empty;
		ISystemInfo systemInfo = ((SDKConfiguration)fqid.ServerId.UserContext.Configuration).FindSystemInfo(fqid.ServerId);
		if (systemInfo != null)
		{
			ICamera camera = systemInfo.Cameras[fqid.ObjectId];
			result = fqid.ObjectId;
			MulticastManager orCreateMulticastManager = MulticastController.GetOrCreateMulticastManager(fqid.ServerId);
			orCreateMulticastManager.UnregisterDevice(camera.Recorder, fqid.ObjectId);
			orCreateMulticastManager.ProcessDeletedMulticastDevices(camera.Recorder);
		}
		return result;
	}

	public override bool SetupMulticast(FQID deviceFqid)
	{
		ISystemInfo systemInfo = ((SDKConfiguration)deviceFqid.ServerId.UserContext.Configuration).FindSystemInfo(deviceFqid.ServerId);
		if (systemInfo == null)
		{
			return false;
		}
		ICamera camera = systemInfo.Cameras[deviceFqid.ObjectId];
		MulticastManager multicastManager = null;
		multicastManager = MulticastController.GetOrCreateMulticastManager(camera.Recorder.ServerId);
		multicastManager.MulticastConfigEvent += ServerCommandController_MulticastConfigEvent;
		multicastManager.RegisterDevice(camera.Recorder, camera.Guid, out var ipAddress, out var port, out var ssrc, out var privacyMask, out var privacyMaskVersion);
		if (!string.IsNullOrEmpty(ipAddress))
		{
			multicastManager.MulticastConfigEvent -= ServerCommandController_MulticastConfigEvent;
			ChangeMulticastConfiguration(deviceFqid.ObjectId, ipAddress, port, ssrc, privacyMask, privacyMaskVersion);
			FireMulticastConfigDone(deviceFqid.ObjectId);
		}
		multicastManager.ProcessNewMulticastDevices(camera.Recorder);
		return true;
	}

	private void ServerCommandController_MulticastConfigEvent(object sender, MulticastConfigEventArgs e)
	{
		ChangeMulticastConfiguration(e.DeviceId, e.IPAddress, e.Port, e.SSRC, e.PrivacyMask, e.PrivacyMaskVersion);
		MulticastController.GetOrCreateMulticastManager(e.Recorder.ServerId).MulticastConfigEvent -= ServerCommandController_MulticastConfigEvent;
		FireMulticastConfigDone(e.DeviceId);
	}
}

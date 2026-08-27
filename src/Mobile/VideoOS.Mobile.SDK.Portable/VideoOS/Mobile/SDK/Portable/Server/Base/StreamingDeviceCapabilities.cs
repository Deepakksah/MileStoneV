using System.Collections.Generic;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public class StreamingDeviceCapabilities : IStreamingDeviceCapabilities, ICapabilities
{
	protected readonly Dictionary<string, string> _capabilities = new Dictionary<string, string>();

	public bool LiveEnabled { get; private set; }

	public bool PlaybackEnabled { get; private set; }

	public bool BookmarkAddEnabled { get; private set; }

	public bool BookmarkDeleteEnabled { get; private set; }

	public bool BookmarkEditEnabled { get; private set; }

	public bool BookmarkViewEnabled { get; private set; }

	public Dictionary<string, string> Capabilities => new Dictionary<string, string>(_capabilities);

	public StreamingDeviceCapabilities()
	{
		LiveEnabled = false;
		PlaybackEnabled = false;
		BookmarkAddEnabled = false;
		BookmarkDeleteEnabled = false;
		BookmarkViewEnabled = false;
		BookmarkEditEnabled = false;
	}

	public virtual void InitFromCommand(Dictionary<string, string> deviceParams)
	{
		_capabilities.Clear();
		LiveEnabled = CapabilitiesHelper.ExtractBool(deviceParams, "Live", LiveEnabled);
		PlaybackEnabled = CapabilitiesHelper.ExtractBool(deviceParams, "Playback", PlaybackEnabled);
		BookmarkAddEnabled = CapabilitiesHelper.ExtractBool(deviceParams, "BookmarkAdd", BookmarkAddEnabled);
		BookmarkDeleteEnabled = CapabilitiesHelper.ExtractBool(deviceParams, "BookmarkDelete", BookmarkDeleteEnabled);
		BookmarkViewEnabled = CapabilitiesHelper.ExtractBool(deviceParams, "BookmarkView", BookmarkViewEnabled);
		BookmarkEditEnabled = CapabilitiesHelper.ExtractBool(deviceParams, "BookmarkEdit", BookmarkEditEnabled);
		CapabilitiesHelper.CopyDictionary(deviceParams, _capabilities);
	}
}

using System;

namespace VideoOS.Mobile.VideoSource;

public interface ILiveSourceConfiguration
{
	TimeSpan ToolkitConnectTimeout { get; set; }
}

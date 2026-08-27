using System.Collections.Specialized;
using System.Runtime.Serialization;
using VideoOS.Mobile.Helpers;

namespace VideoOS.Mobile.Configuration.Section;

[DataContract]
public class MediaProcessingConfig : IConfigSection
{
	public const string NameHardwareDecodingMode = "HardwareDecodingMode";

	public const string NameSoftwareDecodingThreads = "SoftwareDecodingThreads";

	public const string NameFrameCounterLogEnabled = "FrameCounterLogEnabled";

	public const string NameToolkitForkingEnabled = "ToolkitForkingEnabled";

	[DataMember]
	public string HardwareDecodingMode { get; set; }

	[DataMember]
	public string SoftwareDecodingThreads { get; set; }

	[DataMember]
	public bool FrameCounterLogEnabled { get; set; }

	[DataMember]
	public string ToolkitForkingEnabled { get; set; }

	public string SectionName => "MediaProcessing";

	public MediaProcessingConfig(string hardwareDecodingMode)
	{
		HardwareDecodingMode = hardwareDecodingMode;
		FrameCounterLogEnabled = false;
	}

	public bool InitFromCollection(NameValueCollection section)
	{
		bool result = true;
		try
		{
			HardwareDecodingMode = section["HardwareDecodingMode"].Sanitized();
			SoftwareDecodingThreads = section["SoftwareDecodingThreads"].Sanitized();
			ToolkitForkingEnabled = section["ToolkitForkingEnabled"].Sanitized();
		}
		catch
		{
			result = false;
		}
		try
		{
			FrameCounterLogEnabled = section["FrameCounterLogEnabled"].Sanitized().IsTrue();
		}
		catch
		{
			result = false;
		}
		return result;
	}

	public bool ConvToCollection(NameValueCollection serverSection)
	{
		bool result = true;
		try
		{
			serverSection["HardwareDecodingMode"] = HardwareDecodingMode;
			serverSection["SoftwareDecodingThreads"] = SoftwareDecodingThreads;
			serverSection["ToolkitForkingEnabled"] = ToolkitForkingEnabled;
		}
		catch
		{
			result = false;
		}
		try
		{
			serverSection["FrameCounterLogEnabled"] = (FrameCounterLogEnabled ? "True" : "False");
		}
		catch
		{
			result = false;
		}
		return result;
	}
}

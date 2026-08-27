using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "BookmarkSettingInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class BookmarkSettingInfo : IExtensibleDataObject, IBookmarkSettingInfo
{
	private ExtensionDataObject extensionDataField;

	private int DefaultPostTimeSecField;

	private int DefaultPreTimeSecField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public int DefaultPostTimeSec
	{
		get
		{
			return DefaultPostTimeSecField;
		}
		set
		{
			DefaultPostTimeSecField = value;
		}
	}

	[DataMember]
	public int DefaultPreTimeSec
	{
		get
		{
			return DefaultPreTimeSecField;
		}
		set
		{
			DefaultPreTimeSecField = value;
		}
	}
}

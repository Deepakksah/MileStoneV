using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "BookmarkSettingInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class BookmarkSettingInfo : IBookmarkSettingInfo
{
	private int DefaultPostTimeSecField;

	private int DefaultPreTimeSecField;

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

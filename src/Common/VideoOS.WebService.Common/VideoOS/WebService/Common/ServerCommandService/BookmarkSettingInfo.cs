using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("BookmarkSettingInfo: PreTime={DefaultPreTimeSec}, PostTime={DefaultPostTimeSec}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class BookmarkSettingInfo
{
	[DataMember]
	public int DefaultPreTimeSec { get; set; }

	[DataMember]
	public int DefaultPostTimeSec { get; set; }
}

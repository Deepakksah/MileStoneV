using System;
using System.Runtime.Serialization;
using System.Text;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Messaging;

[Serializable]
[DataContract]
public class VideoWallIndicationData
{
	[DataMember]
	public string ActionId { get; set; }

	[DataMember]
	public FQID FQID { get; set; }

	[DataMember]
	public ViewItemInformation[] ViewItemInformationArray { get; set; }

	public VideoWallIndicationData()
	{
	}

	public VideoWallIndicationData(string actionId, FQID fqid, ViewItemInformation[] viewItemInformationArray)
	{
		ActionId = actionId;
		FQID = fqid;
		ViewItemInformationArray = viewItemInformationArray;
	}

	public string ToXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<viewitems>");
		ViewItemInformation[] viewItemInformationArray = ViewItemInformationArray;
		foreach (ViewItemInformation viewItemInformation in viewItemInformationArray)
		{
			stringBuilder.Append(ViewItemUtility.ConvertToXml(viewItemInformation, includeTopNode: false));
		}
		stringBuilder.Append("</viewitems>");
		return stringBuilder.ToString();
	}
}

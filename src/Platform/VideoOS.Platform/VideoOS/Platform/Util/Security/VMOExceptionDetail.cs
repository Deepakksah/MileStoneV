using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace VideoOS.Platform.Util.Security;

[Serializable]
[DataContract]
public class VMOExceptionDetail : ExceptionDetail
{
	[DataMember]
	public string VMOErrorId;

	[DataMember]
	public Dictionary<int, MessageParameter> MsgParams;

	public VMOExceptionDetail(Exception ex, IEnumerable<MessageParameter> msgParams)
		: base(ex)
	{
		MsgParams = new Dictionary<int, MessageParameter>();
		foreach (MessageParameter msgParam in msgParams)
		{
			MsgParams.Add(msgParam.Index, msgParam);
		}
	}
}

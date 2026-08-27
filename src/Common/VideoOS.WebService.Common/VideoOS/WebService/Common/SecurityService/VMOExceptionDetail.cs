using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace VideoOS.WebService.Common.SecurityService;

[Serializable]
[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
public class VMOExceptionDetail : ExceptionDetail
{
	[DataMember]
	public string VMOErrorId;

	[DataMember]
	public Dictionary<int, MessageParameter> MsgParams;

	public VMOExceptionDetail(Exception ex, IEnumerable<MessageParameter> msgParams)
		: base(ex)
	{
		if (msgParams == null)
		{
			throw new ArgumentNullException("msgParams");
		}
		MsgParams = new Dictionary<int, MessageParameter>();
		foreach (MessageParameter msgParam in msgParams)
		{
			MsgParams.Add(msgParam.Index, msgParam);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[DataContract]
public sealed class BackupConfigCompletedResponse : CommonResponseBase
{
	[DataMember]
	public string[] FilesHandled { get; private set; }

	public BackupConfigCompletedResponse(IList<string> filesHandled)
	{
		if (filesHandled == null)
		{
			FilesHandled = new string[0];
		}
		else
		{
			FilesHandled = filesHandled.ToArray();
		}
	}

	public override string ToString()
	{
		return GetType().Name;
	}

	public override void Accept(ICommonResponseVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonResponseVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}

using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[DataContract]
public sealed class BackupConfigStatusResponse : CommonResponseBase
{
	[DataMember]
	public BackupConfigActionStatus ActionStatus { get; set; }

	[DataMember]
	public string ComponentName { get; private set; }

	[DataMember]
	public int CurrentActionProgress { get; set; }

	public BackupConfigStatusResponse(BackupConfigActionStatus status, string componentName, int currentActionProgress)
	{
		ActionStatus = status;
		ComponentName = componentName;
		CurrentActionProgress = currentActionProgress;
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

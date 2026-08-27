using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class SendMultiNotificationCommand : CommonCommandBase, ICommandWithSource
{
	[DataMember]
	public Guid IDNotificationProfile { get; set; }

	[DataMember]
	public Guid EventTypeId { get; set; }

	[DataMember]
	public Guid[] SourceIds { get; set; }

	[DataMember]
	public Guid[] SourceGroupIds { get; set; }

	[DataMember]
	public Guid TriggerId { get; set; }

	[DataMember]
	public Guid RuleId { get; set; }

	[DataMember]
	public DateTime Time { get; set; }

	[DataMember]
	public Guid RecorderId { get; set; }

	Guid ICommandWithSource.SourceId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid("9CAD5067-AB91-44c0-B6BB-B28CF166B272");
		}
	}

	public SendMultiNotificationCommand(Guid notificationProfileId, Guid ruleId, Guid eventTypeId, Guid triggerId, Guid recorderId, Guid[] sourceIds, Guid[] sourceGroupIds, DateTime time)
	{
		IDNotificationProfile = notificationProfileId;
		RuleId = ruleId;
		EventTypeId = eventTypeId;
		TriggerId = triggerId;
		SourceIds = sourceIds;
		SourceGroupIds = sourceGroupIds;
		Time = time;
		RecorderId = recorderId;
	}

	public override void Accept(ICommandVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonCommandVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}

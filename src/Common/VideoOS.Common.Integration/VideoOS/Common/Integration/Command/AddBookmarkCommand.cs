using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class AddBookmarkCommand : CommonCommandBase
{
	[DataMember]
	public Guid[] DeviceIds { get; private set; }

	[DataMember]
	public string Header { get; private set; }

	[DataMember]
	public string Description { get; private set; }

	[DataMember]
	public DateTime TimeTrigged { get; private set; }

	[DataMember]
	public TimeSpan TimeBefore { get; private set; }

	[DataMember]
	public TimeSpan TimeAfter { get; private set; }

	public AddBookmarkCommand(Guid[] deviceIds, string header, string description, DateTime timeTrigged, TimeSpan timeBefore, TimeSpan timeAfter)
	{
		DeviceIds = deviceIds;
		Header = header;
		Description = description;
		TimeTrigged = timeTrigged;
		TimeBefore = timeBefore;
		TimeAfter = timeAfter;
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

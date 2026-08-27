using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class DeleteConfigurationItemCommand : CommonCommandBase
{
	[DataMember]
	public string[] PrimaryIds { get; private set; }

	[DataMember]
	public string[] SecondaryIds { get; private set; }

	[DataMember]
	public string TableTypeName { get; private set; }

	[DataMember]
	public bool UserAcceptedToDeleteEvidenceLockedData { get; set; }

	public DeleteConfigurationItemCommand(string[] ids, string tableTypeName)
		: this(ids, null, tableTypeName)
	{
	}

	public DeleteConfigurationItemCommand(string[] primaryIds, string[] secondaryIds, string tableTypeName)
	{
		PrimaryIds = primaryIds;
		SecondaryIds = secondaryIds;
		TableTypeName = tableTypeName;
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

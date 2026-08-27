using System.Linq;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.ViewGroupItem;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetAllItemsResponse : BaseCommandResponse
{
	public ViewGroupTree AllSubItems { get; private set; }

	public GetAllItemsResponse(Command command)
		: base(command)
	{
		SerializableItem serializableItem = command.Items?.FirstOrDefault();
		if (serializableItem != null)
		{
			AllSubItems = SerializableViewGroupTree.GetViewGroupTree(serializableItem);
		}
	}

	public GetAllItemsResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}

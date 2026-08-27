using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "ViewItemInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public sealed class ViewItemOld
{
	[DataMember]
	public string Id { get; set; }

	[DataMember]
	public int Position { get; set; }

	[DataMember]
	public string DefinitionXml { get; set; }

	[DataMember]
	public ViewItemOldOptions ViewItemOptions { get; set; }

	private static ViewItemOld ConvertToViewItemOld(ViewItem viewItem)
	{
		if (viewItem == null)
		{
			return null;
		}
		return new ViewItemOld
		{
			Id = viewItem.Id,
			DefinitionXml = viewItem.DefinitionXml,
			Position = viewItem.Position,
			ViewItemOptions = new ViewItemOldOptions()
		};
	}

	public static ViewItemOld[] ConvertToViewItemOlds(ViewItem[] viewItems)
	{
		List<ViewItemOld> list = new List<ViewItemOld>();
		foreach (ViewItem viewItem in viewItems)
		{
			list.Add(ConvertToViewItemOld(viewItem));
		}
		return list.ToArray();
	}

	private static ViewItem ConvertToViewItem(ViewItemOld viewItemOld)
	{
		ViewItem viewItem = new ViewItem();
		viewItem.Id = viewItemOld.Id;
		viewItem.DefinitionXml = viewItemOld.DefinitionXml;
		viewItem.Position = viewItemOld.Position;
		viewItem.CustomProperties = viewItemOld.ViewItemOptions.CustomProperties;
		if (!string.IsNullOrEmpty(viewItemOld.ViewItemOptions.UserDefinedId))
		{
			viewItem.Id = viewItemOld.ViewItemOptions.UserDefinedId;
		}
		return viewItem;
	}

	public static ViewItem[] ConvertToViewItems(ViewItemOld[] viewItems)
	{
		List<ViewItem> list = new List<ViewItem>();
		if (viewItems != null)
		{
			foreach (ViewItemOld viewItemOld in viewItems)
			{
				list.Add(ConvertToViewItem(viewItemOld));
			}
		}
		return list.ToArray();
	}
}

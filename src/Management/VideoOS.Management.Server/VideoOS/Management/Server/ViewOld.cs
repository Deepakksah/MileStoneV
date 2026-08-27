using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "ViewInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public sealed class ViewOld
{
	[DataMember]
	public string Id { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Description { get; set; }

	[DataMember]
	public string DefinitionXml { get; set; }

	[DataMember]
	public ViewOptionsOld ViewOptions { get; set; }

	[DataMember]
	public DateTime CreatedDate { get; set; }

	[DataMember]
	public DateTime LastModified { get; set; }

	public static ViewOld CreateView(IGeminiRequestContext requestContext, string viewGroupId, ViewOld newView)
	{
		View view = ConvertToView(newView);
		view.Id = GeminiResourceComponent.GetGuid(newView.ViewOptions.UserDefinedId);
		return ConvertToViewOld(View.CreateView(requestContext, viewGroupId, view));
	}

	public static ViewOld UpdateView(IGeminiRequestContext requestContext, ViewOld viewOld)
	{
		View view = ConvertToView(viewOld);
		return ConvertToViewOld(View.UpdateView(requestContext, view));
	}

	public static void DeleteView(IGeminiRequestContext requestContext, string viewId)
	{
		View.DeleteView(requestContext, viewId);
	}

	private static ViewOld ConvertToViewOld(View view)
	{
		if (view == null)
		{
			return null;
		}
		return new ViewOld
		{
			Id = view.Id,
			Name = view.Name,
			Description = view.Description,
			CreatedDate = view.CreatedDate,
			LastModified = view.LastModified,
			DefinitionXml = view.DefinitionXml,
			ViewOptions = new ViewOptionsOld()
		};
	}

	public static ViewOld[] ConvertToViewOlds(View[] views)
	{
		List<ViewOld> list = new List<ViewOld>();
		foreach (View view in views)
		{
			list.Add(ConvertToViewOld(view));
		}
		return list.ToArray();
	}

	private static View ConvertToView(ViewOld viewOld)
	{
		return new View
		{
			Id = viewOld.Id,
			Name = viewOld.Name,
			Description = viewOld.Description,
			CreatedDate = viewOld.CreatedDate,
			LastModified = viewOld.LastModified,
			DefinitionXml = viewOld.DefinitionXml,
			CustomProperties = viewOld.ViewOptions.CustomProperties,
			ViewItems = ViewItemOld.ConvertToViewItems(viewOld.ViewOptions.ViewItems)
		};
	}
}

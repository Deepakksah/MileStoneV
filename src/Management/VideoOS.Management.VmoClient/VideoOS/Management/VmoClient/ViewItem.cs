using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

[EntityName("ViewItem")]
public class ViewItem : VmoObjectGuidBased
{
	public string DefinitionXml
	{
		get
		{
			return GetPropertyValue<string>("DefinitionXml");
		}
		set
		{
			SetPropertyValue("DefinitionXml", value);
		}
	}

	public string ViewId => GetPropertyValue<string>("ViewId");

	public View View => GetObjectById<View>(ViewId);

	public ViewItem(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void Clear()
	{
		ThrowIfNotCreatedOrDeleted();
		GenericRepositoryFunctions.ClearViewItem(base.Repositories, this);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}

	public void ChangeId(Guid id)
	{
		ThrowIfNotCreatedOrDeleted();
		if (base.Repository.GetVmoObjectFromCache(id.ToString()) != null)
		{
			throw new ViewItemUpdateException(string.Format(CultureInfo.CurrentCulture, VmoResources.VMO60219, id));
		}
		GenericRepositoryFunctions.ChangeViewItemId(base.Repositories, this, id.ToString(), ViewId);
		SetPropertyValue("Id", id.ToString());
		base.Repository.RefreshObject(base.InternalId);
	}
}

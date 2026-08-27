using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("View")]
public class View : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
{
	public string Name
	{
		get
		{
			return GetPropertyValue<string>("Name");
		}
		set
		{
			SetPropertyValue("Name", value);
		}
	}

	public string Description
	{
		get
		{
			return GetPropertyValue<string>("Description");
		}
		set
		{
			SetPropertyValue("Description", value);
		}
	}

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

	public DateTime CreatedDate
	{
		get
		{
			return GetPropertyValue<DateTime>("CreatedDate");
		}
		set
		{
			SetPropertyValue("CreatedDate", value);
		}
	}

	public DateTime LastModified
	{
		get
		{
			return GetPropertyValue<DateTime>("LastModified");
		}
		set
		{
			SetPropertyValue("LastModified", value);
		}
	}

	public CustomViewItem[] CustomViewItem { get; private set; }

	public string ViewGroupId => GetPropertyValue<string>("ParentServerGroupId");

	public string MonitorId => GetPropertyValue<string>("MonitorId");

	public string VideoWallId => GetPropertyValue<string>("VideoWallId");

	public ViewItemCollection ViewItems { get; private set; }

	public View(ViewGroup viewGroup, CustomViewItem[] customViewItems)
		: this(viewGroup.Repositories, viewGroup.InternalId, customViewItems)
	{
	}

	public View(ViewGroup viewGroup, Guid customId, CustomViewItem[] customViewItems)
		: this(viewGroup.Repositories, viewGroup.InternalId, customViewItems)
	{
		SetPropertyValue("Id", customId.ToString());
	}

	public View(IVmoRepositories repositories, string viewGroupId, CustomViewItem[] customViewItems)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
		CustomViewItem = customViewItems;
		SetPropertyValue("ParentServerGroupId", viewGroupId);
	}

	public View(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		ViewItems = new ViewItemCollection(LoadViewItems, this, isInitialized: false);
	}

	public void ApplyView(View view)
	{
		if (view == null)
		{
			throw new ArgumentNullException("view");
		}
		ThrowIfNotCreatedOrDeleted();
		GenericRepositoryFunctions.ApplyViewToView(base.Repositories, view.Id, this);
	}

	public void Clear()
	{
		ThrowIfNotCreatedOrDeleted();
		GenericRepositoryFunctions.ClearView(base.Repositories, this);
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}

	public void Create()
	{
		GenericRepositoryFunctions.CreateView(base.Repositories, this);
	}

	public void Delete()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.DeleteObject(this);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}

	private ViewItem[] LoadViewItems(bool forced)
	{
		return base.Repositories.GetObjectsByParentId<View, ViewItem>(base.Id, forced);
	}
}

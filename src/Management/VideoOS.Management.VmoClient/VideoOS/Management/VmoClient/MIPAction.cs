using System;
using System.Globalization;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MIPAction")]
public class MIPAction : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
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

	public Guid DefiningPluginId
	{
		get
		{
			return new Guid(GetPropertyValue<string>("DefiningPluginId"));
		}
		set
		{
			SetPropertyValue("DefiningPluginId", value.ToString());
		}
	}

	public string DefiningPluginName
	{
		get
		{
			return GetPropertyValue<string>("DefiningPluginName");
		}
		set
		{
			SetPropertyValue("DefiningPluginName", value);
		}
	}

	public string SelectionText
	{
		get
		{
			return GetPropertyValue<string>("SelectionText");
		}
		set
		{
			SetPropertyValue("SelectionText", value);
		}
	}

	public string DescriptionText
	{
		get
		{
			return GetPropertyValue<string>("DescriptionText");
		}
		set
		{
			SetPropertyValue("DescriptionText", value);
		}
	}

	public string PickerDefaultText
	{
		get
		{
			return GetPropertyValue<string>("PickerDefaultText");
		}
		set
		{
			SetPropertyValue("PickerDefaultText", value);
		}
	}

	public MIPKinds MIPKinds
	{
		get
		{
			return new MIPKinds(GetPropertyValue<string>("MIPKinds"));
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			SetPropertyValue("MIPKinds", value.SaveAsXml());
		}
	}

	public string PickerDefaultText2
	{
		get
		{
			return GetPropertyValue<string>("PickerDefaultText2");
		}
		set
		{
			SetPropertyValue("PickerDefaultText2", value);
		}
	}

	public MIPKinds MIPKinds2
	{
		get
		{
			if (TryGetPropertyValue<string>("MIPKinds2", out var value))
			{
				return new MIPKinds(value);
			}
			return null;
		}
		set
		{
			if (value == null)
			{
				SetPropertyValue("MIPKinds2", new MIPKinds().SaveAsXml());
			}
			else
			{
				SetPropertyValue("MIPKinds2", value.SaveAsXml());
			}
		}
	}

	public DateTime CreatedDate => GetPropertyValue<DateTime>("CreatedDate");

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public MIPAction(ManagementServer managementServer, Guid id)
		: this(managementServer.Repositories)
	{
		if (managementServer == null)
		{
			throw new ArgumentNullException("managementServer");
		}
		if (id == Guid.Empty)
		{
			throw new ArgumentException("ID may not be guid empty", "id");
		}
		SetPropertyValue("Id", id.ToString().ToUpper(CultureInfo.InvariantCulture));
	}

	public MIPAction(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
	}

	public MIPAction(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
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

	public void Create()
	{
		base.Repository.CreateObject(this);
	}
}

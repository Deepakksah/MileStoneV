using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

[EntityName("MarkedData")]
public class MarkedData : ICreatable, IUpdatable, IDeletable
{
	private DeviceStickerSizeCollection _stickerSizes;

	public Dictionary<string, object> _objectPropertiesBase;

	public VmoPropertyCollection _objectProperties;

	private readonly IVmoRepositories _repositories;

	public Guid Id
	{
		get
		{
			if (ObjectProperties.GetPropertyValue<string>("Id") == null)
			{
				return Guid.Empty;
			}
			return new Guid(ObjectProperties.GetPropertyValue<string>("Id"));
		}
	}

	public Guid[] DeviceIds
	{
		get
		{
			string propertyValue = ObjectProperties.GetPropertyValue<string>("DeviceIds");
			if (string.IsNullOrEmpty(propertyValue))
			{
				return null;
			}
			return (from f in propertyValue.Split(',')
				select new Guid(f)).ToArray();
		}
		set
		{
			ObjectProperties.SetPropertyValue("DeviceIds", string.Join(",", value.Select((Guid f) => f.ToString()).ToArray()));
		}
	}

	public string Reference
	{
		get
		{
			return ObjectProperties.GetPropertyValue<string>("Reference");
		}
		set
		{
			ObjectProperties.SetPropertyValue("Reference", value);
		}
	}

	public string UserDomainAccountName => ObjectProperties.GetPropertyValue<string>("UserName");

	public string Header
	{
		get
		{
			return ObjectProperties.GetPropertyValue<string>("Header");
		}
		set
		{
			ObjectProperties.SetPropertyValue("Header", value);
		}
	}

	public string Description
	{
		get
		{
			return ObjectProperties.GetPropertyValue<string>("Description");
		}
		set
		{
			ObjectProperties.SetPropertyValue("Description", value);
		}
	}

	public MarkedDataDefinitions.MarkedDataTypes MarkedDataType
	{
		get
		{
			return (MarkedDataDefinitions.MarkedDataTypes)ObjectProperties.GetPropertyValue<int>("MarkedDataType");
		}
		set
		{
			ObjectProperties.SetPropertyValue("MarkedDataType", (int)value);
		}
	}

	public DateTime StartTime
	{
		get
		{
			return ObjectProperties.GetPropertyValue<DateTime>("StartTime");
		}
		set
		{
			ObjectProperties.SetPropertyValue("StartTime", value);
		}
	}

	public DateTime TagTime
	{
		get
		{
			return ObjectProperties.GetPropertyValue<DateTime>("TagTime");
		}
		set
		{
			ObjectProperties.SetPropertyValue("TagTime", value);
		}
	}

	public DateTime EndTime
	{
		get
		{
			return ObjectProperties.GetPropertyValue<DateTime>("EndTime");
		}
		set
		{
			ObjectProperties.SetPropertyValue("EndTime", value);
		}
	}

	public bool UseRetention
	{
		get
		{
			return ObjectProperties.GetPropertyValue<bool>("UseRetention");
		}
		set
		{
			ObjectProperties.SetPropertyValue("UseRetention", value);
		}
	}

	public DateTime RetentionExpire
	{
		get
		{
			return ObjectProperties.GetPropertyValue<DateTime>("RetentionExpire");
		}
		set
		{
			ObjectProperties.SetPropertyValue("RetentionExpire", value);
		}
	}

	public long RetentionSize
	{
		get
		{
			return ObjectProperties.GetPropertyValue<long>("RetentionSize");
		}
		set
		{
			ObjectProperties.SetPropertyValue("RetentionSize", value);
		}
	}

	public DeviceStickerSizeCollection StickerSizes
	{
		get
		{
			if (_stickerSizes == null)
			{
				IEnumerable<DeviceStickerSize> range = from p in ObjectProperties.GetNestedPropertyCollection("StickerSizes")
					select new DeviceStickerSize(new Guid(p.Key), long.Parse((string)p.Value, CultureInfo.InvariantCulture));
				_stickerSizes = new DeviceStickerSizeCollection();
				_stickerSizes.AddRange(range);
			}
			return _stickerSizes;
		}
	}

	public IVmoRepositories Repositories => _repositories;

	public VmoPropertyCollection ObjectProperties
	{
		get
		{
			return _objectProperties;
		}
		set
		{
			_objectProperties = value;
			_objectPropertiesBase = value.ToDictionary();
			ResetCachedObjects();
		}
	}

	public bool IsCreated { get; private set; }

	public bool IsDeleted { get; private set; }

	public MarkedData(string id, MarkedDataReference markedDataReference)
		: this(id, new VmoPropertyCollection(), markedDataReference.Repositories, isCreated: false)
	{
		Reference = markedDataReference.Reference;
		TagTime = markedDataReference.Time;
		DeviceIds = markedDataReference.Devices.Select((string d) => new Guid(d)).ToArray();
	}

	public MarkedData(string internalId, IVmoRepositories repositories)
		: this(internalId, new VmoPropertyCollection(), repositories, isCreated: false)
	{
	}

	public MarkedData(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories, bool isCreated)
	{
		_repositories = repositories;
		_objectProperties = objectProperties;
		_objectPropertiesBase = objectProperties.ToDictionary();
		ObjectProperties.SetPropertyValue("Id", internalId);
		IsCreated = isCreated;
	}

	private void ResetCachedObjects()
	{
		_stickerSizes = null;
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		VMOWebserviceObject vMOWebserviceObject = Repositories.ServerCommunication.QueryMarkedData(Id.ToString());
		if (vMOWebserviceObject == null)
		{
			throw new VmoException(VmoResources.VMO60163);
		}
		ObjectProperties = vMOWebserviceObject.Properties;
	}

	public void Create()
	{
		VMOWebserviceObject vMOWebserviceObject = Repositories.ServerCommunication.CreateMarkedData(ObjectProperties);
		ObjectProperties = vMOWebserviceObject.Properties;
		IsCreated = true;
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		VMOWebserviceObject vMOWebserviceObject = Repositories.ServerCommunication.UpdateMarkedData(new VMOWebserviceObject(ObjectProperties, new VmoRelationCollection(), Id.ToString(), typeof(MarkedData)));
		ObjectProperties = vMOWebserviceObject.Properties;
	}

	public void Delete()
	{
		ThrowIfNotCreatedOrDeleted();
		Repositories.ServerCommunication.DeleteMarkedData(Id.ToString(), MarkedDataType);
		IsDeleted = true;
	}

	public void Undo()
	{
		_objectProperties.Clear();
		_objectProperties.ImportFromDictionary(_objectPropertiesBase);
		ResetCachedObjects();
	}

	public bool IsDirty()
	{
		if (_objectProperties.Count != _objectPropertiesBase.Count)
		{
			return true;
		}
		EqualityComparer<object> equalityComparer = EqualityComparer<object>.Default;
		foreach (KeyValuePair<string, object> objectProperty in _objectProperties)
		{
			if (!_objectPropertiesBase.TryGetValue(objectProperty.Key, out var value))
			{
				return true;
			}
			if (!equalityComparer.Equals(objectProperty.Value, value))
			{
				return true;
			}
		}
		return false;
	}

	public void ThrowIfNotCreated()
	{
		if (!IsCreated)
		{
			throw new InvalidOperationException();
		}
	}

	public void ThrowIfDeleted()
	{
		if (IsDeleted)
		{
			throw new InvalidOperationException();
		}
	}

	public void ThrowIfNotCreatedOrDeleted()
	{
		ThrowIfNotCreated();
		ThrowIfDeleted();
	}
}

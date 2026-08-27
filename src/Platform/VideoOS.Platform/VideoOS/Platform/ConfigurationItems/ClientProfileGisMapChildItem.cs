using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClientProfileGisMapChildItem : IConfigurationChildItem
{
	public Dictionary<string, string> GisMapOpenStreetMapGeographicLayerValues => base.Wrapper.GetEnumValues("GisMapOpenStreetMapGeographicLayer");

	public string GisMapOpenStreetMapGeographicLayer
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GisMapOpenStreetMapGeographicLayer");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisMapOpenStreetMapGeographicLayer", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GisMapOpenStreetMapGeographicLayerLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GisMapOpenStreetMapGeographicLayerLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisMapOpenStreetMapGeographicLayerLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string GisMapOpenStreetMapAlternativeServer
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GisMapOpenStreetMapAlternativeServer");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisMapOpenStreetMapAlternativeServer", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GisMapOpenStreetMapAlternativeServerLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GisMapOpenStreetMapAlternativeServerLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisMapOpenStreetMapAlternativeServerLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> GisMapCreateLayerLocationValues => base.Wrapper.GetEnumValues("GisMapCreateLayerLocation");

	public string GisMapCreateLayerLocation
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GisMapCreateLayerLocation");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisMapCreateLayerLocation", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GisMapCreateLayerLocationLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GisMapCreateLayerLocationLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisMapCreateLayerLocationLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> GisMapCacheCleanUpValues => base.Wrapper.GetEnumValues("GisMapCacheCleanUp");

	public string GisMapCacheCleanUp
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GisMapCacheCleanUp");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisMapCacheCleanUp", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GisMapCacheCleanUpLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GisMapCacheCleanUpLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisMapCacheCleanUpLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string GisMapBingMapKey
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GisMapBingMapKey");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisMapBingMapKey", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GisMapBingMapKeyLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GisMapBingMapKeyLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisMapBingMapKeyLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string GisMapGoogleMapClientId
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GisMapGoogleMapClientId");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisMapGoogleMapClientId", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GisMapGoogleMapClientIdLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GisMapGoogleMapClientIdLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisMapGoogleMapClientIdLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string GisMapGoogleMapPrivateKey
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GisMapGoogleMapPrivateKey");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisMapGoogleMapPrivateKey", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GisMapGoogleMapPrivateKeyLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GisMapGoogleMapPrivateKeyLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisMapGoogleMapPrivateKeyLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string GisMapGoogleMapSigningSecret
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GisMapGoogleMapSigningSecret");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisMapGoogleMapSigningSecret", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GisMapGoogleMapSigningSecretLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GisMapGoogleMapSigningSecretLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisMapGoogleMapSigningSecretLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public ClientProfileGisMapChildItem(ServerId serverId, ConfigurationItem clientProfileGisMapChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, clientProfileGisMapChildItem, parentWrapper)
	{
	}

	public ClientProfileGisMapChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClientProfileGisMapChildItem(FQID fqid)
		: base(fqid)
	{
	}
}

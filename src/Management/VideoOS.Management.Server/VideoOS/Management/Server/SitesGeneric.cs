using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("MasterSite")]
[EntityName("ChildSite")]
[EntityName("Site")]
public class SitesGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		Site site = null;
		try
		{
			site = Site.QuerySite(requestContext, id);
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (SecurityObjectDoesNotExistException)
		{
		}
		return ConvertToGenericObject2(site);
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s((clientVersion >= 1) ? Site.QuerySites(requestContext, ids) : Site.QuerySitesLegacy(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (string.Equals(parentEntityName, "ManagementServer", StringComparison.Ordinal) || (string.Equals(parentEntityName, "ChildSite", StringComparison.Ordinal) && string.Equals(entityName, "ChildSite", StringComparison.Ordinal)))
		{
			if (clientVersion < 1)
			{
				return Site.QuerySlaveSiteIdsLegacy(requestContext, parentId);
			}
			return Site.QuerySlaveSiteIds(requestContext, parentId);
		}
		return new string[0];
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public static GenericObject2 AttachSlaveSite(IGeminiRequestContext requestContext, string slaveSiteId, string slaveSiteAddressesXml, string slaveSiteVersion)
	{
		ServerAddress[] slaveSiteAddresses = ServerAddresses.GetFromXml(slaveSiteAddressesXml).ToArray();
		return ConvertToGenericObject2(Site.AttachSlaveSite(requestContext, slaveSiteId, slaveSiteAddresses, slaveSiteVersion));
	}

	public static GenericObject2 AttachMasterSite(IGeminiRequestContext requestContext, string masterSiteId, string masterSiteAddressesXml, string masterSiteVersion)
	{
		ServerAddress[] masterSiteAddresses = ServerAddresses.GetFromXml(masterSiteAddressesXml).ToArray();
		return ConvertToGenericObject2(Site.AttachMasterSite(requestContext, masterSiteId, masterSiteAddresses, masterSiteVersion));
	}

	public static GenericObject2 AuthroizeMaster(GeminiRequestContext requestContext)
	{
		return ConvertToGenericObject2(Site.AuthorizeMaster(requestContext));
	}

	private static GenericObject2 ConvertToGenericObject2(Site site)
	{
		if (site == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", site.Id));
		list.Add(new ObjectProperty2("Name", site.Name));
		list.Add(new ObjectProperty2("Description", site.Description));
		list.Add(new ObjectProperty2("LastModified", site.LastModified));
		list.Add(new ObjectProperty2("Addresses", ServerAddresses.GetAsXml(site.Addresses)));
		list.Add(new ObjectProperty2("ConnectionState", site.ConnectionState.ToString()));
		list.Add(new ObjectProperty2("LastStatusHandshake", site.LastStatusHandshake));
		list.Add(new ObjectProperty2("MarkedInconsistent", site.MarkedInconsistent));
		list.Add(new ObjectProperty2("MasterSiteId", site.MasterSiteId));
		list.Add(new ObjectProperty2("ServiceAccount", site.ServiceAccount));
		list.Add(new ObjectProperty2("SynchronizationStatus", site.SynchronizationStatus));
		list.Add(new ObjectProperty2("Version", site.Version));
		List<RelationProperty> list2 = new List<RelationProperty>();
		if (site.RelatedSlaveSiteIds != null)
		{
			string[] relatedSlaveSiteIds = site.RelatedSlaveSiteIds;
			foreach (string id in relatedSlaveSiteIds)
			{
				list2.Add(new RelationProperty("Site", id));
			}
		}
		if (site.SiteType == SiteType.Slave)
		{
			return new GenericObject2(site.Id, "ChildSite", list.ToArray(), list2.ToArray());
		}
		if (site.SiteType == SiteType.Master)
		{
			return new GenericObject2(site.Id, "MasterSite", list.ToArray(), list2.ToArray());
		}
		return null;
	}

	private static GenericObject2[] ConvertToGenericObject2s(Site[] sites)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		for (int i = 0; i < sites.Length; i++)
		{
			GenericObject2 genericObject = ConvertToGenericObject2(sites[i]);
			if (genericObject != null)
			{
				list.Add(genericObject);
			}
		}
		return list.ToArray();
	}
}

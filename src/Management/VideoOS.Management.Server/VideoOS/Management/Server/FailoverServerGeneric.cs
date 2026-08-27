using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("FailoverServer")]
[FunctionHandler("SetFailoverRecorderInformation")]
public sealed class FailoverServerGeneric : IGenericObjectSupport2, IGenericFunction
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(FailoverServer.QueryFailoverServer(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(FailoverServer.QueryFailoverServersByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		parentId = ConvertManagementServerIdToTopServerId(parentEntityName, parentId);
		FailoverServer[] array = FailoverServer.QueryFailoverServers(requestContext, parentId);
		List<string> list = new List<string>();
		FailoverServer[] array2 = array;
		foreach (FailoverServer failoverServer in array2)
		{
			list.Add(failoverServer.Id);
		}
		return list.ToArray();
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		FailoverServer failoverServer = ConvertToFailoverServer(properties);
		if (IsManagementServerId(requestContext, failoverServer.ParentGroupId))
		{
			failoverServer.ParentGroupId = ConvertManagementServerIdToTopServerId("ManagementServer", failoverServer.ParentGroupId);
		}
		failoverServer = FailoverServer.CreateFailoverServer(requestContext, failoverServer.ParentGroupId, failoverServer);
		return ConvertToGenericObject2(failoverServer);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		FailoverServer failoverServer = ConvertToFailoverServer(genericObject.Properties);
		failoverServer = FailoverServer.UpdateFailoverServer(requestContext, failoverServer);
		return ConvertToGenericObject2(failoverServer);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		if (FailoverServer.QueryFailoverServer(requestContext, id) == null)
		{
			throw new FailoverServerNotFoundException();
		}
		FailoverServer.DeleteFailoverServer(requestContext, id);
	}

	public static void SetEnabled(IGeminiRequestContext requestContext, string failoverServerId, bool enable)
	{
		FailoverServer.SetEnabled(requestContext, failoverServerId, enable);
	}

	public static void Move(IGeminiRequestContext requestContext, string failoverServerId, string newParentId, int insertAtPosition)
	{
		FailoverServer.Move(requestContext, failoverServerId, newParentId, insertAtPosition);
	}

	private static bool IsManagementServerId(IGeminiRequestContext requestContext, string id)
	{
		Site site = Site.QueryHomeSiteNoSecurity(requestContext);
		if (string.Equals(id, site.Id, StringComparison.OrdinalIgnoreCase) || string.Equals(id, "909749AE-88C4-4780-A381-383D691E98FA", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}

	private static GenericObject2[] SetFailoverRecorderInformation(IGeminiRequestContext requestContext, GenericObject2[] parameters)
	{
		GenericObject2 genericObject = parameters.FirstOrDefault();
		Validation.CheckObjectReference(genericObject, "parameters");
		Validation.CheckObjectReference(genericObject.Properties, "properties");
		string propertyOrDefault = genericObject.Properties.GetPropertyOrDefault<string>("Id");
		bool? propertyOrNullable = genericObject.Properties.GetPropertyOrNullable<bool>("ClientEncryptionEnabled");
		string propertyOrDefault2 = genericObject.Properties.GetPropertyOrDefault<string>("WebServerHostName");
		int? propertyOrNullable2 = genericObject.Properties.GetPropertyOrNullable<int>("WebServerPort");
		string propertyOrDefault3 = genericObject.Properties.GetPropertyOrDefault<string>("Version");
		string propertyOrDefault4 = genericObject.Properties.GetPropertyOrDefault<string>("TimeZone");
		FailoverServer.SetRecorderInformation(requestContext, propertyOrDefault, propertyOrNullable, propertyOrDefault2, propertyOrNullable2, propertyOrDefault3, propertyOrDefault4);
		return null;
	}

	public GenericObject2[] Execute(IGeminiRequestContext requestContext, string functionName, GenericObject2[] parameters, int clientVersion)
	{
		if (functionName == "SetFailoverRecorderInformation")
		{
			return SetFailoverRecorderInformation(requestContext, parameters);
		}
		throw VmoExceptionFactory.VMO60311_VmoFunctionNotSupportedException(functionName ?? "<Null>");
	}

	private static string ConvertManagementServerIdToTopServerId(string parentEntityName, string parentGroupId)
	{
		if (string.Equals(parentEntityName, "ManagementServer", StringComparison.OrdinalIgnoreCase) && !string.Equals(parentGroupId, "D7BF5260-0B76-45F3-B920-9A4CC56D6CD5", StringComparison.OrdinalIgnoreCase))
		{
			parentGroupId = "6A85FFC3-C134-4338-A072-024817CB5820";
		}
		return parentGroupId;
	}

	private static GenericObject2[] ConvertToGenericObject2s(FailoverServer[] failoverServers)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		for (int i = 0; i < failoverServers.Length; i++)
		{
			GenericObject2 genericObject = ConvertToGenericObject2(failoverServers[i]);
			if (genericObject != null)
			{
				list.Add(genericObject);
			}
		}
		return list.ToArray();
	}

	private static GenericObject2 ConvertToGenericObject2(FailoverServer failoverServer)
	{
		if (failoverServer == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", failoverServer.Id));
		list.Add(new ObjectProperty2("Name", failoverServer.Name));
		list.Add(new ObjectProperty2("Description", failoverServer.Description));
		list.Add(new ObjectProperty2("CreatedDate", failoverServer.CreatedDate));
		list.Add(new ObjectProperty2("LastModified", failoverServer.LastModified));
		list.Add(new ObjectProperty2("HotStandbyRecorderId", failoverServer.HotStandbyRecorderId));
		list.Add(new ObjectProperty2("Position", failoverServer.Position));
		list.Add(new ObjectProperty2("Status", ServiceStatus.Running.ToString()));
		list.Add(new ObjectProperty2("Enabled", failoverServer.Enabled));
		list.Add(new ObjectProperty2("HostName", failoverServer.HostName));
		list.Add(new ObjectProperty2("UdpPort", failoverServer.UdpPort));
		list.Add(new ObjectProperty2("DatabasePath", failoverServer.DatabasePath));
		list.Add(new ObjectProperty2("WebServerUri", failoverServer.WebServerUri));
		list.Add(new ObjectProperty2("PublicAccessEnabled", failoverServer.PublicAccessEnabled));
		list.Add(new ObjectProperty2("PublicWebServerHostName", failoverServer.PublicWebServerHostName));
		list.Add(new ObjectProperty2("PublicWebServerPort", failoverServer.PublicWebServerPort));
		list.Add(new ObjectProperty2("WebServerUriConfiguration", failoverServer.WebServerUriConfiguration));
		list.Add(new ObjectProperty2("ServiceId", failoverServer.ServiceId));
		list.Add(new ObjectProperty2("x64", propertyValue: true));
		string[] array = failoverServer.RelationIds.Split(';');
		bool flag = false;
		List<RelationProperty> list2 = new List<RelationProperty>();
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (string.IsNullOrEmpty(text))
			{
				continue;
			}
			if (ServerGroup.IsSpecialType(text, "6A85FFC3-C134-4338-A072-024817CB5820"))
			{
				if (!flag)
				{
					list.Add(new ObjectProperty2("ParentServerGroupId", "6A85FFC3-C134-4338-A072-024817CB5820"));
					flag = true;
				}
				break;
			}
			if (!flag)
			{
				list.Add(new ObjectProperty2("ParentServerGroupId", text));
				flag = true;
			}
			list2.Add(new RelationProperty("FailoverServerGroup", text, isWeakRelation: true));
		}
		return new GenericObject2(failoverServer.Id, "FailoverServer", list.ToArray(), list2.ToArray());
	}

	private static FailoverServer ConvertToFailoverServer(ObjectProperty2[] properties)
	{
		FailoverServer failoverServer = new FailoverServer();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			failoverServer.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		else
		{
			failoverServer.Id = string.Empty;
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			failoverServer.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			failoverServer.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			failoverServer.CreatedDate = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			failoverServer.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "HotStandbyRecorderId"))
		{
			failoverServer.HotStandbyRecorderId = GenericObjectHelper2.GetProperty<string>(properties, "HotStandbyRecorderId");
		}
		if (GenericObjectHelper2.Contains(properties, "Position"))
		{
			failoverServer.Position = GenericObjectHelper2.GetProperty<int>(properties, "Position");
		}
		if (GenericObjectHelper2.Contains(properties, "Enabled"))
		{
			failoverServer.Enabled = GenericObjectHelper2.GetProperty<bool>(properties, "Enabled");
		}
		if (GenericObjectHelper2.Contains(properties, "HostName"))
		{
			failoverServer.HostName = GenericObjectHelper2.GetProperty<string>(properties, "HostName");
		}
		if (GenericObjectHelper2.Contains(properties, "UdpPort"))
		{
			failoverServer.UdpPort = GenericObjectHelper2.GetProperty<int>(properties, "UdpPort");
		}
		if (GenericObjectHelper2.Contains(properties, "DatabasePath"))
		{
			failoverServer.DatabasePath = GenericObjectHelper2.GetProperty<string>(properties, "DatabasePath");
		}
		if (GenericObjectHelper2.Contains(properties, "PublicAccessEnabled"))
		{
			failoverServer.PublicAccessEnabled = GenericObjectHelper2.GetProperty<bool>(properties, "PublicAccessEnabled");
		}
		if (GenericObjectHelper2.Contains(properties, "PublicWebServerHostName"))
		{
			failoverServer.PublicWebServerHostName = GenericObjectHelper2.GetProperty<string>(properties, "PublicWebServerHostName");
		}
		if (GenericObjectHelper2.Contains(properties, "PublicWebServerPort"))
		{
			failoverServer.PublicWebServerPort = GenericObjectHelper2.GetProperty<int>(properties, "PublicWebServerPort");
		}
		if (GenericObjectHelper2.Contains(properties, "ServiceId"))
		{
			failoverServer.ServiceId = GenericObjectHelper2.GetProperty<string>(properties, "ServiceId");
		}
		if (GenericObjectHelper2.Contains(properties, "ParentServerGroupId"))
		{
			failoverServer.ParentGroupId = GenericObjectHelper2.GetProperty<string>(properties, "ParentServerGroupId");
		}
		if (GenericObjectHelper2.Contains(properties, "ClientEncryptionEnabled"))
		{
			failoverServer.ClientEncryptionEnabled = GenericObjectHelper2.GetProperty<bool>(properties, "ClientEncryptionEnabled");
		}
		if (GenericObjectHelper2.Contains(properties, "WebServerHostName"))
		{
			failoverServer.WebServerHostName = GenericObjectHelper2.GetProperty<string>(properties, "WebServerHostName");
		}
		if (GenericObjectHelper2.Contains(properties, "WebServerPort"))
		{
			failoverServer.WebServerPort = GenericObjectHelper2.GetProperty<int>(properties, "WebServerPort");
		}
		return failoverServer;
	}
}

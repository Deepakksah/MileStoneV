using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using VideoOS.Common.Integration;
using VideoOS.Common.SharedComponent;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;
using VideoOS.Management.Common;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public static class ServerCommandServiceViewHandling
{
	private static string _smartClientRootGroupId = "430C2FE2-B233-42DA-B508-1A22EF03383D";

	public static ServerGroup.EnumStructureType _viewGroupStructureType = ServerGroup.EnumStructureType.ViewGroup;

	public static ViewGroupInfo[] GetViewGroups(IGeminiRequestContext requestContext)
	{
		try
		{
			if (requestContext == null)
			{
				throw new ArgumentNullException("requestContext");
			}
			List<ViewGroupInfo> list = new List<ViewGroupInfo>();
			List<ServerGroup> list2 = new List<ServerGroup>();
			Guid privateViewGroupId = CommonIds.PrivateViewGroupId;
			ServerGroup item = ServerGroup.QueryPrivateViewGroup(requestContext, requestContext.AuthenticatedUserSid);
			list2.Add(item);
			ServerGroup[] source = ServerGroup.QueryServerGroupsByParentGroupIdNoSecurity(requestContext, _viewGroupStructureType, _smartClientRootGroupId);
			list2.AddRange(source.Where((ServerGroup s) => !string.Equals(s.Id, CommonIds.PrivateViewGroupId.ToString(), StringComparison.OrdinalIgnoreCase) && !string.Equals(s.Id, _smartClientRootGroupId, StringComparison.OrdinalIgnoreCase)));
			foreach (ServerGroup item2 in list2)
			{
				ServerGroup serverGroup = item2;
				if (!string.Equals(serverGroup.Id, privateViewGroupId.ToString(), StringComparison.OrdinalIgnoreCase))
				{
					IsPermitted(requestContext, requestContext.AuthenticatedUserSid, serverGroup.Id, out var readAllowed, out var writeAllowed);
					if (readAllowed)
					{
						ViewGroupInfo viewGroupInfo = new ViewGroupInfo();
						viewGroupInfo.Id = new Guid(serverGroup.Id);
						viewGroupInfo.Name = item2.Name;
						viewGroupInfo.ReadOnly = !writeAllowed;
						viewGroupInfo.ViewGroupType = ViewGroupType.Normal;
						viewGroupInfo.LastModifiedUser = string.Empty;
						viewGroupInfo.LastModifiedTime = serverGroup.LastModified;
						list.Add(viewGroupInfo);
					}
				}
			}
			return list.ToArray();
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	public static ViewGroup GetViewGroupData(IGeminiRequestContext requestContext, Guid viewGroupId)
	{
		try
		{
			if (requestContext == null)
			{
				throw new ArgumentNullException("requestContext");
			}
			ViewGroup viewGroup = null;
			ServerGroup serverGroup;
			try
			{
				serverGroup = ServerGroup.QueryServerGroupByParentGroupId(requestContext, _smartClientRootGroupId, viewGroupId.ToString());
			}
			catch (ServerGroupNotFoundException)
			{
				serverGroup = null;
			}
			string text = string.Empty;
			Guid privateViewGroupId = CommonIds.PrivateViewGroupId;
			bool readAllowed = false;
			bool writeAllowed = false;
			if (serverGroup == null)
			{
				ServerGroup serverGroup2;
				try
				{
					serverGroup2 = ServerGroup.QueryServerGroupByParentGroupId(requestContext, _smartClientRootGroupId, privateViewGroupId.ToString());
				}
				catch
				{
					serverGroup2 = null;
				}
				if (serverGroup2 != null)
				{
					serverGroup = ServerGroup.QueryServerGroupByParentGroupId(requestContext, serverGroup2.Id, viewGroupId.ToString());
					text = serverGroup2.Name;
					readAllowed = true;
					writeAllowed = true;
				}
			}
			else
			{
				IsPermitted(requestContext, requestContext.AuthenticatedUserSid, serverGroup.Id, out readAllowed, out writeAllowed);
			}
			if (serverGroup != null)
			{
				if (!readAllowed)
				{
					return null;
				}
				ViewGroupInfo viewGroupInfo = new ViewGroupInfo();
				viewGroupInfo.Id = new Guid(serverGroup.Id);
				if (string.IsNullOrEmpty(text))
				{
					text = serverGroup.Name;
				}
				viewGroupInfo.Name = text;
				viewGroupInfo.ReadOnly = !writeAllowed;
				viewGroupInfo.ViewGroupType = ViewGroupType.Normal;
				viewGroupInfo.LastModifiedUser = string.Empty;
				viewGroupInfo.LastModifiedTime = serverGroup.LastModified;
				viewGroup = new ViewGroup();
				viewGroup.ViewGroupInfo = viewGroupInfo;
				viewGroup.ViewGroupData = new VideoOS.WebService.Common.ServerCommandService.ViewGroupData();
				viewGroup.ViewGroupData.DataVersion = 0;
				viewGroup.ViewGroupData.XmlVersion = 2;
				string viewGroupData = ViewHandling.GetViewGroupData(GetViewGroupHierarchy(requestContext, serverGroup), new List<Guid>
				{
					new Guid(serverGroup.Id)
				});
				if (!string.IsNullOrEmpty(viewGroupData))
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(viewGroupData);
					viewGroup.ViewGroupData.ViewGroupDataXml = xmlDocument;
				}
				return viewGroup;
			}
		}
		catch (Exception ex2)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex2, null);
			throw;
		}
		return null;
	}

	private static IViewGroup GetViewGroupHierarchy(IGeminiRequestContext requestContext, ServerGroup serverGroup)
	{
		ViewItem[] viewItems = View.QueryViewItemHeriarchyByServerGroupIdNoSecurity(requestContext, serverGroup.Id);
		View[] views = View.QueryViewHeriarchyByServerGroupIdNoSecurity(requestContext, serverGroup.Id);
		CustomProperty[] customPropertiesByServerGroupHierarchyNoSecurity = CustomProperty.GetCustomPropertiesByServerGroupHierarchyNoSecurity(requestContext, serverGroup.Id);
		ServerGroup[] groups = ServerGroup.QueryServerGroupHeriarchyByIdNoSecurity(requestContext, serverGroup.Id);
		Dictionary<string, ServerIViewLightCollection> allViews = PopulateAllViews(views, viewItems);
		Dictionary<string, ServerICustomPropertyLightCollection> allCustomProperties = PopulateCustomProperties(customPropertiesByServerGroupHierarchyNoSecurity);
		Dictionary<string, ServerIViewGroupLightCollection> allGroups = PopulateViewGroups(groups, allViews, allCustomProperties);
		ServerIViewGroupLight serverIViewGroupLight = CreateTopGroup(serverGroup, allViews, allGroups, allCustomProperties);
		PopulateChildGroups(allGroups, serverIViewGroupLight);
		return serverIViewGroupLight;
	}

	private static Dictionary<string, ServerIViewLightCollection> PopulateAllViews(View[] views, ViewItem[] viewItems)
	{
		Dictionary<string, ServerIViewLightCollection> dictionary = new Dictionary<string, ServerIViewLightCollection>();
		Dictionary<string, ServerIViewItemLightCollection> dictionary2 = (dictionary2 = new Dictionary<string, ServerIViewItemLightCollection>());
		foreach (ViewItem viewItem in viewItems)
		{
			if (!dictionary2.TryGetValue(viewItem.ViewId, out var value))
			{
				value = new ServerIViewItemLightCollection();
				dictionary2.Add(viewItem.ViewId, value);
			}
			value.Add(new ServerIViewItemLight
			{
				Id = new Guid(viewItem.Id),
				DefinitionXml = viewItem.DefinitionXml,
				Position = viewItem.Position
			});
		}
		foreach (View view in views)
		{
			if (!dictionary.TryGetValue(view.ViewGroupId, out var value2))
			{
				value2 = new ServerIViewLightCollection();
				dictionary.Add(view.ViewGroupId, value2);
			}
			if (!dictionary2.TryGetValue(view.Id, out var value3))
			{
				value3 = new ServerIViewItemLightCollection();
			}
			value2.Add(new ServerIViewLight
			{
				Id = new Guid(view.Id),
				DefinitionXml = view.DefinitionXml,
				Name = view.Name,
				ViewItems = value3
			});
		}
		return dictionary;
	}

	private static Dictionary<string, ServerICustomPropertyLightCollection> PopulateCustomProperties(CustomProperty[] customProperties)
	{
		Dictionary<string, ServerICustomPropertyLightCollection> dictionary = new Dictionary<string, ServerICustomPropertyLightCollection>();
		foreach (CustomProperty customProperty in customProperties)
		{
			if (!dictionary.TryGetValue(customProperty.ObjectId, out var value))
			{
				value = new ServerICustomPropertyLightCollection();
				dictionary.Add(customProperty.ObjectId, value);
			}
			value.Add(new ServerICustomPropertyLight
			{
				Key = customProperty.Name,
				Value = customProperty.Value
			});
		}
		return dictionary;
	}

	private static Dictionary<string, ServerIViewGroupLightCollection> PopulateViewGroups(ServerGroup[] groups, Dictionary<string, ServerIViewLightCollection> allViews, Dictionary<string, ServerICustomPropertyLightCollection> allCustomProperties)
	{
		Dictionary<string, ServerIViewGroupLightCollection> dictionary = new Dictionary<string, ServerIViewGroupLightCollection>();
		foreach (ServerGroup serverGroup in groups)
		{
			if (!dictionary.TryGetValue(serverGroup.ParentId, out var value))
			{
				value = new ServerIViewGroupLightCollection();
				dictionary.Add(serverGroup.ParentId, value);
			}
			if (!allViews.TryGetValue(serverGroup.Id, out var value2))
			{
				value2 = new ServerIViewLightCollection();
			}
			if (!allCustomProperties.TryGetValue(serverGroup.Id, out var value3))
			{
				value3 = new ServerICustomPropertyLightCollection();
			}
			value.Add(new ServerIViewGroupLight
			{
				Id = new Guid(serverGroup.Id),
				Name = serverGroup.Name,
				Views = value2,
				CustomProperties = value3
			});
		}
		return dictionary;
	}

	private static ServerIViewGroupLight CreateTopGroup(ServerGroup serverGroup, Dictionary<string, ServerIViewLightCollection> allViews, Dictionary<string, ServerIViewGroupLightCollection> allGroups, Dictionary<string, ServerICustomPropertyLightCollection> allCustomProperties)
	{
		if (!allViews.TryGetValue(serverGroup.Id, out var value))
		{
			value = new ServerIViewLightCollection();
		}
		if (!allGroups.TryGetValue(serverGroup.Id.ToString(), out var value2))
		{
			value2 = new ServerIViewGroupLightCollection();
		}
		if (!allCustomProperties.TryGetValue(serverGroup.Id, out var value3))
		{
			value3 = new ServerICustomPropertyLightCollection();
		}
		return new ServerIViewGroupLight
		{
			Id = new Guid(serverGroup.Id),
			Name = serverGroup.Name,
			Views = value,
			ViewGroups = value2,
			CustomProperties = value3
		};
	}

	private static void PopulateChildGroups(Dictionary<string, ServerIViewGroupLightCollection> allGroups, ServerIViewGroupLight topGroup)
	{
		if (!allGroups.TryGetValue(topGroup.Id.ToString().ToUpper(CultureInfo.InvariantCulture), out var value))
		{
			value = new ServerIViewGroupLightCollection();
		}
		foreach (IViewGroup item in value)
		{
			PopulateChildGroups(allGroups, item as ServerIViewGroupLight);
		}
		topGroup.ViewGroups = value;
	}

	public static SetViewGroupDataResultType SetViewGroupData(IGeminiRequestContext requestContext, Guid viewGroupId, VideoOS.WebService.Common.ServerCommandService.ViewGroupData viewGroupData)
	{
		try
		{
			if (viewGroupData == null)
			{
				throw new ArgumentNullException("viewGroupData");
			}
			VideoOS.Common.SharedComponent.ViewGroupData newViewGroupdata = VideoOS.Common.SharedComponent.ViewGroupData.CreateFromXML(viewGroupData.ViewGroupDataXml.OuterXml);
			IViewGroups viewGroups = new ServerIViewGroupCollection(requestContext, _smartClientRootGroupId);
			return ViewHandling.SetViewGroupData(viewGroupId, newViewGroupdata, viewGroups) switch
			{
				ViewHandling.SetViewGroupDataResultType.ReadOnlyError => SetViewGroupDataResultType.ReadOnlyError, 
				ViewHandling.SetViewGroupDataResultType.Success => SetViewGroupDataResultType.Success, 
				ViewHandling.SetViewGroupDataResultType.UnspecifiedError => SetViewGroupDataResultType.UnspecifiedError, 
				_ => SetViewGroupDataResultType.UnspecifiedError, 
			};
		}
		catch
		{
			return SetViewGroupDataResultType.UnspecifiedError;
		}
	}

	private static void IsPermitted(IGeminiRequestContext requestContext, string userSid, string objectId, out bool readAllowed, out bool writeAllowed)
	{
		readAllowed = requestContext.GetService<SecurityManagerService>().HasPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, userSid, "GENERIC_READ", ServerGroup.GetRootViewGroupId(requestContext, objectId));
		writeAllowed = requestContext.GetService<SecurityManagerService>().HasPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, userSid, "OPERATE", ServerGroup.GetRootViewGroupId(requestContext, objectId));
	}
}

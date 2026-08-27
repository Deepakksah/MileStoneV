using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "ServerGroupInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public sealed class ServerGroupOld
{
	[DataMember]
	public string Id { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Description { get; set; }

	[DataMember]
	public string Path { get; set; }

	[DataMember]
	public string ParentId { get; set; }

	[DataMember]
	public ServerGroupOptions ServerGroupOptions { get; set; }

	[DataMember]
	public DateTime CreatedDate { get; set; }

	[DataMember]
	public DateTime LastModified { get; set; }

	public string StructureType { get; set; }

	public static ServerGroupOld[] QueryServerGroups(IGeminiRequestContext requestContext, string groupStructureType, string parentGroupId)
	{
		Validation.CheckGroupStructureType(ref groupStructureType, "groupStructureType", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref parentGroupId, "parentGroupId", allowNullOrEmpty: false);
		ServerGroup.EnumStructureType structureType = ServerGroup.ConvertFromStringStructureTypeToEnumStructureType(groupStructureType);
		return ConvertToServerGroupOlds(ServerGroup.QueryServerGroupsByParentGroupId(requestContext, structureType, parentGroupId));
	}

	public static ServerGroupOld CreateServerGroup(IGeminiRequestContext requestContext, ServerGroupOld serverGroupOld)
	{
		ServerGroup serverGroup = ConvertToServerGroup(serverGroupOld);
		serverGroup.Id = GeminiResourceComponent.GetGuid(serverGroupOld.ServerGroupOptions.UserDefinedId);
		return ConvertToServerGroupOld(ServerGroup.CreateServerGroup(requestContext, serverGroup));
	}

	private static ServerGroupOld ConvertToServerGroupOld(ServerGroup serverGroup)
	{
		ServerGroupOld serverGroupOld = new ServerGroupOld();
		serverGroupOld.Id = serverGroup.Id;
		serverGroupOld.Name = serverGroup.Name;
		serverGroupOld.CreatedDate = serverGroup.CreatedDate;
		serverGroupOld.LastModified = serverGroup.LastModified;
		serverGroupOld.Description = serverGroup.Description;
		serverGroupOld.ParentId = serverGroup.ParentId;
		serverGroupOld.Path = serverGroup.Path;
		serverGroupOld.ServerGroupOptions = new ServerGroupOptions();
		serverGroupOld.ServerGroupOptions.UserDefinedId = serverGroup.Id;
		serverGroupOld.ServerGroupOptions.CustomProperties = serverGroup.CustomProperties;
		serverGroupOld.StructureType = ServerGroup.ConvertFromEnumStructureTypeToStringStructureType(serverGroup.StructureType);
		return serverGroupOld;
	}

	private static ServerGroupOld[] ConvertToServerGroupOlds(ServerGroup[] serverGroups)
	{
		List<ServerGroupOld> list = new List<ServerGroupOld>();
		foreach (ServerGroup serverGroup in serverGroups)
		{
			list.Add(ConvertToServerGroupOld(serverGroup));
		}
		return list.ToArray();
	}

	private static ServerGroup ConvertToServerGroup(ServerGroupOld serverGroupOld)
	{
		return new ServerGroup
		{
			Id = serverGroupOld.Id,
			Name = serverGroupOld.Name,
			CreatedDate = serverGroupOld.CreatedDate,
			LastModified = serverGroupOld.LastModified,
			Description = serverGroupOld.Description,
			ParentId = serverGroupOld.ParentId,
			Path = serverGroupOld.Path,
			CustomProperties = serverGroupOld.ServerGroupOptions.CustomProperties,
			StructureType = ServerGroup.ConvertFromStringStructureTypeToEnumStructureType(serverGroupOld.StructureType)
		};
	}
}

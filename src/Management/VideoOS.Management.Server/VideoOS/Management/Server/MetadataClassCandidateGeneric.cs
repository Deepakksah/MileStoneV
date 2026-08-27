using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("MetadataClassCandidate")]
public class MetadataClassCandidateGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertMetadataClassCandidateToGenericObject2(MetadataClassCandidate.QueryClassCandidateById(requestContext, id));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "MetadataCategory" && entityName == "MetadataClassCandidate")
		{
			return MetadataClassCandidate.QueryClassCandidateIdsByCategoryId(requestContext, parentId);
		}
		return null;
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertMetadataClassCandidatesToGenericObject2s(MetadataClassCandidate.QueryClassCandidatesByIds(requestContext, ids));
	}

	private static GenericObject2 ConvertMetadataClassCandidateToGenericObject2(MetadataClassCandidate classCandidate)
	{
		if (classCandidate == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", classCandidate.Id));
		list.Add(new ObjectProperty2("Name", classCandidate.Name));
		return new GenericObject2(classCandidate.Id, "MetadataClassCandidate", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertMetadataClassCandidatesToGenericObject2s(MetadataClassCandidate[] classCandidates)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (MetadataClassCandidate classCandidate in classCandidates)
		{
			list.Add(ConvertMetadataClassCandidateToGenericObject2(classCandidate));
		}
		return list.ToArray();
	}

	GenericObject2 IGenericObjectSupport2.UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	GenericObject2 IGenericObjectSupport2.CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	void IGenericObjectSupport2.DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		throw new NotSupportedException();
	}
}

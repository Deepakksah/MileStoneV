using System;

namespace VideoOS.Management.Server;

public interface IMetadataSearchComponent : IDisposable
{
	MetadataFilter QueryFilterById(string id);

	MetadataFilter[] QueryFiltersByIds(string[] ids);

	string[] QueryFilterIdsByCategoryId(string categoryId);

	void UpdateFilterVisibility(string id, bool visibility);

	MetadataCategory QueryCategoryById(string id);

	MetadataCategory[] QueryCategoriesByIds(string[] ids);

	string[] QueryCategoryIds();

	void UpdateCategoryVisibility(string id, bool visibility);

	MetadataObjectDefinition QueryObjectDefinitionById(string id);

	MetadataObjectDefinition[] QueryObjectDefinitionsByIds(string[] ids);

	string[] QuerySearchPathIdsByObjectDefinitionId(string objectDefinitionId);

	MetadataSearchPath QuerySearchPathById(string id);

	MetadataSearchPath[] QuerySearchPathsByIds(string[] ids);

	string[] QueryObjectDefinitionIds();

	MetadataClassCandidate QueryClassCandidateById(string id);

	MetadataClassCandidate[] QueryClassCandidatesByIds(string[] ids);

	string[] QueryClassCandidateIdsByCategoryId(string categoryId);

	MetadataProperty QueryPropertyById(string id);

	MetadataProperty[] QueryPropertiesByIds(string[] ids);

	string[] QueryPropertyIdsByFilterId(string filterId);
}

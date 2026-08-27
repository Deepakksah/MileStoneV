using System;

namespace VideoOS.Platform.Proxy.RestApi.ConfigApiRelated;

public class LinkValue : IRestSerializer
{
	public string Relation { get; set; }

	public string ResourcePlural { get; set; }

	public string Id { get; set; }

	public INameConvert NameConvert { get; set; } = new NameConvert();

	public LinkValue()
	{
	}

	public LinkValue(string relation, string itemType, string id)
	{
		if (string.IsNullOrEmpty(relation))
		{
			throw new ArgumentException("Must have a relation", "relation");
		}
		if (itemType == "System" && string.IsNullOrEmpty(id))
		{
			id = Guid.Empty.ToString();
		}
		if (string.IsNullOrEmpty(id))
		{
			throw new ArgumentException("Empty id", "id");
		}
		Relation = relation;
		ResourcePlural = NameConvert.ItemTypeToResourcePlural(itemType);
		Id = ConfigApiRelatedJsonBuilder.IdToLower(id);
	}

	public LinkValue(string itemType, string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			throw new ArgumentException("Empty id", "id");
		}
		ResourcePlural = NameConvert.ItemTypeToResourcePlural(itemType);
		Id = ConfigApiRelatedJsonBuilder.IdToLower(id);
	}

	public string Serialize(bool NoData)
	{
		ConfigApiRelatedJsonBuilder configApiRelatedJsonBuilder = new ConfigApiRelatedJsonBuilder(array: false);
		configApiRelatedJsonBuilder.AppendKeyString("type", ResourcePlural);
		configApiRelatedJsonBuilder.AppendKeyString("id", Id);
		return configApiRelatedJsonBuilder.ToJson();
	}
}

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Repository;

[ClassDataProperty("RepositoryData")]
[KnownType(typeof(CADRepositoryData))]
[KnownType(typeof(FileRepositoryData))]
[KnownType(typeof(ImageRepositoryData))]
[KnownType(typeof(ShapeFileRepositoryData))]
public class RepositoryData : IUnknownDataProperties, IUnknownInstance
{
	[DataProperty]
	[JsonProperty("id")]
	public Guid Id { get; set; }

	[DataProperty]
	[JsonProperty("name")]
	public string Name { get; set; }

	[DataProperty]
	[JsonProperty("note")]
	public string Note { get; set; }

	[DataProperty]
	[JsonProperty("data")]
	public byte[] Data { get; set; }

	[JsonIgnore]
	public DataProperty[] UnknownData { get; set; }

	[JsonIgnore]
	IUnknownClassData IUnknownInstance.UnknownInstance => new UnknownRepositoryData();
}

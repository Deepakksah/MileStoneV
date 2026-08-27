using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Repository;

[ClassDataProperty("FileRepositoryData")]
public class FileRepositoryData : RepositoryData
{
	[DataProperty]
	[JsonProperty("primaryFileName")]
	public string PrimaryFileName { get; set; }

	[DataProperty]
	[JsonProperty("fileNames")]
	public string[] FileNames { get; set; }

	[DataProperty]
	[JsonProperty("originalPath")]
	public string OriginalPath { get; set; }
}

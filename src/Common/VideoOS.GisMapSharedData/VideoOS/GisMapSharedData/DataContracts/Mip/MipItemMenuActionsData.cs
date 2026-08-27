using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Mip;

[ClassDataProperty("MipItemMenuActionsData")]
public class MipItemMenuActionsData : IUnknownDataProperties, IUnknownInstance
{
	[DataProperty]
	[JsonProperty("actionId")]
	public string ActionId { get; set; }

	[DataProperty]
	[JsonProperty("text")]
	public string Text { get; set; }

	[DataProperty]
	[JsonProperty("enabled")]
	public bool Enabled { get; set; }

	[DataProperty]
	[JsonProperty("checked")]
	public bool Checked { get; set; }

	[DataProperty]
	[JsonProperty("isCheckable")]
	public bool IsCheckable { get; set; }

	[DataProperty]
	[JsonProperty("icon")]
	public byte[] Icon { get; set; }

	[DataProperty]
	[JsonProperty("subActions")]
	public MipItemMenuActionsData[] SubActions { get; set; }

	[JsonIgnore]
	public DataProperty[] UnknownData { get; set; }

	[JsonIgnore]
	IUnknownClassData IUnknownInstance.UnknownInstance => new UnknownMipItemMenuActionsData();
}

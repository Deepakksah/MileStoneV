using VideoOS.IM.Common.Dto.DtoModels.Abstractions.Common;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.Common;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.GlobalSettings;

namespace VideoOS.IM.Common.Dto.DtoModels.Implementations.GlobalSettings;

public class DtoGlobalSettings : ADtoBaseEntity, IDtoGlobalSettings, IDtoBaseEntity
{
	public string GlobalSettingsIndentificator { get; set; }

	public string ValueJSon { get; set; }
}

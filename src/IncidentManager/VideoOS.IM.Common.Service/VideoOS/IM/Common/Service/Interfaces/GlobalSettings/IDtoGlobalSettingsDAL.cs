using VideoOS.IM.Common.Dto.DtoModels.Implementations.GlobalSettings;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.GlobalSettings;
using VideoOS.IM.Common.Service.Interfaces.Common;

namespace VideoOS.IM.Common.Service.Interfaces.GlobalSettings;

public interface IDtoGlobalSettingsDAL : IDtoObjectBaseDAL<long, DtoGlobalSettings, DtoGlobalSettingsResponse, DtoGlobalSettingsListResponse>
{
}

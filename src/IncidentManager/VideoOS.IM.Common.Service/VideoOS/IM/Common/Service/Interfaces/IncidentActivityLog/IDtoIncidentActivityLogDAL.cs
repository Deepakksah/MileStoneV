using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentActivityLog;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.IncidentActivityLog;
using VideoOS.IM.Common.Service.Interfaces.Common;

namespace VideoOS.IM.Common.Service.Interfaces.IncidentActivityLog;

public interface IDtoIncidentActivityLogDAL : IDtoObjectBaseDAL<long, DtoIncidentActivityLog, DtoIncidentActivityLogResponse, DtoIncidentActivityLogListResponse>
{
}

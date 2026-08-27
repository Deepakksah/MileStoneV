using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentComment;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.IncidentComment;
using VideoOS.IM.Common.Service.Interfaces.Common;

namespace VideoOS.IM.Common.Service.Interfaces.IncidentComment;

public interface IDtoIncidentCommentDAL : IDtoObjectBaseDAL<long, DtoIncidentComment, DtoIncidentCommentResponse, DtoIncidentEvidenceListResponse>
{
}

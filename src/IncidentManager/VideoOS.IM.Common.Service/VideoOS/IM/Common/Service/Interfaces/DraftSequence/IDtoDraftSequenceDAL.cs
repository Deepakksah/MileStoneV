using VideoOS.IM.Common.Dto.DtoModels.Implementations.DraftSequence;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.DraftSequence;
using VideoOS.IM.Common.Service.Interfaces.Common;

namespace VideoOS.IM.Common.Service.Interfaces.DraftSequence;

public interface IDtoDraftSequenceDAL : IDtoObjectBaseDAL<long, DtoDraftSequence, DtoDraftSequenceResponse, DtoDraftSequenceListResponse>
{
}

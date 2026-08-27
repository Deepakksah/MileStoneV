using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentSequence;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.IncidentSequence;
using VideoOS.IM.Common.Service.Interfaces;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.WebApi.Filters;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.WebApi.Controllers.webapi.v1;

public class IncidentSequenceController : BaseController
{
	public IncidentSequenceController()
	{
	}

	public IncidentSequenceController(IDtoDAL dataAccessLayer)
		: base(dataAccessLayer)
	{
	}

	[CheckPermission("GENERIC_READ", "77F8F2A9-9DE8-4048-8D63-26BF44DCCED1")]
	public async Task<DtoIncidentSequenceListResponse> GetAsync(string inPagingJson, CancellationToken cancellationToken)
	{
		DtoPaging inPaging = null;
		if (!string.IsNullOrWhiteSpace(inPagingJson))
		{
			inPaging = JsonConvert.DeserializeObject<DtoPaging>(inPagingJson);
		}
		return await DataAccessLayer.GetIncidentSequenceDAL().GetListAsync(inPaging, cancellationToken);
	}

	[CheckPermission("GENERIC_READ", "77F8F2A9-9DE8-4048-8D63-26BF44DCCED1")]
	public async Task<DtoIncidentSequenceResponse> GetAsync(long id, CancellationToken cancellationToken)
	{
		return await DataAccessLayer.GetIncidentSequenceDAL().GetAsync(id, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "77F8F2A9-9DE8-4048-8D63-26BF44DCCED1")]
	public async Task<DtoIncidentSequenceResponse> PostAsync([FromBody] DtoIncidentSequence value, CancellationToken cancellationToken)
	{
		value.NullCheck("", "PostAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\IncidentSequence.cs", 55);
		return await DataAccessLayer.GetIncidentSequenceDAL().CreateAsync(value, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "77F8F2A9-9DE8-4048-8D63-26BF44DCCED1")]
	public async Task<DtoIncidentSequenceResponse> PutAsync(long id, [FromBody] DtoIncidentSequence value, CancellationToken cancellationToken)
	{
		value.NullCheck("", "PutAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\IncidentSequence.cs", 65);
		if (value.Id != id)
		{
			value.Id = id;
		}
		return await DataAccessLayer.GetIncidentSequenceDAL().UpdateAsync(value, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "77F8F2A9-9DE8-4048-8D63-26BF44DCCED1")]
	public async Task<DtoIncidentSequenceResponse> DeleteAsync(long id, CancellationToken cancellationToken)
	{
		return await DataAccessLayer.GetIncidentSequenceDAL().DeleteAsync(id, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "77F8F2A9-9DE8-4048-8D63-26BF44DCCED1")]
	public async Task<DtoIncidentSequenceListResponse> DeleteRangeAsync([NotNull][FromUri] long[] ids, CancellationToken cancellationToken)
	{
		ids.NullCheck("", "DeleteRangeAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\IncidentSequence.cs", 86);
		return await DataAccessLayer.GetIncidentSequenceDAL().DeleteRangeAsync(ids.ToHashSet(), cancellationToken);
	}
}

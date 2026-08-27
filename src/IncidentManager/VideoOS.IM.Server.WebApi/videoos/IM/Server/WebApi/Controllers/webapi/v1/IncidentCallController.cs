using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentCall;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.IncidentCall;
using VideoOS.IM.Common.Service.Interfaces;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.WebApi.Filters;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.WebApi.Controllers.webapi.v1;

public class IncidentCallController : BaseController
{
	public IncidentCallController()
	{
	}

	public IncidentCallController(IDtoDAL dataAccessLayer)
		: base(dataAccessLayer)
	{
	}

	[CheckPermission("GENERIC_READ", "32979A29-2887-4FF5-B568-3876967F1730")]
	public async Task<DtoIncidentCallListResponse> GetAsync(string inPagingJson, CancellationToken cancellationToken)
	{
		DtoPaging inPaging = null;
		if (!string.IsNullOrWhiteSpace(inPagingJson))
		{
			inPaging = JsonConvert.DeserializeObject<DtoPaging>(inPagingJson);
		}
		return await DataAccessLayer.GetIncidentCallDAL().GetListAsync(inPaging, cancellationToken);
	}

	[CheckPermission("GENERIC_READ", "32979A29-2887-4FF5-B568-3876967F1730")]
	public async Task<DtoIncidentCallResponse> GetAsync(long id, CancellationToken cancellationToken)
	{
		return await DataAccessLayer.GetIncidentCallDAL().GetAsync(id, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "32979A29-2887-4FF5-B568-3876967F1730")]
	public async Task<DtoIncidentCallResponse> PostAsync([FromBody] DtoIncidentCall value, CancellationToken cancellationToken)
	{
		value.NullCheck("", "PostAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\IncidentCall.cs", 56);
		return await DataAccessLayer.GetIncidentCallDAL().CreateAsync(value, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "32979A29-2887-4FF5-B568-3876967F1730")]
	public async Task<DtoIncidentCallResponse> PutAsync(long id, [FromBody] DtoIncidentCall value, CancellationToken cancellationToken)
	{
		value.NullCheck("", "PutAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\IncidentCall.cs", 66);
		if (value.Id != id)
		{
			value.Id = id;
		}
		return await DataAccessLayer.GetIncidentCallDAL().UpdateAsync(value, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "32979A29-2887-4FF5-B568-3876967F1730")]
	public async Task<DtoIncidentCallResponse> DeleteAsync(long id, CancellationToken cancellationToken)
	{
		return await DataAccessLayer.GetIncidentCallDAL().DeleteAsync(id, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "32979A29-2887-4FF5-B568-3876967F1730")]
	public async Task<DtoIncidentCallListResponse> DeleteRangeAsync([NotNull][FromUri] long[] ids, CancellationToken cancellationToken)
	{
		ids.NullCheck("", "DeleteRangeAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\IncidentCall.cs", 87);
		return await DataAccessLayer.GetIncidentCallDAL().DeleteRangeAsync(ids.ToHashSet(), cancellationToken);
	}
}

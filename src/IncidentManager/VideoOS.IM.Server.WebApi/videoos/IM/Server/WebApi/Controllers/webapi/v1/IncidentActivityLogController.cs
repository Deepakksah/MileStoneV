using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentActivityLog;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.IncidentActivityLog;
using VideoOS.IM.Common.Service.Interfaces;
using VideoOS.IM.Server.Logger.AuditLog;
using VideoOS.IM.Server.WebApi.Filters;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.WebApi.Controllers.webapi.v1;

public class IncidentActivityLogController : BaseController
{
	public IncidentActivityLogController()
	{
	}

	public IncidentActivityLogController(IDtoDAL dataAccessLayer)
		: base(dataAccessLayer)
	{
	}

	[CheckPermission("GENERIC_READ", "FBFB0321-AB7E-416D-8B8D-A558D24166FB")]
	public async Task<DtoIncidentActivityLogListResponse> GetAsync(string inPagingJson, CancellationToken cancellationToken)
	{
		DtoPaging inPaging = null;
		if (!string.IsNullOrWhiteSpace(inPagingJson))
		{
			inPaging = JsonConvert.DeserializeObject<DtoPaging>(inPagingJson);
		}
		return await DataAccessLayer.GetIncidentActivityLogDAL().GetListAsync(inPaging, cancellationToken);
	}

	[CheckPermission("GENERIC_READ", "FBFB0321-AB7E-416D-8B8D-A558D24166FB")]
	public async Task<DtoIncidentActivityLogResponse> GetAsync(long id, CancellationToken cancellationToken)
	{
		return await DataAccessLayer.GetIncidentActivityLogDAL().GetAsync(id, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "FBFB0321-AB7E-416D-8B8D-A558D24166FB")]
	public async Task<DtoIncidentActivityLogResponse> PostAsync([FromBody] DtoIncidentActivityLog value, CancellationToken cancellationToken)
	{
		value.NullCheck("", "PostAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\IncidentActivityLog.cs", 54);
		DtoIncidentActivityLogResponse response = await DataAccessLayer.GetIncidentActivityLogDAL().CreateAsync(value, cancellationToken);
		if (response != null && response.Success && response.Value != null && response.Value.Description.Contains("ReportPrinted"))
		{
			await WriteAuditLogEntryAsync(AuditLogMessageIds.ReportPrinted, "ReportPrinted", response.Value.IncidentId.ToString());
		}
		return response;
	}
}

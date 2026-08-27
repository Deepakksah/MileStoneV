using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.Incident;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.Incident;
using VideoOS.IM.Common.Service.Interfaces;
using VideoOS.IM.Common.Service.Properties;
using VideoOS.IM.Server.Logger.AuditLog;
using VideoOS.IM.Server.WebApi.Filters;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.WebApi.Controllers.webapi.v1;

public class IncidentController : BaseController
{
	public IncidentController()
	{
	}

	public IncidentController(IDtoDAL dataAccessLayer)
		: base(dataAccessLayer)
	{
	}

	[CheckPermission("GENERIC_READ", "743C1A10-C864-4F9F-8A56-57F31E42D1DB")]
	public async Task<DtoIncidentListResponse> GetAsync(string inPagingJson, CancellationToken cancellationToken)
	{
		DtoPaging inPaging = null;
		if (!string.IsNullOrWhiteSpace(inPagingJson))
		{
			inPaging = JsonConvert.DeserializeObject<DtoPaging>(inPagingJson);
		}
		return await DataAccessLayer.GetIncidentDAL().GetListAsync(inPaging, cancellationToken);
	}

	[CheckPermission("GENERIC_READ", "743C1A10-C864-4F9F-8A56-57F31E42D1DB")]
	public async Task<DtoIncidentResponse> GetAsync(long id, CancellationToken cancellationToken)
	{
		return await DataAccessLayer.GetIncidentDAL().GetAsync(id, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "743C1A10-C864-4F9F-8A56-57F31E42D1DB")]
	public async Task<DtoIncidentResponse> PostAsync([FromBody] DtoIncident value, CancellationToken cancellationToken)
	{
		value.NullCheck("", "PostAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\Incident.cs", 55);
		DtoIncidentResponse response = await DataAccessLayer.GetIncidentDAL().CreateAsync(value, cancellationToken);
		if (response != null && response.Success && response.Value != null)
		{
			await WriteAuditLogEntryAsync(AuditLogMessageIds.IncidentCreated, "IncidentCreated", response.Value.Id.ToString());
		}
		return response;
	}

	[CheckPermission("GENERIC_WRITE", "743C1A10-C864-4F9F-8A56-57F31E42D1DB")]
	public async Task<DtoIncidentResponse> PutAsync(long id, [FromBody] DtoIncident value, CancellationToken cancellationToken)
	{
		value.NullCheck("", "PutAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\Incident.cs", 72);
		if (value.Id != id)
		{
			value.Id = id;
		}
		return await DataAccessLayer.GetIncidentDAL().UpdateAsync(value, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "743C1A10-C864-4F9F-8A56-57F31E42D1DB")]
	public async Task<DtoIncidentResponse> DeleteAsync(long id, CancellationToken cancellationToken)
	{
		DtoIncidentResponse response = await DataAccessLayer.GetIncidentDAL().DeleteAsync(id, cancellationToken);
		if (response != null && response.Success && response.Value != null)
		{
			await WriteAuditLogEntryAsync(AuditLogMessageIds.IncidentDeleted, "IncidentDeleted", response.Value.Id.ToString());
		}
		return response;
	}

	[CheckPermission("GENERIC_WRITE", "743C1A10-C864-4F9F-8A56-57F31E42D1DB")]
	public async Task<DtoIncidentListResponse> DeleteRangeAsync([NotNull][FromUri] long[] ids, CancellationToken cancellationToken)
	{
		ids.NullCheck("", "DeleteRangeAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\Incident.cs", 100);
		DtoIncidentListResponse response = await DataAccessLayer.GetIncidentDAL().DeleteRangeAsync(ids.ToHashSet(), cancellationToken);
		if (response != null && response.Success && response.Value != null)
		{
			foreach (DtoIncident item in response.Value)
			{
				await WriteAuditLogEntryAsync(AuditLogMessageIds.IncidentDeleted, "IncidentDeleted", item.Id.ToString());
			}
		}
		return response;
	}
}

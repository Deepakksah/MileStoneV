using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.DraftSequence;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.DraftSequence;
using VideoOS.IM.Common.Service.Interfaces;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.WebApi.Filters;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.WebApi.Controllers.webapi.v1;

public class DraftSequenceController : BaseController
{
	public DraftSequenceController()
	{
	}

	public DraftSequenceController(IDtoDAL dataAccessLayer)
		: base(dataAccessLayer)
	{
	}

	[CheckPermission("GENERIC_READ", "76DE2B27-DE9C-4F46-865A-E7445D47E70E")]
	public async Task<DtoDraftSequenceListResponse> GetAsync(string inPagingJson, CancellationToken cancellationToken)
	{
		DtoPaging inPaging = null;
		if (!string.IsNullOrWhiteSpace(inPagingJson))
		{
			inPaging = JsonConvert.DeserializeObject<DtoPaging>(inPagingJson);
		}
		return await DataAccessLayer.GetDraftSequenceDAL().GetListAsync(inPaging, cancellationToken);
	}

	[CheckPermission("GENERIC_READ", "76DE2B27-DE9C-4F46-865A-E7445D47E70E")]
	public async Task<DtoDraftSequenceResponse> GetAsync(long id, CancellationToken cancellationToken)
	{
		return await DataAccessLayer.GetDraftSequenceDAL().GetAsync(id, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "76DE2B27-DE9C-4F46-865A-E7445D47E70E")]
	public async Task<DtoDraftSequenceResponse> PostAsync([FromBody] DtoDraftSequence value, CancellationToken cancellationToken)
	{
		value.NullCheck("", "PostAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\DraftSequence.cs", 56);
		return await DataAccessLayer.GetDraftSequenceDAL().CreateAsync(value, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "76DE2B27-DE9C-4F46-865A-E7445D47E70E")]
	public async Task<DtoDraftSequenceResponse> PutAsync(long id, [FromBody] DtoDraftSequence value, CancellationToken cancellationToken)
	{
		value.NullCheck("", "PutAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\DraftSequence.cs", 66);
		if (value.Id != id)
		{
			value.Id = id;
		}
		return await DataAccessLayer.GetDraftSequenceDAL().UpdateAsync(value, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "76DE2B27-DE9C-4F46-865A-E7445D47E70E")]
	public async Task<DtoDraftSequenceResponse> DeleteAsync(long id, CancellationToken cancellationToken)
	{
		return await DataAccessLayer.GetDraftSequenceDAL().DeleteAsync(id, cancellationToken);
	}

	[CheckPermission("GENERIC_WRITE", "76DE2B27-DE9C-4F46-865A-E7445D47E70E")]
	public async Task<DtoDraftSequenceListResponse> DeleteRangeAsync([NotNull][FromUri] long[] ids, CancellationToken cancellationToken)
	{
		ids.NullCheck("", "DeleteRangeAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\WebApi\\Controllers\\webapi\\v1\\DraftSequence.cs", 87);
		return await DataAccessLayer.GetDraftSequenceDAL().DeleteRangeAsync(ids.ToHashSet(), cancellationToken);
	}
}

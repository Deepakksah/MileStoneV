using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;

namespace VideoOS.IM.Common.Dto.DtoResponseObjectModels.Common;

public abstract class DtoResponseBase
{
	public bool Success { get; set; }

	public string Caption { get; set; }

	public string Message { get; set; }

	public string MessageDescription { get; set; }

	public DtoPaging PagingObject { get; set; }
}
public abstract class DtoResponseBase<TDto> : DtoResponseBase
{
	public TDto Value { get; set; }
}

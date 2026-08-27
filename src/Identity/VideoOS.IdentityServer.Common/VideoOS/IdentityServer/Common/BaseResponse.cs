namespace VideoOS.IdentityServer.Common;

public class BaseResponse<T> : IBaseResponse<T> where T : class
{
	public T Data { get; set; }

	public string Message { get; set; }

	public BaseResponse(T value, string message)
	{
		Data = value;
		Message = message;
	}
}

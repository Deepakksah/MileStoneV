namespace VideoOS.IdentityServer.Model.Responses;

public class BaseResponse<T> where T : class
{
	public T Data { get; set; }
}
public class BaseResponse
{
	public object Data { get; set; }

	public BaseResponse()
	{
	}

	public BaseResponse(object data)
	{
		Data = data;
	}
}

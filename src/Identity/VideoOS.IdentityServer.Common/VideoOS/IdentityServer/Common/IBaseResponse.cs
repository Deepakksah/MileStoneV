namespace VideoOS.IdentityServer.Common;

public interface IBaseResponse<T> where T : class
{
	T Data { get; set; }

	string Message { get; set; }
}

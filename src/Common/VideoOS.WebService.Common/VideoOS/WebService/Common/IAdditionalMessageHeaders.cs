namespace VideoOS.WebService.Common;

public interface IAdditionalMessageHeaders
{
	void AddMessageHeader(string ns, string name, string content);
}

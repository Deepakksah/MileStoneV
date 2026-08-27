namespace VideoOS.Mobile.Portable.Interfaces;

public interface IClientContext
{
	string Protocol { get; }

	IRequest CreateRequest(string webAddress);
}

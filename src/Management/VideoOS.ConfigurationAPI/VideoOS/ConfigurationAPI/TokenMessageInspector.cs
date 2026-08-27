using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace VideoOS.ConfigurationAPI;

public class TokenMessageInspector : IDispatchMessageInspector, IClientMessageInspector
{
	private TokenHelper _tokenHelper;

	public TokenMessageInspector(TokenHelper tokenHelper)
	{
		_tokenHelper = tokenHelper;
	}

	public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
	{
		int num = request.Headers.FindHeader("Token", "");
		string text = "";
		if (num != -1)
		{
			text = MessageHeaderToken.ReadHeader(request.Headers.GetReaderAtHeader(num)).Token;
		}
		if (!_tokenHelper.ValidateToken(text))
		{
			throw new FaultException<string>("Token not valid:" + text, "Invalid Token");
		}
		return null;
	}

	public void BeforeSendReply(ref Message reply, object correlationState)
	{
	}

	public void AfterReceiveReply(ref Message reply, object correlationState)
	{
	}

	public object BeforeSendRequest(ref Message request, IClientChannel channel)
	{
		request.Headers.Add(new MessageHeaderToken(_tokenHelper.GetToken()));
		return null;
	}
}

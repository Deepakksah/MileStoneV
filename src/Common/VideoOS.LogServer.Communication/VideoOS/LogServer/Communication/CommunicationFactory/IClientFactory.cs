using System.ServiceModel;

namespace VideoOS.LogServer.Communication.CommunicationFactory;

public interface IClientFactory<T> where T : ICommunicationObject
{
	T Create();
}

using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace VideoOS.EventServer.Communication;

public static class ClientBaseExtensions
{
	private const int MaxItemsInGraph = 1000000;

	public static void EnsureLargeObjectGraphsAllowed<T, TChannel>(this T client) where T : ClientBase<TChannel> where TChannel : class
	{
		if (client == null)
		{
			throw new ArgumentNullException("client");
		}
		foreach (OperationDescription operation in client.Endpoint.Contract.Operations)
		{
			DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
			if (dataContractSerializerOperationBehavior != null)
			{
				dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = 1000000;
			}
		}
	}
}

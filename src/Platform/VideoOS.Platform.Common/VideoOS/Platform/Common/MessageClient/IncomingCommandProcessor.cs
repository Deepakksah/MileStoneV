using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VideoOS.Common.WebSockets;

namespace VideoOS.Platform.Common.MessageClient;

public class IncomingCommandProcessor : IIncomingCommandProcessor, IMessageProcessor<CommandDto>, IDisposable
{
	private readonly Func<JObject, Type, object> _convertor;

	public event EventHandler<MessageCommandBodyDto> MessageReceived;

	public event EventHandler<HelloCommandBodyDto> HelloReceived;

	public event EventHandler<AckCommandBodyDto> AckReceived;

	public IncomingCommandProcessor(Func<JObject, Type, object> convertor)
	{
		_convertor = convertor ?? throw new ArgumentNullException("convertor");
	}

	public IncomingCommandProcessor()
		: this((JObject jObject, Type type) => ((JToken)jObject).ToObject(type))
	{
	}

	public Task ProcessMessageAsync(CommandDto command, CancellationToken cancellationToken)
	{
		switch (command.Type)
		{
		case "ack.v1":
			OnReceived(command.Body, this.AckReceived);
			break;
		case "msg.v1":
			OnReceived(command.Body, this.MessageReceived);
			break;
		case "hello.v1":
			OnReceived(command.Body, this.HelloReceived);
			break;
		}
		return Task.CompletedTask;
	}

	private T ConvertToObject<T>(JObject input)
	{
		return (T)_convertor(input, typeof(T));
	}

	private void OnReceived<T>(JObject body, EventHandler<T> handler)
	{
		T e;
		try
		{
			e = ConvertToObject<T>(body);
		}
		catch (Exception arg)
		{
			EnvironmentManager.Instance?.Log(error: true, "MessageClient", string.Format("Exception occurred while trying to deserialize {0}: {1}", "msg.v1", arg));
			return;
		}
		handler?.Invoke(this, e);
	}

	public void Dispose()
	{
	}
}

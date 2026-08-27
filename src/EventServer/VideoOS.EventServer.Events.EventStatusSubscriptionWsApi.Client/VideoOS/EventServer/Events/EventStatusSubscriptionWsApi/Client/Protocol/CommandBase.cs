using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public abstract class CommandBase : ICommand
{
	public string Command { get; set; }

	public ulong CommandId { get; set; }

	public CommandBase(string command)
	{
		Command = command;
	}

	public string ToJson()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_001c: Expected O, but got Unknown
		//IL_0021: Expected O, but got Unknown
		return JsonConvert.SerializeObject((object)this, new JsonSerializerSettings
		{
			ContractResolver = (IContractResolver)new DefaultContractResolver
			{
				NamingStrategy = (NamingStrategy)new CamelCaseNamingStrategy()
			}
		});
	}
}

using System.IO;
using Newtonsoft.Json;
using VideoOS.Utilities.Validation;

namespace VideoOS.Server.Failover.Configurator;

public class IdpConfiguration
{
	public class IdpSettings
	{
		[JsonProperty]
		public string Authority { get; private set; }

		private IdpSettings()
		{
		}
	}

	[JsonProperty]
	public IdpSettings IdentityProviderSettings { get; private set; }

	public static IdpConfiguration Load(string path)
	{
		path.NullCheck("", "Load", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Server.Failover.Configurator\\IdpConfiguration.cs", 15);
		return JsonConvert.DeserializeObject<IdpConfiguration>(File.ReadAllText(path));
	}

	private IdpConfiguration()
	{
	}
}

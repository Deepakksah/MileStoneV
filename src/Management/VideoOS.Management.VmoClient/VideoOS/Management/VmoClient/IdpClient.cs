using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[DebuggerDisplay("IdpClient: {ClientId}")]
[EntityName("IdpClient")]
public class IdpClient : VmoObjectIntBased
{
	private string[] _uris;

	public string ClientId
	{
		get
		{
			return GetPropertyValue<string>("ClientId");
		}
		private set
		{
			SetPropertyValue("ClientId", value);
		}
	}

	public ICollection<string> RedirectUris
	{
		get
		{
			if (_uris == null)
			{
				VmoPropertyCollection nestedPropertyCollection = base.ObjectProperties.GetNestedPropertyCollection("ClientRedirectUris");
				_uris = nestedPropertyCollection.Select((KeyValuePair<string, object> p) => p.Value as string).ToArray();
			}
			return _uris;
		}
	}

	public IdpClient(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public IdpClient(ManagementServer managementServer, string clientId)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), managementServer?.Repositories, isInitialized: true)
	{
		ClientId = clientId;
		base.ObjectProperties.SetNestedPropertyCollection("ClientRedirectUris", new VmoPropertyCollection());
	}
}

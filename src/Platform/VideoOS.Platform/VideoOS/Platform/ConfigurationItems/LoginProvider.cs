using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LoginProvider : IConfigurationItem
{
	private ObservableCollection<string> _Scopes;

	public RegisteredClaimFolder RegisteredClaimFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "RegisteredClaimFolder")
				{
					return new RegisteredClaimFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public bool Enabled
	{
		get
		{
			return base.Wrapper.ConfigurationItem.EnableProperty.Enabled;
		}
		set
		{
			base.Wrapper.ConfigurationItem.EnableProperty.Enabled = value;
			base.Wrapper.IsDirty = true;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public string ClientId
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ClientId");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ClientId", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ClientSecret
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ClientSecret");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ClientSecret", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ClientSecretHasValue
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ClientSecretHasValue");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public string Authority
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Authority");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Authority", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string CallbackPath
	{
		get
		{
			return base.Wrapper.GetPropertyValue("CallbackPath");
		}
		set
		{
			base.Wrapper.SetPropertyValue("CallbackPath", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ObservableCollection<string> Scopes
	{
		get
		{
			if (_Scopes == null)
			{
				_Scopes = base.Wrapper.GetPropertyArrayString("Scopes");
				_Scopes.CollectionChanged += _Scopes_CollectionChanged;
			}
			return _Scopes;
		}
	}

	public string UserNameClaimType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("UserNameClaimType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("UserNameClaimType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PromptForLogin
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PromptForLogin");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PromptForLogin", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public LoginProvider(ServerId serverId, ConfigurationItem loginProvider)
		: base(serverId, loginProvider)
	{
	}

	public LoginProvider(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public LoginProvider(FQID fqid)
		: base(fqid)
	{
	}

	private void _Scopes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		base.Wrapper.SetPropertyArrayString("Scopes", _Scopes);
	}
}

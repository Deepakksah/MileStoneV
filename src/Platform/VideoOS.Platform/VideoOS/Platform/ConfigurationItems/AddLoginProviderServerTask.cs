using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddLoginProviderServerTask : ServerTask
{
	private ObservableCollection<string> _Scopes;

	public string Id
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Id");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Id", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string Name
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Name");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Name", value);
			base.Wrapper.IsDirty = true;
		}
	}

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

	public bool Enabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Enabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Enabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public AddLoginProviderServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddLoginProvider"));
	}

	private void _Scopes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		base.Wrapper.SetPropertyArrayString("Scopes", _Scopes);
	}
}

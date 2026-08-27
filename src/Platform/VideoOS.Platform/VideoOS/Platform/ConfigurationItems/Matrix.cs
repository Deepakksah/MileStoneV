using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class Matrix : IConfigurationItem
{
	public string Id => base.Wrapper.GetPropertyValue("Id");

	public string Address
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Address");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Address", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int Port
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Port");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Port", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public SecureString SecurePassword
	{
		get
		{
			return base.Wrapper.GetPropertySecureValue("Password");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Password", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public Matrix(ServerId serverId, ConfigurationItem matrix)
		: base(serverId, matrix)
	{
	}

	public Matrix(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public Matrix(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask ChangeSecurityPermissions(string userPath)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ChangeSecurityPermissions");
			configurationApiItemWrapper.SetProperty("UserPath", userPath);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ChangeSecurityPermissions"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Matrix", "Unable to call method 'ChangeSecurityPermissions':" + ex.Message);
		}
		return null;
	}

	public ChangeSecurityPermissionsServerTask ChangeSecurityPermissions()
	{
		try
		{
			return new ChangeSecurityPermissionsServerTask(base.Wrapper.ExecuteMethod("ChangeSecurityPermissions"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Matrix", "Unable to call method 'ChangeSecurityPermissions':" + ex.Message);
		}
		return null;
	}

	public ServerTask MatrixCommand(string matrixCommandType, Collection<string> matrixCameraList)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("MatrixCommand");
			configurationApiItemWrapper.SetProperty("MatrixCommandType", matrixCommandType);
			configurationApiItemWrapper.SetProperty("MatrixCameraList", matrixCameraList);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("MatrixCommand"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Matrix", "Unable to call method 'MatrixCommand':" + ex.Message);
		}
		return null;
	}

	public MatrixCommandServerTask MatrixCommand()
	{
		try
		{
			return new MatrixCommandServerTask(base.Wrapper.ExecuteMethod("MatrixCommand"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Matrix", "Unable to call method 'MatrixCommand':" + ex.Message);
		}
		return null;
	}
}

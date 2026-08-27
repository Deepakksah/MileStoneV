using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MatrixFolder : IConfigurationItem
{
	public ICollection<Matrix> Matrixs
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<Matrix> list = new List<Matrix>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "Matrix")
				{
					list.Add(new Matrix(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public MatrixFolder(ServerId serverId, ConfigurationItem matrixFolder)
		: base(serverId, matrixFolder)
	{
	}

	public MatrixFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public MatrixFolder(FQID fqid)
		: base(fqid)
	{
	}

	public MatrixFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/MatrixFolder")
	{
	}

	public ServerTask AddMatrix(string name, string description, string address, int port, SecureString password)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddMatrix");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("Address", address);
			configurationApiItemWrapper.SetProperty("Port", port.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("Password", password);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddMatrix"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MatrixFolder", "Unable to call method 'AddMatrix':" + ex.Message);
		}
		return null;
	}

	public AddMatrixServerTask AddMatrix()
	{
		try
		{
			return new AddMatrixServerTask(base.Wrapper.ExecuteMethod("AddMatrix"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MatrixFolder", "Unable to call method 'AddMatrix':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveMatrix(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveMatrix");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveMatrix"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MatrixFolder", "Unable to call method 'RemoveMatrix':" + ex.Message);
		}
		return null;
	}

	public RemoveMatrixServerTask RemoveMatrix()
	{
		try
		{
			return new RemoveMatrixServerTask(base.Wrapper.ExecuteMethod("RemoveMatrix"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MatrixFolder", "Unable to call method 'RemoveMatrix':" + ex.Message);
		}
		return null;
	}
}

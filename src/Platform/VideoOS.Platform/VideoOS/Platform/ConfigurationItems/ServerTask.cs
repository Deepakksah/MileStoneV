using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

public class ServerTask
{
	private ConfigurationApiItemWrapper _wrapper;

	public ConfigurationApiItemWrapper Wrapper => _wrapper;

	public int Progress
	{
		get
		{
			string property = _wrapper.GetProperty("Progress");
			int result = 100;
			if (int.TryParse(property, out result))
			{
				return result;
			}
			if (State == StateEnum.Idle)
			{
				return 0;
			}
			return 100;
		}
	}

	public StateEnum State
	{
		get
		{
			StateEnum result = StateEnum.Success;
			if (Enum.TryParse<StateEnum>(_wrapper.GetProperty("State"), out result))
			{
				return result;
			}
			return StateEnum.Success;
		}
	}

	public string ErrorCode => _wrapper.GetProperty("ErrorCode");

	public string ErrorText => _wrapper.GetProperty("ErrorText");

	public string Path => _wrapper.GetProperty("Path");

	public string ItemType => _wrapper.ConfigurationItem.ItemType;

	public Collection<string> Methods
	{
		get
		{
			if (_wrapper.MethodIds == null)
			{
				return new Collection<string>();
			}
			return new Collection<string>(_wrapper.MethodIds);
		}
	}

	public ServerTask(ConfigurationApiItemWrapper wrapper)
	{
		if (wrapper.ConfigurationItem.ItemType != "InvokeResult" && wrapper.ConfigurationItem.ItemType != "InvokeInfo")
		{
			EnvironmentManager.Instance.Log(error: true, "ServerTask", "Unexpected ItemType passed to ServerTask:" + wrapper.ConfigurationItem.ItemType);
		}
		_wrapper = wrapper;
	}

	public void GetTask(string path)
	{
		if (_wrapper == null)
		{
			throw new ArgumentException("Underlaying wrapper is null");
		}
		_wrapper.ConfigurationItem = _wrapper.GetItem(path);
	}

	public void UpdateState()
	{
		if (_wrapper == null || ItemType == "InvokeInfo")
		{
			return;
		}
		if (ItemType == "InvokeResult")
		{
			if (Progress != 100)
			{
				if (string.IsNullOrEmpty(Path))
				{
					throw new MIPException("The UpdateState is expecting a Path property in the InvokeResult, but was not found.");
				}
				_wrapper.ConfigurationItem = _wrapper.GetItem(Path);
			}
		}
		else if (Progress != 100 || State == StateEnum.Idle || State == StateEnum.InProgress)
		{
			_wrapper.ConfigurationItem = _wrapper.GetItem(_wrapper.Path);
		}
	}

	public string GetProperty(string key)
	{
		return Wrapper.GetProperty(key);
	}

	public void SetProperty(string key, string content)
	{
		Wrapper.SetPropertyValue(key, content);
	}

	public ICollection<string> GetPropertyKeys()
	{
		return Wrapper.GetPropertyKeys();
	}

	public List<ValueTypeInfo> GetValueTypeInfoList(string propertyKey)
	{
		return Wrapper.GetValueTypeInfoList(propertyKey);
	}

	public ServerTask ExecuteDefault()
	{
		if (_wrapper.MethodIds != null && _wrapper.MethodIds.Length != 0)
		{
			return new ServerTask(_wrapper.ExecuteMethod(_wrapper.MethodIds[0]));
		}
		throw new MIPException("No default method available");
	}

	public ServerTask ExecuteMethod(string methodId)
	{
		return new ServerTask(_wrapper.ExecuteMethod(methodId));
	}

	public ValidateResult ValidateItem()
	{
		return _wrapper.ValidateItem();
	}
}

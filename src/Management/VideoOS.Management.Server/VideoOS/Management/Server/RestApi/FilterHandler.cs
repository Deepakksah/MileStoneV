using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace VideoOS.Management.Server.RestApi;

public class FilterHandler
{
	private Dictionary<string, Collection<string>> _resourceNameFields;

	private Collection<string> _rootFields;

	private string _rootResourceName;

	public List<string> ResourceNames
	{
		get
		{
			if (_resourceNameFields == null)
			{
				return new List<string>();
			}
			return _resourceNameFields.Keys.ToList();
		}
	}

	public FilterHandler()
	{
	}

	public void SetRootResourceName(string rootResourceName)
	{
		_rootResourceName = rootResourceName;
	}

	public void SetIncludeChildren(string value)
	{
		_resourceNameFields = new Dictionary<string, Collection<string>>();
		string pattern = "\\s*,\\s*(?![^(]*\\))";
		string[] array = Regex.Split(value, pattern);
		foreach (string text in array)
		{
			if (string.IsNullOrEmpty(text))
			{
				continue;
			}
			string[] array2 = text.Split(',', '(', ')');
			if (string.IsNullOrWhiteSpace(array2[0]))
			{
				continue;
			}
			string key = array2[0];
			if (array2.Length == 1)
			{
				_resourceNameFields[key] = null;
				continue;
			}
			Collection<string> collection = new Collection<string>();
			if (!string.IsNullOrEmpty(array2[array2.Length - 1]))
			{
				throw new ArgumentException("Incorrect formatted includeChildren format");
			}
			for (int j = 1; j < array2.Length - 1; j++)
			{
				collection.Add(array2[j]);
			}
			_resourceNameFields[key] = collection;
		}
	}

	public void SetIncludeFields(string paranCommaList)
	{
		if (paranCommaList.Length < 3 || paranCommaList[0] != '(' || paranCommaList[paranCommaList.Length - 1] != ')')
		{
			throw new ArgumentException("Incorrect formatted includeFields value");
		}
		paranCommaList = paranCommaList.Substring(1, paranCommaList.Length - 2);
		string[] list = paranCommaList.Split(',');
		_rootFields = new Collection<string>(list);
	}

	public bool IncludeResourceName(string resourceName)
	{
		if (_resourceNameFields == null)
		{
			return true;
		}
		foreach (string key in _resourceNameFields.Keys)
		{
			if (string.Compare(resourceName, key, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return true;
			}
		}
		return false;
	}

	public bool Any()
	{
		if (_resourceNameFields != null)
		{
			return _resourceNameFields.Any();
		}
		return false;
	}

	public bool None()
	{
		return !Any();
	}

	public bool IncludeField(string resourceName, string fieldName)
	{
		if (resourceName == _rootResourceName)
		{
			if (_rootFields != null)
			{
				return _rootFields.Contains(fieldName);
			}
			return true;
		}
		if (_resourceNameFields != null && _resourceNameFields.TryGetValue(resourceName, out var value))
		{
			return value?.Contains(fieldName) ?? true;
		}
		return true;
	}
}

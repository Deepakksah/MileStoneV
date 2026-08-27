using System;
using System.Collections;
using System.Collections.Generic;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Management.Server;

public class ServerICustomPropertyCollection : ICustomProperties, IEnumerable<ICustomProperty>, IEnumerable
{
	private IGeminiRequestContext _requestContext;

	private string _parentId;

	private bool _readAccessVerified;

	private Dictionary<string, ICustomProperty> _customProperties;

	public ICustomProperty this[string index]
	{
		get
		{
			SecurePopulate();
			if (_customProperties.TryGetValue(index, out var value))
			{
				return value;
			}
			return null;
		}
	}

	public ServerICustomPropertyCollection(IGeminiRequestContext requestContext, string parentId)
		: this(requestContext, parentId, readAccessVerified: false)
	{
	}

	public ServerICustomPropertyCollection(IGeminiRequestContext requestContext, string parentId, bool readAccessVerified)
	{
		if (requestContext == null)
		{
			throw new ArgumentException("requestContext cannot be null", "requestContext");
		}
		_requestContext = requestContext;
		_parentId = parentId;
		_readAccessVerified = readAccessVerified;
	}

	public bool Contains(string propertyName)
	{
		SecurePopulate();
		return _customProperties.ContainsKey(propertyName);
	}

	public IEnumerator<ICustomProperty> GetEnumerator()
	{
		SecurePopulate();
		return _customProperties.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private void SecurePopulate()
	{
		if (_customProperties == null)
		{
			_customProperties = new Dictionary<string, ICustomProperty>(StringComparer.OrdinalIgnoreCase);
			CustomProperty[] array = ((!_readAccessVerified) ? CustomProperty.GetCustomProperties(_requestContext, _parentId) : CustomProperty.GetCustomPropertiesNoSecurity(_requestContext, _parentId));
			CustomProperty[] array2 = array;
			foreach (CustomProperty customProperty in array2)
			{
				_customProperties.Add(customProperty.Name, new ServerICustomProperty(_requestContext, customProperty));
			}
		}
	}
}

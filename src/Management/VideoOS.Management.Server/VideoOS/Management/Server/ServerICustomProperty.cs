using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Management.Server;

public class ServerICustomProperty : ICustomProperty
{
	private IGeminiRequestContext _requestContext;

	private CustomProperty _customProperty;

	public string Value
	{
		get
		{
			return _customProperty.Value;
		}
		set
		{
			_customProperty.Value = value;
		}
	}

	public string Key
	{
		get
		{
			return _customProperty.Name;
		}
		set
		{
			_customProperty.Name = value;
		}
	}

	public ServerICustomProperty(IGeminiRequestContext requestContext, CustomProperty customProperty)
	{
		_requestContext = requestContext;
		_customProperty = customProperty;
	}

	public void Create()
	{
		CustomProperty.UpdateCustomProperties(_requestContext, _customProperty.ObjectId, new CustomProperty[1] { _customProperty });
	}

	public void Delete()
	{
		CustomProperty.DeleteCustomProperty(_requestContext, _customProperty.ObjectId, _customProperty.Name);
	}

	public void Update()
	{
		CustomProperty.UpdateCustomProperties(_requestContext, _customProperty.ObjectId, new CustomProperty[1] { _customProperty });
	}
}

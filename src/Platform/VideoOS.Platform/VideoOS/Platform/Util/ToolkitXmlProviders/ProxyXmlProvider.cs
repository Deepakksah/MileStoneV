using System;
using System.Reflection;
using System.Xml;

namespace VideoOS.Platform.Util.ToolkitXmlProviders;

public class ProxyXmlProvider : BaseToolkitXmlProvider
{
	private readonly BaseToolkitXmlProvider _inner;

	public override ToolkitProviders ToolkitProvider => _inner.ToolkitProvider;

	public override ToolkitTypes ToolkitType => _inner.ToolkitType;

	public ProxyXmlProvider(BaseToolkitXmlProvider inner)
	{
		_inner = inner;
		if (_inner == null)
		{
			throw new ArgumentNullMIPException("ProxyXmlProvider should be initialized with valid instance of BaseToolkitXmlProvider");
		}
	}

	protected override void WriteTransformationStart(XmlWriter xw)
	{
		CallInnerMethod("WriteTransformationStart", new object[1] { xw });
	}

	protected override void WriteTransformationEnd(XmlWriter xw)
	{
		CallInnerMethod("WriteTransformationEnd", new object[1] { xw });
	}

	private void CallInnerMethod(string methodName, object[] parameters)
	{
		MethodInfo method = _inner.GetType().GetMethod(methodName);
		if (null != method)
		{
			method.Invoke(_inner, parameters);
		}
	}

	public override void UpdateConfiguration(Action<string, string> configurationUpdater)
	{
		_inner.UpdateConfiguration(configurationUpdater);
	}
}

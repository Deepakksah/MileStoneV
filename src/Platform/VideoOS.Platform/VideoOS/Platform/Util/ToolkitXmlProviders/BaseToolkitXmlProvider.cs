using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Platform.Util.ToolkitXmlProviders;

public abstract class BaseToolkitXmlProvider : IToolkitXmlProvider, IToolkitXmlProviderParams
{
	private IToolkitXmlProvider _next;

	public abstract ToolkitProviders ToolkitProvider { get; }

	public abstract ToolkitTypes ToolkitType { get; }

	public string ConfigurationXml => GetConfigurationXml();

	private string GetConfigurationXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			GetConfigurationXmlInternal(xmlWriter, new RootToolkitXmlProviderParams());
			xmlWriter.Flush();
			return stringBuilder.ToString();
		}
		finally
		{
			stringWriter?.Close();
		}
	}

	private void GetConfigurationXmlInternal(XmlWriter xw, IToolkitXmlProviderParams parentParams)
	{
		int num;
		if (parentParams.ToolkitProvider == ToolkitProvider)
		{
			num = ((parentParams.ToolkitType != ToolkitType) ? 1 : 0);
			if (num == 0)
			{
				goto IL_0067;
			}
		}
		else
		{
			num = 1;
		}
		xw.WriteStartElement("toolkit");
		xw.WriteAttributeString("type", ToolkitParams.NameFromTypes(ToolkitType));
		xw.WriteElementString("provider", ToolkitParams.NameFromProviders(ToolkitProvider));
		xw.WriteStartElement("config");
		goto IL_0067;
		IL_0067:
		WriteTransformationStart(xw);
		if (_next is BaseToolkitXmlProvider baseToolkitXmlProvider)
		{
			baseToolkitXmlProvider.GetConfigurationXmlInternal(xw, this);
		}
		WriteTransformationEnd(xw);
		if (num != 0)
		{
			xw.WriteEndElement();
			xw.WriteEndElement();
		}
	}

	protected abstract void WriteTransformationStart(XmlWriter xw);

	protected abstract void WriteTransformationEnd(XmlWriter xw);

	public IToolkitXmlProvider Chain(IToolkitXmlProvider child)
	{
		if (_next == null)
		{
			_next = child;
		}
		else
		{
			_next.Chain(child);
		}
		return this;
	}

	public abstract void UpdateConfiguration(Action<string, string> configurationUpdater);
}

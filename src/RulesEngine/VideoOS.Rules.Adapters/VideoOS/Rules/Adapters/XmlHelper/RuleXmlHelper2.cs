namespace VideoOS.Rules.Adapters.XmlHelper;

public class RuleXmlHelper2 : IRuleXmlHelper
{
	public SourcesHelper SourceXmlToLists(string xmlSource)
	{
		return RuleXmlHelper.SourceXmlToLists(xmlSource);
	}

	public string SourceListToXml(SourcesHelper sourcesHelper)
	{
		return RuleXmlHelper.SourceListToXml(sourcesHelper);
	}
}

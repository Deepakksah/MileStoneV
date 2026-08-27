namespace VideoOS.Rules.Adapters.XmlHelper;

public interface IRuleXmlHelper
{
	SourcesHelper SourceXmlToLists(string xmlSource);

	string SourceListToXml(SourcesHelper sourcesHelper);
}

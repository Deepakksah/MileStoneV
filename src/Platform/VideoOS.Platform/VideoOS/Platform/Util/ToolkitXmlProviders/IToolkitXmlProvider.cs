namespace VideoOS.Platform.Util.ToolkitXmlProviders;

public interface IToolkitXmlProvider : IToolkitXmlProviderParams
{
	IToolkitXmlProvider Chain(IToolkitXmlProvider child);
}

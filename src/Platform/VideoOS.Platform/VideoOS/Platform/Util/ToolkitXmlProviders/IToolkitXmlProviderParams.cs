namespace VideoOS.Platform.Util.ToolkitXmlProviders;

public interface IToolkitXmlProviderParams
{
	ToolkitProviders ToolkitProvider { get; }

	ToolkitTypes ToolkitType { get; }
}

namespace VideoOS.Platform.Util.ToolkitXmlProviders;

public abstract class MediaProcessingToolkitXmlProvider : BaseToolkitXmlProvider
{
	public override ToolkitProviders ToolkitProvider => ToolkitProviders.MediaProcessing;

	public override ToolkitTypes ToolkitType => ToolkitTypes.Source;
}

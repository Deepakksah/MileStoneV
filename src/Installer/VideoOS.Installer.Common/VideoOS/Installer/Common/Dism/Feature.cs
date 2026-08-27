namespace VideoOS.Installer.Common.Dism;

public class Feature
{
	public static readonly string IISCommonHttpFeature = "IIS-CommonHttpFeatures";

	public static readonly string IISWebserverRole = "IIS-WebServerRole";

	public static readonly string IISWebserver = "IIS-WebServer";

	public static readonly string IISStaticContent = "IIS-StaticContent";

	public static readonly string IISDefaultDocument = "IIS-DefaultDocument";

	public static readonly string IISDirectoryBrowsing = "IIS-DirectoryBrowsing";

	public static readonly string IISHttpErrors = "IIS-HttpErrors";

	public static readonly string IISApplicationDevelopment = "IIS-ApplicationDevelopment";

	public static readonly string IISHealthAndDiagnostics = "IIS-HealthAndDiagnostics";

	public static readonly string IISHttpLogging = "IIS-HttpLogging";

	public static readonly string IISRequestMonitor = "IIS-RequestMonitor";

	public static readonly string IISSecurity = "IIS-Security";

	public static readonly string IISBasicAuthentication = "IIS-BasicAuthentication";

	public static readonly string IISWindowsAuthentication = "IIS-WindowsAuthentication";

	public static readonly string IISRequestFiltering = "IIS-RequestFiltering";

	public static readonly string IISPerformance = "IIS-Performance";

	public static readonly string IISHttpCompressionStatic = "IIS-HttpCompressionStatic";

	public static readonly string IISWebserverManagementTools = "IIS-WebServerManagementTools";

	public static readonly string IISManagementConsole = "IIS-ManagementConsole";

	public static readonly string WasWindowsActivationService = "WAS-WindowsActivationService";

	public static readonly string WasProcessModel = "WAS-ProcessModel";

	public static readonly string WasConfigurationApi = "WAS-ConfigurationAPI";

	public static readonly string IIS_ISApiFilter = "IIS-ISAPIFilter";

	public static readonly string IIS_ISApiExtensions = "IIS-ISAPIExtensions";

	public static readonly string IISAspNet45 = "IIS-ASPNET45";

	public static readonly string IISNetFxExtensibility45 = "IIS-NetFxExtensibility45";

	public static readonly string WcfHttpActivation45 = "WCF-HTTP-Activation45";

	public static readonly string IISApplicationInit = "IIS-ApplicationInit";

	public static readonly string ServerMediaFoundation = "ServerMediaFoundation";

	public static readonly string IISWebSockets = "IIS-WebSockets";

	public string Name { get; set; }

	public PackageFeatureState State { get; set; }

	private Feature()
	{
	}

	public Feature(string name, PackageFeatureState state)
	{
		Name = name;
		State = state;
	}
}

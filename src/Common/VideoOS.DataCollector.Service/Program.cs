using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoOS.Common.Crypto;
using VideoOS.Common.Logging;
using VideoOS.DataCollector.Media;
using VideoOS.DataCollector.Oem;
using VideoOS.DataCollector.Service;
using VideoOS.DataCollector.Service.Common.BackOff;
using VideoOS.DataCollector.Service.Common.Behaviours;
using VideoOS.DataCollector.Service.Common.ExtensionMethods;
using VideoOS.DataCollector.Service.Common.Hosting;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterDataCollector;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterServiceApplications;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.ReportMachineConfiguration;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.EventHandlers.RefreshActivePerformanceCounters;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.EventHandlers.RefreshInstalledServices;
using VideoOS.DataCollector.Service.Persistence;
using VideoOS.DataCollector.Service.Rule.Entities.Sample;
using VideoOS.DataCollector.Service.Sample.Factory;
using VideoOS.DataCollector.Service.Sample.Services;
using VideoOS.DataCollector.Service.Sample.Services.Rule;
using VideoOS.DataCollector.Service.Services;
using VideoOS.DataCollector.Service.Services.Hosted;
using VideoOS.IdentityServer.Common.Extensions.ClientCredentialsTokenCache;
using VideoOS.Utilities.OemResource;
using VideoOS.WebService.Common.Net.Extensions.ServiceProxy;

public class Program
{
	private static async Task _003CMain_003E_0024(string[] args)
	{
		DcAdHocLogConfiguration dcAdHocLogConfiguration = new DcAdHocLogConfiguration(new DataCollectorResources(new ResourcesFolderLocator()));
		LogConfigurationProvider val = new LogConfigurationProvider((IAdHocLogConfiguration)(object)dcAdHocLogConfiguration);
		LoggerFactory loggerFactory = new LoggerFactory((ILogConfiguration)(object)val);
		ILogger log = loggerFactory.CreateLogger("");
		try
		{
			WebApplicationOptions options = new WebApplicationOptions
			{
				Args = args,
				ContentRootPath = (WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : null)
			};
			WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(options);
			webApplicationBuilder.Host.UseWindowsService(delegate(WindowsServiceLifetimeOptions option)
			{
				option.ServiceName = "DataCollector";
			});
			webApplicationBuilder.Services.AddSingleton<ILogConfiguration, LogConfigurationProvider>();
			webApplicationBuilder.Services.AddSingleton<IAdHocLogConfiguration, DcAdHocLogConfiguration>();
			HostBuilderExtensions.UseVideoOSLogging((IHostBuilder)webApplicationBuilder.Host);
			webApplicationBuilder.Services.PostConfigure(delegate(HostOptions hostOptions)
			{
				hostOptions.ShutdownTimeout = TimeSpan.FromSeconds(60.0);
			});
			webApplicationBuilder.Services.AddMemoryCache();
			webApplicationBuilder.Services.AddMediatR(delegate(MediatRServiceConfiguration cfg)
			{
				cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
			});
			webApplicationBuilder.Services.AddSingleton<INativeMethods, NativeMethodsAccessor>();
			webApplicationBuilder.Services.AddSingleton<IConfigurationCache, ConfigurationCache>();
			webApplicationBuilder.Services.AddSingleton<IApplicationIdService, ApplicationIdService>();
			webApplicationBuilder.Services.AddSingleton<IResourcesFolderLocator, ResourcesFolderLocator>();
			webApplicationBuilder.Services.AddSingleton<IDataCollectorResources, DataCollectorResources>();
			webApplicationBuilder.Services.AddSingleton<IFailoverServerResources, FailoverServerResources>();
			webApplicationBuilder.Services.AddFileSecretProvider(delegate(IServiceProvider sp, FileSecretProviderOptions fileSecretProviderOptions)
			{
				fileSecretProviderOptions.Folder = sp.GetRequiredService<IDataCollectorResources>().CommonApplicationDataFolder;
			});
			webApplicationBuilder.Services.ConfigureAndValidateOptions();
			DataCollectorSettings dataCollectorConfig = webApplicationBuilder.Configuration.GetValid<DataCollectorSettings>();
			webApplicationBuilder.Services.AddClientCredentialsTokenCache(delegate(TokenCacheOptions tokenCacheOptions)
			{
				tokenCacheOptions.AuthorizationServer = dataCollectorConfig.AuthorizationServerUri;
				tokenCacheOptions.ClientId = dataCollectorConfig.ClientRegistrationId.ToString();
			}).AddDataCollectorServiceProxy(delegate(ServerProxyOptions serverProxyOptions)
			{
				serverProxyOptions.ManagementServerAddress = new Uri(dataCollectorConfig.ManagementServerAddress);
			});
			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
			webApplicationBuilder.Services.AddAuthentication("Bearer").AddJwtBearer(delegate(JwtBearerOptions o)
			{
				o.Audience = "datacollector";
				o.Authority = dataCollectorConfig.AuthorizationServerUri.ToString();
				o.RequireHttpsMetadata = dataCollectorConfig.AuthorizationServerUri.Scheme == Uri.UriSchemeHttps;
			});
			webApplicationBuilder.Services.AddAuthorization();
			webApplicationBuilder.Services.AddSingleton<IPerformanceCounterValueChannel, PerformanceCounterValueChannel>();
			webApplicationBuilder.Services.AddSingleton<PerformanceCounterReaderService>();
			webApplicationBuilder.Services.AddTransient<WaitForAppStartupService>();
			webApplicationBuilder.Services.AddTransient<IRegisterInstalledServicesService, RegisterInstalledServicesService>();
			webApplicationBuilder.Services.AddTransient<IMachineProperties, MachineProperties>();
			webApplicationBuilder.Services.AddSingleton<INotifyComplete, NotifyCompleteService>();
			webApplicationBuilder.Services.AddSingleton<IWarningLoggedStateService, WarningLoggedStateService>();
			webApplicationBuilder.Services.AddTransient<IPerformanceCounterInstancesService, PerformanceCounterInstancesService>();
			webApplicationBuilder.Services.AddTransient<IInstallerServicesService, InstallerServicesService>();
			webApplicationBuilder.Services.AddSingleton<IActivePerformanceCounterRepository, ActivePerformanceCounterRepository>();
			webApplicationBuilder.Services.AddSingleton<ILiveCounterRepository, LiveCounterRepository>();
			webApplicationBuilder.Services.AddSingleton<IInstallerServicesRepository, InstallerServicesRepository>();
			webApplicationBuilder.Services.AddSingleton<IRuleRepository, RuleRepository>();
			webApplicationBuilder.Services.AddSingleton<IRuleStateChangeChannel, RuleStateChangeChannel>();
			webApplicationBuilder.Services.AddSingleton<IServiceSampler, ServiceSampler>();
			webApplicationBuilder.Services.AddSingleton<IServiceControlManager, ServiceControlManager>();
			webApplicationBuilder.Services.AddSingleton<INvidiaSampler, NvidiaDataSampler>();
			webApplicationBuilder.Services.AddSingleton<IPerformanceCounterValueSampler, PerformanceCounterValueSampler>();
			webApplicationBuilder.Services.AddSingleton<ISamplerFactory, SamplerFactory>();
			webApplicationBuilder.Services.AddSingleton<ICounterRuleFactory, CounterRuleFactory>();
			webApplicationBuilder.Services.AddSingleton<INvidiaGPURuleFactory, NvidiaGPURuleFactory>();
			webApplicationBuilder.Services.AddSingleton<IDataCollection, DataCollectionProxy>();
			webApplicationBuilder.Services.AddSingleton<INvidiaInformationWrapper, NvidiaInformationWrapper>();
			webApplicationBuilder.Services.AddExponentialBackOffFactory();
			webApplicationBuilder.Services.AddSingleton<IStateHandlerFactory, StateHandlerFactory>();
			webApplicationBuilder.Services.AddTransient(typeof(IPipelineBehavior<, >), typeof(LoggingBehaviour<, >));
			webApplicationBuilder.Services.AddTransient(typeof(IPipelineBehavior<, >), typeof(SendBehaviour<, >));
			webApplicationBuilder.Services.AddHostedService<BatchSenderService>();
			webApplicationBuilder.Services.AddHostedService<ReportMachinePropertiesService>();
			webApplicationBuilder.Services.AddHostedService<RegisterServiceApplicationsService>();
			webApplicationBuilder.Services.AddHostedService<ReportGpuCounterInstancesService>();
			webApplicationBuilder.Services.AddHostedService<ConfigurationRefreshService>();
			webApplicationBuilder.Services.AddHostedService<CollectPerformanceCounterService>();
			webApplicationBuilder.Services.AddHostedService<RuleConfigurationRefreshService>();
			webApplicationBuilder.Services.AddHostedService<StateSenderService>();
			webApplicationBuilder.Services.AddHostedService<SampleRulesService>();
			webApplicationBuilder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
			webApplicationBuilder.Services.AddGrpc();
			webApplicationBuilder.WebHost.ConfigureKestrelEndpoint();
			WebApplication webApplication = webApplicationBuilder.Build();
			webApplication.UseRouting();
			webApplication.UseGrpcWeb();
			webApplication.UseAuthentication();
			webApplication.UseAuthorization();
			webApplication.MapGrpcService<DataCollectorGrpcService>().EnableGrpcWeb();
			ILogger<Program> requiredService = webApplication.Services.GetRequiredService<ILogger<Program>>();
			IOptions<PerformanceCounterOptions> requiredService2 = webApplication.Services.GetRequiredService<IOptions<PerformanceCounterOptions>>();
			StartupLogging.Log(requiredService, requiredService2);
			await webApplication.RunAsync();
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception value)
		{
			log.LogError($"{value}");
			Environment.Exit(1);
		}
		finally
		{
			log.LogInformation("DataCollector is shutting down.");
			loggerFactory.Dispose();
		}
	}
}

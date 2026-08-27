using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.Json.Serialization;
using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.EntityFramework.Options;
using FluentValidation;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using VideoOS.IdentityServer.Behaviours;
using VideoOS.IdentityServer.Configuration;
using VideoOS.IdentityServer.Configuration.Interfaces;
using VideoOS.IdentityServer.Data;
using VideoOS.IdentityServer.DomainEventInfrastructure;
using VideoOS.IdentityServer.DomainEvents.VmsUser;
using VideoOS.IdentityServer.Endpoints;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.EventLogInfrastructure;
using VideoOS.IdentityServer.Features.ClientManagement.Register;
using VideoOS.IdentityServer.Features.SCIM.CreateUser;
using VideoOS.IdentityServer.Features.SCIM.DeleteUser;
using VideoOS.IdentityServer.Features.SCIM.GetSchema;
using VideoOS.IdentityServer.Features.SCIM.GetUser;
using VideoOS.IdentityServer.Features.SCIM.PatchUser;
using VideoOS.IdentityServer.Features.SCIM.PutUser;
using VideoOS.IdentityServer.Features.SCIM.Shared;
using VideoOS.IdentityServer.Filters;
using VideoOS.IdentityServer.GrantValidators;
using VideoOS.IdentityServer.Helper;
using VideoOS.IdentityServer.Localization;
using VideoOS.IdentityServer.Middleware;
using VideoOS.IdentityServer.Model.Responses;
using VideoOS.IdentityServer.Oem;
using VideoOS.IdentityServer.Repositories;
using VideoOS.IdentityServer.Repositories.Context;
using VideoOS.IdentityServer.Repositories.Interfaces;
using VideoOS.IdentityServer.Resources;
using VideoOS.IdentityServer.Security;
using VideoOS.IdentityServer.Services;
using VideoOS.IdentityServer.Services.CleanupInactiveUsers;
using VideoOS.IdentityServer.Services.Interfaces;
using VideoOS.IdentityServer.Services.Providers;
using VideoOS.IdentityServer.Services.WellKnownConfigurationRetriever;
using VideoOS.IdentityServer.Validators;
using VideoOS.Utilities.Timers;

namespace VideoOS.IdentityServer;

public class Startup
{
	private readonly IServiceProvider _serviceProvider;

	private readonly IWebHostEnvironment _environment;

	public IConfiguration Configuration { get; }

	public Startup(IConfiguration config, IServiceProvider serviceProvider, IWebHostEnvironment environment)
	{
		_serviceProvider = serviceProvider;
		_environment = environment;
		Configuration = config;
	}

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddSingleton<IOemResources, OemResourcesProvider>();
		services.AddSingleton<IDatabaseConfiguration, DatabaseConfiguration>();
		services.Configure<IdentityServerConfiguration>(Configuration);
		services.AddControllers(delegate(MvcOptions options)
		{
			options.Filters.Add<CheckModelForNullAttribute>();
			options.Filters.Add<ValidateModelFilterAttribute>();
			options.Filters.Add<ApiExceptionFilterAttribute>();
			options.Filters.Add(typeof(TrackActionPerformanceFilter));
		}).AddJsonOptions(delegate(JsonOptions options)
		{
			options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
			options.JsonSerializerOptions.WriteIndented = true;
		}).AddNewtonsoftJson(delegate(MvcNewtonsoftJsonOptions options)
		{
			options.SerializerSettings.Formatting = Formatting.Indented;
			options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
		});
		services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
		services.AddScoped<ILoginProviderRepository, LoginProviderRepository>();
		services.TryAddScoped<ILoginProviderRestrictions, LoginProviderRestrictions>();
		services.AddScoped<IAdminIdentityRepository, AdminIdentityRepository>();
		services.AddScoped<ITrustedIssuerRepository, TrustedIssuerRepository>();
		services.AddScoped<IEventLogRepository, EventLogRepository>();
		services.AddTransient<IWindowsAuthenticationProvider, WindowsAuthenticationProvider>();
		services.AddTransient<IExternalTokenValidator, ExternalTokenValidator>();
		services.AddTransient<IUserProvisioner, UserProvisioner>();
		services.AddTransient<IUserInfoProvider, UserInfoProvider>();
		services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
		services.AddScoped<IDbContextProvider<IdentityDbContextWrapper>, EfContextProvider<IdentityDbContextWrapper>>();
		services.Replace(new ServiceDescriptor(typeof(IPasswordHasher<VmsUser>), typeof(PasswordHasherWithLegacySupport), ServiceLifetime.Scoped));
		services.AddSingleton(delegate(IServiceProvider serviceProvider)
		{
			IOptions<PasswordHasherOptions> options = Options.Create(new PasswordHasherOptions
			{
				CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3,
				IterationCount = 10000
			});
			ILogger<PasswordHasherWithLegacySupport> requiredService = serviceProvider.GetRequiredService<ILogger<PasswordHasherWithLegacySupport>>();
			return new PasswordHasherForOldUserMigration(options, serviceProvider, requiredService);
		});
		services.AddSingleton(typeof(IPasswordRequirements), typeof(PasswordRequirements));
		services.AddScoped<CustomCookieEventsHandler>();
		services.AddAuthentication(delegate(Microsoft.AspNetCore.Authentication.AuthenticationOptions options)
		{
			options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
			options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
			options.DefaultScheme = IdentityConstants.ApplicationScheme;
		}).AddLocalApi().AddCookie("IDP.ForcePasswordChange", delegate(CookieAuthenticationOptions options)
		{
			options.LoginPath = "/Identity/Account/Login";
			options.SlidingExpiration = false;
			options.ExpireTimeSpan = TimeSpan.FromMinutes(5.0);
			options.Cookie.Name = ".IDP.ForceChangePassword";
			options.Cookie.HttpOnly = true;
			options.Cookie.SecurePolicy = (Configuration.Get<IdentityServerConfiguration>().AllowInsecureCookies ? CookieSecurePolicy.None : CookieSecurePolicy.SameAsRequest);
			options.Cookie.SameSite = SameSiteMode.Lax;
			options.LogoutPath = "/Identity/Account/Logout/Index";
			options.ReturnUrlParameter = "ReturnUrl";
			options.EventsType = typeof(CustomCookieEventsHandler);
		});
		services.Configure(delegate(IISServerOptions iis)
		{
			iis.AuthenticationDisplayName = "Windows";
			iis.AutomaticAuthentication = false;
		});
		services.AddAuthorization(delegate(AuthorizationOptions options)
		{
			options.AddPolicy("IdentityServerAccessToken", delegate(AuthorizationPolicyBuilder policy)
			{
				policy.RequireClaim("scope", "write:client");
				policy.AddAuthenticationSchemes("IdentityServerAccessToken");
				policy.RequireAuthenticatedUser();
			});
			options.AddPolicy("IdpManagementPolicy", delegate(AuthorizationPolicyBuilder policy)
			{
				policy.RequireClaim("scope", "idp_management");
				policy.AddAuthenticationSchemes("IdentityServerAccessToken");
				policy.RequireAuthenticatedUser();
			});
			options.AddPolicy("IdpAuditEventPolicy", delegate(AuthorizationPolicyBuilder policy)
			{
				policy.RequireClaim("scope", "idp_log_event");
				policy.AddAuthenticationSchemes("IdentityServerAccessToken");
				policy.RequireAuthenticatedUser();
			});
			options.AddPolicy("IdpChangePasswordPolicy", delegate(AuthorizationPolicyBuilder policy)
			{
				policy.AddAuthenticationSchemes("IDP.ForcePasswordChange", IdentityConstants.ApplicationScheme);
				policy.RequireAuthenticatedUser();
			});
		});
		IDbContextConfigure builderConfigure = _serviceProvider.GetService<IDbContextConfigure>();
		if (builderConfigure == null)
		{
			builderConfigure = new ProductionDbContextConfigure(new DatabaseConfiguration());
			services.Add(new ServiceDescriptor(typeof(IDbContextConfigure), builderConfigure));
		}
		services.AddDbContext<ApplicationDbContext>(builderConfigure.ContextOptionsBuilder);
		services.AddDbContextWithInterceptor<IdentityDbContextWrapper>(builderConfigure.ContextOptionsBuilder).AddDomainEventInterceptor().AddEventLogInterceptor()
			.Register();
		services.AddDbContextWithInterceptor<SeededConfigurationDbContext>(builderConfigure.ContextOptionsBuilder).AddEventLogInterceptor().Register();
		services.AddDbContext<DataProtectionKeysDbContext>(builderConfigure.ContextOptionsBuilder);
		string applicationName = _environment.ContentRootPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		IDataProtectionBuilder builder = services.AddDataProtection().SetApplicationName(applicationName);
		builder.PersistKeysToDbContext<DataProtectionKeysDbContext>();
		if (string.IsNullOrEmpty(Configuration.Get<IdentityServerConfiguration>().DataProtectionSettings.ProtectKeysWithCertificate.Thumbprint))
		{
			builder.ProtectKeysWithDpapi();
		}
		else
		{
			builder.ProtectKeysWithCertificate(Configuration.Get<IdentityServerConfiguration>().DataProtectionSettings.ProtectKeysWithCertificate.Thumbprint);
			if (Configuration.Get<IdentityServerConfiguration>().DataProtectionSettings.ProtectKeysWithCertificate.CleanUpNonCertificateKeys)
			{
				services.AddHostedService<KeyCleanupService>();
			}
		}
		services.AddIdentity<VmsUser, IdentityRole>(delegate(IdentityOptions options)
		{
			options.SignIn.RequireConfirmedEmail = false;
			options.User.RequireUniqueEmail = false;
			options.User.AllowedUserNameCharacters = null;
			options.ClaimsIdentity.UserIdClaimType = "sub";
			options.ClaimsIdentity.UserNameClaimType = "name";
			options.Lockout.AllowedForNewUsers = true;
			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Configuration.Get<IdentityServerConfiguration>().LockoutSettings.LockoutTimeSpanInMinutes);
			options.Lockout.MaxFailedAccessAttempts = Configuration.Get<IdentityServerConfiguration>().LockoutSettings.MaxFailedAccessAttempts;
			options.Password.RequireDigit = false;
			options.Password.RequireLowercase = false;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireUppercase = false;
			options.Password.RequiredLength = 0;
			options.Password.RequiredUniqueChars = 0;
		}).AddEntityFrameworkStores<IdentityDbContextWrapper>().AddDefaultTokenProviders()
			.AddPasswordValidator<PasswordValidator>()
			.AddUserStore<VmsUserStore>();
		services.AddTransient<IUserStore<VmsUser>, VmsUserStore>();
		services.TryAddScoped<VmsUserManager>();
		services.AddOptions<KeyManagementSettings>().Bind(Configuration.GetSection("KeyManagementSettings")).ValidateDataAnnotations()
			.ValidateOnStart();
		services.AddIdentityServer(delegate(IdentityServerOptions options)
		{
			options.Authentication.CookieAuthenticationScheme = IdentityConstants.ApplicationScheme;
			options.Logging = new LoggingOptions
			{
				TokenRequestSensitiveValuesFilter = new HashSet<string> { "client_secret", "password", "client_assertion", "refresh_token", "device_code", "code", "username", "subject_token" }
			};
			options.UserInteraction.ErrorUrl = "/Error";
			options.IssuerUri = Configuration.Get<IdentityServerConfiguration>().IdentityProviderSettings.Authority;
			options.InputLengthRestrictions.ClientSecret = 200;
			options.AccessTokenJwtType = "JWT";
			options.EmitStaticAudienceClaim = true;
			options.Events.RaiseSuccessEvents = false;
			options.Events.RaiseFailureEvents = false;
			options.Events.RaiseErrorEvents = false;
			options.Discovery.CustomEntries.Add("external_providers_uri", "~/.well-known/openid-configuration/externalproviders");
			options.Discovery.CustomEntries.Add("server_version", $"{25}.{3}.{1}");
			options.LicenseKey = "eyJhbGciOiJQUzI1NiIsImtpZCI6IklkZW50aXR5U2VydmVyTGljZW5zZWtleS83Y2VhZGJiNzgxMzA0NjllODgwNjg5MTAyNTQxNGYxNiIsInR5cCI6ImxpY2Vuc2Urand0In0.eyJpc3MiOiJodHRwczovL2R1ZW5kZXNvZnR3YXJlLmNvbSIsImF1ZCI6IklkZW50aXR5U2VydmVyIiwiaWF0IjoxNzQ3Nzg1NjAwLCJleHAiOjE3ODA1MzEyMDAsImNvbXBhbnlfbmFtZSI6Ik1pbGVzdG9uZSBTeXN0ZW1zIEEvUyIsImNvbnRhY3RfaW5mbyI6ImNvbnRhY3RAZHVlbmRlc29mdHdhcmUuY29tIiwiZWRpdGlvbiI6IkJ1c2luZXNzIiwiaWQiOiI3OTkxIiwiZmVhdHVyZSI6WyJpc3YiLCJ1bmxpbWl0ZWRfY2xpZW50cyJdLCJwcm9kdWN0IjoiTWlsZXN0b25lIFhQcm90ZWN0In0.VPAIGeexSqZMrH1tdImnbgHAwYvyfqWha3jFm8WjpiVMHpn3nJGwLls3LPAGTf4KesTDx8YtDKs03sitGerDWI9SnoZxbTn8zInby3DKKtakKt4lSIdCYZokeCOhmD70fKrh9-EDpDtAqdZWNfje6xAf5m1I7E6vRHIY_gtqGGrF2GZY3cmHohamxEhrPnWqwAYIXDMomEJWDk5RgpbrOH7C2lgcyulF6fpz5z2Zs3y_5h3C7sukSbml5dmixb1VUfltImoWRA7co01-bySdTHPuLxygmFWps4FbOkruKUFIAV6up9DtTl6L7lrq8bkQP0seS7FnyXPhIyrnQnvofg";
			options.KeyManagement.RotationInterval = TimeSpan.FromDays(Configuration.Get<IdentityServerConfiguration>().KeyManagementSettings.RotationIntervalInDays);
			options.KeyManagement.PropagationTime = TimeSpan.FromDays(Configuration.Get<IdentityServerConfiguration>().KeyManagementSettings.PropagationTimeInDays);
			options.KeyManagement.RetentionDuration = TimeSpan.FromDays(Configuration.Get<IdentityServerConfiguration>().KeyManagementSettings.RetentionDurationInDays);
		}).AddConfigurationStore(delegate(ConfigurationStoreOptions options)
		{
			options.ConfigureDbContext = builderConfigure.ContextOptionsBuilder;
			options.DefaultSchema = "dbo";
		}).AddOperationalStore(delegate(OperationalStoreOptions options)
		{
			options.ConfigureDbContext = builderConfigure.ContextOptionsBuilder;
			options.EnableTokenCleanup = true;
			options.TokenCleanupInterval = 30;
			options.DefaultSchema = "dbo";
		})
			.AddAspNetIdentity<VmsUser>()
			.AddExtensionGrantValidator<WinAuthGrantValidator>()
			.AddExtensionGrantValidator<WindowsCredentialsGrantValidator>()
			.AddExtensionGrantValidator<TokenExchangeGrantValidator>()
			.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
			.AddProfileService<ProfileService>()
			.AddAppAuthRedirectUriValidator();
		services.Configure(delegate(PasswordHasherOptions options)
		{
			options.IterationCount = 210000;
		});
		services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
		services.AddSingleton<IDateTimeHelper, DateTimeHelper>();
		services.AddSingleton<IKeyConverter, KeyConverter>();
		services.AddTransient<IEventLogService, EventLogService>();
		services.AddHostedService<MigratorHostedService>();
		services.AddHostedService((IServiceProvider serviceProvider) => new DeleteEventLogsHostedService(serviceProvider.GetService<IServiceProvider>(), new ThreadingTimerWrapper(), serviceProvider.GetService<IDateTimeHelper>(), serviceProvider.GetService<ILogger<DeleteEventLogsHostedService>>()));
		services.AddHostedService<InitializeExternalProvidersService>();
		services.AddScoped<IInactiveUserCleanupService, InactiveUserCleanupService>();
		services.AddHostedService<InactiveUserCleanupBackgroundService>();
		services.AddSingleton<StringLocalizer>();
		services.Configure(delegate(RequestLocalizationOptions options)
		{
			options.DefaultRequestCulture = LocalizationContants.DefaultCulture;
			options.SupportedCultures = LocalizationContants.SupportedCultures;
			options.SupportedUICultures = LocalizationContants.SupportedCultures.ToList();
			options.RequestCultureProviders = new List<IRequestCultureProvider>
			{
				new QueryStringRequestCultureProvider(),
				new CookieRequestCultureProvider
				{
					CookieName = ".IDP.Culture"
				}
			};
		});
		services.AddMvc().AddRazorPagesOptions(delegate(RazorPagesOptions options)
		{
			options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/ChangePassword", string.Empty);
			options.Conventions.AddAreaFolderApplicationModelConvention("Identity", "/", delegate(PageApplicationModel model)
			{
				model.Filters.Add(new RedirectAntiForgeryValidationFailedResultFilter());
			});
			options.Conventions.AddAreaPageApplicationModelConvention("Identity", "/Account/Manage/ChangePassword", delegate(PageApplicationModel model)
			{
				model.Filters.Add(new TypeFilterAttribute(typeof(ForceLoginFilter)));
			});
		}).ConfigureApiBehaviorOptions(delegate(ApiBehaviorOptions options)
		{
			options.InvalidModelStateResponseFactory = delegate(ActionContext ctx)
			{
				ErrorResponse error = ErrorResponse.FromInvalidModelState(ctx.ModelState);
				return new BadRequestObjectResult(error);
			};
		})
			.AddViewLocalization()
			.AddDataAnnotationsLocalization(delegate(MvcDataAnnotationsLocalizationOptions options)
			{
				options.DataAnnotationLocalizerProvider = (Type _, IStringLocalizerFactory factory) => factory.Create(typeof(IdentityResource));
			});
		services.AddRazorPages().AddViewLocalization();
		services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidationAttributeAdapterProvider>();
		services.AddMediatR(delegate(MediatRServiceConfiguration cfg)
		{
			cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
			cfg.NotificationPublisherType = typeof(TaskWhenAllPublisher);
		});
		services.ConfigureApplicationCookie(delegate(CookieAuthenticationOptions options)
		{
			options.LoginPath = "/Identity/Account/Login";
			options.SlidingExpiration = true;
			options.ExpireTimeSpan = TimeSpan.FromMinutes(Configuration.Get<IdentityServerConfiguration>().LoginSettings.ExpireTimeInMinutes);
			options.Cookie.Name = ".IDP.Application";
			options.Cookie.HttpOnly = true;
			options.Cookie.SecurePolicy = (Configuration.Get<IdentityServerConfiguration>().AllowInsecureCookies ? CookieSecurePolicy.None : CookieSecurePolicy.SameAsRequest);
			options.Cookie.SameSite = SameSiteMode.Lax;
			options.LogoutPath = "/Identity/Account/Logout/Index";
			options.ReturnUrlParameter = "ReturnUrl";
			options.EventsType = typeof(CustomCookieEventsHandler);
		});
		services.ConfigureExternalCookie(delegate(CookieAuthenticationOptions options)
		{
			options.Cookie.HttpOnly = true;
			options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
			options.Cookie.SameSite = SameSiteMode.Lax;
		});
		services.AddSingleton<IPostConfigureOptions<OpenIdConnectOptions>, OpenIdConnectPostConfigureOptions>();
		services.AddScoped<IExternalProviderManager, ExternalProviderManager>();
		services.AddSingleton<DataProtectionPurposeStrings>();
		services.AddWellKnownConfigurationRetriever();
		services.Configure(delegate(CookieTempDataProviderOptions options)
		{
			options.Cookie.Name = ".IDP.TempDataProvider";
		});
		services.AddSession(delegate(SessionOptions options)
		{
			options.Cookie.Name = ".IDP.Session";
			options.Cookie.HttpOnly = true;
			options.Cookie.SecurePolicy = CookieSecurePolicy.None;
			options.Cookie.SameSite = SameSiteMode.Strict;
			options.Cookie.IsEssential = true;
		});
		services.AddAntiforgery(delegate(AntiforgeryOptions options)
		{
			options.Cookie.Name = ".IDP.XSRF-CSRF";
			options.Cookie.HttpOnly = true;
			options.Cookie.SecurePolicy = CookieSecurePolicy.None;
			options.Cookie.SameSite = SameSiteMode.Strict;
		});
		services.AddVmsUserDomainEvents();
		services.AddDomainEventInfrastructure();
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		services.AddTransient(typeof(IPipelineBehavior<, >), typeof(ValidationBehavior<, >));
		services.AddScimFeatureServices();
		services.AddCreateScimUser();
		services.AddPutScimUser();
		services.AddPatchScimUser();
		services.AddGetScimSchema();
		services.AddGetScimUser();
		services.AddDeleteScimUser();
		services.AddClientRegistration();
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
	{
		app.UseMiddleware<RequestResponseLogMiddleware>(Array.Empty<object>());
		app.UseExceptionHandler("/error-api");
		app.UseMiddleware<CultureMiddleware>(Array.Empty<object>());
		app.UseForwardedHeaders(BuildForwardedHeadersOptions());
		app.UseCookiePolicy(new CookiePolicyOptions
		{
			HttpOnly = HttpOnlyPolicy.Always,
			Secure = (Configuration.Get<IdentityServerConfiguration>().AllowInsecureCookies ? CookieSecurePolicy.None : CookieSecurePolicy.SameAsRequest),
			MinimumSameSitePolicy = SameSiteMode.Lax
		});
		app.UseDomainEventRegistrations();
		app.UseRequestLocalization(app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);
		app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");
		app.UseScimFeature();
		app.UseMiddleware<ApiDisableStatusCodeFeatureMiddleware>(Array.Empty<object>());
		app.UseStaticFiles();
		app.UseRouting();
		app.UseSession();
		app.UseIdentityServer();
		JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
		app.UseAuthentication();
		app.UseAuthorization();
		app.UseEndpoints(delegate(IEndpointRouteBuilder endpoints)
		{
			endpoints.MapDefaultControllerRoute();
			endpoints.MapRazorPages().RequireAuthorization();
		});
		app.MapEndpoints();
	}

	private ForwardedHeadersOptions BuildForwardedHeadersOptions()
	{
		ForwardedHeadersOptions forwardedHeadersOptions = new ForwardedHeadersOptions();
		forwardedHeadersOptions.ForwardedHeaders = ForwardedHeaders.All;
		ProxySettings proxySettings = Configuration.Get<IdentityServerConfiguration>().ProxySettings;
		forwardedHeadersOptions.ForwardLimit = proxySettings?.ForwardLimit ?? 1;
		forwardedHeadersOptions.KnownNetworks.Clear();
		forwardedHeadersOptions.KnownProxies.Clear();
		if (proxySettings?.KnownNetworks != null)
		{
			foreach (KeyValuePair<string, int> knownNetwork in proxySettings.KnownNetworks)
			{
				forwardedHeadersOptions.KnownNetworks.Add(new Microsoft.AspNetCore.HttpOverrides.IPNetwork(IPAddress.Parse(knownNetwork.Key), knownNetwork.Value));
			}
		}
		if (proxySettings?.KnownProxies != null)
		{
			foreach (string knownProxy in proxySettings.KnownProxies)
			{
				forwardedHeadersOptions.KnownProxies.Add(IPAddress.Parse(knownProxy));
			}
		}
		return forwardedHeadersOptions;
	}
}

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Server.Admin.Utilities;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class IISHelper_Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("VideoOS.Server.Admin.Utilities.IISHelper.Resources", typeof(IISHelper_Resources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string CertificateHttps_add_securebindings => ResourceManager.GetString("CertificateHttps_add_securebindings", resourceCulture);

	public static string CertificateHttps_certificate_and_https_binding_ok => ResourceManager.GetString("CertificateHttps_certificate_and_https_binding_ok", resourceCulture);

	public static string CertificateHttps_certificate_binding_hash_not_in_store => ResourceManager.GetString("CertificateHttps_certificate_binding_hash_not_in_store", resourceCulture);

	public static string CertificateHttps_certificate_hash_ok => ResourceManager.GetString("CertificateHttps_certificate_hash_ok", resourceCulture);

	public static string CertificateHttps_certificate_hashes_not_equal => ResourceManager.GetString("CertificateHttps_certificate_hashes_not_equal", resourceCulture);

	public static string CertificateHttps_certificate_or_https_binding_not_ok => ResourceManager.GetString("CertificateHttps_certificate_or_https_binding_not_ok", resourceCulture);

	public static string CertificateHttps_certificate_store_hash_binding_hash => ResourceManager.GetString("CertificateHttps_certificate_store_hash_binding_hash", resourceCulture);

	public static string CertificateHttps_check_certificate_and_https_binding => ResourceManager.GetString("CertificateHttps_check_certificate_and_https_binding", resourceCulture);

	public static string CertificateHttps_clear_securebindings => ResourceManager.GetString("CertificateHttps_clear_securebindings", resourceCulture);

	public static string CertificateHttps_COMException => ResourceManager.GetString("CertificateHttps_COMException", resourceCulture);

	public static string CertificateHttps_creating_https_binding => ResourceManager.GetString("CertificateHttps_creating_https_binding", resourceCulture);

	public static string CertificateHttps_creating_ssl_certificate_binding => ResourceManager.GetString("CertificateHttps_creating_ssl_certificate_binding", resourceCulture);

	public static string CertificateHttps_deleting_https_binding => ResourceManager.GetString("CertificateHttps_deleting_https_binding", resourceCulture);

	public static string CertificateHttps_deleting_ssl_certificate_binding => ResourceManager.GetString("CertificateHttps_deleting_ssl_certificate_binding", resourceCulture);

	public static string CertificateHttps_expected_ssl_port_and_found_string => ResourceManager.GetString("CertificateHttps_expected_ssl_port_and_found_string", resourceCulture);

	public static string CertificateHttps_for_IIS_version => ResourceManager.GetString("CertificateHttps_for_IIS_version", resourceCulture);

	public static string CertificateHttps_retry => ResourceManager.GetString("CertificateHttps_retry", resourceCulture);

	public static string CertificateHttps_secure_bindings_ok => ResourceManager.GetString("CertificateHttps_secure_bindings_ok", resourceCulture);

	public static string CertificateHttps_service_started => ResourceManager.GetString("CertificateHttps_service_started", resourceCulture);

	public static string CertificateHttps_service_stopped => ResourceManager.GetString("CertificateHttps_service_stopped", resourceCulture);

	public static string CertificateHttps_set_securebindings => ResourceManager.GetString("CertificateHttps_set_securebindings", resourceCulture);

	public static string CertificateHttps_ssl_port_in_binding_not_found => ResourceManager.GetString("CertificateHttps_ssl_port_in_binding_not_found", resourceCulture);

	public static string CertificateHttps_ssl_port_in_certificate_ok => ResourceManager.GetString("CertificateHttps_ssl_port_in_certificate_ok", resourceCulture);

	public static string CertificateHttps_starting_services => ResourceManager.GetString("CertificateHttps_starting_services", resourceCulture);

	public static string CertificateHttps_stopping_services => ResourceManager.GetString("CertificateHttps_stopping_services", resourceCulture);

	public static string Could_not_find_IIS_directory => ResourceManager.GetString("Could_not_find_IIS_directory", resourceCulture);

	public static string Could_not_find_the_IIS_registry => ResourceManager.GetString("Could_not_find_the_IIS_registry", resourceCulture);

	public static string Could_not_get_name_of_the_web_site => ResourceManager.GetString("Could_not_get_name_of_the_web_site", resourceCulture);

	public static string Could_not_get_root_website_or_one_of_its_children => ResourceManager.GetString("Could_not_get_root_website_or_one_of_its_children", resourceCulture);

	public static string Creating_certificate => ResourceManager.GetString("Creating_certificate", resourceCulture);

	public static string Creating_https_binding => ResourceManager.GetString("Creating_https_binding", resourceCulture);

	public static string Deleting_certificate => ResourceManager.GetString("Deleting_certificate", resourceCulture);

	public static string Error_Add_SSL_cert => ResourceManager.GetString("Error_Add_SSL_cert", resourceCulture);

	public static string Error_adsutil_not_found => ResourceManager.GetString("Error_adsutil_not_found", resourceCulture);

	public static string Error_authenticating_user_service => ResourceManager.GetString("Error_authenticating_user_service", resourceCulture);

	public static string Error_Cannot_create_certificate => ResourceManager.GetString("Error_Cannot_create_certificate", resourceCulture);

	public static string Error_Cannot_import_certificate => ResourceManager.GetString("Error_Cannot_import_certificate", resourceCulture);

	public static string Error_Cannot_read_certificate_store => ResourceManager.GetString("Error_Cannot_read_certificate_store", resourceCulture);

	public static string Error_certutil_not_found => ResourceManager.GetString("Error_certutil_not_found", resourceCulture);

	public static string Error_cscript_not_found => ResourceManager.GetString("Error_cscript_not_found", resourceCulture);

	public static string Error_Delete_secure_bindings => ResourceManager.GetString("Error_Delete_secure_bindings", resourceCulture);

	public static string Error_Get_secure_bindings => ResourceManager.GetString("Error_Get_secure_bindings", resourceCulture);

	public static string Error_httpcfg_not_found => ResourceManager.GetString("Error_httpcfg_not_found", resourceCulture);

	public static string Error_Https_binding_on_website_and_port_has_no_certificate_associated => ResourceManager.GetString("Error_Https_binding_on_website_and_port_has_no_certificate_associated", resourceCulture);

	public static string Error_Iis_controlled_start => ResourceManager.GetString("Error_Iis_controlled_start", resourceCulture);

	public static string Error_Iis_controlled_stop => ResourceManager.GetString("Error_Iis_controlled_stop", resourceCulture);

	public static string Error_Iis_no_services_to_start => ResourceManager.GetString("Error_Iis_no_services_to_start", resourceCulture);

	public static string Error_Invalid_certificate_hash => ResourceManager.GetString("Error_Invalid_certificate_hash", resourceCulture);

	public static string Error_Invalid_IIS_version => ResourceManager.GetString("Error_Invalid_IIS_version", resourceCulture);

	public static string Error_net_not_found => ResourceManager.GetString("Error_net_not_found", resourceCulture);

	public static string Error_netsh_not_found => ResourceManager.GetString("Error_netsh_not_found", resourceCulture);

	public static string Error_No_default_website => ResourceManager.GetString("Error_No_default_website", resourceCulture);

	public static string Error_Query_SSL_cert => ResourceManager.GetString("Error_Query_SSL_cert", resourceCulture);

	public static string Error_Set_secure_bindings => ResourceManager.GetString("Error_Set_secure_bindings", resourceCulture);

	public static string Error_Set_SSL_cert => ResourceManager.GetString("Error_Set_SSL_cert", resourceCulture);

	public static string Error_Show_SSL_cert => ResourceManager.GetString("Error_Show_SSL_cert", resourceCulture);

	public static string Failed_to_grant_access_for_user => ResourceManager.GetString("Failed_to_grant_access_for_user", resourceCulture);

	public static string Management_server_address => ResourceManager.GetString("Management_server_address", resourceCulture);

	public static string Not_yet_synchronized_abort => ResourceManager.GetString("Not_yet_synchronized_abort", resourceCulture);

	public static string Not_yet_synchronized_Check_authentication_timed_out => ResourceManager.GetString("Not_yet_synchronized_Check_authentication_timed_out", resourceCulture);

	public static string Not_yet_synchronized_Error_occurred_while_trying_to_check_authentication => ResourceManager.GetString("Not_yet_synchronized_Error_occurred_while_trying_to_check_authentication", resourceCulture);

	public static string Restarting_the_IIS_service => ResourceManager.GetString("Restarting_the_IIS_service", resourceCulture);

	public static string Retrying_in_4_seconds => ResourceManager.GetString("Retrying_in_4_seconds", resourceCulture);

	public static string Service_up_but_not_yet_synchronized => ResourceManager.GetString("Service_up_but_not_yet_synchronized", resourceCulture);

	public static string Service_up_but_not_yet_synchronized_Error_occurred_while_trying_to_check_authentication => ResourceManager.GetString("Service_up_but_not_yet_synchronized_Error_occurred_while_trying_to_check_authentication", resourceCulture);

	public static string SettingPreloadEnabled => ResourceManager.GetString("SettingPreloadEnabled", resourceCulture);

	public static string Starting_the_IIS_service => ResourceManager.GetString("Starting_the_IIS_service", resourceCulture);

	public static string Stopping_the_IIS_service => ResourceManager.GetString("Stopping_the_IIS_service", resourceCulture);

	public static string Synchronization_complete => ResourceManager.GetString("Synchronization_complete", resourceCulture);

	public static string Unable_to_find_the_web_site => ResourceManager.GetString("Unable_to_find_the_web_site", resourceCulture);

	public static string up_but_not_synchronized => ResourceManager.GetString("up_but_not_synchronized", resourceCulture);

	public IISHelper_Resources()
	{
	}
}

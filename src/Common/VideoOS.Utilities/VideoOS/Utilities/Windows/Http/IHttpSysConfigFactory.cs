namespace VideoOS.Utilities.Windows.Http;

public interface IHttpSysConfigFactory
{
	IHttpSysConfig CreateConfig();

	IHttpSysConfigReader CreateConfigReader();
}

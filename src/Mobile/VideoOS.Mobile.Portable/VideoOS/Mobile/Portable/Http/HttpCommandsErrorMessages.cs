using System.Text;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.Http;

[ExcludeFromCodeCoverage]
public static class HttpCommandsErrorMessages
{
	public static readonly string GETResponce = "<HTML><BODY><p><b>Mobile Server - Bad Method Type - Use POST</b></p></BODY></HTML>";

	public static readonly byte[] GETResponceBytes = Encoding.UTF8.GetBytes(GETResponce);

	public static readonly string ParsingError = "<HTML><BODY><p><b>Mobile Server - Incorrect command format - Check syntax</b></p></BODY></HTML>";

	public static readonly byte[] ParsingErrorBytes = Encoding.UTF8.GetBytes(ParsingError);

	public static readonly string EmptyrequestError = "<HTML><BODY><p><b>Mobile Server - Incorrect command format - No command extracted</b></p></BODY></HTML>";

	public static readonly byte[] EmptyrequestErrorBytes = Encoding.UTF8.GetBytes(EmptyrequestError);
}

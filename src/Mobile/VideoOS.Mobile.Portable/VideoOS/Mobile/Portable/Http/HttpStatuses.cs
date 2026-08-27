using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.Http;

public enum HttpStatuses
{
	[Description("Switching Protocols")]
	SwitchingProtocol = 101,
	[Description("OK")]
	Ok = 200,
	[Description("Moved Permanently")]
	MovedPermanently = 301,
	[Description("Found")]
	Found = 302,
	[Description("Not Modified")]
	NotModified = 304,
	[Description("Temporary Redirect")]
	TemporaryRedirect = 307,
	[Description("Bad Request")]
	BadRequest = 400,
	[Description("Unauthorized")]
	Unauthorized = 401,
	[Description("Forbidden")]
	Forbidden = 403,
	[Description("Not Found")]
	NotFound = 404,
	[Description("Method Not Allowed")]
	MethodNotAllowed = 405,
	[Description("Request Timeout")]
	RequestTimeout = 408,
	[Description("Gone")]
	Gone = 410,
	[Description("Request Entity Too Large")]
	RequestEntityTooLarge = 413,
	[Description("Internal Server Error")]
	InternalServerError = 500,
	[Description("Not Implemented")]
	NotImplemented = 501,
	[Description("Service Unavailable")]
	ServiceUnavailable = 503,
	[Description("HTTP Version Not Supported")]
	HttpVersionNotSupported = 505
}

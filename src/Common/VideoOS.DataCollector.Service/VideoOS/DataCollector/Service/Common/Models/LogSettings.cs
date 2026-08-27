using Microsoft.Extensions.Logging;

namespace VideoOS.DataCollector.Service.Common.Models;

public record LogSettings(LogLevel LogLevel = LogLevel.Information, string Message = "");

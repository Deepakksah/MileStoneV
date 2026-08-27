using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.ConfigurationItems;

namespace VideoOS.Mobile.ManagementServer;

public class ManagementServerOptions
{
	public const int DefaultBookmarkPreTime = 3;

	public const int DefaultBookmarkPostTime = 30;

	public const int DefaultAuditPlaybackSequenceLogLength = 60;

	public const bool DefaultAuditUserAccessLogging = false;

	public const bool DefaultCollectUsageData = false;

	public bool CollectUsageData { get; }

	public bool LogAuditUserAccessLogging { get; }

	public TimeSpan LogAuditPlaybackWindowSize { get; }

	public TimeSpan BookmarkPreTime { get; }

	public TimeSpan BookmarkPostTime { get; }

	public Dictionary<string, string> LoginProvidersAuthority { get; }

	public ManagementServerOptions()
	{
		CollectUsageData = false;
		LogAuditUserAccessLogging = true;
		LogAuditPlaybackWindowSize = TimeSpan.FromMinutes(1.0);
		BookmarkPreTime = TimeSpan.FromSeconds(3.0);
		BookmarkPostTime = TimeSpan.FromSeconds(30.0);
	}

	public ManagementServerOptions(ToolOptionServerLog logServerOptions, ToolOptionPrivacySetting privacySettingsOptions, ToolOptionBookmark bookmarkOptions, ICollection<LoginProvider> loginProviders)
		: this()
	{
		LogAuditPlaybackWindowSize = TimeSpan.FromSeconds(logServerOptions?.AuditPlaybackSequenceLogLength ?? 60);
		LogAuditUserAccessLogging = logServerOptions?.AuditUserAccessLogging ?? false;
		CollectUsageData = privacySettingsOptions?.CollectUsageDataEnabled ?? false;
		BookmarkPreTime = TimeSpan.FromSeconds(bookmarkOptions?.BookmarkPreTime ?? 3);
		BookmarkPostTime = TimeSpan.FromSeconds(bookmarkOptions?.BookmarkPostTime ?? 30);
		LoginProvidersAuthority = loginProviders?.ToDictionary((LoginProvider l) => l.Id.ToLower(), (LoginProvider l) => l.Authority);
	}
}

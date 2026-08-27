using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public delegate void OnResponse<in T>(T responseParams) where T : BaseCommandResponse;

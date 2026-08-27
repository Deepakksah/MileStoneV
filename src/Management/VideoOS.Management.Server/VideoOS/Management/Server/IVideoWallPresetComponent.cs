using System;

namespace VideoOS.Management.Server;

public interface IVideoWallPresetComponent : IDisposable
{
	void CreateVideoWallPreset(string videoWallId, VideoWallPreset videoWallPreset, Guid sessionId);

	void DeleteVideoWallPreset(string videoWallPresetId, Guid sessionId);

	string GetVideoWallId(string videoWallPresetId);

	VideoWallPreset QueryVideoWallPreset(string videoWallPresetId);

	string[] QueryVideoWallPresetIds(string videoWallId);

	VideoWallPreset[] QueryVideoWallPresetsByIds(string[] videoWallPresetIds);

	void UpdateVideoWallPreset(VideoWallPreset videoWallPreset, Guid sessionId);
}

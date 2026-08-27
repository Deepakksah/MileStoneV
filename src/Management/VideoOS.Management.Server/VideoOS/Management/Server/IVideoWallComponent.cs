using System;

namespace VideoOS.Management.Server;

public interface IVideoWallComponent : IDisposable
{
	void CreateVideoWall(VideoWall videoWall, string userSid);

	VideoWall QueryVideoWall(string videoWallId);

	VideoWall[] QueryVideoWallsByIds(string[] videoWallIds);

	void UpdateVideoWall(VideoWall videoWall, Guid sessionId);

	void DeleteVideoWall(string videoWallId, Guid sessionId);

	string[] QueryVideoWallIds();
}

using System;
using System.IO;
using System.Reflection;

namespace VideoOS.Platform.Data.MediaProviderInternal;

public class JpegMediaProviderChannel : MediaProviderChannel
{
	private static readonly byte[] _noVideoJpeg;

	static JpegMediaProviderChannel()
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		using Stream stream = executingAssembly.GetManifestResourceStream(executingAssembly.GetName().Name + ".Resources.video_push_no_video.jpg");
		_noVideoJpeg = new byte[stream.Length];
		stream.Read(_noVideoJpeg, 0, (int)stream.Length);
	}

	public JpegMediaProviderChannel(int channel)
		: base(channel)
	{
	}

	protected override MediaData GetMediaDataInIdle()
	{
		return new MediaData
		{
			Bytes = _noVideoJpeg,
			DateTime = DateTime.UtcNow,
			MediaContent = MediaContent.Jpeg
		};
	}
}

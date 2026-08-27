using System;

namespace VideoOS.Management.Server;

public interface ICompressorComponent : IDisposable
{
	Compressors[] QueryCompressors();

	Compressors QueryCompressor(long handler);

	void DeleteCompressor(long handler);

	void UpdateCompressor(long handler, Guid idCodec, int flags, int quality, int keyFrames, int dataRate, byte[] configData);
}

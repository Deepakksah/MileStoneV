namespace VideoOS.Platform.Primitives;

public interface IReadOnlyBuffer
{
	unsafe byte* Data { get; }

	ulong Length { get; }

	void CopyTo(ulong sourceIndex, byte[] destination, ulong destinationIndex, ulong length);

	void CopyTo(long sourceIndex, byte[] destination, long destinationIndex, long length);
}

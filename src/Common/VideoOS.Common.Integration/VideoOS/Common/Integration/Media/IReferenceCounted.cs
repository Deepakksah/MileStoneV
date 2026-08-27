namespace VideoOS.Common.Integration.Media;

public interface IReferenceCounted
{
	void IncrementReferenceCount();

	void DecrementReferenceCount();
}

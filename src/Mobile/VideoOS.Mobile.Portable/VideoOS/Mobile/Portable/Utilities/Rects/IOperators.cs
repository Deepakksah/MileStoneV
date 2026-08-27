namespace VideoOS.Mobile.Portable.Utilities.Rects;

public interface IOperators<T>
{
	T Add(T first, T second);

	T Sub(T first, T second);

	bool Greater(T first, T second);

	bool Lower(T first, T second);
}

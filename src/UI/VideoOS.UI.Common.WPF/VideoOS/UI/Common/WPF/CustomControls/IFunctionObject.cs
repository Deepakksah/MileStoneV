namespace VideoOS.UI.Common.WPF.CustomControls;

public interface IFunctionObject
{
	void Do();
}
public interface IFunctionObject<T>
{
	void Do(T input);

	T Do();
}
public interface IFunctionObject<in T1, out T2>
{
	T2 Do(T1 input);
}

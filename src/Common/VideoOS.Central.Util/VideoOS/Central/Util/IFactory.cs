namespace VideoOS.Central.Util;

public interface IFactory<T>
{
	T Create();
}
public interface IFactory<T, TArg1>
{
	T Create(TArg1 arg1);
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace msclr.interop;

public class marshal_context : IDisposable
{
	public class internal_marshaler_003Cwchar_t_0020const_0020_002A_002CSystem_003A_003AString_0020_005E_002C1_003E
	{
		public unsafe static char* marshal_as(string* _from, LinkedList<object> _clean_up_list)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out char* result);
			context_node_003Cwchar_t_0020const_0020_002A_002CSystem_003A_003AString_0020_005E_003E value = new context_node_003Cwchar_t_0020const_0020_002A_002CSystem_003A_003AString_0020_005E_003E(&result, *_from);
			_clean_up_list.AddLast(value);
			return result;
		}
	}

	public readonly LinkedList<object> _clean_up_list = new LinkedList<object>();

	private void _007Emarshal_context()
	{
		LinkedList<object>.Enumerator enumerator = _clean_up_list.GetEnumerator();
		if (enumerator.MoveNext())
		{
			do
			{
				if (enumerator.Current is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
			while (enumerator.MoveNext());
		}
		GC.KeepAlive(this);
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007Emarshal_context();
		}
		else
		{
			base.Finalize();
		}
	}

	public virtual sealed void Dispose()
	{
		Dispose(A_0: true);
		GC.SuppressFinalize(this);
		GC.KeepAlive(this);
	}

	public unsafe char* marshal_as_003Cwchar_t_0020const_0020_002A_002CSystem_003A_003AString_003E(string _from)
	{
		char* result = internal_marshaler_003Cwchar_t_0020const_0020_002A_002CSystem_003A_003AString_0020_005E_002C1_003E.marshal_as(&_from, _clean_up_list);
		GC.KeepAlive(this);
		return result;
	}
}

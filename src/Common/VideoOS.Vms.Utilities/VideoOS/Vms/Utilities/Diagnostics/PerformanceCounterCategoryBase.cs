using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace VideoOS.Vms.Utilities.Diagnostics;

public abstract class PerformanceCounterCategoryBase
{
	protected static class NativeMethods
	{
		[DllImport("Kernel32.dll")]
		public static extern bool QueryPerformanceCounter(out long value);

		[DllImport("Kernel32.dll")]
		public static extern bool QueryPerformanceFrequency(out long value);
	}

	private const uint LargeFileMappingSize = 1048576u;

	public abstract string CategoryName { get; }

	public static long QueryPerformanceCounter()
	{
		try
		{
			NativeMethods.QueryPerformanceCounter(out var value);
			return value;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return 0L;
		}
	}

	public static long QueryPerformanceFrequency()
	{
		try
		{
			NativeMethods.QueryPerformanceFrequency(out var value);
			return value;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return 0L;
		}
	}

	public PerformanceCounterCategoryBase()
	{
	}

	public abstract void CreateCategory();

	public virtual void DeleteCategory()
	{
		try
		{
			if (PerformanceCounterCategory.Exists(CategoryName))
			{
				ClearInstances();
				PerformanceCounterCategory.Delete(CategoryName);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
	}

	protected void SetLargeFileMappingSize()
	{
		Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\" + CategoryName + "\\Performance", "FileMappingSize", 1048576u, RegistryValueKind.DWord);
	}

	private void ClearInstances()
	{
		try
		{
			PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory(CategoryName);
			string[] instanceNames = performanceCounterCategory.GetInstanceNames();
			foreach (string instanceName in instanceNames)
			{
				PerformanceCounter[] counters = performanceCounterCategory.GetCounters(instanceName);
				foreach (PerformanceCounter performanceCounter in counters)
				{
					PerformanceCounter performanceCounter2 = new PerformanceCounter(CategoryName, performanceCounter.CounterName, instanceName, readOnly: false);
					if (!performanceCounter2.ReadOnly)
					{
						performanceCounter2.RawValue = 0L;
						performanceCounter2.Close();
						performanceCounter2.RemoveInstance();
					}
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
	}
}

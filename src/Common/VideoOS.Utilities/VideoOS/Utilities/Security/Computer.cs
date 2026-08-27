#define TRACE
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace VideoOS.Utilities.Security;

public static class Computer
{
	public static string GetName()
	{
		return Environment.MachineName;
	}

	public static Win32Api.NetJoinStatus GetJoinStatus(out string joinName)
	{
		joinName = string.Empty;
		string text = null;
		IntPtr domain = IntPtr.Zero;
		Win32Api.NetJoinStatus status = Win32Api.NetJoinStatus.NetSetupUnknownStatus;
		try
		{
			if (Win32Api.NetGetJoinInformation(null, out domain, out status) == 0 && status != Win32Api.NetJoinStatus.NetSetupUnknownStatus)
			{
				text = Marshal.PtrToStringUni(domain);
			}
		}
		finally
		{
			if (domain != IntPtr.Zero)
			{
				Win32Api.NetApiBufferFree(domain);
			}
		}
		if (text == null)
		{
			text = string.Empty;
		}
		joinName = text;
		return status;
	}

	private static SecurityIdentifier GetSecurityIdentifier(string name)
	{
		byte[] array = null;
		uint cbSid = 0u;
		StringBuilder stringBuilder = new StringBuilder();
		uint cchReferencedDomainName = (uint)stringBuilder.Capacity;
		if (!Win32Api.LookupAccountName(null, name, array, ref cbSid, stringBuilder, ref cchReferencedDomainName, out var peUse))
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error != 122)
			{
				throw new Win32Exception(lastWin32Error);
			}
			stringBuilder = new StringBuilder((int)cchReferencedDomainName);
			array = new byte[cbSid];
			stringBuilder.EnsureCapacity((int)cchReferencedDomainName);
			if (!Win32Api.LookupAccountName(null, name, array, ref cbSid, stringBuilder, ref cchReferencedDomainName, out peUse))
			{
				lastWin32Error = Marshal.GetLastWin32Error();
				throw new Win32Exception(lastWin32Error);
			}
		}
		if (!Win32Api.ConvertSidToStringSid(array, out var ptrSid))
		{
			throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to convert SID to a string.");
		}
		string sddlForm = Marshal.PtrToStringAuto(ptrSid);
		Win32Api.LocalFree(ptrSid);
		return new SecurityIdentifier(sddlForm);
	}

	public static SecurityIdentifier GetSecurityIdentifier()
	{
		Win32Api.OSVERSIONINFOEX osvi = default(Win32Api.OSVERSIONINFOEX);
		osvi.dwOSVersionInfoSize = (uint)Marshal.SizeOf(osvi);
		if (!Win32Api.GetVersionEx(ref osvi))
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		if (osvi.wProductType != Win32Api.OSVERSIONINFOEX.ProductType.VER_NT_DOMAIN_CONTROLLER)
		{
			return GetSecurityIdentifier(Environment.MachineName);
		}
		return GetSecurityIdentifier(Domain.GetComputerDomain().Name);
	}

	public static string[] GetNetworkAdapterMacAddresses(bool includeDisabledAdapters)
	{
		List<string> list = new List<string>();
		using ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
		foreach (ManagementObject instance in managementClass.GetInstances())
		{
			try
			{
				if (includeDisabledAdapters || instance["IPEnabled"].ToString() == "True")
				{
					list.Add(instance["MACAddress"].ToString().Trim().ToUpperInvariant());
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
		}
		return list.ToArray();
	}

	public static string[] GetNetworkAdapterMacAddresses()
	{
		return GetNetworkAdapterMacAddresses(includeDisabledAdapters: false);
	}

	private static string Dehex(string str)
	{
		string text = "";
		string text2 = "";
		for (int i = 0; i < str.Length; i++)
		{
			if (i > 0 && i % 2 == 0)
			{
				int value = int.Parse(text2, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
				text += Convert.ToChar(value);
				text2 = "";
			}
			text2 += str[i];
		}
		if (text2.Length > 0)
		{
			int value2 = int.Parse(text2, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			text += Convert.ToChar(value2);
		}
		return text;
	}

	private static string BEW2LEW(string str)
	{
		string text = "";
		for (int i = 0; i < str.Length; i += 2)
		{
			text = string.Concat(text, string.Concat(str[i + 1], str[i]));
		}
		return text;
	}

	public static string[] GetHardDiskSerialNumbers()
	{
		List<string> list = new List<string>();
		using ManagementClass managementClass = new ManagementClass("Win32_PhysicalMedia");
		foreach (ManagementObject instance in managementClass.GetInstances())
		{
			try
			{
				if (instance["SerialNumber"] == null)
				{
					continue;
				}
				string text = instance["SerialNumber"].ToString().Trim();
				if (text.Length == 0)
				{
					Trace.WriteLine("Skip serial with length 0");
					continue;
				}
				if (text.Length == 1 && text[0] == '\u001f')
				{
					Trace.WriteLine("Skip serial with single char (31)");
					continue;
				}
				string text2 = null;
				try
				{
					text2 = BEW2LEW(Dehex(text));
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
				}
				if (text2 == null)
				{
					list.Add(text);
				}
				else
				{
					list.Add(text2);
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
		}
		return list.ToArray();
	}

	public static string GetMotherboardSerialNumber()
	{
		using ManagementClass managementClass = new ManagementClass("Win32_BaseBoard");
		foreach (ManagementObject instance in managementClass.GetInstances())
		{
			if (instance["SerialNumber"] != null)
			{
				return instance["SerialNumber"].ToString().Trim();
			}
		}
		return string.Empty;
	}

	public static void ReplaceAllDescendantPermissions(DirectoryInfo directoryInfo, FileSystemAccessRule accessRule)
	{
		DirectorySecurity accessControl = directoryInfo.GetAccessControl();
		CanonicalizeDacl(accessControl);
		accessControl.AddAccessRule(accessRule);
		directoryInfo.SetAccessControl(accessControl);
		FileInfo[] files = directoryInfo.GetFiles();
		foreach (FileInfo obj in files)
		{
			FileSecurity accessControl2 = obj.GetAccessControl();
			CanonicalizeDacl(accessControl2);
			accessControl2.AddAccessRule(accessRule);
			obj.SetAccessControl(accessControl2);
		}
		directoryInfo.GetDirectories().ToList().ForEach(delegate(DirectoryInfo d)
		{
			ReplaceAllDescendantPermissions(d, accessRule);
		});
	}

	public static void CanonicalizeDacl(NativeObjectSecurity objectSecurity)
	{
		if (objectSecurity == null)
		{
			throw new ArgumentNullException("objectSecurity");
		}
		if (objectSecurity.AreAccessRulesCanonical)
		{
			return;
		}
		RawSecurityDescriptor rawSecurityDescriptor = new RawSecurityDescriptor(objectSecurity.GetSecurityDescriptorSddlForm(AccessControlSections.Access));
		List<CommonAce> list = new List<CommonAce>();
		List<CommonAce> list2 = new List<CommonAce>();
		List<CommonAce> list3 = new List<CommonAce>();
		List<CommonAce> list4 = new List<CommonAce>();
		List<CommonAce> list5 = new List<CommonAce>();
		AceEnumerator enumerator = rawSecurityDescriptor.DiscretionaryAcl.GetEnumerator();
		while (enumerator.MoveNext())
		{
			CommonAce commonAce = (CommonAce)enumerator.Current;
			if ((commonAce.AceFlags & AceFlags.Inherited) == AceFlags.Inherited)
			{
				list3.Add(commonAce);
				continue;
			}
			switch (commonAce.AceType)
			{
			case AceType.AccessAllowed:
				list4.Add(commonAce);
				break;
			case AceType.AccessDenied:
				list.Add(commonAce);
				break;
			case AceType.AccessAllowedObject:
				list5.Add(commonAce);
				break;
			case AceType.AccessDeniedObject:
				list2.Add(commonAce);
				break;
			}
		}
		int aceIndex = 0;
		RawAcl newDacl = new RawAcl(rawSecurityDescriptor.DiscretionaryAcl.Revision, rawSecurityDescriptor.DiscretionaryAcl.Count);
		list.ForEach(delegate(CommonAce x)
		{
			newDacl.InsertAce(aceIndex++, x);
		});
		list2.ForEach(delegate(CommonAce x)
		{
			newDacl.InsertAce(aceIndex++, x);
		});
		list4.ForEach(delegate(CommonAce x)
		{
			newDacl.InsertAce(aceIndex++, x);
		});
		list5.ForEach(delegate(CommonAce x)
		{
			newDacl.InsertAce(aceIndex++, x);
		});
		list3.ForEach(delegate(CommonAce x)
		{
			newDacl.InsertAce(aceIndex++, x);
		});
		if (aceIndex == rawSecurityDescriptor.DiscretionaryAcl.Count)
		{
			rawSecurityDescriptor.DiscretionaryAcl = newDacl;
			objectSecurity.SetSecurityDescriptorSddlForm(rawSecurityDescriptor.GetSddlForm(AccessControlSections.Access));
		}
	}
}

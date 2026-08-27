using System;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.Core.Cab;

[StructLayout(LayoutKind.Sequential)]
public class FDINotification
{
	private int cb;

	private string psz1;

	private string psz2;

	private string psz3;

	private IntPtr userData;

	private IntPtr hf;

	private short date;

	private short time;

	private short attribs;

	private short setID;

	private short iCabinet;

	private short iFolder;

	private FDIError fdie;

	public int Size => cb;

	public int FilePosition
	{
		get
		{
			return cb;
		}
		set
		{
			cb = value;
		}
	}

	public string str1 => psz1;

	public string str2 => psz2;

	public string str3 => psz3;

	public IntPtr FileHandle => hf;

	public short FileDate => date;

	public short FileTime => time;

	public short FileAttributes => attribs;

	public FDIError ErrorCode => fdie;
}

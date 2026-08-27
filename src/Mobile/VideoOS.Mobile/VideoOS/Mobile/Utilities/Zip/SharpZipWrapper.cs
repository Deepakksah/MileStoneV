using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace VideoOS.Mobile.Utilities.Zip;

public class SharpZipWrapper
{
	public static bool AddFilesToZipPackage(string zipName, IEnumerable<SharpZipFiles> files, string password)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		ZipEntryRenamer zipEntryRenamer = new ZipEntryRenamer();
		ZipOutputStream val = new ZipOutputStream((Stream)File.Create(zipName));
		try
		{
			if (!string.IsNullOrEmpty(password))
			{
				val.Password = password;
			}
			val.UseZip64 = (UseZip64)1;
			foreach (SharpZipFiles file in files)
			{
				ZipEntry val2 = new ZipEntry(Path.Combine(file.FilePathInZip, zipEntryRenamer.CorrectFileName(file, files)))
				{
					DateTime = File.GetLastWriteTime(file.FilePathName),
					Size = new FileInfo(file.FilePathName).Length,
					CompressionMethod = file.CompressionMethod
				};
				if (!string.IsNullOrEmpty(val.Password))
				{
					val2.AESKeySize = 256;
				}
				val.PutNextEntry(val2);
				using (FileStream fileStream = File.OpenRead(file.FilePathName))
				{
					fileStream.CopyTo((Stream)(object)val);
				}
				val.CloseEntry();
			}
			((DeflaterOutputStream)val).IsStreamOwner = true;
			((DeflaterOutputStream)val).Finish();
			((Stream)(object)val).Close();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return true;
	}
}

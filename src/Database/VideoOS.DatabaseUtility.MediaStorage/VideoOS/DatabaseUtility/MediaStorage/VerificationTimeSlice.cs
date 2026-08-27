using System;

namespace VideoOS.DatabaseUtility.MediaStorage;

public struct VerificationTimeSlice(string tablePath, VerificationStatus status, string certificate, DatabaseTimeSlice timeSlice, DateTime lastVerifiedTimeStamp)
{
	public string TablePath = tablePath;

	public VerificationStatus Status = status;

	public string Certificate = certificate;

	public DatabaseTimeSlice TimeSlice = timeSlice;

	public DateTime LastVerifiedTimeStamp = lastVerifiedTimeStamp;
}

using System;

namespace VideoOS.Common.SharedTimeProfile;

public interface IAppointmentDay
{
	TimeProfileAppointmentDay GetAppointments(DateTime day);
}

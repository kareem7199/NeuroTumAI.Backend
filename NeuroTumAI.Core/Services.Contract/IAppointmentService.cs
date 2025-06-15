using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Appointments;
using NeuroTumAI.Core.Entities.Appointment;
using NeuroTumAI.Core.Specifications.AppointmentSpecs;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IAppointmentService
	{
		Task<Appointment> BookAppointmentAsync(BookAppointmentDto model, string userId);
		Task<Appointment> CancelAppointmentAsync(string userId, int appointmentId);
		Task<IReadOnlyList<Appointment>> GetDoctorAppointmentsAsync(string userId, int clinicId, DateOnly date);
		Task<IReadOnlyList<Appointment>> GetPatientAppointmentsAsync(string userId, AppointmentSpecParams specParams);
		Task<int> GetPatientAppointmentsCountAsync(string userId, AppointmentSpecParams specParams);

	}
}

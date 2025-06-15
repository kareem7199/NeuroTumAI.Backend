using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.Dashboard;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.Appointment;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.AppointmentSpecs;
using NeuroTumAI.Core.Specifications.ClinicSpecs;
using NeuroTumAI.Core.Specifications.DoctorSpecs;

namespace NeuroTumAI.Service.Services.DashboardService
{
	public class DashboardService : IDashboardService
	{
		private readonly IUnitOfWork _unitOfWork;

		public DashboardService(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}
        public async Task<DashboardStatsDto> GetStatisticsAsync()
		{
			var clinicRepo = _unitOfWork.Repository<Clinic>();
			var doctorRepo = _unitOfWork.Repository<Doctor>();
			var appointmentRepo = _unitOfWork.Repository<Appointment>();

			return new DashboardStatsDto
			{
				Clinics = await clinicRepo.GetCountAsync(new ClinicSpecifications()),
				PendingClinics = await clinicRepo.GetCountAsync(new PendingClinicCountSpecifications(new PendingClinicSpecParams())),
				Doctors = await doctorRepo.GetCountAsync(new DoctorSpecifications()),
				PendingDoctors = await doctorRepo.GetCountAsync(new PendingDoctorCountSpecifications(new PendingDoctorSpecParams())),
				Appointments = await appointmentRepo.GetCountAsync(new AppointmentSpecifications())
			};
		}
	}
}

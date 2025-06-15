using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.ContactUs;
using NeuroTumAI.Core.Dtos.Review;
using NeuroTumAI.Core.Entities.Appointment;
using NeuroTumAI.Core.Entities.Contact_Us;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Resources.Responses;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.AppointmentSpecs;
using NeuroTumAI.Core.Specifications.ContactUsMessageSpecs;
using NeuroTumAI.Core.Specifications.PatientSpecs;
using NeuroTumAI.Core.Specifications.ReviewSpecs;
using NeuroTumAI.Service.Services.DoctorService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Service.Services.ContactUsService
{
	public class ContactUsService : IContactUsService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILocalizationService _localizationService;
		private readonly IEmailService _emailService;

		public ContactUsService(IUnitOfWork unitOfWork, ILocalizationService localizationService, IEmailService emailService)
		{
			_unitOfWork = unitOfWork;
			_localizationService = localizationService;
			_emailService = emailService;
		}

		public async Task<ContactUsMessage> GetMessageAsync(int messageId)
		{
			var ContactUsRepo = _unitOfWork.Repository<ContactUsMessage>();
			var contactUsSpecs = new ContactUsMessageSpecifications(messageId);

			var message = await ContactUsRepo.GetWithSpecAsync(contactUsSpecs);
			if (message is null)
				throw new NotFoundException("Message not found");

			return message;
		}

		public async Task<IReadOnlyList<ContactUsMessage>> GetPendingMessagesAsync(PendingMessagesSpecParams specParams)
		{
			var ContactUsRepo = _unitOfWork.Repository<ContactUsMessage>();
			var contactUsSpecs = new ContactUsMessageSpecifications(specParams);

			return await ContactUsRepo.GetAllWithSpecAsync(contactUsSpecs);
		}

		public async Task<int> GetPendingMessagesCountAsync(PendingMessagesSpecParams specParams)
		{

			var ContactUsRepo = _unitOfWork.Repository<ContactUsMessage>();
			var contactUsSpecs = new ContactUsMessageCountSpecifications(specParams);

			return await ContactUsRepo.GetCountAsync(contactUsSpecs);
		}

		public async Task ReplyAsync(int messageId, string message)
		{

			var ContactUsRepo = _unitOfWork.Repository<ContactUsMessage>();
			var contactUsSpecs = new ContactUsMessageSpecifications(messageId);
			var contactUsMessage = await ContactUsRepo.GetWithSpecAsync(contactUsSpecs);

			if (contactUsMessage is null)
				throw new NotFoundException("Message not found");

			await _emailService.SendAsync(
				contactUsMessage.Patient.ApplicationUser.Email,
				$"Response to Your Support Ticket #{messageId}",
				message
			);

			contactUsMessage.Status = MessageStatus.Closed;

			ContactUsRepo.Update(contactUsMessage);

			await _unitOfWork.CompleteAsync();
		}

		public async Task<ContactUsMessage> SendMessageAsync(ContactUsDto model, string userId)
		{
			var patientRepo = _unitOfWork.Repository<Patient>();
			var patientSpec = new PatientSpecifications(userId);
			var patient = await patientRepo.GetWithSpecAsync(patientSpec);


			var ContactUsRepo = _unitOfWork.Repository<ContactUsMessage>();

			var contactMessage = new ContactUsMessage
			{
				PatientId = patient.Id,
				Patient = patient,
				Message = model.Message,
			};

			ContactUsRepo.Add(contactMessage);
			await _unitOfWork.CompleteAsync();

			return contactMessage;
		}
	}
}

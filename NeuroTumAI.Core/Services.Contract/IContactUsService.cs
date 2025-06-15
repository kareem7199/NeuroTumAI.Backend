using NeuroTumAI.Core.Dtos.Appointments;
using NeuroTumAI.Core.Dtos.ContactUs;
using NeuroTumAI.Core.Entities.Appointment;
using NeuroTumAI.Core.Entities.Contact_Us;
using NeuroTumAI.Core.Specifications.ContactUsMessageSpecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IContactUsService
	{
		Task<ContactUsMessage> SendMessageAsync(ContactUsDto model, string userId);
		Task<ContactUsMessage> GetMessageAsync(int messageId);
		Task<IReadOnlyList<ContactUsMessage>> GetPendingMessagesAsync(PendingMessagesSpecParams specParams);
		Task<int> GetPendingMessagesCountAsync(PendingMessagesSpecParams specParams);
		Task ReplyAsync(int messageId, string message);

	}
}

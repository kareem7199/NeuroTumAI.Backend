using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.ContactUs;
using NeuroTumAI.Core.Entities.Contact_Us;
using NeuroTumAI.Core.Dtos.Review;
using NeuroTumAI.Core.Services.Contract;
using System.Security.Claims;

namespace NeuroTumAI.APIs.Controllers.ContactUs
{
    public class ContactUsController : BaseApiController
    {
        private readonly IContactUsService _ContactUsService;
		private readonly IMapper _mapper;

		public ContactUsController(IContactUsService ContactUsService, IMapper mapper)
        {
            _ContactUsService = ContactUsService;
			_mapper = mapper;
		}

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<ContactUsMessageToReturnDto>> SendMessage(ContactUsDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var message = await _ContactUsService.SendMessageAsync(model, userId);
            
            return Ok(_mapper.Map<ContactUsMessageToReturnDto>(message));
        }
    }
}

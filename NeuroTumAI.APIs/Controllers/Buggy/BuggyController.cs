using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Resources.Shared;

namespace NeuroTumAI.APIs.Buggy
{
    public class BuggyController : BaseApiController
    {
        private readonly ILocalizationService _localizationService;

        public BuggyController(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        [HttpGet("notfound")] // GET: api/buggy/notfound
        public IActionResult GetNotFoundRequest()
        {
            throw new NotFoundException(_localizationService.GetMessage<SharedResources>("NotFound"));
        }

        [HttpGet("servererror")] // GET: api/buggy/servererror
        public IActionResult GetServerError()
        {
            throw new Exception(_localizationService.GetMessage<SharedResources>("InternalServerError")); // 500
        }

        [HttpGet("badrequest")] // GET: api/buggy/badrequest
        public IActionResult GetBadRequest()
        {
            throw new BadRequestException(_localizationService.GetMessage<SharedResources>("BadRequest"));
        }

        [HttpGet("validationerror")]
        public IActionResult PostValidationError()
        {
            throw new ValidationException(_localizationService.GetMessage<SharedResources>("ValidationError"))
            {
                Errors = [
                    _localizationService.GetMessage<SharedResources>("RequiredField"),
                    _localizationService.GetMessage<SharedResources>("MinLength", 1)
                ]
            };
        }
    }
}

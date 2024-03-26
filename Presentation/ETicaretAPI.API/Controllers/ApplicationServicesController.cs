using ETicaretAPI.Application.Abstractions.Services.Configurations;
using ETicaretAPI.Application.Consts;
using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class ApplicationServicesController : ControllerBase
    {
        readonly IApplicationService _applicationService;

        public ApplicationServicesController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        [AuthorizeDefination(ActionType = ActionType.Reading, Defination = "Get Authorize Defination Endpoints", Menu = AuthorizeDefinationConstants.ApplicationServices)]
        public IActionResult GetAuthorizeDefinationEndpoints()
        {
            var datas = _applicationService.GetAuthorizeDefinationEndpoints(typeof(Program));
            return Ok(datas);
        }
    }
}
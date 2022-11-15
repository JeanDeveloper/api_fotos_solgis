using Microsoft.AspNetCore.Mvc;
using SolgisFotos.Domain.Request;
using SolgisFotos.Domain.Services;
using SolgisFotos.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolgisFotos.Controllers
{

    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]

    public class PersonalController : ControllerBase
    {
        private readonly IPhotoPersonalService _apiController;


        public PersonalController (IPhotoPersonalService apiController)
        {
            _apiController = apiController;
        }

        [HttpPost("upload-photo-personal")]
        public async Task<IActionResult> UploadPhotoPersonal([FromForm] PhotoPersonalRequest request)
        {
            try
            {
                var result = await _apiController.Upload(request);
                return Ok(result);
            }
            catch (Exception e)
            {
                LogUtil.LogErrorOnDb(e);
                throw;
            }
        }


    }
}

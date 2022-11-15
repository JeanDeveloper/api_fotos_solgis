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

    public class PhotoController : ControllerBase
    {

        private readonly IPhotoService _apiController;

        public PhotoController(IPhotoService apiService)
        {
            _apiController = apiService;
        }


        [HttpPost("upload-photo")]
        public async Task<IActionResult> UploadPhoto([FromForm] PhotoMovimientoRequest request)
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

using SolgisFotos.Domain.Request;
using SolgisFotos.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolgisFotos.Domain.Services
{
    public interface IPhotoService
    {
        Task<PhotoResponse> Upload(PhotoMovimientoRequest request);


    }
}

using SolgisFotos.Domain.Repository;
using SolgisFotos.Domain.Request;
using SolgisFotos.Domain.Response;
using SolgisFotos.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolgisFotos.Services
{
    public class ApiService : IPhotoService
    {

        Task<PhotoResponse> IPhotoService.Upload(PhotoMovimientoRequest request)
        {
            return PhotoRepository.Insert(request);

        }

    }

    public class ApiPersonalService : IPhotoPersonalService
    {

        Task<PhotoResponse> IPhotoPersonalService.Upload(PhotoPersonalRequest request)
        {
            return PhotoPersonalRepository.Insert(request);

        }

    }

}

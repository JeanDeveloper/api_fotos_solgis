using Microsoft.AspNetCore.Http;


namespace SolgisFotos.Domain.Request
{
    public class PhotoMovimientoRequest
    {
        public const int GuiaType = 1;
        public const int MaterialType = 2;

        public IFormFile file { get; set; }
        public string creado_por { get; set; }
        public int datoAcceso { get; set; }
        public int cod_movimiento { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Dapper;
using SolgisFotos.Domain.Response;
using SolgisFotos.Domain.Request;
using SolgisFotos.Domain.Entities;
using SolgisFotos.Utilities;

namespace SolgisFotos.Domain.Repository
{
    public static class PhotoRepository
    {

        public static async Task<PhotoResponse> Insert(PhotoMovimientoRequest request)
        {
            try
            {
                var fileRoot = Path.Combine(Startup.root, "Photos");
                var folder = CreateFolderTypeMovimiento(Convert.ToInt32(request.datoAcceso));
                var folderPath = Path.Combine(fileRoot, folder);
                var filePath = Path.Combine(folderPath, request.file.FileName);
                Photo photo = new Photo();

                using (var fileStream = File.Create(filePath))
                {
                    await request.file.CopyToAsync(fileStream);
                    fileStream.Flush();
                }

                var url = $"/Photos/{folder}/{request.file.FileName}";

                double tamanio = request.file.Length;
                tamanio = tamanio / 1000000;
                tamanio = Math.Round(tamanio, 2);

                photo.foto_mov_id = request.cod_movimiento;
                photo.extension = Path.GetExtension(request.file.FileName).Substring(1);
                photo.nombre = Path.GetFileNameWithoutExtension(request.file.FileName);
                photo.tamanio = tamanio;
                photo.ubicacion = filePath;


                return new PhotoResponse
                {
                    message = "Guardado",
                    url = url,
                    status = InsertToDb(photo, request.cod_movimiento, request.datoAcceso, request.creado_por)
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                LogUtil.LogErrorOnDb(e);
                throw;
            }
        }



        public static int InsertToDb(Photo photo, int cod_movimiento, int datoAcceso, String creado_por )
        {

            try
            {

                var parameters = new
                {
                    cod_movimiento = cod_movimiento,
                    nombre = photo.nombre,
                    extension = photo.extension,
                    datoAcceso = datoAcceso,
                    tamanio = photo.tamanio,
                    ubicacion = photo.ubicacion,
                    creado_por = creado_por
                };


                const string query =
                    "EXECUTE CONTROLCLIENTES2018.dbo.AppSolgis_Insertar_foto @cod_movimiento, @nombre, @extension, @datoAcceso, @tamanio, @ubicacion, @creado_por;";

                using (var connection = BDConnection.GetConnection())
                {
                    connection.Query(query, parameters);

                }
                return PhotoResponse.Save;


            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                LogUtil.LogErrorOnDb(e);
                return PhotoResponse.Error;

            }

            
        }





        public static string CreateFolderTypeMovimiento(int type)
        {
            var folder = "Otros";
            switch (type)
            {
                case PhotoMovimientoRequest.GuiaType:
                    folder = "Guia";
                    break;
                case PhotoMovimientoRequest.MaterialType:
                    folder = "Material";
                    break;

            }

            CreateFolder(folder);
            return folder;
        }


        private static void CreateFolder(string folderName)
        {
            var rootPath = Path.Combine(Startup.root, "Photos");
            if (!Directory.Exists(Path.Combine(rootPath, folderName)))
                Directory.CreateDirectory(Path.Combine(rootPath, folderName));
        }






    }
}

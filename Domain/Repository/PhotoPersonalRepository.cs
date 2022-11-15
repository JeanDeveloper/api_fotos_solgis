using SolgisFotos.Domain.Entities;
using SolgisFotos.Domain.Request;
using SolgisFotos.Domain.Response;
using SolgisFotos.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace SolgisFotos.Domain.Repository
{
    public static class PhotoPersonalRepository
    {

        public static async Task<PhotoResponse> Insert(PhotoPersonalRequest request)
        {
            try
            {
                var fileRoot = Path.Combine(Startup.root, "Photos");
                var folder = "Personal";
                var folderPath = Path.Combine(fileRoot, folder);
                var filePath = Path.Combine(folderPath, request.nombre);
                PhotoPersonal photo = new PhotoPersonal();

                using (var fileStream = File.Create(filePath))
                {
                    await request.file.CopyToAsync(fileStream);
                    fileStream.Flush();
                }

                var url = $"/Photos/{folder}/{request.nombre}";

                double tamanio = request.file.Length;
                tamanio = tamanio / 1000000;
                tamanio = Math.Round(tamanio, 2);

                photo.nombre = Path.GetFileNameWithoutExtension(request.file.FileName);
                photo.tamanio = tamanio;
                photo.ubicacion = filePath;

                return new PhotoResponse
                {
                    message = "Guardado",
                    url = url,
                    status = InsertToDb(photo, request.nombre)
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                LogUtil.LogErrorOnDb(e);
                throw;
            }

        }



        public static int InsertToDb(PhotoPersonal photo, String nombre)
        {

            try
            {

                var parameters = new
                {
                    nombre = nombre,
                    tamanio = photo.tamanio,
                    codigo_personal = nombre.Substring(0, nombre.IndexOf('.')),
                    ubicacion = photo.ubicacion,
                    
                };


                const string query =
                    "EXECUTE CONTROLCLIENTES2018.dbo.AppSolgis_Insertar_Foto_Personal @nombre, @tamanio, @codigo_personal, @ubicacion";

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


    }





}

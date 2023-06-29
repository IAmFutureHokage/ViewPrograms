using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Project.Core.Entities;
using Project.Core.Enum;
using Project.Core.Exceptions;
using Project.Core.Models.Response;
using Project.Core.OperationInterfaces;
using Project.Core.RepositoryInterfaces;
using Project.DAL.Repositories;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project.BLL.Services
{
    public class FileService : IFileService
    {
        private readonly string _fileFolder;

            public FileService(IConfiguration configuration)
            {
                _fileFolder = configuration["FileFolder:FileFolder"];
             
        }

            public async Task<string> SaveFile(IFormFile file)
            {
                // Проверка размера файла (не более 2 МБ)
                if (file.Length > 2 * 1024 * 1024)
                {
                    throw new Exception("File is too large");
                }

                // Проверка типа файла (должен быть изображением)
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                if (!allowedTypes.Contains(file.ContentType))
                {
                    throw new Exception("Invalid file type");
                }

                // Создание уникального имени файла
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                // Загрузка файла на сервер
                var filePath = Path.Combine(_fileFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Возвращение пути к файлу
                return "/uploads/" + uniqueFileName;
            }
        }
    }

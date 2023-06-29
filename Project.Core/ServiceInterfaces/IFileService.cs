using Microsoft.AspNetCore.Http;
using Project.Core.Entities;
using Project.Core.Models.Response;
using System.Threading.Tasks;

namespace Project.Core.OperationInterfaces
{
    public interface IFileService
    {
        Task<string> SaveFile(IFormFile file);
    }
}

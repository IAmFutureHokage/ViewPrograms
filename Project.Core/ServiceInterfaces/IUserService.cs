using Microsoft.AspNetCore.Http;
using Project.Core.Entities;
using Project.Core.Models.Response;
using System.Threading.Tasks;

namespace Project.Core.OperationInterfaces
{
    public interface IUserService : IServiceBase
    {
        User Add(User user, string password);
        User GetByUserNameAuth(string userLogin);
        User GetByUserName(string userLogin);
        Task<User> GetByUserNameAsync(string userLogin);
        void UpdateUser(User user);
        void UpdateUser(User user, string password);
        void Delete(User entity);
        


    }
}

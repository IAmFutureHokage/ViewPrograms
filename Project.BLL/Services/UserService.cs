using Project.Core.Entities;
using Project.Core.Enum;
using Project.Core.Exceptions;
using Project.Core.OperationInterfaces;
using Project.Core.RepositoryInterfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Project.BLL.Services
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;


        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User Add(User user, string password)
        {
            var existingUser = GetByUserName(user.Login);
            if (existingUser != null)
                throw new LogicException(ExceptionMessage.USER_ALREADY_EXISTS);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            return Add(user);
        }
        public User GetByUserNameAuth(string userLogin)
        {
            var user = _unitOfWork.Users.Find(x => x.Login == userLogin).FirstOrDefault();
            return user;
        }

        public User GetByUserName(string userLogin)
        {
            var user = _unitOfWork.Users.Find(x => x.Login == userLogin).FirstOrDefault();
            return user;
        }



        public async Task<User> GetByUserNameAsync(string userLogin)
        {
            var user = (await _unitOfWork.Users.FindAsync(x => x.Login.ToLower() == userLogin.ToLower())).FirstOrDefault();
            return user;
        }

        public void UpdateUser(User user)
        {
            _unitOfWork.Users.Update(user);
            _unitOfWork.Complete();

        }
        public void UpdateUser(User user, string password)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            _unitOfWork.Users.Update(user);
            _unitOfWork.Complete();

        }

        public void Delete(User entity)
        {
            if (entity == null) return;
            entity.Deleted = true;
            _unitOfWork.Users.Update(entity);
            _unitOfWork.Complete();
        }

    }
}

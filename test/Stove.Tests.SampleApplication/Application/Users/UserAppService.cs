using JetBrains.Annotations;

using Stove.Application.Services;
using Stove.Domain.Repositories;
using Stove.Tests.SampleApplication.Domain.Entities;

namespace Stove.Tests.SampleApplication.Application
{
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly IRepository<User> _userRepository;

        public UserAppService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUserByName(string name)
        {
            return _userRepository.FirstOrDefault(x => x.Name == name);
        }
    }

    public interface IUserAppService
    {
        [CanBeNull]
        User GetUserByName([NotNull] string name);
    }
}

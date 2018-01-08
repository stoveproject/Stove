using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

using Stove.Domain.Repositories;
using Stove.Domain.Services;
using Stove.Tests.SampleApplication.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain
{
    public class SomeDomainService : DomainService
    {
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<User> _repository;

        public SomeDomainService(IRepository<User> repository, IRepository<Message> messageRepository)
        {
            _repository = repository;
            _messageRepository = messageRepository;
        }

        public User GetUserByName(string name)
        {
            User user = null;
            UseUow(() =>
            {
                user = _repository.FirstOrDefault(x => x.Name == name);
            });

            return user;
        }

        public async Task<User> GetUserByName_async(string name)
        {
            User user = null;
            await UseUow(async () =>
            {
                user = await _repository.FirstOrDefaultAsync(x => x.Name == name);
            }, CancellationToken.None);

            return user;
        }

        public async Task<User> GetUserByName_async_With_IsolationLevel(string name)
        {
            User user = null;

            await UseUow(async () =>
            {
                user = await _repository.FirstOrDefaultAsync(x => x.Name == name);
            }, IsolationLevel.ReadCommitted);

            return user;
        }

        public User GetUserByName_with_isolationlevel(string name)
        {
            User user = null;

            UseUow(() =>
            {
                user = _repository.FirstOrDefault(x => x.Name == name);
            }, IsolationLevel.Chaos);

            return user;
        }

        public User GetUserByName_isTransactional(string name)
        {
            User user = null;

            UseUow(() =>
            {
                user = _repository.FirstOrDefault(x => x.Name == name);
            }, true);

            return user;
        }

        public async Task<User> GetUserByName_async_isTransactional(string name)
        {
            User user = null;

            await UseUow(async () =>
            {
                user = await _repository.FirstOrDefaultAsync(x => x.Name == name);
            }, true);

            return user;
        }

        public async Task<Message> CreateMessageAndGet(string message)
        {
            Message msg = null;
            await UseUowIfNot(async () =>
            {
                msg = await _messageRepository.InsertAsync(new Message(message));
            });

            return msg;
        }
    }
}

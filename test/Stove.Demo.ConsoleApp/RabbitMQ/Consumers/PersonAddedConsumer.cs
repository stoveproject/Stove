using System.Threading.Tasks;

using MassTransit;

using Stove.Demo.ConsoleApp.Entities;
using Stove.Demo.ConsoleApp.RabbitMQ.Messages;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.MQ;

namespace Stove.Demo.ConsoleApp.RabbitMQ.Consumers
{
    public class PersonAddedConsumer : ConsumerBase, IConsumer<IPersonAddedMessage>
    {
        private readonly IRepository<Person> _personRepository;

        public PersonAddedConsumer(IRepository<Person> personRepository)
        {
            _personRepository = personRepository;
        }

        public Task Consume(ConsumeContext<IPersonAddedMessage> context)
        {
            string name = context.Message.Name;

            using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin())
            {
                Person foundedPerson = _personRepository.FirstOrDefault(x => x.Name == name);
            }

            return Task.FromResult(0);
        }
    }
}

using System.Threading.Tasks;

using MassTransit;

using Stove.Demo.WebApi.Core.Application.RabbitMQ.Messages;
using Stove.Demo.WebApi.Core.Domain.Entities;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.MQ;

namespace Stove.Demo.WebApi.Core.Application.RabbitMQ.Consumers
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
				try
				{
					Person foundedPerson = _personRepository.FirstOrDefault(x => x.Name == name);
				}
				catch (System.Exception ex)
				{

					throw ex;
				}
              
            }

            return Task.FromResult(0);
        }
    }
}

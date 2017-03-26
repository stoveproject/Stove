using System;

using Shouldly;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Tests.SampleApplication.Domain.Entities;
using Stove.Timing;

using Xunit;

namespace Stove.Tests.SampleApplication.Auditing
{
    public class AuditedEntity_Tests : SampleApplicationTestBase
    {
        private readonly IRepository<Message> _messageRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public AuditedEntity_Tests()
        {
            Building(builder => { }).Ok();

            _unitOfWorkManager = The<IUnitOfWorkManager>();
            _messageRepository = The<IRepository<Message>>();
        }

        [Fact]
        public void Should_Write_Audit_Properties()
        {
            StoveSession.UserId = 2;
            Message createdMessage;
            using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin())
            {
                //Act: Create a new entity
                createdMessage = _messageRepository.Insert(new Message("Hello World!"));

                createdMessage.IsTransient().ShouldBe(true);

                //Assert: Check creation properties
                createdMessage.CreatorUserId.ShouldBe(StoveSession.UserId);
                createdMessage.CreationTime.ShouldBeGreaterThan(Clock.Now.Subtract(TimeSpan.FromSeconds(10)));
                createdMessage.CreationTime.ShouldBeLessThan(Clock.Now.Add(TimeSpan.FromSeconds(10)));

                _unitOfWorkManager.Current.SaveChanges();

                createdMessage.IsTransient().ShouldBe(false);

                uow.Complete();
            }

            Message selectedMessage;
            using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin())
            {
                //Act: Select the same entity
                selectedMessage = _messageRepository.Get(createdMessage.Id);

                //Assert: Select should not change audit properties
                selectedMessage.ShouldBe(createdMessage); //They should be same since Entity class overrides == operator.

                selectedMessage.CreationTime.ShouldBe(createdMessage.CreationTime);
                selectedMessage.CreatorUserId.ShouldBe(createdMessage.CreatorUserId);

                selectedMessage.LastModifierUserId.ShouldBeNull();
                selectedMessage.LastModificationTime.ShouldBeNull();

                selectedMessage.IsDeleted.ShouldBeFalse();
                selectedMessage.DeleterUserId.ShouldBeNull();
                selectedMessage.DeletionTime.ShouldBeNull();

                //Act: Update the entity
                selectedMessage.Subject = "test message 1 - updated";
                _messageRepository.Update(selectedMessage);

                _unitOfWorkManager.Current.SaveChanges();

                //Assert: Modification properties should be changed
                selectedMessage.LastModifierUserId.ShouldBe(StoveSession.UserId);
                selectedMessage.LastModificationTime.ShouldNotBeNull();
                selectedMessage.LastModificationTime.Value.ShouldBeGreaterThan(Clock.Now.Subtract(TimeSpan.FromSeconds(10)));
                selectedMessage.LastModificationTime.Value.ShouldBeLessThan(Clock.Now.Add(TimeSpan.FromSeconds(10)));

                uow.Complete();
            }

            using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin())
            {
                //Act: Delete the entity
                _messageRepository.Delete(selectedMessage);

                _unitOfWorkManager.Current.SaveChanges();

                //Assert: Deletion audit properties should be set
                selectedMessage.IsDeleted.ShouldBe(true);
                selectedMessage.DeleterUserId.ShouldBe(StoveSession.UserId);
                selectedMessage.DeletionTime.ShouldNotBeNull();
                selectedMessage.DeletionTime.Value.ShouldBeGreaterThan(Clock.Now.Subtract(TimeSpan.FromSeconds(10)));
                selectedMessage.DeletionTime.Value.ShouldBeLessThan(Clock.Now.Add(TimeSpan.FromSeconds(10)));

                uow.Complete();
            }
        }
    }
}

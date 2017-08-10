using System;
using System.Reflection;

using Shouldly;

using Stove.BackgroundJobs;
using Stove.Reflection.Extensions;

using Xunit;

namespace Stove.Hangfire.Tests
{
    public class HangfireBackgroundJobs_Tests : HangfireApplicationTestBase
    {
        public HangfireBackgroundJobs_Tests()
        {
            Building(builder => { builder.RegisterServices(r => r.RegisterAssemblyByConvention(typeof(HangfireBackgroundJobs_Tests).GetAssembly())); }).Ok();
        }

        [Fact]
        public void Hangfire_should_work_and_BackgroundJobManager_should_enqueue_any_backgroundjob()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var backgroundJobManager = The<IBackgroundJobManager>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action enqueueAction = () =>
            {
                backgroundJobManager.EnqueueAsync<SimpleJob, SimpleJobArgs>(new SimpleJobArgs
                {
                    Name = "Oguzhan"
                });
            };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            enqueueAction.ShouldNotThrow();
        }

        [Fact]
        public void BackgroundJobManager_should_equeue_any_backgroundjob_with_a_delay()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var backgroundJobManager = The<IBackgroundJobManager>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action enqueueAction = () =>
            {
                backgroundJobManager.EnqueueAsync<SimpleJob, SimpleJobArgs>(new SimpleJobArgs
                {
                    Name = "Oguzhan"
                }, BackgroundJobPriority.Normal, TimeSpan.FromSeconds(10));
            };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            enqueueAction.ShouldNotThrow();
        }
    }
}

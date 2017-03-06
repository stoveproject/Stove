using System;
using System.Reflection;

using Hangfire;

using Shouldly;

using Stove.BackgroundJobs;

using Xunit;

namespace Stove.Hangfire.Tests
{
    public class HangfireScheduleJobs_Tests : HangfireApplicationTestBase
    {
        public HangfireScheduleJobs_Tests()
        {
            Building(builder => { builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly())); }).Ok();
        }

        [Fact]
        public void Hangfire_should_work_and_ScheduleJobManager_should_schedule_any_backgroundjob()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var scheduleJobManager = The<IScheduleJobManager>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action scheduleAction = () =>
            {
                scheduleJobManager.ScheduleAsync<SimpleJob, SimpleJobArgs>(new SimpleJobArgs
                {
                    Name = "Oguzhan"
                }, Cron.Minutely());
            };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            scheduleAction.ShouldNotThrow();
        }
    }
}

using System;

namespace Stove.Demo.ConsoleApp.BackgroundJobs
{
    [Serializable]
    public class SimpleBackgroundJobArgs
    {
        public string Message { get; set; }
    }
}

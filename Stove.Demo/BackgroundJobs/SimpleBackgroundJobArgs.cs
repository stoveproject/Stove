using System;

namespace Stove.Demo.BackgroundJobs
{
    [Serializable]
    public class SimpleBackgroundJobArgs
    {
        public string Message { get; set; }
    }
}

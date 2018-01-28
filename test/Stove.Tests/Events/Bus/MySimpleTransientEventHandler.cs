using System;
using System.Collections.Generic;

using Stove.Events;
using Stove.Events.Bus.Handlers;

namespace Stove.Tests.Events.Bus
{
    public class MySimpleTransientEventHandler : IEventHandler<MySimpleEvent>, IDisposable
    {
        public static int HandleCount { get; set; }

        public static int DisposeCount { get; set; }

        public void Dispose()
        {
            ++DisposeCount;
        }

        public void Handle(MySimpleEvent @event, EventHeaders headers)
        {
            ++HandleCount;
        }
    }
}

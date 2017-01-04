using System;

using Shouldly;

using Stove.Json;
using Stove.Timing;

using Xunit;

namespace Stove.Tests.Json
{
    public class JsonSerializationHelper_Tests
    {
        [Fact]
        public void Test_1()
        {
            string str = JsonSerializationHelper.SerializeWithType(new MyClass1("John"));
            var result = (MyClass1)JsonSerializationHelper.DeserializeWithType(str);
            result.ShouldNotBeNull();
            result.Name.ShouldBe("John");
        }

        [Fact]
        public void Test_2()
        {
            Clock.Provider = ClockProviders.Utc;

            var str = "Stove.Tests.Json.JsonSerializationHelper_Tests+MyClass2, Stove.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null|{\"Date\":\"2016-04-13T16:58:10.526+08:00\"}";
            var result = (MyClass2)JsonSerializationHelper.DeserializeWithType(str);
            result.ShouldNotBeNull();
            result.Date.ShouldBe(new DateTime(2016, 04, 13, 08, 58, 10, 526, Clock.Kind));
            result.Date.Kind.ShouldBe(Clock.Kind);
        }

        [Fact]
        public void Test_3()
        {
            Clock.Provider = ClockProviders.Local;

            var myClass = new MyClass2(new DateTime(2016, 04, 13, 08, 58, 10, 526, Clock.Kind));
            string str = JsonSerializationHelper.SerializeWithType(myClass);
            var result = (MyClass2)JsonSerializationHelper.DeserializeWithType(str);

            result.Date.ShouldBe(new DateTime(2016, 04, 13, 08, 58, 10, 526, Clock.Kind));
            result.Date.Kind.ShouldBe(Clock.Kind);
        }

        public class MyClass1
        {
            public MyClass1()
            {
            }

            public MyClass1(string name)
            {
                Name = name;
            }

            public string Name { get; set; }
        }

        public class MyClass2
        {
            public MyClass2(DateTime date)
            {
                Date = date;
            }

            public DateTime Date { get; set; }
        }
    }
}

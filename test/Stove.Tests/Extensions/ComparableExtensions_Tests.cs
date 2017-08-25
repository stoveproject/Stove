using System;

using Shouldly;

using Stove.Extensions;

using Xunit;

namespace Stove.Tests.Extensions
{
    public class ComparableExtensions_Tests
    {
        [Fact]
        public void IsBetween_Test()
        {
            //Number
            var number = 5;
            number.IsBetween(1, 10).ShouldBe(true);
            number.IsBetween(1, 5).ShouldBe(true);
            number.IsBetween(5, 10).ShouldBe(true);
            number.IsBetween(10, 20).ShouldBe(false);

            //DateTime
            var dateTimeValue = new DateTime(2014, 10, 4, 18, 20, 42, 0);
            dateTimeValue.IsBetween(new DateTime(2014, 1, 1), new DateTime(2015, 1, 1)).ShouldBe(true);
            dateTimeValue.IsBetween(new DateTime(2015, 1, 1), new DateTime(2016, 1, 1)).ShouldBe(false);
        }

        [Theory]
        [InlineData(5,1,5)]        
        [InlineData(5, 1, int.MaxValue)]
        [InlineData(5, 5, int.MaxValue)]
        [InlineData(5, 5, 5)]        
        [InlineData(5, int.MinValue, int.MaxValue)]
        public void Integer_given_positive_integers_within_range(int val,int min, int max)
        {
            val.IsBetween(min, max).ShouldBe(true);
        }

        [Theory]
        [InlineData(6, int.MinValue, 5)]
        [InlineData(6, 7, int.MaxValue)]
        [InlineData(6, int.MaxValue, int.MaxValue)]
        public void Integer_given_positive_integers_outof_range(int val, int min, int max)
        {
            val.IsBetween(min, max).ShouldBe(false);
        }


        [Theory]
        [InlineData(-5, -5, -5)]
        [InlineData(-5, int.MinValue, -5)]
        [InlineData(-5, int.MinValue, int.MaxValue)]

        public void Integer_given_negative_integers_within_range(int val, int min, int max)
        {
            val.IsBetween(min, max).ShouldBe(true);
        }

        [Theory]
        [InlineData(-10, int.MinValue, int.MinValue)]
        [InlineData(-10, int.MinValue, -11)]
        [InlineData(-10, -9, int.MaxValue)]
        public void Integer_given_negative_integers_outof_range(int val, int min, int max)
        {
            val.IsBetween(min, max).ShouldBe(false);
        }


        [Theory]
        [InlineData(5.0001, 1.0001, 5.0001)]
        [InlineData(5.0001, 1.0001, double.MaxValue)]
        [InlineData(5.0001, 5.0001, double.MaxValue)]
        [InlineData(5.0001, 5.0001, 5.0001)]
        [InlineData(5.0001, double.MinValue, double.MaxValue)]
        public void Integer_given_positive_doubles_within_range(double val, double min, double max)
        {
            val.IsBetween(min, max).ShouldBe(true);
        }



        [Theory]
        [InlineData(6.0001, double.MinValue, 6.0000)]
        [InlineData(6.0001, 7.00001, double.MaxValue)]
        [InlineData(6.0001, double.MaxValue, double.MaxValue)]
        public void Integer_given_positive_doubles_outof_range(double val, double min, double max)
        {
            val.IsBetween(min, max).ShouldBe(false);
        }



        [Theory]
        [InlineData(-5.0001, -5.0001, -5.0001)]
        [InlineData(-5.0001, double.MinValue, -5.0001)]
        [InlineData(-5.0001, double.MinValue, double.MaxValue)]

        public void Integer_given_negative_doubles_within_range(double val, double min, double max)
        {
            val.IsBetween(min, max).ShouldBe(true);
        }

        [Theory]
        [InlineData(-10.0001, double.MinValue, double.MinValue)]
        [InlineData(-10.0001, double.MinValue, -10.0002)]
        [InlineData(-10.0001, -9.99999, double.MaxValue)]
        public void Integer_given_negative_doubles_outof_range(double val, double min, double max)
        {
            val.IsBetween(min, max).ShouldBe(false);
        }
    }
}
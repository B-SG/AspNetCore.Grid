﻿using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class BooleanFilterTests
    {
        private BooleanFilter filter;
        private IQueryable<GridModel> items;
        private Expression<Func<GridModel, Boolean>> booleanExpression;
        private Expression<Func<GridModel, Boolean?>> nBooleanExpression;

        public BooleanFilterTests()
        {
            items = new[]
            {
                new GridModel(),
                new GridModel { IsChecked = true, NIsChecked = false },
                new GridModel { IsChecked = false, NIsChecked = true }
            }.AsQueryable();

            filter = new BooleanFilter();
            booleanExpression = (model) => model.IsChecked;
            nBooleanExpression = (model) => model.NIsChecked;
        }

        #region Apply(Expression expression)

        [Fact]
        public void Apply_BadValue_ReturnsNull()
        {
            filter.Method = "equals";
            filter.Values = new[] { "Test" };

            Assert.Null(filter.Apply(booleanExpression.Body));
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData("true", true)]
        [InlineData("TRUE", true)]
        [InlineData("false", false)]
        [InlineData("FALSE", false)]
        public void Apply_NullableEqualsFilter(String value, Boolean? isChecked)
        {
            filter.Method = "equals";
            filter.Values = new [] { value };

            IEnumerable actual = items.Where(nBooleanExpression, filter);
            IEnumerable expected = items.Where(model => model.NIsChecked == isChecked);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_MultipleNullableEqualsFilter()
        {
            filter.Method = "equals";
            filter.Values = new[] { "", "false" };

            IEnumerable actual = items.Where(nBooleanExpression, filter);
            IEnumerable expected = items.Where(model => model.NIsChecked != true);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData("true", true)]
        [InlineData("TRUE", true)]
        [InlineData("false", false)]
        [InlineData("FALSE", false)]
        public void Apply_EqualsFilter(String value, Boolean? isChecked)
        {
            filter.Method = "equals";
            filter.Values = new[] { value };

            IEnumerable actual = items.Where(booleanExpression, filter);
            IEnumerable expected = items.Where(model => model.IsChecked == isChecked);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_MultipleEqualsFilter()
        {
            filter.Method = "equals";
            filter.Values = new[] { "true", "false" };

            IEnumerable expected = items;
            IEnumerable actual = items.Where(booleanExpression, filter);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData("true", true)]
        [InlineData("TRUE", true)]
        [InlineData("false", false)]
        [InlineData("FALSE", false)]
        public void Apply_NullableNotEqualsFilter(String value, Boolean? isChecked)
        {
            filter.Method = "not-equals";
            filter.Values = new[] { value };

            IEnumerable actual = items.Where(nBooleanExpression, filter);
            IEnumerable expected = items.Where(model => model.NIsChecked != isChecked);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_MultipleNullableNotEqualsFilter()
        {
            filter.Method = "not-equals";
            filter.Values = new[] { "", "false" };

            IEnumerable actual = items.Where(nBooleanExpression, filter);
            IEnumerable expected = items.Where(model => model.NIsChecked == true);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData("true", true)]
        [InlineData("TRUE", true)]
        [InlineData("false", false)]
        [InlineData("FALSE", false)]
        public void Apply_NotEqualsFilter(String value, Boolean? isChecked)
        {
            filter.Method = "not-equals";
            filter.Values = new[] { value };

            IEnumerable actual = items.Where(booleanExpression, filter);
            IEnumerable expected = items.Where(model => model.IsChecked != isChecked);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_MultipleNotEqualsFilter()
        {
            filter.Method = "not-equals";
            filter.Values = new[] { "true", "false" };

            Assert.Empty(items.Where(booleanExpression, filter));
        }

        [Fact]
        public void Apply_BadMethod_ReturnsNull()
        {
            filter.Method = "test";
            filter.Values = new[] { "false" };

            Assert.Null(filter.Apply(booleanExpression.Body));
        }

        #endregion
    }
}

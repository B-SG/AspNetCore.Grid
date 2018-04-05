﻿using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class DateTimeFilterTests : BaseGridFilterTests
    {
        private Expression<Func<GridModel, DateTime?>> nDateExpression;
        private Expression<Func<GridModel, DateTime>> dateExpression;
        private IQueryable<GridModel> items;
        private DateTimeFilter filter;

        public DateTimeFilterTests()
        {
            items = new[]
            {
                new GridModel { Date = new DateTime(2013, 01, 01), NDate = null },
                new GridModel { Date = new DateTime(2014, 01, 01), NDate = new DateTime(2015, 01, 01) },
                new GridModel { Date = new DateTime(2015, 01, 01), NDate = new DateTime(2014, 01, 01) }
            }.AsQueryable();

            nDateExpression = (model) => model.NDate;
            dateExpression = (model) => model.Date;
            filter = new DateTimeFilter();
        }

        #region Apply(Expression expression)

        [Fact]
        public void Apply_NotDateTimeValue_ReturnsItems()
        {
            filter.Value = "Test";

            Assert.Null(filter.Apply(dateExpression.Body));
        }

        [Fact]
        public void Apply_NullableEqualsFilter()
        {
            filter.Value = new DateTime(2014, 01, 01).ToString();
            filter.Type = "equals";

            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);
            IEnumerable expected = items.Where(model => model.NDate == new DateTime(2014, 01, 01));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_EqualsFilter()
        {
            filter.Value = new DateTime(2014, 01, 01).ToString();
            filter.Type = "equals";

            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);
            IEnumerable expected = items.Where(model => model.Date == new DateTime(2014, 01, 01));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableNotEqualsFilter()
        {
            filter.Value = new DateTime(2014, 01, 01).ToString();
            filter.Type = "not-equals";

            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);
            IEnumerable expected = items.Where(model => model.NDate != new DateTime(2014, 01, 01));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NotEqualsFilter()
        {
            filter.Value = new DateTime(2014, 01, 01).ToString();
            filter.Type = "not-equals";

            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);
            IEnumerable expected = items.Where(model => model.Date != new DateTime(2014, 01, 01));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableEarlierThanFilter()
        {
            filter.Value = new DateTime(2014, 01, 01).ToString();
            filter.Type = "earlier-than";

            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);
            IEnumerable expected = items.Where(model => model.NDate < new DateTime(2014, 01, 01));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_EalierThanFilter()
        {
            filter.Value = new DateTime(2014, 01, 01).ToString("d");
            filter.Type = "earlier-than";

            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);
            IEnumerable expected = items.Where(model => model.Date < new DateTime(2014, 01, 01));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableLaterThanFilter()
        {
            filter.Value = new DateTime(2014, 01, 01).ToString();
            filter.Type = "later-than";

            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);
            IEnumerable expected = items.Where(model => model.NDate > new DateTime(2014, 01, 01));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_LaterThanFilter()
        {
            filter.Value = new DateTime(2014, 01, 01).ToString("D");
            filter.Type = "later-than";

            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);
            IEnumerable expected = items.Where(model => model.Date > new DateTime(2014, 01, 01));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableEarlierThanOrEqualFilter()
        {
            filter.Value = new DateTime(2014, 01, 01).ToString();
            filter.Type = "earlier-than-or-equal";

            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);
            IEnumerable expected = items.Where(model => model.NDate <= new DateTime(2014, 01, 01));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_EarlierThanOrEqualFilter()
        {
            filter.Value = new DateTime(2014, 01, 01).ToString("d");
            filter.Type = "earlier-than-or-equal";

            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);
            IEnumerable expected = items.Where(model => model.Date <= new DateTime(2014, 01, 01));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableLaterThanOrEqualFilter()
        {
            filter.Value = new DateTime(2014, 01, 01).ToString();
            filter.Type = "later-than-or-equal";

            IEnumerable actual = Filter(items, filter.Apply(nDateExpression.Body), nDateExpression);
            IEnumerable expected = items.Where(model => model.NDate >= new DateTime(2014, 01, 01));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_LaterThanOrEqualFilter()
        {
            filter.Value = new DateTime(2014, 01, 01).ToString("d");
            filter.Type = "later-than-or-equal";

            IEnumerable actual = Filter(items, filter.Apply(dateExpression.Body), dateExpression);
            IEnumerable expected = items.Where(model => model.Date >= new DateTime(2014, 01, 01));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NotSupportedType_ReturnsNull()
        {
            filter.Value = new DateTime(2014, 01, 01).ToString("d");
            filter.Type = "test";

            Assert.Null(filter.Apply(dateExpression.Body));
        }

        #endregion
    }
}

using Microsoft.EntityFrameworkCore;
using Moq;
using ProjectIndependence.API.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Tests.Static
{
    public static class CreateMockSet
    {
        public static Mock<DbSet<T>> CreateMockDbSet<T>(IEnumerable<T> mockData) where T : EntityBase
        {
            var queryableData = mockData.AsQueryable();

            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(queryableData.Provider);

            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Expression)
                .Returns(queryableData.Expression);

            mockSet.As<IQueryable<T>>()
                .Setup(m => m.ElementType)
                .Returns(queryableData.ElementType);

            mockSet.As<IQueryable<T>>()
                .Setup(m => m.GetEnumerator())
                .Returns(queryableData.GetEnumerator());

            return mockSet;
        }
    }
}

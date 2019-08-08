using System;
using GraphQL.EntityFrameworkCore.DynamicLinq.Helpers;
using GraphQL.EntityFrameworkCore.DynamicLinq.Resolvers;
using GraphQL.EntityFrameworkCore.DynamicLinq.Tests.Utils.Types;
using Moq;
using Xunit;

namespace GraphQL.EntityFrameworkCore.DynamicLinq.Tests.Helpers
{
    public class QueryArgumentInfoHelperTests
    {
        private readonly Mock<IPropertyPathResolver> _propertyPathResolverMock;

        private readonly QueryArgumentInfoHelper _sut;

        public QueryArgumentInfoHelperTests()
        {
            _propertyPathResolverMock = new Mock<IPropertyPathResolver>();
            _propertyPathResolverMock.Setup(pr => pr.Resolve(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<Type>()))
                .Returns((Type sourceType, string sourcePath, Type destinationType) => sourcePath);

            _sut = new QueryArgumentInfoHelper(_propertyPathResolverMock.Object);
        }

        [Fact]
        public void PopulateQueryArgumentInfoList_With_GraphType_With_NestedGraphTypes_ReturnsCorrectQueryArgumentInfoList()
        {
            // Act
            var result = _sut.PopulateQueryArgumentInfoList<ReservationType>();

            int x = 0;
        }
    }
}

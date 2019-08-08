using System;
using System.Linq;
using FluentAssertions;
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
        public void PopulateQueryArgumentInfoList_With_GraphType_ReturnsCorrectQueryArgumentInfoList()
        {
            // Arrange
            _propertyPathResolverMock.Setup(pr => pr.Resolve(It.IsAny<Type>(), "Id", It.IsAny<Type>())).Returns("Idee");

            // Act
            var list = _sut.PopulateQueryArgumentInfoList<GuestType>();

            // Assert
            list.Select(q => q.GraphPath).Should().BeEquivalentTo("Id", "Name", "RegisterDate");
            list.Select(q => q.EntityPath).Should().BeEquivalentTo("Idee", "Name", "RegisterDate");
        }

        [Fact]
        public void PopulateQueryArgumentInfoList_With_GraphType_With_NestedGraphTypes_ReturnsCorrectQueryArgumentInfoList()
        {
            // Act
            var list = _sut.PopulateQueryArgumentInfoList<ReservationType>();

            // Assert
            list.Select(q => q.GraphPath).Should().BeEquivalentTo(
                "Id",
                "CheckinDate",
                "CheckoutDate",
                "GuestId",
                "GuestName",
                "GuestRegisterDate",
                "RoomId",
                "RoomName",
                "RoomNumber",
                "RoomAllowedSmoking",
                "RoomStatus",
                "RoomRoomDetailId",
                "RoomRoomDetailWindows",
                "RoomRoomDetailBeds"
            );
            list.Select(q => q.EntityPath).Should().BeEquivalentTo(
                "Id",
                "CheckinDate",
                "CheckoutDate",
                "Guest.Id",
                "Guest.Name",
                "Guest.RegisterDate",
                "Room.Id",
                "Room.Name",
                "Room.Number",
                "Room.AllowedSmoking",
                "Room.Status",
                "Room.RoomDetail.Id",
                "Room.RoomDetail.Windows", "Room.RoomDetail.Beds"
            );
        }
    }
}

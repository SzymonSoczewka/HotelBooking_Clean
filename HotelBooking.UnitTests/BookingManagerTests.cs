using System;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;

        public BookingManagerTests(){
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);
        }

        [Fact]
        public void FindAvailableRoom_StartDateNotLaterThanEndDate_ThrowsArgumentException()
        {
            // Arrange
            DateTime dateStart = DateTime.Today.AddDays(10);
            DateTime dateEnd = DateTime.Today.AddDays(1);
            // Act
            void act() => bookingManager.FindAvailableRoom(dateStart, dateEnd);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }
        [Fact]
        public void FindAvailableRoom_StartDateInThePast_ThrowsArgumentException()
        {
            // Arrange
            DateTime dateStart = DateTime.Today.AddDays(-5);
            DateTime dateEnd = DateTime.Today;
            // Act
            void act() => bookingManager.FindAvailableRoom(dateStart, dateEnd);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime dateStart = DateTime.Today.AddDays(1);
            DateTime dateEnd = dateStart;
            // Act
            int roomId = bookingManager.FindAvailableRoom(dateStart, dateEnd);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

    }
}

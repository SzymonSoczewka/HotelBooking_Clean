using System;
using System.Collections.Generic;
using HotelBooking.Core;
using Xunit;
using Moq;

namespace HotelBooking.UnitTests
{
    /*
     
                                                                       ,_____ ,
                                                                      ,._ ,_. 7\
                                                                     j `-'     /
                                                                     |o_, o    \
                                                                    .`_y_`-,'   !
                                                                    |/   `, `._ `-,
                                                                    |_     \   _.'*\
                                                                      >--,-'`-'*_*'``---.
                                                                      |\_* _*'-'         '
                                                                     /    `               \
                                                                     \.         _ .       /
                                                                      '`._     /   )     /
                                                                       \  |`-,-|  /c-'7 /
                                                                        ) \ (_,| |   / (_
                                                                       ((_/   ((_;)  \_)))

      */
    public class BookingManagerTests
    {
        private readonly Mock<IRepository<Booking>> bookingRepository;
        private readonly Mock<IRepository<Room>> roomRepository;
        private readonly IBookingManager bookingManager;

        public BookingManagerTests(){
            roomRepository = new Mock<IRepository<Room>>();
            bookingRepository = new Mock<IRepository<Booking>>();

            var rooms = new List<Room>
            {
                new Room { Id=1, Description="2 person room with single beds." },
                new Room { Id=2, Description="2 person room with one big bed." },
            };

            DateTime fullyOccupiedStartDate = DateTime.Today.AddDays(5);
            DateTime fullyOccupiedEndDate = DateTime.Today.AddDays(20);

            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=fullyOccupiedStartDate, EndDate=fullyOccupiedEndDate, IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=fullyOccupiedStartDate, EndDate=fullyOccupiedEndDate, IsActive=true, CustomerId=2, RoomId=2 },
            };

            roomRepository.Setup(x => x.GetAll()).Returns(rooms);
            bookingRepository.Setup(x => x.GetAll()).Returns(bookings);
            bookingManager = new BookingManager(bookingRepository.Object, roomRepository.Object);
        }

        [Fact]
        public void FindAvailableRoom_StartDateLaterThanEndDate_ThrowsArgumentException()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(10);
            DateTime endDate = DateTime.Today.AddDays(1);
            // Act
            void act() => bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
            Assert.Throws<ArgumentException>(act);
        }
        [Fact]
        public void FindAvailableRoom_StartDateInThePast_ThrowsArgumentException()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(-5);
            DateTime endDate = DateTime.Today;
            // Act
            void act() => bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_ReturnsRoomIdNotMinusOne()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = startDate;
            // Act
            int roomId = bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        [Fact]
        public void CreateBooking_StartDateLaterThanEndDate_ThrowsArgumentException()
        {
            // Arrange
            Booking booking = new()
            {
                StartDate = DateTime.Today.AddDays(10),
                EndDate = DateTime.Today.AddDays(1)
            };
            // Act
            void act() => bookingManager.CreateBooking(booking);
            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void CreateBooking_StartDateInThePast_ThrowsArgumentException()
        {
            // Arrange
            Booking booking = new()
            {
                StartDate = DateTime.Today.AddDays(-5),
                EndDate = DateTime.Today
            };
            // Act
            void act() => bookingManager.CreateBooking(booking);
            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void CreateBooking_RoomUnavailable_ReturnsFalse()
        {
            // Arrange
            Booking booking = new()
            {
                StartDate = DateTime.Today.AddDays(6),
                EndDate = DateTime.Today.AddDays(8)
            };
            // Act
            bool isCreated = bookingManager.CreateBooking(booking);
            // Assert
            Assert.False(isCreated);
        }

        [Fact]
        public void CreateBooking_AvailableRoom_ReturnsTrue()
        {
            // Arrange
            Booking booking = new()
            {
                StartDate = DateTime.Today.AddDays(21),
                EndDate = DateTime.Today.AddDays(25)
            };
            // Act
            bool isCreated = bookingManager.CreateBooking(booking);
            // Assert
            Assert.True(isCreated);
        }

        [Fact]
        public void GetFullyOccupiedDates_StartDateLaterThanEndDate_ThrowsArgumentException()
        {
            //Arrange
            DateTime startDate = DateTime.Today.AddDays(10);
            DateTime endDate = DateTime.Today.AddDays(1);
            //Act
            void act() => bookingManager.GetFullyOccupiedDates(startDate, endDate);
            //Assert
            Assert.Throws<ArgumentException>(act);
        }


        [Fact]
        public void GetFullyOccupiedDates_TestDatesMatchFullyOccupiedDates_ReturnsListCount16()
        {
            //Arrange
            DateTime startDate = DateTime.Today.AddDays(5);
            DateTime endDate = DateTime.Today.AddDays(20);
            //Act
            List<DateTime> fullyOccupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);
            //Assert
            Assert.Equal(16, fullyOccupiedDates.Count);
        }

        [Fact]
        public void GetFullyOccupiedDates_StartDayAdd21EndDateAdd25_ReturnsEmptyList()
        {
            //Arrange
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(4);
            //Act
            List<DateTime> fullyOccupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);
            //Assert
            Assert.Empty(fullyOccupiedDates);
        }
    }
}

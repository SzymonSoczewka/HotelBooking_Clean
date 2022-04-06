using HotelBooking.Core;
using System;
using TechTalk.SpecFlow;
using HotelBooking.SpecflowTests.Fakes;
using Moq;
using Xunit;

namespace HotelBooking.SpecflowTests.Steps
{
    [Binding]
    public class BookingManagerSteps
    {
        private IBookingManager bookingManager;
        private DateTime occupiedDateStart;
        private DateTime occupiedDateEnd;
        private static int occupiedNumStart = 10;
        private static int occupiedNumEnd = 20;
        private static int customerId = 1;
        private static int roomID = 1;
        private static int id = 2;
        private bool result;

        private Mock<IRepository<Booking>> mockBookingRepository;
        private Mock<IRepository<Room>> mockRoomRepository;
        private Mock<IRepository<Customer>> mockCustomerRepository;
        private Mock<IBookingManager> mockBookingManager;

        private IBookingManager fakeBookingManager;

        public BookingManagerSteps()
        {
            occupiedDateStart = DateTime.Today.AddDays(occupiedNumStart);
            occupiedDateEnd = DateTime.Today.AddDays(occupiedNumEnd);

            var bookingList = new Booking[] { new Booking() { StartDate = occupiedDateStart.AddDays(-1), EndDate = occupiedDateEnd, RoomId = 1, CustomerId = 1, IsActive = true, Id = 1 }, new Booking() { StartDate = occupiedDateStart, EndDate = occupiedDateEnd, RoomId = 2, CustomerId = 2, IsActive = true, Id = 2 } };
            var roomsList = new Room[] { new Room() { Description = "1", Id = 1 }, new Room() { Description = "2", Id = 2 } };

            mockBookingRepository = new Mock<IRepository<Booking>>();
            mockRoomRepository = new Mock<IRepository<Room>>();
            mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockBookingManager = new Mock<IBookingManager>();

            mockRoomRepository.Setup(x => x.GetAll()).Returns(() => roomsList);

            mockBookingRepository.Setup(x => x.GetAll()).Returns(() => bookingList);

            fakeBookingManager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);

            //Dates are fully occupied in the FakeBookingRepository
            IRepository<Booking> bookingRepository = new FakeBookingRepository(occupiedDateStart, occupiedDateEnd);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);

        }

        [Given(@"start date before occupied time")]
        public void GivenStartDateBeforeOccupiedTime()
        {
            occupiedDateStart = DateTime.Today.AddDays(occupiedNumStart - 3);
        }

        [Given(@"end date before occupied time")]
        public void GivenEndDateBeforeOccupiedTime()
        {
            occupiedDateEnd = DateTime.Today.AddDays(occupiedNumStart - 2);
        }

        [When(@"creating a booking")]
        public void WhenCreatingABooking()
        {
            Booking booking = new Booking() { CustomerId = customerId, StartDate = occupiedDateStart, EndDate = occupiedDateEnd, RoomId = roomID, Id = id };

            result = fakeBookingManager.CreateBooking(booking);
        }

        [Then(@"the booking should be created")]
        public void ThenTheBookingShouldBeCreated()
        {
            Assert.True(result);
        }

        [Given(@"start date after occupied time")]
        public void GivenStartDateAfterOccupiedTime()
        {
            occupiedDateStart = DateTime.Today.AddDays(occupiedNumEnd + 2);
        }

        [Given(@"end date after occupied time")]
        public void GivenEndDateAfterOccupiedTime()
        {
            occupiedDateEnd = DateTime.Today.AddDays(occupiedNumEnd + 3);
        }

        [Then(@"the booking should not be created")]
        public void ThenTheBookingShouldNotBeCreated()
        {
            Assert.False(result);
        }

        [Given(@"start date in occupied time")]
        public void GivenStartDateInOccupiedTime()
        {
            occupiedDateStart = DateTime.Today.AddDays(occupiedNumStart);
        }

        [Given(@"end date in occupied time")]
        public void GivenEndDateInOccupiedTime()
        {
            occupiedDateEnd = DateTime.Today.AddDays(occupiedNumEnd);
        }

        [Given(@"start date after, and > end date")]
        public void GivenStartDateAfterAndEndDate()
        {
            occupiedDateStart = DateTime.Today.AddDays(occupiedNumStart + 4);
        }

    }
}
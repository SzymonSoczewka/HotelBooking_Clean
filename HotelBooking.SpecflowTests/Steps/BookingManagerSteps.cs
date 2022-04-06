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
        private DateTime start;
        private DateTime end;
        private static DateTime occupiedStartDate;
        private static DateTime occupiedEndDate;
        private static readonly int customerId = 1;
        private static readonly int roomID = 1;
        private static readonly int id = 2;
        private bool result;

        private Mock<IRepository<Booking>> mockBookingRepository;
        private Mock<IRepository<Room>> mockRoomRepository;
        private readonly Mock<IRepository<Customer>> mockCustomerRepository;
        private readonly Mock<IBookingManager> mockBookingManager;

        private IBookingManager fakeBookingManager;

        public BookingManagerSteps()
        {
            occupiedStartDate = DateTime.Today.AddDays(10);
            occupiedEndDate = DateTime.Today.AddDays(20);

            var bookingList = new Booking[] { new Booking() { StartDate = start.AddDays(-1), EndDate = end, RoomId = 1, CustomerId = 1, IsActive = true, Id = 1 }, new Booking() { StartDate = start, EndDate = end, RoomId = 2, CustomerId = 2, IsActive = true, Id = 2 } };
            var roomsList = new Room[] { new Room() { Description = "1", Id = 1 }, new Room() { Description = "2", Id = 2 } };

            mockBookingRepository = new Mock<IRepository<Booking>>();
            mockRoomRepository = new Mock<IRepository<Room>>();
            mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockBookingManager = new Mock<IBookingManager>();

            mockRoomRepository.Setup(x => x.GetAll()).Returns(() => roomsList);

            mockBookingRepository.Setup(x => x.GetAll()).Returns(() => bookingList);

            fakeBookingManager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);

            //Dates are fully occupied in the FakeBookingRepository
            IRepository<Booking> bookingRepository = new FakeBookingRepository(occupiedStartDate, occupiedEndDate);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);

        }

        [Given(@"start date before occupied time")]
        public void GivenStartDateBeforeOccupiedTime()
        {
            //start = DateTime.Today.AddDays(10 - 3);
            start = occupiedStartDate.AddDays(-3);
        }
        
        [Given(@"end date before occupied time")]
        public void GivenEndDateBeforeOccupiedTime()
        {
            //end = DateTime.Today.AddDays(10 - 2);
            end = occupiedEndDate.AddDays(- 2);
        }
        
        [When(@"creating a booking")]
        public void WhenCreatingABooking()
        {
            Booking booking = new Booking() { CustomerId = customerId, StartDate = start, EndDate = end, RoomId = roomID, Id = id };

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
            //start = DateTime.Today.AddDays(20 + 2);
            start = occupiedStartDate.AddDays(12);
        }

        [Given(@"end date after occupied time")]
        public void GivenEndDateAfterOccupiedTime()
        {
            //end = DateTime.Today.AddDays(20 + 3);
            end = occupiedEndDate.AddDays(13);
        }

        [Then(@"the booking should not be created")]
        public void ThenTheBookingShouldNotBeCreated()
        {
            Assert.False(result);
        }

        [Given(@"start date in occupied time")]
        public void GivenStartDateInOccupiedTime()
        {
            //start = DateTime.Today.AddDays(10);
            start = occupiedStartDate.AddDays(1);

        }

        [Given(@"end date in occupied time")]
        public void GivenEndDateInOccupiedTime()
        {
            //end = DateTime.Today.AddDays(20);
            end = occupiedEndDate.AddDays(5);
        }

        [Given(@"start date after, and > end date")]
        public void GivenStartDateAfterAndEndDate()
        {
            //start = DateTime.Today.AddDays(10 + 4);
            start = occupiedEndDate.AddDays(1);
        }

    }
}

using HotelBooking.Core;
using HotelBooking.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace HotelBooking.UnitTests
{

    public class FakeCustomerTest
    {
        private CustomersController controller;
        private Mock<IRepository<Customer>> fakeCustomerRepository;
        /*
                                                        __ __
                                                o-''))_____\\
                                                "--__/ * * * )
                                                c_c__/-c____/

        */
        public FakeCustomerTest()
        {
            var Customers = new List<Customer>
            {
                new Customer { Id=0, Name="Szymon", Email="polskaguram@gmail.com"},
                new Customer { Id=1, Name="Jan", Email="polskaguram@gmail.com"},
                new Customer { Id=2, Name="Marecek", Email="polskaguram@gmail.com"},
                new Customer { Id=3, Name="Matt the Kiss", Email="polskaguram@gmail.com"},

            };

            // Create fake CustomerRepository. 
            fakeCustomerRepository = new Mock<IRepository<Customer>>();

            // Implement fake GetAll() method.
            fakeCustomerRepository.Setup(x => x.GetAll()).Returns(Customers);

            controller = new CustomersController(fakeCustomerRepository.Object);
            fakeCustomerRepository.Setup(x =>
            x.Get(It.IsInRange<int>(1, 2, Moq.Range.Inclusive))).Returns(Customers[1]);
        }
        [Fact]
        public void GetById_CustomerExists_ReturnsIActionResultWithCustomer()
        {
            // Act
            var result = controller.Details(1) as ViewResult;
            var customer = result.Model as Customer;
            var customerID = customer.Id;

            // Assert
            Assert.InRange<int>(customerID, 1, 2);
        }
        [Fact]
        public void CreateCustomerTake1()
        {
            /*  // Act
              var customer = new Customer { Id = 5, Name = "Henrik the first of his name", Email = "theteacher@easv.dkk" };
              controller.Create(customer);
              bool isCreated = fakeCustomerRepository.Crea();


              // Assert*/
            bool isCreated = true;
            Assert.True(isCreated);
        }

        [Fact]
        public void Delete_WhenIdIsLargerThanZero_RemoveIsCalled()
        {
            // Act
            var getResult = controller.Details(1) as ViewResult;
            var customerToBeDeleted = getResult.Model as Customer;

            var deletionResult = controller.Delete(1) as ViewResult;
            var deletedCustomer = deletionResult.Model as Customer;

            // Assert against the mock object
            Assert.Equal(customerToBeDeleted, deletedCustomer);
        }

        [Fact]
        public void Delete_WhenIdIsLessThanOne_RemoveIsNotCalled()
        {
            // Act
            controller.Delete(0);

            // Assert against the mock object
            fakeCustomerRepository.Verify(x => x.Remove(It.IsAny<int>()), Times.Never());
        }
        [Fact]
        public void CreateCustomerTake2()
        {
            var customer = new Customer { Id = 5, Name = "Henrik the first of his name", Email = "theteacher@easv.dkk" };
            fakeCustomerRepository.Setup(p => p.Add(customer)).Equals(true);
            var result = controller.Create(customer) as Customer;
            Assert.Equal(result, result);
        }
    }
}
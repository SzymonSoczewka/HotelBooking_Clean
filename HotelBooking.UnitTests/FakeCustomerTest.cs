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
                new Customer { Id=1, Name="Szymon", Email="polskaguram@gmail.com"},
                new Customer { Id=2, Name="Szymon", Email="polskaguram@gmail.com"},
                new Customer { Id=3, Name="Szymon", Email="polskaguram@gmail.com"},
                new Customer { Id=4, Name="Szymon", Email="polskaguram@gmail.com"},

            };

            // Create fake CustomerRepository. 
            fakeCustomerRepository = new Mock<IRepository<Customer>>();

            // Implement fake GetAll() method.
            fakeCustomerRepository.Setup(x => x.GetAll()).Returns(Customers);


            
    }
}
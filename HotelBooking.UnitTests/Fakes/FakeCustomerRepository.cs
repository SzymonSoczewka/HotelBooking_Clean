using HotelBooking.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.UnitTests.Fakes
{
    class FakeCustomerRepository : IRepository<Customer>
    {
       
        public FakeCustomerRepository()
        {
         
        }
        public bool addWasCalled = false;

        public void Add(Customer entity)
        {
            addWasCalled = true;
        }
        public bool editWasCalled = false;

        public void Edit(Customer entity)
        {
            editWasCalled = true;
        }

        public Customer Get(int id)
        {
            return new Customer { Id = 1, Name = "Soczweka", Email="polandia@poland.pl" };
        }

        public IEnumerable<Customer> GetAll()
        {
             List<Customer> Customers = new List<Customer>
            {
                new Customer { Id=1, Name="Szymon", Email="polskaguram@gmail.com"},
                new Customer { Id=2, Name="Jan", Email="polskaguram@gmail.com"},
                new Customer { Id=3, Name="Marecek", Email="polskaguram@gmail.com"},
                new Customer { Id=4, Name="Matt the Kiss", Email="polskaguram@gmail.com"},

            };
            return Customers;
        }
        public bool removeWasCalled = false;

        public void Remove(int id)
        {
            removeWasCalled = true;
        }
    }
}

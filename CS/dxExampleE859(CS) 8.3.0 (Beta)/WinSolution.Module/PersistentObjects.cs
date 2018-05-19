using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using System.ComponentModel;

namespace Northwind {

    [DefaultClassOptions]
    [Persistent("Customers")]
    [DefaultProperty("ContactName")]
    public class Customer : XPLiteObject {
        public Customer(Session session) : base(session) { }

        [Key]
        public string CustomerID;

        public string CompanyName;
        public string ContactTitle;
        public string ContactName;
        public string Country;
        public string City;

        [Association("CustomerOrders", typeof(Order))]
        public XPCollection<Order> Orders {
            get {
                return GetCollection<Order>("Orders");
            }
        }
    }
    [DefaultClassOptions]
    [Persistent("Orders")]
    public class Order : XPLiteObject {
        public Order(Session session) : base(session) { }

        [Key(true)]
        public int OrderID;
        public DateTime ShippedDate;

        [Persistent("CustomerID"), Association("CustomerOrders")]
        public Customer Customer;

        [Persistent("EmployeeID"), Association("EmployeeOrders")]
        public Employee Employee;

        public decimal Freight;
    }
    [DefaultClassOptions]
    [Persistent("Employees")]
    [DefaultProperty("FullName")]
    public class Employee : XPLiteObject {
        public Employee(Session session) : base(session) { }

        [Key(true)]
        public int EmployeeID;
        public string FirstName;
        public string LastName;
        public string FullName {
            get { return FirstName+" "+LastName; }
        }
        [Association("EmployeeOrders")]
        public XPCollection<Order> Orders {
            get {
                return GetCollection<Order>("Orders");
            }
        }
    }
}
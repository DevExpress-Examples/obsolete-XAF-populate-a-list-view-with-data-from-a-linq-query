using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using System.ComponentModel;
using System.Linq;
using XAF.LinqToXpo;

namespace NorthwindDemo.Module {
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

        //Dennis: This attribute is required to specify a
        [QueryProjections("Employee_Linq;Orders_Sum_Linq")]
        public static IQueryable OrdersLinq(Session s) {
            XPQuery<Order> orders = new XPQuery<Order>(s);
            var queryOrders = from o in orders
                              orderby o.Employee.FirstName ascending
                              group o by o.Employee.FirstName + " " + o.Employee.LastName into oo
                              where oo.Sum(o => o.Freight) > 10000
                              select new {
                                  OrderID = oo.Sum(o => o.OrderID),
                                  Employee_Linq = oo.Key,
                                  Orders_Sum_Linq = oo.Sum(o => o.Freight)
                              };
            return queryOrders;
        }
    }
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
        //Dennis: There is no CustomQueryProperties attribute here, because we return entire persistent object as a result.
        public static IQueryable CustomersLinq(Session s) {
            XPQuery<Customer> customers = new XPQuery<Customer>(s);
            var queryCustomers = from c in customers
                                 where (c.Country == "Germany" && c.ContactTitle == "Sales Representative")
                                 orderby c.ContactName
                                 select c;
            return queryCustomers;
        }
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
            get { return FirstName + " " + LastName; }
        }
        [Association("EmployeeOrders")]
        public XPCollection<Order> Orders {
            get {
                return GetCollection<Order>("Orders");
            }
        }

        [QueryProjections("FullName;Orders_Max_Linq")]
        public static IQueryable EmployeesLinq(Session s) {
            XPQuery<Employee> employees = new XPQuery<Employee>(s);
            var queryEmployees = from e in employees
                                 select new {
                                     e.EmployeeID,
                                     FullName = e.FirstName + " " + e.LastName,
                                     Orders_Max_Linq = e.Orders.Max(o => o.Freight)
                                 };
            return queryEmployees;
        }
    }
}
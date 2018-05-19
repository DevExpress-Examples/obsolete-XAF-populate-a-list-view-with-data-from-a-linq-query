using System;
using DevExpress.ExpressApp;
using WinWebSolution.Module;
using Dennis.Linq;
using System.Linq;
using DevExpress.Xpo;

namespace WinWebSolution.Module {
    public class LinqCollectionSourceProvider {
        public static CollectionSourceBase GetCollectionSource(ObjectSpace objectSpace, Type objectType, string listViewID) {
            CollectionSourceBase cs = null;
            switch (listViewID) {
                case "Customer_ListView_Linq":

                    XPQuery<Customer> customers = new XPQuery<Customer>(objectSpace.Session);
                    var queryCustomers = from c in customers
                                         where (c.Country == "Germany" && c.ContactTitle == "Sales Representative")
                                         orderby c.ContactName
                                         select c;
                    cs = new LinqCollectionSource(objectSpace, objectType, queryCustomers);
                    break;
                case "Order_ListView_Linq":
                    XPQuery<Order> orders = new XPQuery<Order>(objectSpace.Session);
                    var queryOrders = from o in orders
                                      orderby o.Employee.FirstName ascending
                                      group o by o.Employee.FirstName + " " + o.Employee.LastName into oo
                                      where oo.Sum(o => o.Freight) > 10000
                                      select new {
                                          OrderID = oo.Sum(o => o.OrderID),
                                          Employee_Linq = oo.Key,
                                          Orders_Sum_Linq = oo.Sum(o => o.Freight)
                                      };
                    cs = new LinqCollectionSource(objectSpace, objectType, queryOrders);
                    break;
                case "Employee_ListView_Linq":
                    XPQuery<Employee> employees = new XPQuery<Employee>(objectSpace.Session);
                    var queryEmployees = from e in employees
                                         select new {
                                             e.EmployeeID,
                                             FullName = e.FirstName + " " + e.LastName,
                                             Orders_Max_Linq = e.Orders.Max(o => o.Freight)
                                         };

                    cs = new LinqCollectionSource(objectSpace, objectType, queryEmployees);
                    break;
            }
            return cs;
        }
    }
}
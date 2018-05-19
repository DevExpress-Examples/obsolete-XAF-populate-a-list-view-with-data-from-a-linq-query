using System;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp;
using Northwind;
using DevExpress.Xpo;
using System.Linq;
using Dennis.Linq;

namespace WinSolution.Win {
    public partial class WinSolutionWindowsFormsApplication : WinApplication {
        public WinSolutionWindowsFormsApplication() {
            InitializeComponent();
        }
        private void WinSolutionWindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
            e.Updater.Update();
            e.Handled = true;
        }
        #region Linq Support in XAF
        protected override CollectionSourceBase CreateCollectionSourceCore(DevExpress.ExpressApp.ObjectSpace objectSpace, Type objectType, string listViewID) {
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
                                      select new
                                      {
                                          Employee_Linq = oo.Key,
                                          Orders_Sum_Linq = oo.Sum(o => o.Freight)
                                      };
                    cs = new LinqCollectionSource(objectSpace, objectType, queryOrders);
                    break;
                case "Employee_ListView_Linq":
                    XPQuery<Employee> employees = new XPQuery<Employee>(objectSpace.Session);
                    var queryEmployees = from e in employees
                                         select new
                                         {
                                             FullName = e.FirstName + " " + e.LastName,
                                             Orders_Max_Linq = e.Orders.Max(o => o.Freight)
                                         };

                    cs = new LinqCollectionSource(objectSpace, objectType, queryEmployees);
                    break;
                default:
                    cs = base.CreateCollectionSourceCore(objectSpace, objectType, listViewID);
                    break;
            }
            return cs;
        }
        #endregion
    }
}

Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports WinWebSolution.Module
Imports Dennis.Linq
Imports System.Linq
Imports DevExpress.Xpo

Namespace WinWebSolution.Module
	Public Class LinqCollectionSourceProvider
		Public Shared Function GetCollectionSource(ByVal objectSpace As ObjectSpace, ByVal objectType As Type, ByVal listViewID As String) As CollectionSourceBase
			Dim cs As CollectionSourceBase = Nothing
			Select Case listViewID
				Case "Customer_ListView_Linq"

					Dim customers As New XPQuery(Of Customer)(objectSpace.Session)
					Dim queryCustomers = _
						From c In customers _
						Where (c.Country Is "Germany" AndAlso c.ContactTitle = "Sales Representative") _
						Order By c.ContactName _
						Select c
					cs = New LinqCollectionSource(objectSpace, objectType, queryCustomers)
				Case "Order_ListView_Linq"
					Dim orders As New XPQuery(Of Order)(objectSpace.Session)
					Dim queryOrders = _
						From o In orders _
						Order By o.Employee.FirstName Ascending _
						Group o By GroupKey = o.Employee.FirstName & " " & o.Employee.LastName Into oo = Group _
						Where oo.Sum(Function(o) o.Freight) > 10000 _
						Select New With {Key .OrderID = oo.Sum(Function(o) o.OrderID), Key .Employee_Linq = GroupKey, Key .Orders_Sum_Linq = oo.Sum(Function(o) o.Freight)}
					cs = New LinqCollectionSource(objectSpace, objectType, queryOrders)
				Case "Employee_ListView_Linq"
					Dim employees As New XPQuery(Of Employee)(objectSpace.Session)
					Dim queryEmployees = _
						From e In employees _
						Select New With {Key e.EmployeeID, Key .FullName = e.FirstName & " " & e.LastName, Key .Orders_Max_Linq = e.Orders.Max(Function(o) o.Freight)}

					cs = New LinqCollectionSource(objectSpace, objectType, queryEmployees)
			End Select
			Return cs
		End Function
	End Class
End Namespace
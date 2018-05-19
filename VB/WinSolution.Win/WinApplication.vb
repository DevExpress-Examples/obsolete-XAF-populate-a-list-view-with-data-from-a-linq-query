Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp.Win
Imports DevExpress.ExpressApp
Imports Northwind
Imports DevExpress.Xpo
Imports System.Linq
Imports Dennis.Linq

Namespace WinSolution.Win
	Partial Public Class WinSolutionWindowsFormsApplication
		Inherits WinApplication
		Public Sub New()
			InitializeComponent()
		End Sub
		Private Sub WinSolutionWindowsFormsApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs) Handles MyBase.DatabaseVersionMismatch
			e.Updater.Update()
			e.Handled = True
		End Sub
		#Region "Linq Support in XAF"
		Protected Overrides Function CreateCollectionSourceCore(ByVal objectSpace As DevExpress.ExpressApp.ObjectSpace, ByVal objectType As Type, ByVal listViewID As String) As CollectionSourceBase
			Dim cs As CollectionSourceBase = Nothing
			Select Case listViewID
				Case "Customer_ListView_Linq"
					Dim customers As XPQuery(Of Customer) = New XPQuery(Of Customer)(objectSpace.Session)
					Dim queryCustomers = From c In customers _
					                     Where (c.Country = "Germany" AndAlso c.ContactTitle = "Sales Representative") _
					                     Order By c.ContactName _
					                     Select c
					cs = New LinqCollectionSource(objectSpace, objectType, queryCustomers)
				Case "Order_ListView_Linq"
					Dim orders As XPQuery(Of Order) = New XPQuery(Of Order)(objectSpace.Session)
                    Dim queryOrders = From o In orders _
                                      Order By o.Employee.FirstName Ascending _
                                      Group o By o.Employee.FirstName Into oo = Group _
                                      Where oo.Sum(Function(o) o.Freight) > 10000 _
                                      Select New With {Key .Employee_Linq = FirstName, Key .Orders_Sum_Linq = oo.Sum(Function(o) o.Freight)}
                    cs = New LinqCollectionSource(objectSpace, objectType, CType(queryOrders, IQueryable))
				Case "Employee_ListView_Linq"
					Dim employees As XPQuery(Of Employee) = New XPQuery(Of Employee)(objectSpace.Session)
                    Dim queryEmployees = From e In employees _
                                         Select New With {e.FirstName, Key .Orders_Max_Linq = e.Orders.Max(Function(o) o.Freight)}

					cs = New LinqCollectionSource(objectSpace, objectType, queryEmployees)
				Case Else
					cs = MyBase.CreateCollectionSourceCore(objectSpace, objectType, listViewID)
			End Select
			Return cs
		End Function
		#End Region
	End Class
End Namespace

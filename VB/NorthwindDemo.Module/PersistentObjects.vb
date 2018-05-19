Imports System
Imports DevExpress.Xpo
Imports DevExpress.Persistent.Base
Imports System.ComponentModel
Imports System.Linq
Imports XAF.LinqToXpo

Namespace NorthwindDemo.Module
    <DefaultClassOptions, Persistent("Orders")> _
    Public Class Order
        Inherits XPLiteObject

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub

        <Key(True)> _
        Public OrderID As Integer
        Public ShippedDate As Date

        <Persistent("CustomerID"), Association("CustomerOrders")> _
        Public Customer As Customer

        <Persistent("EmployeeID"), Association("EmployeeOrders")> _
        Public Employee As Employee

        Public Freight As Decimal

        'Dennis: This attribute is required to specify a
        <QueryProjections("Employee_Linq;Orders_Sum_Linq")> _
        Public Shared Function OrdersLinq(ByVal s As Session) As IQueryable
            Dim orders As New XPQuery(Of Order)(s)
            Dim queryOrders = From o In orders _
                Order By o.Employee.FirstName Ascending _
                Group o By GroupKey = o.Employee.FirstName & " " & o.Employee.LastName Into oo = Group _
                Where oo.Sum(Function(o) o.Freight) > 10000 _
                Select New With { _
                    Key .OrderID = oo.Sum(Function(o) o.OrderID), _
                    Key .Employee_Linq = GroupKey, _
                    Key .Orders_Sum_Linq = oo.Sum(Function(o) o.Freight) _
                }
            Return queryOrders
        End Function
    End Class
    <DefaultClassOptions, Persistent("Customers"), DefaultProperty("ContactName")> _
    Public Class Customer
        Inherits XPLiteObject

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub

        <Key> _
        Public CustomerID As String

        Public CompanyName As String
        Public ContactTitle As String
        Public ContactName As String
        Public Country As String
        Public City As String

        <Association("CustomerOrders", GetType(Order))> _
        Public ReadOnly Property Orders() As XPCollection(Of Order)
            Get
                Return GetCollection(Of Order)("Orders")
            End Get
        End Property
        'Dennis: There is no CustomQueryProperties attribute here, because we return entire persistent object as a result.
        Public Shared Function CustomersLinq(ByVal s As Session) As IQueryable
            Dim customers As New XPQuery(Of Customer)(s)
            Dim queryCustomers = From c In customers _
                Where (c.Country = "Germany" AndAlso c.ContactTitle = "Sales Representative") _
                Order By c.ContactName _
                Select c
            Return queryCustomers
        End Function
    End Class
    <DefaultClassOptions, Persistent("Employees"), DefaultProperty("FullName")> _
    Public Class Employee
        Inherits XPLiteObject

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub

        <Key(True)> _
        Public EmployeeID As Integer
        Public FirstName As String
        Public LastName As String
        Public ReadOnly Property FullName() As String
            Get
                Return FirstName & " " & LastName
            End Get
        End Property
        <Association("EmployeeOrders")> _
        Public ReadOnly Property Orders() As XPCollection(Of Order)
            Get
                Return GetCollection(Of Order)("Orders")
            End Get
        End Property

        <QueryProjections("FullName;Orders_Max_Linq")> _
        Public Shared Function EmployeesLinq(ByVal s As Session) As IQueryable
            Dim employees As New XPQuery(Of Employee)(s)
            Dim queryEmployees = From e In employees _
                Select New With { _
                    Key e.EmployeeID, _
                    Key .FullName = e.FirstName & " " & e.LastName, _
                    Key .Orders_Max_Linq = e.Orders.Max(Function(o) o.Freight) _
                }
            Return queryEmployees
        End Function
    End Class
End Namespace
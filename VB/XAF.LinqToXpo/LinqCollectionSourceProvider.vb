Imports System
Imports System.Linq
Imports DevExpress.Xpo
Imports System.Reflection
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Xpo
Imports System.Collections.Generic

Namespace XAF.LinqToXpo
    Public NotInheritable Class LinqCollectionSourceHelper

        Private Sub New()
        End Sub

        Public Shared Sub CreateCustomCollectionSource(ByVal sender As Object, ByVal e As CreateCustomCollectionSourceEventArgs)
            Dim listViewInfo As IModelListViewLinq = TryCast(DirectCast(sender, XafApplication).FindModelView(e.ListViewID), IModelListViewLinq)
            If listViewInfo Is Nothing Then
                Return
            End If
            If String.IsNullOrEmpty(listViewInfo.XPQueryMethod) Then
                Return
            End If
            Dim query As IQueryable = LinqCollectionSourceHelper.InvokeMethod(e.ObjectType, listViewInfo.XPQueryMethod, CType(e.ObjectSpace, XPObjectSpace).Session)
            If query Is Nothing Then
                Return
            End If
            e.CollectionSource = New LinqCollectionSource(e.ObjectSpace, e.ObjectType, query)
        End Sub
        Public Shared Function GetXPQueryMethods(ByVal type As Type) As String()
            Dim names As New List(Of String)()
            Dim methods() As MethodInfo = type.GetMethods(System.Reflection.BindingFlags.Public Or System.Reflection.BindingFlags.Static)
            For Each mi As MethodInfo In methods
                If IsCompatibleMethod(mi) Then
                    names.Add(mi.Name)
                End If
            Next mi
            Return names.ToArray()
        End Function
        Public Shared Function IsCompatibleMethod(ByVal mi As MethodInfo) As Boolean
            Dim pis() As ParameterInfo = mi.GetParameters()
            Return mi.ReturnType IsNot Nothing AndAlso GetType(IQueryable).IsAssignableFrom(mi.ReturnType) AndAlso pis.Length = 1 AndAlso pis(0).ParameterType.IsAssignableFrom(GetType(Session))
        End Function
        Public Shared Function InvokeMethod(ByVal type As Type, ByVal name As String, ByVal session As Session) As IQueryable
            Dim method As MethodInfo = FindMethod(type, name)
            If method Is Nothing Then
                Return Nothing
            End If
            Return DirectCast(method.Invoke(Nothing, New Object() { session }), IQueryable)
        End Function
        Private Shared Function FindMethod(ByVal type As Type, ByVal name As String) As MethodInfo
            Dim methods() As MethodInfo = type.GetMethods(System.Reflection.BindingFlags.Public Or System.Reflection.BindingFlags.Static)
            For Each mi As MethodInfo In methods
                If mi.Name = name AndAlso IsCompatibleMethod(mi) Then
                    Return mi
                End If
            Next mi
            Return Nothing
        End Function
        Public Shared Function GetDisplayableProperties(ByVal type As Type, ByVal name As String) As String()
            Dim method As MethodInfo = FindMethod(type, name)
            If method Is Nothing Then
                Return Nothing
            End If
            For Each attribute As QueryProjectionsAttribute In method.GetCustomAttributes(GetType(QueryProjectionsAttribute), False)
                Return attribute.Projections.Split(";"c)
            Next attribute
            Return Nothing
        End Function
    End Class
    <AttributeUsage(AttributeTargets.Method)> _
    Public NotInheritable Class QueryProjectionsAttribute
        Inherits Attribute

        Public Sub New(ByVal projections As String)
            Me.Projections = projections
        End Sub
        'Comma-separated names of object properties to be included into the query results.
        Private privateProjections As String
        Public Property Projections() As String
            Get
                Return privateProjections
            End Get
            Private Set(ByVal value As String)
                privateProjections = value
            End Set
        End Property
    End Class
End Namespace

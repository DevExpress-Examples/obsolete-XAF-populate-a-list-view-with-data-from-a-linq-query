Imports System
Imports System.Linq
Imports DevExpress.Xpo
Imports System.Collections
Imports System.ComponentModel
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.DC
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.Xpo

Namespace XAF.LinqToXpo
    Public Class LinqCollectionSource
        Inherits CollectionSourceBase

        Public Const DefaultSuffix As String = "_Linq"
        Private objectTypeInfoCore As ITypeInfo
        Private collectionCore As BindingList(Of Object)
        Protected Sub New(ByVal objectSpace As IObjectSpace, ByVal mode As CollectionSourceMode)
            MyBase.New(objectSpace, mode)
        End Sub
        Protected Sub New(ByVal objectSpace As IObjectSpace)
            MyBase.New(objectSpace)
        End Sub
        Public Sub New(ByVal objectSpace As IObjectSpace, ByVal objectType As Type, ByVal query As IQueryable)
            MyBase.New(objectSpace)
            objectTypeInfoCore = XafTypesInfo.Instance.FindTypeInfo(objectType)
            Me.Query = query
        End Sub
        Private privateQuery As IQueryable
        Public Property Query() As IQueryable
            Get
                Return privateQuery
            End Get
            Private Set(ByVal value As IQueryable)
                privateQuery = value
            End Set
        End Property
        Protected Overrides Function CreateCollection() As Object
            DirectCast(Query, XPQueryBase).Session = CType(ObjectSpace, XPObjectSpace).Session
            Return ConvertQueryToCollection(Query)
        End Function
        Public Function ConvertQueryToCollection(ByVal sourceQuery As IQueryable) As IList
            collectionCore = New BindingList(Of Object)()
            For Each item In sourceQuery
                collectionCore.Add(item)
            Next item
            Return collectionCore
        End Function
        Public Overrides Function IsObjectFitForCollection(ByVal obj As Object) As Boolean?
            Return collectionCore.Contains(obj)
        End Function
        Public Overrides ReadOnly Property ObjectTypeInfo() As ITypeInfo
            Get
                Return objectTypeInfoCore
            End Get
        End Property
        Public Overrides Sub Dispose()
            MyBase.Dispose()
            collectionCore = Nothing
            Query = Nothing
        End Sub
        Protected Overrides Sub ApplyCriteriaCore(ByVal criteria As CriteriaOperator)
            ' This method has no implementation.
        End Sub
    End Class
End Namespace
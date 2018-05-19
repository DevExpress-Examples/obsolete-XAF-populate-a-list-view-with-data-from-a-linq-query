Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports System.Collections
Imports System.Linq
Imports DevExpress.Xpo
Imports System.ComponentModel

Namespace Dennis.Linq
	Public Class LinqCollectionSource
		Inherits CollectionSource
		Public Const DefaultSuffix As String = "_Linq"
		Private collectionCore As IBindingList = Nothing
		Public Function ConvertQueryToCollection(ByVal sourceQuery As IQueryable) As IList
			collectionCore = New BindingList(Of Object)()
			For Each item In sourceQuery
				collectionCore.Add(item)
			Next item
			Return collectionCore
		End Function
		Private queryCore As IQueryable = Nothing
		Public Property Query() As IQueryable
			Get
				Return queryCore
			End Get
			Set(ByVal value As IQueryable)
				queryCore = value
			End Set
		End Property
		Protected Overrides Function RecreateCollection(ByVal criteria As CriteriaOperator, ByVal sortings As SortingCollection) As Object
			CType(Query, XPQueryBase).Session = ObjectSpace.Session
			Return ConvertQueryToCollection(Query)
		End Function
		Public Sub New(ByVal objectSpace As ObjectSpace, ByVal objectType As Type)
			MyBase.New(objectSpace, objectType)
		End Sub
		Public Sub New(ByVal objectSpace As ObjectSpace, ByVal objectType As Type, ByVal query As IQueryable)
			MyBase.New(objectSpace, objectType)
			Me.Query = query
		End Sub
		Public Overrides Function IsObjectFitForCollection(ByVal obj As Object) As Nullable(Of Boolean)
			Return collectionCore.Contains(obj)
		End Function
	End Class
End Namespace
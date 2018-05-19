Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports System.Collections
Imports System.Linq
Imports DevExpress.Xpo

Namespace Dennis.Linq
	#Region "Linq Support in XAF"
	Public Class LinqCollectionSource
		Inherits CollectionSource
		Public Const DefaultSuffix As String = "_Linq"
		Public Function ConvertQueryToCollection(ByVal sourceQuery As IQueryable) As IList
			Dim list As List(Of Object) = New List(Of Object)()
			For Each item In sourceQuery
				list.Add(item)
			Next item
			Return list
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
		Protected Overrides Function RecreateCollection(ByVal criteria As CriteriaOperator, ByVal sortings As SortingCollection) As IList
			Return ConvertQueryToCollection(Query)
		End Function
		Public Sub New(ByVal objectSpace As ObjectSpace, ByVal objectType As Type)
			MyBase.New(objectSpace, objectType)
		End Sub
		Public Sub New(ByVal objectSpace As ObjectSpace, ByVal objectType As Type, ByVal query As IQueryable)
			MyBase.New(objectSpace, objectType)
			Me.Query = query
		End Sub
	End Class
	#End Region
End Namespace

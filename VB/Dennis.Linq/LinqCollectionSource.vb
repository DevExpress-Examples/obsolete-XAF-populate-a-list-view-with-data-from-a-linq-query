' Developer Express Code Central Example:
' How to populate the list view with data from a LINQ query
' 
' See the http://www.devexpress.com/scid=K18107 KB Article for more information.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E859


Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports DevExpress.Xpo
Imports System.Collections
Imports System.ComponentModel
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.DC
Imports DevExpress.Data.Filtering

Namespace Dennis.Linq
	Public Class LinqCollectionSource
		Inherits CollectionSourceBase
		Public Const DefaultSuffix As String = "_Linq"
		Private collectionCore As IBindingList
		Private objectTypeInfoCore As ITypeInfo
		Public Function ConvertQueryToCollection(ByVal sourceQuery As IQueryable) As IList
			collectionCore = New BindingList(Of Object)()
			For Each item In sourceQuery
				collectionCore.Add(item)
			Next item
			Return collectionCore
		End Function
		Private queryCore As IQueryable = Nothing
		Protected Sub New(ByVal objectSpace As IObjectSpace, ByVal mode As CollectionSourceMode)
			MyBase.New(objectSpace, mode)
		End Sub
		Protected Sub New(ByVal objectSpace As IObjectSpace)
			MyBase.New(objectSpace)
		End Sub
		Public Sub New(ByVal objectSpace As IObjectSpace, ByVal objectType As Type, ByVal query As IQueryable)
			MyBase.New(objectSpace)
			objectTypeInfoCore = XafTypesInfo.Instance.FindTypeInfo(objectType)
			queryCore = query
		End Sub
		Public Property Query() As IQueryable
			Get
				Return queryCore
			End Get
			Set(ByVal value As IQueryable)
				queryCore = value
			End Set
		End Property
		Protected Overrides Function CreateCollection() As Object
			CType(Query, XPQueryBase).Session = (CType(ObjectSpace, ObjectSpace)).Session
			Return ConvertQueryToCollection(Query)
		End Function
		Public Overrides Function IsObjectFitForCollection(ByVal obj As Object) As Boolean?
			Return collectionCore.Contains(obj)
		End Function
		Protected Overrides Sub ApplyCriteriaCore(ByVal criteria As CriteriaOperator)
		End Sub
		Public Overrides ReadOnly Property ObjectTypeInfo() As ITypeInfo
			Get
				Return objectTypeInfoCore
			End Get
		End Property
	End Class
End Namespace
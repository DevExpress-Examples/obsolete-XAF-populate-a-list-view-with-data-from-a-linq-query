' Developer Express Code Central Example:
' How to populate the list view with data from a LINQ query
' 
' See the http://www.devexpress.com/scid=K18107 KB Article for more information.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E859


Imports Microsoft.VisualBasic
Imports DevExpress.ExpressApp
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp.Model
Imports DevExpress.ExpressApp.Model.NodeGenerators
Imports DevExpress.ExpressApp.Model.Core
Imports System

Namespace Dennis.Linq
	Public NotInheritable Partial Class LinqModule
		Inherits ModuleBase
		Public Sub New()
			InitializeComponent()
		End Sub
		Public Overrides Sub Setup(ByVal application As XafApplication)
			MyBase.Setup(application)
			AddHandler application.CreateCustomCollectionSource, AddressOf LinqCollectionSourceHelper.CreateCustomCollectionSource
		End Sub
		Public Overrides Sub ExtendModelInterfaces(ByVal extenders As ModelInterfaceExtenders)
			MyBase.ExtendModelInterfaces(extenders)
			extenders.Add(Of IModelListView, IModelListViewLinq)()
		End Sub
		Public Overrides Sub AddGeneratorUpdaters(ByVal updaters As ModelNodesGeneratorUpdaters)
			MyBase.AddGeneratorUpdaters(updaters)
			updaters.Add(New ModelListViewLinqNodesGeneratorUpdater())
			updaters.Add(New ModelListViewLinqColumnsNodesGeneratorUpdater())
		End Sub
	End Class

	Public Interface IModelListViewLinq
	Inherits IModelNode
		Property XPQueryMethod() As String
	End Interface
	Public Class ModelListViewLinqNodesGeneratorUpdater
		Inherits ModelNodesGeneratorUpdater(Of ModelViewsNodesGenerator)
		Public Overrides Sub UpdateNode(ByVal node As ModelNode)
			For Each classInfo As IModelClass In node.Application.BOModel
				If classInfo.TypeInfo.IsPersistent Then
					If (Not String.IsNullOrEmpty(classInfo.Name)) Then
						For Each method As String In LinqCollectionSourceHelper.GetXPQueryMethods(classInfo.TypeInfo.Type)
							Dim id As String = ModelListViewNodesGenerator.GetListViewId(classInfo.TypeInfo.Name) & "_" & method & LinqCollectionSource.DefaultSuffix
							Dim listViewInfo As IModelListView = (TryCast(node.Application.Views.GetNode(id), IModelListView))
							If listViewInfo Is Nothing Then
								listViewInfo = node.AddNode(Of IModelListView)(id)
							End If
							listViewInfo.ModelClass = classInfo
							CType(listViewInfo, IModelListViewLinq).XPQueryMethod = method
						Next method
					End If
				End If
			Next classInfo
		End Sub
	End Class
	Public Class ModelListViewLinqColumnsNodesGeneratorUpdater
		Inherits ModelNodesGeneratorUpdater(Of ModelListViewColumnsNodesGenerator)
		Public Overrides Sub UpdateNode(ByVal node As ModelNode)
			Dim linqViewInfo As IModelListViewLinq = TryCast(node.Parent, IModelListViewLinq)
			If linqViewInfo IsNot Nothing AndAlso (Not String.IsNullOrEmpty(linqViewInfo.XPQueryMethod)) Then
				Dim listViewInfo As IModelListView = CType(linqViewInfo, IModelListView)
				Dim columns() As String = LinqCollectionSourceHelper.GetDisplayableProperties(listViewInfo.ModelClass.TypeInfo.Type, linqViewInfo.XPQueryMethod)
				If columns IsNot Nothing Then
					If listViewInfo.Columns Is Nothing Then
						listViewInfo.AddNode(Of IModelColumns)("Columns")
					End If
					Dim i As Integer = listViewInfo.Columns.Count
					Do While i > 0
						i -= 1
						Dim col As IModelColumn = listViewInfo.Columns.Item(i)
						If Array.IndexOf(columns, col.Id) < 0 Then
						   col.Remove()
						End If
					Loop
					For Each column As String In columns
						Dim col As IModelColumn = (TryCast(listViewInfo.Columns.GetNode(column), IModelColumn))
						If col Is Nothing Then
							col = listViewInfo.Columns.AddNode(Of IModelColumn)(column)
							col.PropertyName = column
						End If
					Next column
				End If
			End If
		End Sub
	End Class
End Namespace
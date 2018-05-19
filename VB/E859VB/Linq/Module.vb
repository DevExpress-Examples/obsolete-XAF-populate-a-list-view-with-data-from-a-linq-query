Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.NodeWrappers
Imports DevExpress.ExpressApp.InfoGenerators
Imports DevExpress.Xpo

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
		Public Overrides Sub UpdateModel(ByVal model As Dictionary)
			MyBase.UpdateModel(model)
			Dim applicatioNodeWrapper As New ApplicationNodeWrapper(model)
			For Each classInfo As ClassInfoNodeWrapper In applicatioNodeWrapper.BOModel.Classes
				If classInfo.ClassTypeInfo.IsPersistent Then
					If (Not String.IsNullOrEmpty(classInfo.Name)) Then
						For Each method As String In LinqCollectionSourceHelper.GetXPQueryMethods(classInfo.ClassTypeInfo.Type)
							Dim linqViewInfo As New ListViewInfoNodeWrapper()
							linqViewInfo.ClassName = classInfo.Name
							linqViewInfo.Id = ListViewNodeGenerator.GetListViewId(linqViewInfo.ClassName) & "_" & method & LinqCollectionSource.DefaultSuffix
							linqViewInfo.Node.SetAttribute("XPQueryMethod", method)
							Dim columns() As String = LinqCollectionSourceHelper.GetDisplayableProperties(classInfo.ClassTypeInfo.Type, method)
							If columns Is Nothing Then
								ListViewNodeGenerator.Generate(linqViewInfo, classInfo)
							Else
								linqViewInfo.Node.AddChildNode(ColumnsNodeWrapper.NodeName)
								For Each column As String In columns
									linqViewInfo.Columns.AddColumn(column)
								Next column
							End If
							If applicatioNodeWrapper.Views.FindViewById(linqViewInfo.Id) Is Nothing Then
								applicatioNodeWrapper.Views.Node.AddChildNode(linqViewInfo.Node)
							End If
						Next method
					End If
				End If
			Next classInfo
		End Sub
	End Class
End Namespace

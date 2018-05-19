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
		Public Overrides Sub UpdateModel(ByVal model As Dictionary)
			MyBase.UpdateModel(model)
			Dim applicatioNodeWrapper As New ApplicationNodeWrapper(model)
			For Each classInfo As ClassInfoNodeWrapper In applicatioNodeWrapper.BOModel.Classes
				If XafTypesInfo.Instance.FindTypeInfo(classInfo.ClassTypeInfo.Type).FindAttribute(Of PersistentAttribute)() IsNot Nothing Then
					If (Not String.IsNullOrEmpty(classInfo.Name)) Then
						Dim linqViewInfo As New ListViewInfoNodeWrapper()
						linqViewInfo.ClassName = classInfo.Name
						linqViewInfo.Id = ListViewNodeGenerator.GetListViewId(linqViewInfo.ClassName) + LinqCollectionSource.DefaultSuffix
						linqViewInfo.Node.AddChildNode(ColumnsNodeWrapper.NodeName)
						If applicatioNodeWrapper.Views.FindViewById(linqViewInfo.Id) Is Nothing Then
							applicatioNodeWrapper.Views.Node.AddChildNode(linqViewInfo.Node)
						End If
					End If
				End If
			Next classInfo
		End Sub
	End Class
End Namespace

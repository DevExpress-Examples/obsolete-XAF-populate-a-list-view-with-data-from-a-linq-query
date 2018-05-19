Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports WinWebSolution.Module

Namespace WinWebSolution.Win
	Partial Public Class WinWebSolutionWindowsFormsApplication
		Protected Overrides Function CreateCollectionSourceCore(ByVal objectSpace As ObjectSpace, ByVal objectType As Type, ByVal listViewID As String) As CollectionSourceBase
			Dim cs As CollectionSourceBase = LinqCollectionSourceProvider.GetCollectionSource(objectSpace, objectType, listViewID)
			If cs Is Nothing Then
				cs = MyBase.CreateCollectionSourceCore(objectSpace, objectType, listViewID)
			End If
			Return cs
		End Function
	End Class
End Namespace
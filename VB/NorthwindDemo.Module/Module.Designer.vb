' Developer Express Code Central Example:
' How to populate the list view with data from a LINQ query
' 
' See the http://www.devexpress.com/scid=K18107 KB Article for more information.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E859


Imports Microsoft.VisualBasic
Imports System
Namespace NorthwindDemo.Module
	Partial Public Class NorthwindDemoModule
		''' <summary> 
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary> 
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Component Designer generated code"

		''' <summary> 
		''' Required method for Designer support - do not modify 
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			' 
			' NorthwindDemoModule
			' 
			Me.RequiredModuleTypes.Add(GetType(DevExpress.ExpressApp.SystemModule.SystemModule))
			Me.RequiredModuleTypes.Add(GetType(DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule))
			Me.RequiredModuleTypes.Add(GetType(Dennis.Linq.LinqModule))

		End Sub

		#End Region
	End Class
End Namespace

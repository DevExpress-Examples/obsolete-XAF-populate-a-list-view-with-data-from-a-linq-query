Imports System
Imports System.Collections.Generic
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Updating

Namespace NorthwindDemo.Module
    Public NotInheritable Partial Class NorthwindDemoModule
        Inherits ModuleBase

        Public Sub New()
            InitializeComponent()
        End Sub
        Public Overrides Function GetModuleUpdaters(ByVal objectSpace As IObjectSpace, ByVal versionFromDB As Version) As IEnumerable(Of DevExpress.ExpressApp.Updating.ModuleUpdater)
            Return ModuleUpdater.EmptyModuleUpdaters
        End Function
    End Class
End Namespace

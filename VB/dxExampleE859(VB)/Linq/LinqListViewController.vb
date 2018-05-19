Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.SystemModule

Namespace Dennis.Linq
#Region "Linq Support in XAF"
    Public Class LinqListViewController
        Inherits ViewController
        Private Const DefaultReason As String = "LinqListViewController is active"
        Protected Overrides Sub OnActivated()
            MyBase.OnActivated()
            Dim flag As Boolean = Not View.Id.EndsWith(LinqCollectionSource.DefaultSuffix)
            Frame.GetController(Of ListViewProcessCurrentObjectController)().Active(DefaultReason) = flag
            Frame.GetController(Of DeleteObjectsViewController)().Active(DefaultReason) = flag
            Frame.GetController(Of NewObjectViewController)().Active(DefaultReason) = flag
            Frame.GetController(Of FilterController)().Active(DefaultReason) = flag
        End Sub
        Protected Overrides Sub OnDeactivating()
            MyBase.OnDeactivating()
            Frame.GetController(Of ListViewProcessCurrentObjectController)().Active.RemoveItem(DefaultReason)
            Frame.GetController(Of DeleteObjectsViewController)().Active.RemoveItem(DefaultReason)
            Frame.GetController(Of NewObjectViewController)().Active.RemoveItem(DefaultReason)
            Frame.GetController(Of FilterController)().Active.RemoveItem(DefaultReason)
        End Sub
    End Class
#End Region
End Namespace

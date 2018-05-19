Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.SystemModule

Namespace XAF.LinqToXpo
    Public Class DisableStandardActionsForLinqListViewController
        Inherits ViewController(Of ListView)

        Private Const LinqCollectionSourceActiveKey As String = "LinqCollectionSource is used"

        Private Sub SetControllerActivity(Of T As Controller)(ByVal action As System.Action(Of Controller))
            Dim controller As T = Frame.GetController(Of T)()
            If controller IsNot Nothing Then
                action(controller)
            End If
        End Sub

        Protected Overrides Sub OnActivated()
            MyBase.OnActivated()
            Dim flag As Boolean = Not View.Id.EndsWith(LinqCollectionSource.DefaultSuffix)
            SetControllerActivity(Of ListViewProcessCurrentObjectController)(Sub(controller As Controller)
                controller.Active(LinqCollectionSourceActiveKey) = flag
            End Sub)
            SetControllerActivity(Of DeleteObjectsViewController)(Sub(controller As Controller)
                controller.Active(LinqCollectionSourceActiveKey) = flag
            End Sub)
            SetControllerActivity(Of NewObjectViewController)(Sub(controller As Controller)
                controller.Active(LinqCollectionSourceActiveKey) = flag
            End Sub)
            SetControllerActivity(Of FilterController)(Sub(controller As Controller)
                controller.Active(LinqCollectionSourceActiveKey) = flag
            End Sub)
        End Sub
        Protected Overrides Sub OnDeactivated()
            MyBase.OnDeactivated()
            SetControllerActivity(Of ListViewProcessCurrentObjectController)(Sub(controller As Controller)
                controller.Active.RemoveItem(LinqCollectionSourceActiveKey)
            End Sub)
            SetControllerActivity(Of DeleteObjectsViewController)(Sub(controller As Controller)
                controller.Active.RemoveItem(LinqCollectionSourceActiveKey)
            End Sub)
            SetControllerActivity(Of NewObjectViewController)(Sub(controller As Controller)
                controller.Active.RemoveItem(LinqCollectionSourceActiveKey)
            End Sub)
            SetControllerActivity(Of FilterController)(Sub(controller As Controller)
                controller.Active.RemoveItem(LinqCollectionSourceActiveKey)
            End Sub)
        End Sub
    End Class
End Namespace

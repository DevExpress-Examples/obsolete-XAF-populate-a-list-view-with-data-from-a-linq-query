using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;

namespace XAF.LinqToXpo {
    public class DisableStandardActionsForLinqListViewController : ViewController<ListView> {
        private const string LinqCollectionSourceActiveKey = "LinqCollectionSource is used";
        
        private void SetControllerActivity<T>(System.Action<Controller> action) where T : Controller {
            T controller = Frame.GetController<T>();
            if (controller != null) {
                action(controller);
            }
        }

        protected override void OnActivated() {
            base.OnActivated();
            bool flag = !View.Id.EndsWith(LinqCollectionSource.DefaultSuffix);
            SetControllerActivity<ListViewProcessCurrentObjectController>((Controller controller) => { controller.Active[LinqCollectionSourceActiveKey] = flag; });
            SetControllerActivity<DeleteObjectsViewController>((Controller controller) => { controller.Active[LinqCollectionSourceActiveKey] = flag; });
            SetControllerActivity<NewObjectViewController>((Controller controller) => { controller.Active[LinqCollectionSourceActiveKey] = flag; });
            SetControllerActivity<FilterController>((Controller controller) => { controller.Active[LinqCollectionSourceActiveKey] = flag; });
        }
        protected override void OnDeactivated() {
            base.OnDeactivated();
            SetControllerActivity<ListViewProcessCurrentObjectController>((Controller controller) => { controller.Active.RemoveItem(LinqCollectionSourceActiveKey); });
            SetControllerActivity<DeleteObjectsViewController>((Controller controller) => { controller.Active.RemoveItem(LinqCollectionSourceActiveKey); });
            SetControllerActivity<NewObjectViewController>((Controller controller) => { controller.Active.RemoveItem(LinqCollectionSourceActiveKey); });
            SetControllerActivity<FilterController>((Controller controller) => { controller.Active.RemoveItem(LinqCollectionSourceActiveKey); });
        }
    }
}

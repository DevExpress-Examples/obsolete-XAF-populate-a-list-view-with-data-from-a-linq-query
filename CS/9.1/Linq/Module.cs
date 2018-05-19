using DevExpress.ExpressApp;
using DevExpress.ExpressApp.NodeWrappers;
using DevExpress.ExpressApp.InfoGenerators;
using DevExpress.Xpo;

namespace Dennis.Linq {
    public sealed partial class LinqModule : ModuleBase {
        public LinqModule() {
            InitializeComponent();
        }
        public override void UpdateModel(Dictionary model) {
            base.UpdateModel(model);
            ApplicationNodeWrapper applicatioNodeWrapper = new ApplicationNodeWrapper(model);
            foreach (ClassInfoNodeWrapper classInfo in applicatioNodeWrapper.BOModel.Classes) {
                if (XafTypesInfo.Instance.FindTypeInfo(classInfo.ClassTypeInfo.Type).FindAttribute<PersistentAttribute>()!=null){
                    if (!string.IsNullOrEmpty(classInfo.Name)) {
                        ListViewInfoNodeWrapper linqViewInfo = new ListViewInfoNodeWrapper();
                        linqViewInfo.ClassName = classInfo.Name;
                        linqViewInfo.Id = ListViewNodeGenerator.GetListViewId(linqViewInfo.ClassName) + LinqCollectionSource.DefaultSuffix;
                        linqViewInfo.Node.AddChildNode(ColumnsNodeWrapper.NodeName);
                        if (applicatioNodeWrapper.Views.FindViewById(linqViewInfo.Id) == null) {
                            applicatioNodeWrapper.Views.Node.AddChildNode(linqViewInfo.Node);
                        }
                    }
                }
            }
        }
    }
}

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.NodeWrappers;
using DevExpress.ExpressApp.InfoGenerators;
using DevExpress.Xpo;

namespace Dennis.Linq {
    public sealed partial class LinqModule : ModuleBase {
        public LinqModule() {
            InitializeComponent();
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            application.CreateCustomCollectionSource+=new System.EventHandler<CreateCustomCollectionSourceEventArgs>(LinqCollectionSourceHelper.CreateCustomCollectionSource);
        }
        public override void UpdateModel(Dictionary model) {
            base.UpdateModel(model);
            ApplicationNodeWrapper applicatioNodeWrapper = new ApplicationNodeWrapper(model);
            foreach (ClassInfoNodeWrapper classInfo in applicatioNodeWrapper.BOModel.Classes) {
                if (classInfo.ClassTypeInfo.IsPersistent){
                    if (!string.IsNullOrEmpty(classInfo.Name)) {
                        foreach (string method in LinqCollectionSourceHelper.GetXPQueryMethods(classInfo.ClassTypeInfo.Type)) {
                            ListViewInfoNodeWrapper linqViewInfo = new ListViewInfoNodeWrapper();
                            linqViewInfo.ClassName = classInfo.Name;
                            linqViewInfo.Id = ListViewNodeGenerator.GetListViewId(linqViewInfo.ClassName) + "_" + method + LinqCollectionSource.DefaultSuffix;
                            linqViewInfo.Node.SetAttribute("XPQueryMethod", method);
                            string[] columns = LinqCollectionSourceHelper.GetDisplayableProperties(classInfo.ClassTypeInfo.Type, method);
                            if (columns == null)
                                ListViewNodeGenerator.Generate(linqViewInfo, classInfo);
                            else {
                                linqViewInfo.Node.AddChildNode(ColumnsNodeWrapper.NodeName);
                                foreach (string column in columns)
                                    linqViewInfo.Columns.AddColumn(column);
                            }
                            if (applicatioNodeWrapper.Views.FindViewById(linqViewInfo.Id) == null) {
                                applicatioNodeWrapper.Views.Node.AddChildNode(linqViewInfo.Node);
                            }
                        }
                    }
                }
            }
        }
    }
}

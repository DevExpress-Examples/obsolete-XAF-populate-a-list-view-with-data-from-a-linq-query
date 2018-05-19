using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;

namespace XAF.LinqToXpo {
    public sealed partial class XafLinqToXpoModule : ModuleBase {
        public XafLinqToXpoModule() {
            InitializeComponent();
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            application.CreateCustomCollectionSource += new System.EventHandler<CreateCustomCollectionSourceEventArgs>(LinqCollectionSourceHelper.CreateCustomCollectionSource);
        }
        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            base.ExtendModelInterfaces(extenders);
            extenders.Add<IModelListView, IModelListViewLinq>();
        }
        public override void AddGeneratorUpdaters(ModelNodesGeneratorUpdaters updaters) {
            base.AddGeneratorUpdaters(updaters);
            updaters.Add(new ModelListViewLinqNodesGeneratorUpdater());
            updaters.Add(new ModelListViewLinqColumnsNodesGeneratorUpdater());
        }
    }

    public interface IModelListViewLinq : IModelNode {
        string XPQueryMethod { get; set; }
    }
    public class ModelListViewLinqNodesGeneratorUpdater : ModelNodesGeneratorUpdater<ModelViewsNodesGenerator> {
        public override void UpdateNode(ModelNode node) {
            foreach (IModelClass modelClass in node.Application.BOModel) {
                if (modelClass.TypeInfo.IsPersistent) {
                    if (!string.IsNullOrEmpty(modelClass.Name)) {
                        foreach (string method in LinqCollectionSourceHelper.GetXPQueryMethods(modelClass.TypeInfo.Type)) {
                            string id = ModelListViewNodesGenerator.GetListViewId(modelClass.TypeInfo.Type) + "_" + method + LinqCollectionSource.DefaultSuffix;
                            IModelListView listViewInfo = (node.Application.Views.GetNode(id) as IModelListView);
                            if (listViewInfo == null) {
                                listViewInfo = node.AddNode<IModelListView>(id);
                            }
                            listViewInfo.ModelClass = modelClass;
                            ((IModelListViewLinq)listViewInfo).XPQueryMethod = method;
                        }
                    }
                }
            }
        }
    }
    public class ModelListViewLinqColumnsNodesGeneratorUpdater : ModelNodesGeneratorUpdater<ModelListViewColumnsNodesGenerator> {
        public override void UpdateNode(ModelNode node) {
            IModelListViewLinq linqViewInfo = node.Parent as IModelListViewLinq;
            if (linqViewInfo != null && !string.IsNullOrEmpty(linqViewInfo.XPQueryMethod)) {
                IModelListView listViewInfo = (IModelListView)linqViewInfo;
                string[] columns = LinqCollectionSourceHelper.GetDisplayableProperties(listViewInfo.ModelClass.TypeInfo.Type, linqViewInfo.XPQueryMethod);
                if (columns != null) {
                    if (listViewInfo.Columns == null) {
                        listViewInfo.AddNode<IModelColumns>("Columns");
                    }
                    for (int i = listViewInfo.Columns.Count; i > 0; ) {
                        i--;
                        IModelColumn col = listViewInfo.Columns[i];
                        if (Array.IndexOf(columns, col.Id) < 0) {
                           col.Remove();
                        }
                    }
                    foreach (string column in columns) {
                        IModelColumn col = (listViewInfo.Columns.GetNode(column) as IModelColumn);
                        if (col == null) {
                            col = listViewInfo.Columns.AddNode<IModelColumn>(column);
                            col.PropertyName = column;
                        }
                    }
                }
            }
        }
    }
}
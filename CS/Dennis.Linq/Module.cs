// Developer Express Code Central Example:
// How to populate the list view with data from a LINQ query
// 
// See the http://www.devexpress.com/scid=K18107 KB Article for more information.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E859

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.NodeWrappers;
using DevExpress.ExpressApp.InfoGenerators;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Model.Core;
using System;

namespace Dennis.Linq {
    public sealed partial class LinqModule : ModuleBase {
        public LinqModule() {
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
            foreach (IModelClass classInfo in node.Application.BOModel) {
                if (classInfo.TypeInfo.IsPersistent) {
                    if (!string.IsNullOrEmpty(classInfo.Name)) {
                        foreach (string method in LinqCollectionSourceHelper.GetXPQueryMethods(classInfo.TypeInfo.Type)) {
                            string id = ModelListViewNodesGenerator.GetListViewId(classInfo.TypeInfo.Name) + "_" + method + LinqCollectionSource.DefaultSuffix;
                            IModelListView listViewInfo = (node.Application.Views.GetNode(id) as IModelListView);
                            if (listViewInfo == null) {
                                listViewInfo = node.AddNode<IModelListView>(id);
                            }
                            listViewInfo.ModelClass = classInfo;
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
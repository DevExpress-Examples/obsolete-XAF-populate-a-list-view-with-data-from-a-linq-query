using System;
using DevExpress.ExpressApp;
using WinWebSolution.Module;

namespace WinWebSolution.Win {
    partial class WinWebSolutionWindowsFormsApplication {
        protected override CollectionSourceBase CreateCollectionSourceCore(ObjectSpace objectSpace, Type objectType, string listViewID) {
            CollectionSourceBase cs = LinqCollectionSourceProvider.GetCollectionSource(objectSpace, objectType, listViewID);
            if (cs == null) {
                cs = base.CreateCollectionSourceCore(objectSpace, objectType, listViewID);
            }
            return cs;
        }
    }
}
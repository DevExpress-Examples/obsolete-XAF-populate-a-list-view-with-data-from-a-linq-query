using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;

namespace NorthwindDemo.Module {
    public sealed partial class NorthwindDemoModule : ModuleBase {
        public NorthwindDemoModule() {
            InitializeComponent();
        }
        public override IEnumerable<DevExpress.ExpressApp.Updating.ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            return ModuleUpdater.EmptyModuleUpdaters;
        }
    }
}

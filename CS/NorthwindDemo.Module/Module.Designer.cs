// Developer Express Code Central Example:
// How to populate the list view with data from a LINQ query
// 
// See the http://www.devexpress.com/scid=K18107 KB Article for more information.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E859

namespace NorthwindDemo.Module {
    partial class NorthwindDemoModule {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            // 
            // NorthwindDemoModule
            // 
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule));
            this.RequiredModuleTypes.Add(typeof(XAF.LinqToXpo.XafLinqToXpoModule));

        }

        #endregion
    }
}

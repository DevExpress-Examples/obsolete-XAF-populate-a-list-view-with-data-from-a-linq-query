// Developer Express Code Central Example:
// How to populate the list view with data from a LINQ query
// 
// See the http://www.devexpress.com/scid=K18107 KB Article for more information.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E859

using System;
using System.Linq;
using DevExpress.Xpo;
using System.Collections;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;

namespace Dennis.Linq {
    public class LinqCollectionSource : CollectionSourceBase {
        public const string DefaultSuffix = "_Linq";
        private IBindingList collectionCore;
        private ITypeInfo objectTypeInfoCore;
        public IList ConvertQueryToCollection(IQueryable sourceQuery) {
            collectionCore = new BindingList<object>();
            foreach (var item in sourceQuery) { collectionCore.Add(item); }
            return collectionCore;
        }
        private IQueryable queryCore = null;
        protected LinqCollectionSource(IObjectSpace objectSpace, CollectionSourceMode mode)
            : base(objectSpace, mode) {
        }
        protected LinqCollectionSource(IObjectSpace objectSpace)
            : base(objectSpace) {
        }
        public LinqCollectionSource(IObjectSpace objectSpace, Type objectType, IQueryable query)
            : base(objectSpace) {
            objectTypeInfoCore = XafTypesInfo.Instance.FindTypeInfo(objectType);
            queryCore = query;
        }
        public IQueryable Query {
            get { return queryCore; }
            set { queryCore = value; }
        }
        protected override object CreateCollection() {
            ((XPQueryBase)Query).Session = ((XPObjectSpace)ObjectSpace).Session;
            return ConvertQueryToCollection(Query);
        }
        public override bool? IsObjectFitForCollection(object obj) {
            return collectionCore.Contains(obj);
        }
        protected override void ApplyCriteriaCore(CriteriaOperator criteria) { }
        public override ITypeInfo ObjectTypeInfo {
            get { return objectTypeInfoCore; }
        }
    }
}
using System;
using System.Linq;
using DevExpress.Xpo;
using System.Collections;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;

namespace XAF.LinqToXpo {
    public class LinqCollectionSource : CollectionSourceBase {
        public const string DefaultSuffix = "_Linq";
        private ITypeInfo objectTypeInfoCore;
        private BindingList<object> collectionCore;
        protected LinqCollectionSource(IObjectSpace objectSpace, CollectionSourceMode mode)
            : base(objectSpace, mode) {
        }
        protected LinqCollectionSource(IObjectSpace objectSpace)
            : base(objectSpace) {
        }
        public LinqCollectionSource(IObjectSpace objectSpace, Type objectType, IQueryable query)
            : base(objectSpace) {
            objectTypeInfoCore = XafTypesInfo.Instance.FindTypeInfo(objectType);
            Query = query;
        }
        public IQueryable Query {
            get;
            private set;
        }
        protected override object CreateCollection() {
            ((XPQueryBase)Query).Session = ((XPObjectSpace)ObjectSpace).Session;
            return ConvertQueryToCollection(Query);
        }
        public IList ConvertQueryToCollection(IQueryable sourceQuery) {
            collectionCore = new BindingList<object>();
            foreach (var item in sourceQuery) { collectionCore.Add(item); }
            return collectionCore;
        }
        public override bool? IsObjectFitForCollection(object obj) {
            return collectionCore.Contains(obj);
        }
        public override ITypeInfo ObjectTypeInfo {
            get { return objectTypeInfoCore; }
        }
        public override void Dispose() {
            base.Dispose();
            collectionCore = null;
            Query = null;
        }
        protected override void ApplyCriteriaCore(CriteriaOperator criteria) { 
            // This method has no implementation.
        }
    }
}
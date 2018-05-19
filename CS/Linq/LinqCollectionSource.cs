using System;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using System.Collections;
using System.Linq;
using DevExpress.Xpo;
using System.ComponentModel;

namespace Dennis.Linq {
    public class LinqCollectionSource : CollectionSource {
        public const string DefaultSuffix = "_Linq";
        private IBindingList collectionCore = null;
        public IList ConvertQueryToCollection(IQueryable sourceQuery) {
            collectionCore = new BindingList<object>();
            foreach (var item in sourceQuery) { collectionCore.Add(item); }
            return collectionCore;
        }
        private IQueryable queryCore = null;
        public IQueryable Query {
            get { return queryCore; }
            set { queryCore = value; }
        }
        protected override IList RecreateCollection(CriteriaOperator criteria, SortingCollection sortings) {
            ((XPQueryBase)Query).Session = ObjectSpace.Session;
            return ConvertQueryToCollection(Query);
        }
        public LinqCollectionSource(ObjectSpace objectSpace, Type objectType) : base(objectSpace, objectType) { }
        public LinqCollectionSource(ObjectSpace objectSpace, Type objectType, IQueryable query)
            : base(objectSpace, objectType) {
            this.Query = query;
        }
        public override bool? IsObjectFitForCollection(object obj) {
            return collectionCore.Contains(obj);
        }
    }
}
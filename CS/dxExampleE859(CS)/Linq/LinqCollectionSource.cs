using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using System.Collections;
using System.Linq;
using DevExpress.Xpo;

namespace Dennis.Linq {
    #region Linq Support in XAF
    public class LinqCollectionSource : CollectionSource {
        public const string DefaultSuffix = "_Linq";
        public IList ConvertQueryToCollection(IQueryable sourceQuery) {
            List<object> list = new List<object>();
            foreach (var item in sourceQuery) { list.Add(item); }
            return list;
        }
        private IQueryable queryCore = null;
        public IQueryable Query {
            get {
                return queryCore;
            }
            set {
                queryCore = value;
            }
        }
        protected override IList RecreateCollection(CriteriaOperator criteria, SortingCollection sortings) {
            return ConvertQueryToCollection(Query);
        }
        public LinqCollectionSource(ObjectSpace objectSpace, Type objectType) : base(objectSpace, objectType) { }
        public LinqCollectionSource(ObjectSpace objectSpace, Type objectType, IQueryable query)
            : base(objectSpace, objectType) {
            this.Query = query;
        }
    }
    #endregion
}

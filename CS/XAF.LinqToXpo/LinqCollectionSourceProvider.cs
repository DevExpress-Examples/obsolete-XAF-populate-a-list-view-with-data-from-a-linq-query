using System;
using System.Linq;
using DevExpress.Xpo;
using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using System.Collections.Generic;

namespace XAF.LinqToXpo {
    public static class LinqCollectionSourceHelper {
        public static void CreateCustomCollectionSource(object sender, CreateCustomCollectionSourceEventArgs e) {
            IModelListViewLinq listViewInfo = ((XafApplication)sender).FindModelView(e.ListViewID) as IModelListViewLinq;
            if (listViewInfo == null) {
                return;
            }
            if (string.IsNullOrEmpty(listViewInfo.XPQueryMethod)) {
                return;
            }
            IQueryable query = LinqCollectionSourceHelper.InvokeMethod(e.ObjectType, listViewInfo.XPQueryMethod, ((XPObjectSpace)e.ObjectSpace).Session);
            if (query == null) {
                return;
            }
            e.CollectionSource = new LinqCollectionSource(e.ObjectSpace, e.ObjectType, query);
        }
        public static string[] GetXPQueryMethods(Type type) {
            List<string> names = new List<string>();
            MethodInfo[] methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (MethodInfo mi in methods) {
                if (IsCompatibleMethod(mi)) {
                    names.Add(mi.Name);
                }
            }
            return names.ToArray();
        }
        public static bool IsCompatibleMethod(MethodInfo mi) {
            ParameterInfo[] pis = mi.GetParameters();
            return mi.ReturnType != null && typeof(IQueryable).IsAssignableFrom(mi.ReturnType)
                    && pis.Length == 1 && pis[0].ParameterType.IsAssignableFrom(typeof(Session));
        }
        public static IQueryable InvokeMethod(Type type, string name, Session session) {
            MethodInfo method = FindMethod(type, name);
            if (method == null) {
                return null;
            }
            return (IQueryable)method.Invoke(null, new object[] { session });
        }
        private static MethodInfo FindMethod(Type type, string name) {
            MethodInfo[] methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (MethodInfo mi in methods) {
                if (mi.Name == name && IsCompatibleMethod(mi)) {
                    return mi;
                }
            }
            return null;
        }
        public static string[] GetDisplayableProperties(Type type, string name) {
            MethodInfo method = FindMethod(type, name);
            if (method == null) {
                return null;
            }
            foreach (QueryProjectionsAttribute attribute in method.GetCustomAttributes(typeof(QueryProjectionsAttribute), false)) {
                return attribute.Projections.Split(';');
            }
            return null;
        }
    }
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class QueryProjectionsAttribute : Attribute {
        public QueryProjectionsAttribute(string projections) {
            this.Projections = projections;
        }
        //Comma-separated names of object properties to be included into the query results.
        public string Projections {
            get;
            private set;
        }
    }
}

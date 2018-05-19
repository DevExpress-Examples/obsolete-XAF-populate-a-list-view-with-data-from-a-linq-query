using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System.Reflection;
using DevExpress.Xpo;

namespace Dennis.Linq {
    public static class LinqCollectionSourceHelper {
        public static void CreateCustomCollectionSource(object sender, CreateCustomCollectionSourceEventArgs e) {
            IModelListViewLinq listViewInfo = ((XafApplication)sender).FindModelView(e.ListViewID) as IModelListViewLinq;
            if (listViewInfo == null) return;
            if (string.IsNullOrEmpty(listViewInfo.XPQueryMethod)) return;
            IQueryable query = LinqCollectionSourceHelper.InvokeMethod(e.ObjectType, listViewInfo.XPQueryMethod, ((ObjectSpace)e.ObjectSpace).Session);
            if (query == null) return;
            e.CollectionSource = new LinqCollectionSource(e.ObjectSpace, e.ObjectType, query);
        }
        public static string[] GetXPQueryMethods(Type type) {
            List<string> names = new List<string>();
            MethodInfo[] methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (MethodInfo mi in methods) {
                if (IsCompatibleMethod(mi))
                    names.Add(mi.Name);
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
            if (method == null) return null;
            return (IQueryable)method.Invoke(null, new object[] { session });
        }
        private static MethodInfo FindMethod(Type type, string name) {
            MethodInfo[] methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (MethodInfo mi in methods) {
                if (mi.Name == name && IsCompatibleMethod(mi)) return mi;
            }
            return null;
        }
        public static string[] GetDisplayableProperties(Type type, string name) {
            MethodInfo method = FindMethod(type, name);
            if (method == null) return null;
            foreach(CustomQueryPropertiesAttribute attribute in method.GetCustomAttributes(typeof(CustomQueryPropertiesAttribute), false))
                if(attribute.Name == "DisplayableProperties") return attribute.Value.Split(';');
            return null;
        }
    }
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CustomQueryPropertiesAttribute : Attribute {
        string theName;
        string theValue;
        public string Name { get { return theName; } }
        public string Value { get { return theValue; } }
        public CustomQueryPropertiesAttribute(string theName, string theValue) {
            this.theName = theName;
            this.theValue = theValue;
        }
    }

}

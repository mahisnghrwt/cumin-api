using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api {
    public static class Helper {
        public const string SOCK_MSG = "sockMsg";
        public const string NO_UID_ERROR_MSG = "Could not extract userId from access token.";

        public static int GetUid(HttpContext context) {
            try {
                return Convert.ToInt32(context.Items["userId"]);
            } catch {
                return -1;
            }
        }
        
        public static void Mapper<T>(Object source, ref T target) {
            var props = source.GetType().GetProperties();
            foreach (var prop in props) {
                string propName = prop.Name;
                Object propVal = prop.GetValue(source);
                if (propVal == null)
                    continue;
                if (prop.PropertyType == typeof(string)) {
                    if ((string)propVal != "EMPTY")
                        typeof(T).GetProperty(propName).SetValue(target, (string)propVal);
                } else {
                    // Get value from Nullable<T>
                    var nullableType = prop.PropertyType;
                    var method = nullableType.GetProperty("HasValue").GetMethod;
                    bool propHasValue = (bool)method.Invoke(propVal, null);
                    if (propHasValue)
                        typeof(T).GetProperty(propName).SetValue(target, nullableType.GetProperty("Value").GetValue(propVal));
                }
            }
        }

    }
}

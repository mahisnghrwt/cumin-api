using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
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
        /// <summary>
        /// For use in dynamic patch requests.
        /// As we know, model binding sets the missing properties as null. In some models a null string can be a valid value. In a patch request user only specifies properties that have to be patched.
        /// So, how do we know a null string property is intended by user or result of model binding?
        /// Dynamic Patch Model must follow these conventions:
        /// 1. If null is valid value for string property, assign "EMPTY" as default value to it.
        /// 2. Except string, primitive types must use Nullable Class
        /// </summary>
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

        public static void Mapper<T>(JsonElement source, ref T target) {
            foreach(var prop in source.EnumerateObject()) {
                var propName = prop.Name.Substring(0, 1).ToUpper() + prop.Name.Substring(1);
                var targetProp = typeof(T).GetProperty(propName);
                if (targetProp != null) {
                    targetProp.SetValue(target, JsonSerializer.Deserialize(prop.Value.GetRawText(), targetProp.PropertyType));
                }
            }
        }

    }
}

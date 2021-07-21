using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace cumin_api {
    public class JsonLowercaseNamingPolicy : JsonNamingPolicy {
        public override string ConvertName(string name) {
            return name.ToLower();
        }
    }
}

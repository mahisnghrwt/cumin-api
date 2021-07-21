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
        


    }
}

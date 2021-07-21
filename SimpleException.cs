using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api {
    public class SimpleException: Exception {
        public SimpleException() { }

        public SimpleException(string message) : base(message) { }

        public SimpleException(string message, Exception inner) : base(message, inner) { }
    }
}

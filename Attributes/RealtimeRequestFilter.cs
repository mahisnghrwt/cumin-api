using cumin_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace cumin_api.Attributes {
  
    public class RealtimeRequestFilter: ResultFilterAttribute {
        private readonly IWebSocketService webSockService;

        public RealtimeRequestFilter(IWebSocketService webSockService) {
            this.webSockService = webSockService;
        }

        override public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next) {
            if (!(context.Result is EmptyResult)) {
                await webSockService.Send(context.HttpContext.Items["sockMsg"]);
                await next();
            } else {
                context.Cancel = true;
            }
        }
    }
}

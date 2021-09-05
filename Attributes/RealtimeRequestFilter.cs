using cumin_api.Enums;
using cumin_api.Models.Socket;
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
        private readonly SockService sockService;

        public RealtimeRequestFilter(SockService sockService) {
            this.sockService = sockService;
            //this.webSockService = webSockService;
        }

        override public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next) {
            if (!(context.Result is EmptyResult)) {
                ObjectResult result = context.Result as ObjectResult;

                if (result == null) {
                    if ((context.Result as StatusCodeResult).StatusCode != 200) {
                        //   context.Cancel = true;
                        await next();
                        return;
                    }
                }
                try {
                    SocketMessageHeader sockHeader = (SocketMessageHeader)context.HttpContext.Items["SocketMessageHeader"];
                    Object sockMessage = context.HttpContext.Items["SocketMessage"];

                    if (sockHeader.broadcastType == (int)SOCKET_MESSAGE_TYPE.BROADCAST) {
                        await sockService.Broadcast(sockHeader.targetId, sockMessage, new HashSet<int>());
                    }
                    else if (sockHeader.broadcastType == (int)SOCKET_MESSAGE_TYPE.TARGET) {
                        await sockService.Target(sockHeader.targetId, sockMessage);
                    }
                    else {
                        throw new Exception();
                    }

                } catch (Exception e) {
                    throw e;
                }

                //await webSockService.Send(context.HttpContext.Items["sockMsg"]);
                await next();
            } else {
                context.Cancel = true;
            }
        }
    }
}

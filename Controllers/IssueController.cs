using cumin_api.Attributes;
using cumin_api.Enums;
using cumin_api.Models;
using cumin_api.Models.DTOs;
using cumin_api.Models.Requests;
using cumin_api.Models.SocketMsgs;
using cumin_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    [CustomAuthorization]
    public class IssueController: ControllerBase {
        private const string INVALID_REPORTER_ERROR = "Reporter does not match the token!";
        private readonly IIssueService issueService;

        public IssueController(IIssueService issueService) {
            this.issueService = issueService;
        }        

        [HttpGet("project/{pid}")]
        public IActionResult GetByProjectId(int pid) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            try {
                return Ok(issueService.GetByProjectId(pid, uid));
            } catch (SimpleException e){
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }
        }

        // sprint?sprintId={sid}&projectId={pid}
        [HttpGet]
        public IActionResult Get() {
            const string MISSING_QUERY_PARAM = "Missing query parameter projectId";

            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });


            int? sid = null;
            int pid = -1;

            foreach(var q in HttpContext.Request.Query) {
                Console.WriteLine(q.Key);
                Console.WriteLine(q.Value);
            }

            if (HttpContext.Request.Query.ContainsKey("sprintId")) {
                sid = Convert.ToInt32(HttpContext.Request.Query["sprintId"]);
            }

            if (HttpContext.Request.Query.ContainsKey("projectId")) {
                pid = Convert.ToInt32(HttpContext.Request.Query["projectId"]);
            }

            if (pid == -1) {
                return Unauthorized(new { message = MISSING_QUERY_PARAM});
            }

            try {
                return Ok(issueService.GetBySprintId(sid, pid, uid));
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        public IActionResult PostIssue([FromBody] Issue request) {
            // first check if the reporter has sent this request
            if (Helper.GetUid(HttpContext) != request.ReporterId) {
                return Unauthorized(new { message = INVALID_REPORTER_ERROR});
            }

            Issue issue;

            try {
                issue = issueService.Create(request);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            var sockMsgs = new List<Object>();
            sockMsgs.Add(new GeneralSockMsg {
                id = issue.ProjectId,
                type = (int)CommonEnums.WEBSOCKET_MESSAGE_TYPE.BROADCAST,
                eventName = (int)SOCKET_EVENT.ISSUE_CREATED,
                payload = issue
            });
            HttpContext.Items[Helper.SOCK_MSG] = sockMsgs;

            return Ok(issue);
        }

        /*
         * Create controller specific to update request, but all endpoints use common update method in the service
         */

        [HttpPut]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        public IActionResult Patch([FromBody] Issue issue) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            List<Issue> issueStates;

            try {
                issueStates = (List<Issue>)issueService.Update(issue, uid);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            var sockMsgs = new List<Object>();
            sockMsgs.Add(new GeneralSockMsg {
                id = issueStates[0].ProjectId,
                type = (int)CommonEnums.WEBSOCKET_MESSAGE_TYPE.BROADCAST,
                eventName = (int)SOCKET_EVENT.ISSUE_UPDATED,
                payload = issueStates
            });
            HttpContext.Items[Helper.SOCK_MSG] = sockMsgs;

            return Ok(issueStates[0]);
        }

        [HttpPatch("status")]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        public IActionResult PatchStatus([FromBody] IssuePatchStatusRequest request) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            List<Issue> issueStates = new List<Issue>();

            try {
                // get the request issue by id
                var issue = issueService.Get(request.Id, uid);
                issueStates.Add(new Issue(issue));
                // patch the model
                issue.Status = request.Status;
                // update the db
                var updated = issueService.UpdateOnly(issue, uid);
                issueStates.Add(updated);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            var sockMsgs = new List<Object>();
            sockMsgs.Add(new GeneralSockMsg {
                id = issueStates[0].ProjectId,
                type = (int)CommonEnums.WEBSOCKET_MESSAGE_TYPE.BROADCAST,
                eventName = (int)SOCKET_EVENT.ISSUE_UPDATED,
                payload = issueStates
            });
            HttpContext.Items[Helper.SOCK_MSG] = sockMsgs;

            return Ok(issueStates[0]);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        public IActionResult DeleteById(int id) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            Issue issue;

            try {
                issue = issueService.Delete(id, uid);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            var sockMsgs = new List<Object>();
            sockMsgs.Add(new GeneralSockMsg {
                id = issue.ProjectId,
                type = (int)CommonEnums.WEBSOCKET_MESSAGE_TYPE.BROADCAST,
                eventName = (int)SOCKET_EVENT.ISSUE_CREATED,
                payload = issue
            });
            HttpContext.Items[Helper.SOCK_MSG] = sockMsgs;

            return Ok(issue);
        }


    }
}

using cumin_api.Attributes;
using cumin_api.Enums;
using cumin_api.Models;
using cumin_api.Models.DTOs;
using cumin_api.Models.Socket;
using cumin_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Controllers {
    [Route("api/v1/project/{projectId}/[controller]")]
    [ApiController]
    [CustomAuthorization]
    public class IssueController: ControllerBase {
        private const string INVALID_REPORTER_ERROR = "Reporter does not match the token!";
        const string sockMsgHeaderStr = "SocketMessageHeader";
        const string sockMsgStr = "SocketMessage";
        private readonly Services.v2.IssueService issueService;

        public IssueController(Services.v2.IssueService issueService) {
            this.issueService = issueService;
        }        

        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        [HttpGet]
        public IActionResult GetAllIssuesInProject(int projectId) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            List<Func<Issue, bool>> filters = new List<Func<Issue, bool>>();
            // add project filter
            filters.Add(issue_ => issue_.ProjectId == projectId);


            // look for the sprintId filter id requested by the user
            bool hasSprintId = Int32.TryParse(HttpContext.Request.Query["sprintId"], out int sprintId);

            // sprint id filter
            if (hasSprintId == true) {
                filters.Add(issue_ => {
                    if (sprintId == -1)
                        return issue_.SprintId.HasValue == false;
                    return issue_.SprintId == sprintId;
                });
            }

            try {
                return Ok(issueService.GetAllFiltered(filters));
            } catch (SimpleException e){
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }
        }

        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        [HttpGet("{issueId}")]
        public IActionResult GetById(int issueId, int projectId) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });


            try {
                Issue issue = issueService.FindById(issueId);
                if (issue.ProjectId != projectId) {
                    return Unauthorized();
                }
                return Ok(issue);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }
        }

        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        [HttpPost]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        public IActionResult CreateIssue([FromBody] IssueCreationDto dto, int projectId) {
            // first check if the reporter has sent this request
            if (Helper.GetUid(HttpContext) != dto.ReporterId) {
                return Unauthorized(new { message = INVALID_REPORTER_ERROR});
            }

            if (dto.ProjectId != projectId) {
                return BadRequest();
            }

            Issue issue = new Issue { Title = dto.Title, Description = dto.Description, Type = dto.Type, ProjectId = dto.ProjectId, ReporterId = dto.ReporterId, SprintId = dto.SprintId};
            Issue issue_;
            try {
                issue_ = issueService.Add(issue);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            SocketMessageHeader sockHeader = new SocketMessageHeader { targetId = projectId, broadcastType = (int)SOCKET_MESSAGE_TYPE.BROADCAST };
            SocketMessage sockMsg = new SocketMessage { eventName = (int)Enums.SOCKET_EVENT.ISSUE_CREATED, payload = issue_ };

            HttpContext.Items[sockMsgHeaderStr] = sockHeader;
            HttpContext.Items[sockMsgStr] = sockMsg;

            return Ok(issue_);
        }


        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        [HttpPatch("status")]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        public IActionResult PatchStatus([FromBody] IssueStatusPatchDto dto, int projectId) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            List<Issue> issueStates = new List<Issue>();
            try {
                // get the request issue by id
                var issue = issueService.FindById(dto.Id);
                if (issue.ProjectId != projectId) {
                    return BadRequest();
                }

                issueStates.Add(new Issue(issue));
                // patch the model
                issue.Status = dto.Status;
                // update the db
                var patchedIssue = issueService.Update(issue);
                issueStates.Add(patchedIssue);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            SocketMessageHeader sockHeader = new SocketMessageHeader { targetId = projectId, broadcastType = (int)SOCKET_MESSAGE_TYPE.BROADCAST };
            SocketMessage sockMsg = new SocketMessage { eventName = (int)Enums.SOCKET_EVENT.ISSUE_UPDATED, payload = issueStates };

            HttpContext.Items[sockMsgHeaderStr] = sockHeader;
            HttpContext.Items[sockMsgStr] = sockMsg;

            return Ok(issueStates[1]);
        }

      

        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        [HttpDelete("{issueId}")]
        // [ServiceFilter(typeof(RealtimeRequestFilter))]
        public IActionResult DeleteById(int issueId, int projectId) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            try {
                issueService.DeleteInProject(issueId, projectId);
                return Ok();
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            //var sockMsgs = new List<Object>();
            //sockMsgs.Add(new GeneralSockMsg {
            //    id = issue.ProjectId,
            //    type = (int)CommonEnums.WEBSOCKET_MESSAGE_TYPE.BROADCAST,
            //    eventName = (int)SOCKET_EVENT.ISSUE_CREATED,
            //    payload = issue
            //});
            //HttpContext.Items[Helper.SOCK_MSG] = sockMsgs;

            //return Ok(issue);
        }

        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        [HttpPatch]
        [ServiceFilter(typeof(RealtimeRequestFilter))]
        public IActionResult Patch([FromBody] IssuePatchDto dto, int projectId) {
            var uid = Helper.GetUid(HttpContext);
            if (uid == -1)
                return Unauthorized(new { message = Helper.NO_UID_ERROR_MSG });

            List<Issue> issueStates = new List<Issue>();
            try {
                // get the request issue by id
                var issue = issueService.FindById(dto.Id);
                issueStates.Add(new Issue(issue));

                try {
                    var props = dto.GetType().GetProperties();
                    for (int i = 0; i < props.Length; i++) {
                        var propName = props[i].Name;
                        var propType = props[i].PropertyType.Name;
                        // propInfo for issue
                        var propTypeIss_ = issue.GetType().GetProperty(propName);

                        if (propType == typeof(int?).Name) {
                            int? val = (int?)props[i].GetValue(dto);
                            if (val.HasValue) {
                                if (val.Value == -1) {
                                    // set val as null in db
                                    propTypeIss_.SetValue(issue, null);
                                } else {
                                    propTypeIss_.SetValue(issue, val.Value);
                                }
                            }
                        } else if (propType == typeof(string).Name) {
                            // if string is not null set val
                            string val = (string)props[i].GetValue(dto);

                            if (val != null) {
                                propTypeIss_.SetValue(issue, val);
                            }
                        } else {
                            // throw error or something
                            // should not happen in first place
                        }
                    }
                } catch {
                    // something went wrong with our system :_(
                }


                if (issue.ProjectId != projectId) {
                    return BadRequest();
                }

                // update the db
                var patchedIssue = issueService.Update(issue);
                issueStates.Add(patchedIssue);
            } catch (SimpleException e) {
                return Unauthorized(new { message = e.Message });
            } catch (DbUpdateException e) {
                return Unauthorized(new { message = e.Message });
            }

            SocketMessageHeader sockHeader = new SocketMessageHeader { targetId = projectId, broadcastType = (int)SOCKET_MESSAGE_TYPE.BROADCAST };
            SocketMessage sockMsg = new SocketMessage { eventName = (int)Enums.SOCKET_EVENT.ISSUE_UPDATED, payload = issueStates };

            HttpContext.Items[sockMsgHeaderStr] = sockHeader;
            HttpContext.Items[sockMsgStr] = sockMsg;

            return Ok(issueStates[1]);
        }


    }
}

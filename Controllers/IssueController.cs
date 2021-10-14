using AutoMapper;
using cumin_api.Attributes;
using cumin_api.Enums;
using cumin_api.Models;
using cumin_api.Models.DTOs;
using cumin_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace cumin_api.Controllers {
    [Route("api/v1/project/{projectId}/[controller]")]
    [ApiController]
    [CustomAuthorization]
    public class IssueController: ControllerBase {
        private readonly Services.v2.IssueService issueService;
        private readonly IMapper mapper;

        public IssueController(Services.v2.IssueService issueService, IMapper mapper) {
            this.issueService = issueService;
            this.mapper = mapper;
        }

        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        [HttpPost]
        public async Task<IActionResult> CreateIssue([FromBody] IssueCreationDto dto, int projectId) {
            int userId = Convert.ToInt32(HttpContext.Items["userId"]);
            Issue issue = mapper.Map<Issue>(dto);
            issue.ReporterId = userId;
            issue.ProjectId = projectId;

            Issue issue_ = await issueService.AddAsync(issue);
            return Ok(issue_);
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
        [HttpGet]
        public IActionResult GetAll(int projectId, bool? detailed) {
            if (!detailed.HasValue || detailed.Value == false)
                return Ok(issueService.GetAllIssuesByProject(projectId));
            var issues = issueService.GetAllIssuesByProjectDetailed(projectId);
            List<IssueDetailedDto> issues_ = mapper.Map<List<IssueDetailedDto>>(issues);
            return Ok(issues_);
        }

        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        [HttpGet("sprint/{sprintId?}")]
        public IActionResult GetBySprintId(int projectId, int? sprintId = null) {
            try {
                var issues = issueService.GetAllWithSprint(sprintId, projectId).ToList();
                return Ok(issues);
            } catch (Exception e) {
                throw e;
            }
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
        }

        [ServiceFilter(typeof(Filters.ProjectUrlBasedAuthorizationFilter))]
        [HttpPatch("{issueId}")]
        public async Task<IActionResult> PatchIssue([FromBody] JsonElement dto, int issueId, int projectId) {
            var issue = await issueService.FindAsync(i => i.Id == issueId && i.ProjectId == projectId);
            try {
                Helper.Mapper(dto, ref issue);
                await issueService.UpdateAsync(issue);
                return Ok(issue);
            } catch (Exception e) {
                throw e;
            }
        }

    }
}

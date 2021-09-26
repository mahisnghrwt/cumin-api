using AutoMapper;
using cumin_api.Attributes;
using cumin_api.Enums;
using cumin_api.Filters;
using cumin_api.Models;
using cumin_api.Models.DTOs;
using cumin_api.Models.Socket;
using cumin_api.Services.v2;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace cumin_api.Controllers {
    
    [ApiController]
    [Route("api/v1/project/{projectId}/roadmap/{roadmapId}/epic")]
    [CustomAuthorization]
    public class EpicController2: ControllerBase {

        private readonly EpicService epicService;
        private readonly IMapper mapper;
        public EpicController2(EpicService epicService, IMapper mapper) {
            this.epicService = epicService;
            this.mapper = mapper;
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [ServiceFilter(typeof(RoadmapAuthorizationFilter))]
        [HttpPost]
        public async Task<IActionResult> CreateEpic([FromBody] EpicCreationDto dto, int projectId, int roadmapId) {
            try {
                int uid = Convert.ToInt32(HttpContext.Items["userId"]);
                Epic epic = new Epic { StartDate = dto.StartDate, EndDate = dto.EndDate, Title = dto.Title, ProjectId = projectId, Color = dto.Color };
                var roadmapEpic = await epicService.AddToRoadmapAsync(epic, dto.Row, roadmapId);
                return Ok(mapper.Map<EpicDto>(roadmapEpic));
            } catch (Exception e) {
                throw e;
                return Unauthorized();
            }
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [ServiceFilter(typeof(RoadmapAuthorizationFilter))]
        [HttpDelete("{epicId}")]
        public async Task<IActionResult> DeleteEpic(int epicId, int roadmapId) {
            try {
                var success = await epicService.DeleteFromRoadmapAsync(epicId, roadmapId);
                if (!success) return BadRequest();
            } catch (Exception e) {
                throw e;
                return Unauthorized();
            }
            return Ok();
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [ServiceFilter(typeof(RoadmapAuthorizationFilter))]
        [HttpPatch("{epicId}")]
        public IActionResult PatchEpic(int epicId, int roadmapId, int projectId, [FromBody] EpicUpdateDto dto) {
            try {
                // if the epic is in the roadmap
                var epic = epicService.GetById(epicId);
                Helper.Mapper(dto, ref epic);
                // update
                var patched = epicService.Update(epic);

                return Ok(patched);
            } catch (Exception e) {
                throw e;
                return Unauthorized();
            }
        }

    }
}

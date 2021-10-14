using AutoMapper;
using cumin_api.Attributes;
using cumin_api.Enums;
using cumin_api.Filters;
using cumin_api.Models;
using cumin_api.Models.DTOs;
using cumin_api.Services.v2;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace cumin_api.Controllers {
    
    [ApiController]
    [Route("api/v1/project/{projectId}/epic")]
    [CustomAuthorization]
    public class EpicController: ControllerBase {

        private readonly EpicService epicService;
        private readonly IMapper mapper;
        public EpicController(EpicService epicService, IMapper mapper) {
            this.epicService = epicService;
            this.mapper = mapper;
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [HttpGet]
        public IActionResult GetEpics(int projectId) {
            return Ok(epicService.GetAllInProject(projectId));
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [HttpPost]
        public async Task<IActionResult> CreateEpic([FromBody] EpicCreationDto dto, int projectId) {
            try {
                Epic epic = new Epic { StartDate = dto.StartDate, EndDate = dto.EndDate, Title = dto.Title, ProjectId = projectId, Color = dto.Color, Row = dto.Row };
                var epicAdded = await epicService.AddToProjectAsync(epic, projectId);
                return Ok(epic);
            } catch (Exception e) {
                if (e is SimpleException) {
                    return BadRequest(e.Message);
                }
                throw e;
            }
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [HttpDelete("{epicId}")]
        public async Task<IActionResult> DeleteEpic(int epicId, int projectId) {
            try {
                await epicService.DeleteAsync(epicId, projectId);
            } catch (Exception e) {
                if (e is SimpleException) {
                    return BadRequest(e.Message);
                }
                throw e;
            }
            return Ok();
        }

        [ServiceFilter(typeof(ProjectUrlBasedAuthorizationFilter))]
        [ServiceFilter(typeof(RoleAuthorizationFilter))]
        [HttpPatch("{epicId}")]
        public async Task<IActionResult> PatchEpic(int epicId, int projectId, [FromBody] EpicUpdateDto dto) {
            try {
                var epic = await epicService.FindById(epicId, projectId);
                // Custom mapper, only map the requested properties of epic
                Helper.Mapper(dto, ref epic);
                await epicService.UpdateAsync(epic);
                return Ok(epic);
            } catch (Exception e) {
                throw e;
            }
        }

    }
}

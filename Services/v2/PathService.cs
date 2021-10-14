using cumin_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Services.v2 {
    public class PathService:DbService2<Path> {
        public PathService(CuminApiContext context) : base(context) { }

        public IEnumerable<Path> GetAllPathsInProject(int projectId) {
            return dbSet.Where(x => x.ProjectId == projectId).ToList();
        }

        public async Task<Path> AddToProjectAsync(Path path, int projectId) {
            var hasHeadEpic = await context.Epics.AnyAsync(e => e.Id == path.FromEpicId && e.ProjectId == projectId);
            var hasTailEpic = await context.Epics.AnyAsync(e => e.Id == path.ToEpicId && e.ProjectId == projectId);

            if (!hasHeadEpic || !hasTailEpic) {
                throw new SimpleException($"Endpoint epics do not exist.");
            }

            var path_ = await dbSet.AddAsync(path);
            await context.SaveChangesAsync();
            return path_.Entity;
        }

        public async Task DeleteFromProjectAsync(int pathId, int projectId) {
            var path = await dbSet.FirstOrDefaultAsync(p => p.Id == pathId && p.ProjectId == projectId);
            if (path == null) {
                throw new SimpleException($"Path not found. Id - {pathId}, ProjectId - {projectId}");
            }

            dbSet.Remove(path);
            await context.SaveChangesAsync();
        }
    }
}

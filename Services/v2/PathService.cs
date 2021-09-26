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
            return dbSet.Where(x => x.ProjectId == projectId);
        }

        public async Task<Path> AddToRoadmapAsync(Path path, int roadmapId) {
            // check if the epic exists in the roadmap
            var pathSaved = await dbSet.AddAsync(path);
            await pathSaved.Reference(p => p.FromEpic).LoadAsync();
            await pathSaved.Reference(p => p.ToEpic).LoadAsync();
            await context.Entry(pathSaved.Entity.FromEpic).Collection(e => e.RoadmapEpics).LoadAsync();
            await context.Entry(pathSaved.Entity.ToEpic).Collection(e => e.RoadmapEpics).LoadAsync();

            var valid = true;
            valid = valid && pathSaved.Entity.ToEpic.RoadmapEpics.Any(re => re.RoadmapId == roadmapId);
            valid = valid && pathSaved.Entity.FromEpic.RoadmapEpics.Any(re => re.RoadmapId == roadmapId);

            var roadmapPath = await context.RoadmapPaths.AddAsync(new RoadmapPath { Path = pathSaved.Entity, RoadmapId = roadmapId });

            await context.SaveChangesAsync();

            return pathSaved.Entity;
        }

        public async Task<bool> DeleteFromRoadmapAsync(int pathId, int roadmapId) {
            var roadmapPath = context.RoadmapPaths.FirstOrDefault(p => p.PathId == pathId && p.RoadmapId == roadmapId);
            if (roadmapPath == null) return false;

            context.RoadmapPaths.Remove(roadmapPath);

            var pathUsed = context.RoadmapPaths.Count(rp => rp.PathId == pathId);
            // if the path is only used in our roadmap
            if (pathUsed == 1) {
                await context.Entry(roadmapPath).Reference(rp => rp.Path).LoadAsync();
                dbSet.Remove(roadmapPath.Path);
            }

            await context.SaveChangesAsync();
            return true;
        }
    }
}

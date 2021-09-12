﻿using cumin_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Services.v2 {
    public class RoadmapService : DbService2<Roadmap> {
        public RoadmapService(CuminApiContext context) : base(context) { }

        public Roadmap GetFull(int roadmapId) {
            return dbSet
                .AsNoTracking()
                .Include(r => r.RoadmapEpics)
                .Include(r => r.RoadmapPaths)
                .FirstOrDefault(r => r.Id == roadmapId);
        }

        public Roadmap GetMainInProject(int projectId) {
            return dbSet
                .Include(r => r.RoadmapEpics)
                .Include(r => r.RoadmapPaths)
                .FirstOrDefault(r => r.ProjectId == projectId && r.CreatorId == null);
        }

        public async Task<Roadmap> CloneMain(int targetRoadmapId) {
            // get the target roadmap
            var targetRoadmap = FindById(targetRoadmapId);
            if (targetRoadmap == null) return null;

            int projectId = targetRoadmap.ProjectId;

            var mainRoadmap = GetMainInProject(projectId);
            if (mainRoadmap == null) return null;

            return await Clone(targetRoadmapId, mainRoadmap.Id, projectId);
        }

        // Not sure if projectID is necessary :/
        public async Task<Roadmap> Clone(int targetRoadmapId, int sourceRoadmapId, int projectId) {
            // get the target roadmap
            var targetRoadmap = Find(r => r.Id == targetRoadmapId && r.ProjectId == projectId);
            if (targetRoadmap == null) return null;

            // delete current epics and paths in target roadmap
            // deleting this way, to include it in Transaction
            foreach (RoadmapEpic roadmapEpic in targetRoadmap.RoadmapEpics) {
                context.Entry(roadmapEpic).State = EntityState.Deleted;
            }
            foreach (RoadmapPath roadmapPath in targetRoadmap.RoadmapPaths) {
                context.Entry(roadmapPath).State = EntityState.Deleted;
            }

            Roadmap sourceRoadmap = Find(r => r.Id == sourceRoadmapId && r.ProjectId == projectId);
            if (sourceRoadmap == null) return null;

            // clone all epics
            foreach (RoadmapEpic roadmapEpic in sourceRoadmap.RoadmapEpics) {
                context.RoadmapEpics.Add(new RoadmapEpic { RoadmapId = targetRoadmapId, EpicId = roadmapEpic.EpicId });
            }

            // clone all paths
            foreach (RoadmapPath roadmapPath in sourceRoadmap.RoadmapPaths) {
                context.RoadmapPaths.Add(new RoadmapPath { RoadmapId = targetRoadmapId, PathId = roadmapPath.PathId });
            }

            await context.SaveChangesAsync();

            return targetRoadmap;
        }

    }
}
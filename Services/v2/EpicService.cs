using cumin_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Services.v2 {
    public class EpicService : DbService<Epic> {
        public EpicService(CuminApiContext context) : base(context) { }

        public Epic GetById(int id) {
            return dbSet.FirstOrDefault(x => x.Id == id);
        }

        public Epic GetDetailedById(int id) {
            return dbSet
                .Include(x => x.RoadmapEpics)
                .FirstOrDefault(x => x.Id == id);
        }

        public async Task<bool> DeleteFromRoadmapAsync(int epicId, int roadmapId) {
            // if this is the only roadmap using this epic, then remove the epic along with the roadmap-epic relationship.
            var roadmapEpics = await context.RoadmapEpics.Include(re => re.Epic).Where(re => re.EpicId == epicId).ToListAsync();
            if (roadmapEpics.Count == 0) {
                return false;
            } else {
                // check if the roadmap has that epic
                var roadmapEpic = roadmapEpics.FirstOrDefault(re => re.RoadmapId == roadmapId);
                if (roadmapEpic == null) return false;
                // remove the roadmap-epic relationship
                context.RoadmapEpics.Remove(roadmapEpic);

                // pull all epics 1 row up, we do not want to leave any empty rows
                var targetRow = roadmapEpic.Row;
                var epics = await context.RoadmapEpics.Where(re => re.RoadmapId == roadmapId).ToListAsync();
                foreach(var epic in epics) {
                    if (epic.Row > targetRow)
                        epic.Row--;
                }

                // decrement the total rows in roadmap
                var roadmap = context.Roadmaps.FirstOrDefault(r => r.Id == roadmapId);
                roadmap.Rows--;

                if (roadmapEpics.Count == 1) {
                    dbSet.Remove(roadmapEpics[0].Epic);
                }

                await context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<RoadmapEpic> AddToRoadmapAsync(Epic epic, int row, int roadmapId) {
            var roadmap = context.Roadmaps.FirstOrDefault(r => r.Id == roadmapId);
            // check if the row is valid
            if (roadmap.Rows != row) return null;

            // rows used in roadmap
            roadmap.Rows++;

            var epicT = dbSet.Add(epic);
            var re = context.RoadmapEpics.Add(new RoadmapEpic { Epic = epicT.Entity, RoadmapId = roadmapId, Row = row});
            await context.SaveChangesAsync();

            return re.Entity;
        }

        public IEnumerable<Epic> GetAllInProject(int pid) {
            return dbSet.Where(x => x.ProjectId == pid);
        }
    }
}
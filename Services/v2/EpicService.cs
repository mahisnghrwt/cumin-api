using cumin_api.Models;
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

        public async Task<Epic> AddToRoadmap(Epic epic, int roadmapId) {
            var epicT = dbSet.Add(epic);
            context.RoadmapEpics.Add(new RoadmapEpic { EpicId = epicT.Entity.Id, RoadmapId = roadmapId });
            await context.SaveChangesAsync();

            return epicT.Entity;
        }

        public IEnumerable<Epic> GetAllInProject(int pid) {
            return dbSet.Where(x => x.ProjectId == pid);
        }
    }
}

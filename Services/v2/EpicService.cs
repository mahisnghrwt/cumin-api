using cumin_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cumin_api.Services.v2 {
    public class EpicService : DbService2<Epic> {
        public EpicService(CuminApiContext context) : base(context) { }

        public async Task<Epic> FindById(int id, int projectId) {
            return await dbSet.FirstOrDefaultAsync(x => x.Id == id && x.ProjectId == projectId);
        }
        
        public async Task DeleteAsync(int epicId, int projectId) {
            // find the epic with id and projectId
            var epic = await dbSet.FirstOrDefaultAsync(e => e.Id == epicId && e.ProjectId == projectId);
            if (epic == null) {
                throw new SimpleException($"Epic not found in the project. EpicId = {epicId}, ProjectId = {projectId}");
            }
            // pull epics below this row up by 1 row
            // So we do not have any empty rows in between in canvas
            int thresholdRow = epic.Row;
            var epics = dbSet.Where(e => e.Row > thresholdRow);
            await epics.ForEachAsync(e => e.Row--);

            dbSet.Remove(epic);
            await context.SaveChangesAsync();
        }

        public async Task<Epic> AddToProjectAsync(Epic epic, int projectId) {
            // (1-indexed)
            int rowsOccupied = await context.Epics.CountAsync(e => e.ProjectId == projectId);
            // check if is epic added to last row
            if (epic.Row != rowsOccupied) {
                throw new SimpleException($"Epic must be placed into last row only. Last row - {rowsOccupied}, Requested row - {epic.Row}");
            }

            var epicT = dbSet.Add(epic);
            await context.SaveChangesAsync();

            return epicT.Entity;
        }

        public IEnumerable<Epic> GetAllInProject(int pid) {
            return dbSet.Include(e => e.Issues).Where(x => x.ProjectId == pid).ToList();
        }
    }
}
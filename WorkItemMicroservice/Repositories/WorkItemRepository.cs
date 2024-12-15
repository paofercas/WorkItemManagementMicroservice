using WorkItemMicroservice.Data;
using WorkItemMicroservice.Models;

namespace WorkItemMicroservice.Repositories
{
    public class WorkItemRepository : IWorkItemRepository
    {
        private readonly WorkItemDbContext _context;

        public WorkItemRepository(WorkItemDbContext context)
        {
            _context = context;
        }

        public IEnumerable<WorkItem> GetAllWorkItems()
        {
            return _context.WorkItems.ToList();
        }

        public WorkItem GetWorkItemById(int id)
        {
            return _context.WorkItems.FirstOrDefault(w => w.Id == id);
        }

        public void AddWorkItem(WorkItem workItem)
        {

            _context.WorkItems.Add(workItem);
            _context.SaveChanges();

        }


        public void UpdateWorkItem(WorkItem workItem)
        {
            var existing = GetWorkItemById(workItem.Id);
            if (existing != null)
            {
                existing.Description = workItem.Description;
                existing.Relevance = workItem.Relevance;
                existing.DueDate = workItem.DueDate;
                existing.AssignedUser = workItem.AssignedUser;
                existing.Status = workItem.Status;
                _context.SaveChanges();
            }
        }

        public void DeleteWorkItem(int id)
        {
            var workItem = GetWorkItemById(id);
            if (workItem != null)
            {
                _context.WorkItems.Remove(workItem);
                _context.SaveChanges();
            }
        }
    }
}

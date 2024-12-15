using System.Collections.Generic;
using WorkItemMicroservice.Models;

namespace WorkItemMicroservice.Repositories
{
    public interface IWorkItemRepository
    {
        IEnumerable<WorkItem> GetAllWorkItems();
        WorkItem GetWorkItemById(int id);
        void AddWorkItem(WorkItem workItem);
        void UpdateWorkItem(WorkItem workItem);
        void DeleteWorkItem(int id);
    }
}

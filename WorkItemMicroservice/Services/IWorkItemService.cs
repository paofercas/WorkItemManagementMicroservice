using System.Collections.Generic;
using System.Threading.Tasks;
using WorkItemMicroservice.Models;

namespace WorkItemMicroservice.Services
{
    public interface IWorkItemService
    {
        IEnumerable<WorkItem> GetAllWorkItems();
        WorkItem GetWorkItemById(int id);
        Task<WorkItem> CreateWorkItemAsync(WorkItem workItem);
        void UpdateWorkItem(WorkItem workItem);
        void DeleteWorkItem(int id);
    }
}

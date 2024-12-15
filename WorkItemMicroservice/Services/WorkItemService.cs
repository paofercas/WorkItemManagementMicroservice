using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WorkItemMicroservice.Models;
using WorkItemMicroservice.Repositories;

namespace WorkItemMicroservice.Services
{
    public class WorkItemService : IWorkItemService
    {
        private readonly IWorkItemRepository _workItemRepository;
        private readonly HttpClient _httpClient;
        private readonly string _userServiceUrl = "https://localhost:7149/api/users"; //Change the port number according to your localhost

        public WorkItemService(IWorkItemRepository workItemRepository, IHttpClientFactory httpClientFactory)
        {
            _workItemRepository = workItemRepository;
            _httpClient = httpClientFactory.CreateClient();
        }

        public IEnumerable<WorkItem> GetAllWorkItems()
        {
            return _workItemRepository.GetAllWorkItems();
        }

        public WorkItem GetWorkItemById(int id)
        {
            return _workItemRepository.GetWorkItemById(id);
        }

        public async Task<WorkItem> CreateWorkItemAsync(WorkItem workItem)
        {


            var assignedUser = await AssignUserAsync(workItem);
            workItem.AssignedUser = assignedUser;
            workItem.Status = Status.Pending;
            _workItemRepository.AddWorkItem(workItem);
            await UpdateUserItemCountsAsync(assignedUser, workItem.Relevance, true);
            return workItem;
        }


        public void UpdateWorkItem(WorkItem workItem)
        {
            _workItemRepository.UpdateWorkItem(workItem);
        }

        public void DeleteWorkItem(int id)
        {
            var workItem = _workItemRepository.GetWorkItemById(id);
            if (workItem != null)
            {
                Task.Run(() => UpdateUserItemCountsAsync(workItem.AssignedUser, workItem.Relevance, false)).Wait();
                _workItemRepository.DeleteWorkItem(id);
            }
        }

        private async Task<string> AssignUserAsync(WorkItem workItem)
        {
            var users = await _httpClient.GetFromJsonAsync<IEnumerable<UserDto>>(_userServiceUrl);
            if (users == null || !users.Any())
                throw new Exception("No users available for assignment.");

            DateTime today = DateTime.Today;
            bool isDueSoon = (workItem.DueDate - today).TotalDays < 3;

            if (isDueSoon)
            {
                var availableUsers = users
                    .Where(u => u.HighRelevanceCount < 4)
                    .OrderBy(u => u.PendingItemsCount)
                    .ToList();
                var user = availableUsers.FirstOrDefault();
                if (user != null)
                    return user.Username;
            }
            else
            {
                if (workItem.Relevance == Relevance.High)
                {
                    var availableUsers = users
                        .Where(u => u.HighRelevanceCount < 4)
                        .OrderBy(u => u.PendingItemsCount)
                        .ToList();
                    var user = availableUsers.FirstOrDefault();
                    if (user != null)
                        return user.Username;
                }
                else
                {
                    var availableUsers = users
                        .Where(u => u.HighRelevanceCount < 4)
                        .OrderBy(u => u.PendingItemsCount)
                        .ToList();
                    var user = availableUsers.FirstOrDefault();
                    if (user != null)
                        return user.Username;
                }
            }

            throw new Exception("No suitable user found for assignment.");
        }

        private async Task UpdateUserItemCountsAsync(string username, Relevance relevance, bool increment)
        {
            var user = await _httpClient.GetFromJsonAsync<UserDto>($"{_userServiceUrl}/{username}");
            if (user != null)
            {
                if (increment)
                {
                    user.PendingItemsCount += 1;
                    if (relevance == Relevance.High)
                        user.HighRelevanceCount += 1;
                }
                else
                {
                    user.PendingItemsCount -= 1;
                    if (relevance == Relevance.High)
                        user.HighRelevanceCount -= 1;
                }
                var response = await _httpClient.PutAsJsonAsync($"{_userServiceUrl}/{username}", user);
                response.EnsureSuccessStatusCode();
            }
        }
    }

    public class UserDto
    {
        public string Username { get; set; }
        public int HighRelevanceCount { get; set; }
        public int PendingItemsCount { get; set; }
    }
}

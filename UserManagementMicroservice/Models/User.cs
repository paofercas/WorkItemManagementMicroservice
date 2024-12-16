namespace UserManagementMicroservice.Models
{
    public class User
    {
        public string? Username { get; set; }
        public int HighRelevanceCount { get; set; }
        public int PendingItemsCount { get; set; }
    }
}



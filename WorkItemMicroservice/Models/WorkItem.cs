using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WorkItemMicroservice.Models
{
    public enum Relevance
    {
        Low,
        High
    }

    public enum Status
    {
        Pending,
        Completed
    }

    public class WorkItem
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Relevance is mandatory")]
        public Relevance Relevance { get; set; }

        [Required(ErrorMessage = "The expiration date is mandatory")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "The assigned user is mandatory")]
        public string AssignedUser { get; set; }

        public Status Status { get; set; }
    }

}

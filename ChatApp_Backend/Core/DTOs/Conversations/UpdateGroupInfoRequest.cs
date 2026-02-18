using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Conversations
{
    public class UpdateGroupInfoRequest
    {
        [StringLength(100, ErrorMessage = "Group name cannot exceed 100 characters")]
        public string? GroupName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Range(0, 3600, ErrorMessage = "Slow mode must be between 0 and 3600 seconds")]
        public int? SlowMode { get; set; }
    }
}

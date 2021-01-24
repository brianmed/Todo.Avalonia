using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Todo.Entities
{
    [Index(nameof(DayCreated))]
    public class TodoEntity
    {
        public long TodoEntityId { get; set; }

        public string? Title { get; set; }

        [Required]
        public int DayCreated { get; set; }

        public bool IsDone { get; set; }
    
        [DefaultValue(typeof(DateTime), "")]        
        public DateTime Updated { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Inserted { get; set; } = DateTime.UtcNow;
    }
}
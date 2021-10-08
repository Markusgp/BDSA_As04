using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment4.Core
{
    public class TagDTO
    {
        public int Id {get;set;}

        [Required]
        [StringLength(50)]
        public string Name {get;set;}

        public ICollection<TaskDTO> Tasks {get;set;}
    }
}
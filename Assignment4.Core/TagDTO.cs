using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment4.Core
{
    public record TagDTO(int Id, string Name, IReadOnlyCollection<TaskDTO> Tasks);
    public record TagCreateDTO
    {
        [Required]
        [StringLength(50)]
        public string Name {get;set;}

        public ICollection<TaskDTO> Tasks {get;set;}
    }

    public record TagUpdateDTO : TagCreateDTO
    {
        public int Id { get; init; }
    }
}
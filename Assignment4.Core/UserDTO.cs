using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment4.Core
{
    public record UserDTO(int Id, string Name, string Email, IReadOnlyCollection<TaskDTO> Tasks);
    public record UserCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Name {get;set;}

        [Required]
        public string Email {get;set;}

        public ICollection<TaskDTO> Tasks {get;set;}
    }

    public record UserUpdateDTO : TagCreateDTO
    {
        public int Id { get; init; }
    }
}
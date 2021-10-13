using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment4.Core
{
    public record UserDTO(int Id, string Name, string Email);
    public record UserCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Name {get;set;}


        [EmailAddress]
        [Required]
        [StringLength(50)]
        public string Email {get;set;}

        public ICollection<TaskDTO> Tasks {get;set;}
    }

    public record UserUpdateDTO : UserCreateDTO
    {
        public int Id { get; init; }
    }
}
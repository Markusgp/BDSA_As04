using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment4.Core
{
    public record TaskDTO(int Id, string Title, string Description, int? AssignedToId, ICollection<string> Tags, State State);

    public record TaskDetailsDTO(int Id, string Title, int? AssignedToId, string AssignedToName, string AssignedToEmail, string Description, State State, ICollection<string> Tags) : TaskDTO(Id, Title, Description, AssignedToId, Tags, State);

    public record TaskCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Title {get;set;}

        public int? AssignedToId {get;set;}

        public string AssignedToName { get; init; }

        public string AssignedToEmail { get; init; }

        public string Description {get;set;}

        [Required]
        public State State {get;set;}
        
        public DateTime StateUpdated {get;set;}

        public ICollection<string> Tags {get;set;}
    }

    public record TaskUpdateDTO : TaskCreateDTO
    {
        public int Id { get; init; }
    }
}
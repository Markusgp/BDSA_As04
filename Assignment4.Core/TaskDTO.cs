using System.Collections.Generic;

namespace Assignment4.Core
{
    public record TaskDTO
    {
        public TaskDTO(int id, string title, string description, int? assignedToId, IReadOnlyCollection<TagDTO> tags, State state)
        {
            Id = id;
            Title = title;
            Description = description;
            AssignedToId = assignedToId;
            Tags = tags;
            State = state;
        }

        public int Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public int? AssignedToId { get; init; }
        public IReadOnlyCollection<TagDTO> Tags { get; init; }
        public State State { get; init; }
    }
}
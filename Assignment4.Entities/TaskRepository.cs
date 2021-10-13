using Assignment4.Core;
using System.Collections.Generic;
using System.Data;
using System;
using System.Linq;

namespace Assignment4.Entities
{
    public class TaskRepository : ITaskRepository
    {
        private readonly KanbanContext _connection;
        //private bool disposedValue;

        public TaskRepository(KanbanContext connection)
        {
            _connection = connection;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public (Response Response, int TaskId) Create(TaskCreateDTO task)
        {
            var assignedUser = GetUser(task.AssignedToId);
            if (assignedUser == null) return (Response.BadRequest, -1);
            
            var entity = new Task
            {
                Title = task.Title,
                Description = task.Description,
                AssignedTo = assignedUser,
                Tags = GetTagsFromString(task.Tags).ToList(),
                State = task.State
            };
            entity.State = State.New;
            entity.StateUpdated = DateTime.UtcNow;
            // set to e.g. 2 am if this is untestable

            _connection.Tasks.Add(entity);
            _connection.SaveChanges();

            return (Response.Created,entity.Id);
        }

        public IReadOnlyCollection<TaskDTO> ReadAll()
        {
            return _connection.Tasks
                    .Select(c => new TaskDTO(c.Id, c.Title, c.Description, c.AssignedTo.Id, GetTags(c.Tags).ToList(), c.State))
                    .ToList().AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {
            return _connection.Tasks
                    .Where(c => c.State == State.Removed)
                    .Select(c => new TaskDTO(c.Id, c.Title, c.Description, c.AssignedTo.Id, GetTags(c.Tags).ToList(), c.State))
                    .ToList().AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            return _connection.Tasks
                    .Where(c => c.Tags.Contains(GetTag(tag)))
                    .Select(c => new TaskDTO(c.Id, c.Title, c.Description, c.AssignedTo.Id, GetTags(c.Tags).ToList(), c.State))
                    .ToList().AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
        {
            return _connection.Tasks
                    .Where(c => c.AssignedTo.Id == userId)
                    .Select(c => new TaskDTO(c.Id, c.Title, c.Description, c.AssignedTo.Id, GetTags(c.Tags).ToList(), c.State))
                    .ToList().AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
        {
            return _connection.Tasks
                    .Where(c => c.State == state)
                    .Select(c => new TaskDTO(c.Id, c.Title, c.Description, c.AssignedTo.Id, GetTags(c.Tags).ToList(), c.State))
                    .ToList().AsReadOnly();
        }

        public TaskDetailsDTO Read(int taskId)
        {
            var tasks = from c in _connection.Tasks
                         where c.Id == taskId
                         select new TaskDTO(c.Id, c.Title, c.Description, c.AssignedTo.Id, GetTags(c.Tags).ToList(), c.State);

            var task = tasks.FirstOrDefault();

            return new TaskDetailsDTO
            (
                taskId,
                task.Title,
                task.AssignedToId,
                GetUser(task.AssignedToId).Name,
                GetUser(task.AssignedToId).Email,
                task.Description,
                task.State,
                task.Tags
            );
        }

        public Response Update(TaskUpdateDTO task)
        {
            var entity = _connection.Tasks.Find(task.Id);

            if (entity == null)
            {
                return Response.NotFound;
            }

            entity.Title = task.Title;
            entity.Description = task.Description;
            entity.AssignedTo = GetUser(task.AssignedToId);
            entity.Tags = GetTagsFromString(task.Tags).ToList();
            entity.State = task.State;
            entity.StateUpdated = DateTime.UtcNow;

            _connection.SaveChanges();

            return Response.Updated;
        }

        public Response Delete(int taskId)
        {
            var entity = _connection.Tasks.Find(taskId);

            if (entity == null)
            {
                return Response.NotFound;
            }

            switch (entity.State)
            {
                case State.New: {
                    _connection.Tasks.Remove(entity);
                    _connection.SaveChanges();
                }
                    return Response.Deleted;
                case State.Active: entity.State = State.Removed;
                    return Response.Updated;
                case State.Resolved:
                    return Response.Conflict;
                case State.Closed:
                    return Response.Conflict;
                case State.Removed:
                    return Response.Conflict;
                default:
                    return Response.BadRequest;
            }
        }




        private User GetUser(int? assignedToId)
        {
            if (assignedToId == null) return null;
            return _connection.Users.FirstOrDefault(c => c.Id == assignedToId) ??
            new User { Id = (int) assignedToId };
        }

        private IEnumerable<Tag> GetTagsFromString(ICollection<string> tags)
        {
            foreach (var item in tags)
            {
                var tag = _connection.Tags
                    .Where(t => t.Name == item)
                    .FirstOrDefault();

                yield return tag;
            }
            
        }

        private IEnumerable<string> GetTags(ICollection<Tag> tags) 
        {
            foreach (var item in tags)
            {
                var tag = _connection.Tags
                    .Where(t => t.Name == item.Name)
                    .FirstOrDefault();

                yield return tag.Name;
            }
        }

        private Tag GetTag(string tag)
        {
            return _connection.Tags.FirstOrDefault(c => c.Name == tag) ??
            new Tag { Name = tag };
        }
    }
}
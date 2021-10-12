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
        private bool disposedValue;

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
            var entity = new Task
            {
                Title = task.Title,
                Description = task.Description,
                AssignedTo = GetUser(task.AssignedToId),
                Tags = GetTags(task.Tags),
                State = task.State
            };

            _connection.Tasks.Add(entity);

            _connection.SaveChanges();

            return (Response.Created,entity.Id);
        }

        public IReadOnlyCollection<TaskDTO> ReadAll()
        {
            return _connection.Tasks
                    .Select(c => new TaskDTO(c.Id, c.Title, c.Description, c.AssignedTo.Id, GetTags(c.Tags), c.State))
                    .ToList().AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            return _connection.Tasks
                    .Where(c => c.Tags.Contains(GetTag(tag)))
                    .Select(c => new TaskDTO(c.Id, c.Title, c.Description, c.AssignedTo.Id, GetTags(c.Tags), c.State))
                    .ToList().AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
        {
            return _connection.Tasks
                    .Where(c => c.AssignedTo.Id == userId)
                    .Select(c => new TaskDTO(c.Id, c.Title, c.Description, c.AssignedTo.Id, GetTags(c.Tags), c.State))
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
                         select new TaskDTO(c.Id, c.Title, c.Description, c.AssignedTo.Id, GetTags(c.Tags), c.State);

            var task = tasks.FirstOrDefault();

            return new TaskDetailsDTO
            {
                Id = taskId,
                Title = task.Title,
                AssignedToId = task.AssignedToId,
                AssignedToName = GetUser(task.AssignedToId).Name,
                AssignedToEmail = GetUser(task.AssignedToId).Email,
                Description = task.Description,
                State = task.State,
                Tags = GetTags(task.Tags).ToList()
            };
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
            entity.Tags = GetTags(task.Tags);
            entity.State = task.State;

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

            _connection.Tasks.Remove(entity);
            _connection.SaveChanges();

            return Response.Deleted;
        }




        private User GetUser(int? assignedToId)
        {
            if (assignedToId == null) return null;
            return _connection.Users.FirstOrDefault(c => c.Id == assignedToId) ??
            new User { Id = (int) assignedToId };
        }

        private IEnumerable<TagDTO> GetTags(ICollection<string> tags)
        {
            foreach (var item in tags)
            {
                var tag = _connection.Tags
                    .Where(t => t.Name == item)
                    .FirstOrDefault();

                yield return new TagDTO
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Tasks = tag.Tasks
                };
            }
            
        }

        private Tag GetTag(string tag)
        {
            return _connection.Tags.FirstOrDefault(c => c.Name == tag) ??
            new Tag { Name = tag };
        }
    }
}
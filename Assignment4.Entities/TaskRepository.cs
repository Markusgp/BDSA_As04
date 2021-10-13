using Assignment4.Core;
using System.Collections.Generic;
using System.Data;
using System;
using System.Linq;
using static Assignment4.Core.Response;
using static Assignment4.Core.State;

namespace Assignment4.Entities.Tests
{
    public class TaskRepository : ITaskRepository
    {
        private readonly KanbanContext _connection;
        //private bool disposedValue;

        public TaskRepository(KanbanContext connection)
        {
            _connection = connection;
        }

        public (Response Response, int TaskId) Create(TaskCreateDTO task)
        {
            var assignedUser = GetUser(task.AssignedToId);
            if (assignedUser == null) return (BadRequest, -1);
            
            var entity = new Task
            {
                Title = task.Title,
                Description = task.Description,
                AssignedTo = assignedUser,
                Created = DateTime.Now,
                StateUpdated = DateTime.Now,
                Tags = new List<Tag>(),
                State = New
            };

            _connection.Tasks.Add(entity);
            _connection.SaveChanges();

            return (Created, entity.Id);
        }

        public IReadOnlyCollection<TaskDTO> ReadAll()
        {
            var tasks = from t in _connection.Tasks
                        select new TaskDTO(t.Id, 
                                           t.Title, 
                                           t.AssignedTo.Name,
                                           (from tag in t.Tags select tag.Name).ToList().AsReadOnly(),
                                           t.State);

            return tasks.ToList().AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {
            var tasks = from t in _connection.Tasks
                        where t.State == Removed
                        select new TaskDTO(t.Id, 
                                           t.Title, 
                                           t.AssignedTo.Name,
                                           (from tag in t.Tags select tag.Name).ToList().AsReadOnly(),
                                           t.State);

            return tasks.ToList().AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag) =>
            _connection.Tasks.Where(t => t.Tags.Select(ta => ta.Name)
                                            .Contains(tag))
                          .Select(t => new TaskDTO(t.Id, 
                                                   t.Title, 
                                                   t.AssignedTo.Name,
                                                   (from tag in t.Tags select tag.Name).ToList().AsReadOnly(),
                                                   t.State))
                          .ToList()
                          .AsReadOnly();

        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
        {
            var tasks = from t in 
            _connection.Tasks
                        where t.AssignedTo.Id == userId
                        select new TaskDTO(t.Id, 
                                           t.Title, 
                                           t.AssignedTo.Name,
                                           (from tag in t.Tags select tag.Name).ToList().AsReadOnly(),
                                           t.State);

            return tasks.ToList().AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
        {
            var tasks = from t in _connection.Tasks
                        where t.State == state
                        select new TaskDTO(t.Id, 
                                           t.Title, 
                                           t.AssignedTo.Name,
                                           (from tag in t.Tags select tag.Name).ToList().AsReadOnly(),
                                           t.State);

            return tasks.ToList().AsReadOnly();
        }

        public TaskDetailsDTO Read(int taskId)
        {
            var tasks = from t in _connection.Tasks
                        where t.Id == taskId
                        select new TaskDetailsDTO(t.Id, 
                                                  t.Title, 
                                                  t.Description,
                                                  t.Created,
                                                  t.AssignedTo.Name,
                                                  (from tag in t.Tags select tag.Name).ToList().AsReadOnly(),
                                                  t.State,
                                                  t.StateUpdated);
            
            return tasks.FirstOrDefault();
        }

        public Response Update(TaskUpdateDTO task)
        {
            var entity = _connection.Tasks.Find(task.Id);

            if (entity == null)
            {
                return NotFound;
            }

            if (task.AssignedToId != null && GetUser(task.AssignedToId) == null)
            {
                return BadRequest;
            }

            if (task.Title != null)         entity.Title = task.Title;
            if (task.AssignedToId != null)  entity.AssignedTo = GetUser(task.AssignedToId);
            if (task.Description != null)   entity.Description = task.Description;
            if (entity.State != task.State) entity.StateUpdated = DateTime.Now;
            entity.State = task.State;

            if (task.Tags != null) 
            {
                foreach (string tagName in task.Tags)
                {
                    var tag = GetTag(tagName);
                    if (tag == null) 
                    {
                        tag = new Tag { Name = tagName };
                        _connection.Tags.Add(tag);
                    }
                    entity.Tags.Add(tag);
                }
            }          
            _connection.SaveChanges();

            return Updated;
        }

        public Response Delete(int taskId)
        {
            var entity = _connection.Tasks.Find(taskId);

            if (entity == null)
            {
                return NotFound;
            }
        
            if (entity.State == New) 
            {
                _connection.Tasks.Remove(entity);
                _connection.SaveChanges();

                return Deleted;
            }

            if (entity.State == Active) {
                entity.StateUpdated = DateTime.Now;
                entity.State = Removed;

                return Deleted;
            }

            return Conflict;  
        }




        private User GetUser(int? assignedToId)
        {
            if (assignedToId == null) return null;
            var existing = _connection.Users.Where(u => u.Id == assignedToId).Select(u => u);
            return existing.FirstOrDefault();
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

        private Tag GetTag(string tag)
        {
            if (tag == null) return null;
            var existing = _connection.Tags.Where(t => t.Name == tag).Select(t => t);
            return existing.FirstOrDefault();
        }
    }
}
using System.Collections.Generic;
using Assignment4.Core;
using System.Data;
using System;
using System.Linq;


namespace Assignment4.Entities
{
    public class TagRepository : ITagRepository
    {
        private readonly KanbanContext _connection;
        private bool disposedValue;

        public TagRepository(KanbanContext connection)
        {
            _connection = connection;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
        public (Response Response, int TagId) Create(TagCreateDTO tag)
        {
            var entity = new Tag
            {
                Name = tag.Name,
                Tasks = GetTasksToTask(tag.Tasks).ToList()
            };

            _connection.Tags.Add(entity);

            _connection.SaveChanges();

            return (Response.Created,entity.Id);
        }

        public TagDTO Read(int tagId)
        {
            var tags = from c in _connection.Tags
                         where c.Id == tagId
                         select new TagDTO(c.Id, c.Name, GetTasksToDTO(c.Tasks).ToList());

            var tag = tags.FirstOrDefault();

            return new TagDTO
            (
                tagId,
                tag.Name,
                tag.Tasks
            );
        }

        public IReadOnlyCollection<TagDTO> ReadAll()
        {
             return _connection.Tags
                    .Select(c => new TagDTO(c.Id, c.Name, GetTasksToDTO(c.Tasks).ToList()))
                    .ToList().AsReadOnly();
        }

        public Response Update(TagUpdateDTO tag)
        {
            var entity = _connection.Tags.Find(tag.Id);

            if (entity == null)
            {
                return Response.NotFound;
            }

            entity.Name = tag.Name;
            entity.Tasks = GetTasksToTask(tag.Tasks).ToList();

            _connection.SaveChanges();

            return Response.Updated;
        }

        public Response Delete(int tagId, bool force = false)
        {
            var entity = _connection.Tags.Find(tagId);

            if (entity == null)
            {
                return Response.NotFound;
            }

            _connection.Tags.Remove(entity);
            _connection.SaveChanges();

            return Response.Deleted;
        }


        private IEnumerable<TaskDTO> GetTasksToDTO(IEnumerable<Task> tasks) {
            foreach (var item in tasks)
            {
                var task = _connection.Tasks
                    .Where(t => t.Id == item.Id)
                    .FirstOrDefault();

                yield return new TaskDTO
                (
                    task.Id,
                    task.Title,
                    task.Description,
                    task.AssignedTo.Id,
                    task.Tags.Select(t => t.Name).ToList(),
                    task.State
                );
            }
        }

        private IEnumerable<Task> GetTasksToTask(IEnumerable<TaskDTO> tasks) {
            foreach (var item in tasks)
            {
                var task = _connection.Tasks
                    .Where(t => t.Id == item.Id)
                    .FirstOrDefault();
                yield return task;
            }
        }
    }
}
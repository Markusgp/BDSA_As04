using System.Collections.Generic;
using Assignment4.Core;
using System.Data;
using System;
using System.Linq;
using static Assignment4.Core.Response;


namespace Assignment4.Entities
{
    public class TagRepository : ITagRepository
    {
        private readonly KanbanContext _connection;

        public TagRepository(KanbanContext connection)
        {
            _connection = connection;
        }

        public (Response Response, int TagId) Create(TagCreateDTO tag)
        {
            var sameName = from t in _connection.Tags
                           where t.Name == tag.Name
                           select t.Id;

            if (sameName.Count() != 0) 
            {
                return (Conflict, sameName.First());
            }

            var entity = new Tag { Name = tag.Name };
            _connection.Tags.Add(entity);
            _connection.SaveChanges();
            return (Created, entity.Id);
        }

        public IReadOnlyCollection<TagDTO> ReadAll()
        {
            var tags = from t in _connection.Tags
                       select new TagDTO(t.Id, t.Name);

            return tags.ToList().AsReadOnly();
        }

        public TagDTO Read(int tagId)
        {
            var tags = from c in _connection.Tags
                         where c.Id == tagId
                         select new TagDTO(c.Id, c.Name);

            return tags.FirstOrDefault();
        }

        public Response Update(TagUpdateDTO tag)
        {
            var entity = _connection.Tags.Find(tag.Id);

            if (entity == null)
            {
                return NotFound;
            }

            entity.Name = tag.Name;
            _connection.SaveChanges();

            return Updated;
        }

        public Response Delete(int tagId, bool force = false)
        {
            var entity = _connection.Tags.Find(tagId);
            
            if (entity == null)
            {
                return NotFound;
            }

            if (entity.Tasks.Count != 0 && !force) return Conflict;

            _connection.Tags.Remove(entity);
            _connection.SaveChanges();

            return Deleted;
        }
    }
}
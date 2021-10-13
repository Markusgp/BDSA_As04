using System.Collections.Generic;
using Assignment4.Core;
using System.Data;
using System;
using System.Linq;


namespace Assignment4.Entities
{
    /*
    public class UserRepository : IUserRepository
    {
        private readonly KanbanContext _connection;
        private bool disposedValue;

        public UserRepository(KanbanContext connection)
        {
            _connection = connection;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public (Response Response, int UserId) Create(UserCreateDTO user)
        {
            var entity = new User
            {
                Name = user.Name,
                Email = user.Email,
                Tasks = GetTasksToTask(user.Tasks).ToList()
            };

            if (Enumerable.Any(_connection.Users, u => u.Email == entity.Email))
                return (Response.Conflict, -1);

            _connection.Users.Add(entity);
            _connection.SaveChanges();

            return (Response.Created,entity.Id);
        }

        public UserDTO Read(int userId)
        {
            var users = from c in _connection.Users
                         where c.Id == userId
                         select new UserDTO(c.Id, c.Name, c.Email, GetTasksToDTO(c.Tasks).ToList());

            var user = users.FirstOrDefault();

            return new UserDTO
            (
                userId,
                user.Name,
                user.Email,
                user.Tasks
            );
        }

        public IReadOnlyCollection<UserDTO> ReadAll()
        {
            return _connection.Users
                    .Select(c => new UserDTO(c.Id, c.Name, c.Email, GetTasksToDTO(c.Tasks).ToList()))
                    .ToList().AsReadOnly();
        }

        public Response Update(UserUpdateDTO user)
        {
            var entity = _connection.Users.Find(user.Id);

            if (entity == null)
            {
                return Response.NotFound;
            }

            entity.Name = user.Name;
            entity.Email = user.Email;
            entity.Tasks = GetTasksToTask(user.Tasks).ToList();

            _connection.SaveChanges();

            return Response.Updated;
        }

        public Response Delete(int userId, bool force = false)
        {
            var entity = _connection.Users.Find(userId);

            if (entity == null)
            {
                return Response.NotFound;
            }

            if (entity.Tasks.Count != 0 && !force) return Response.Conflict;
            
            _connection.Users.Remove(entity);
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
    }*/
}
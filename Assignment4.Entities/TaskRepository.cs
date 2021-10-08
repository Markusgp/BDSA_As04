using Assignment4.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public IEnumerable<TaskDTO> All()
        {
            return _connection.Tasks
                    .Select(c => new TaskDTO(c.Id, c.Title, c.Description, c.AssignedTo.Id, GetTags(c.Tags).ToList(), c.State))
                    .ToList().AsReadOnly();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public (Response Response, int TaskId) Create(TaskCreateDTO task)
        {
            var entity = new Task
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                AssignedTo = GetUser(task.AssignedToId),
                Tags = GetTags(task.Tags).ToList(),
                State = task.State
            };

            _connection.Tasks.Add(entity);

            _connection.SaveChanges();

            return (Response.Created,entity.Id);
        }

        public IReadOnlyCollection<TaskDTO> ReadAll()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
        {
            throw new NotImplementedException();
        }

        public TaskDetailsDTO Read(int taskId)
        {
            throw new NotImplementedException();
        }

        public Response Update(TaskUpdateDTO task)
        {
            throw new NotImplementedException();
        }

        Response Delete(int taskId)
        {
            throw new NotImplementedException();
        }
    }
}



        /* public IEnumerable<TaskDTO> All()
        {
            var cmdText = @"SELECT *
                            FROM Tasks AS c
                            ORDER BY c.Name";

            using var command = new SqlCommand(cmdText, _connection);

            OpenConnection();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                yield return new TaskDTO
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title"),
                    Description = reader.GetString("Description"),
                    AssignedToId = reader.GetInt32("AssignedToId"),
                    //Tags = reader.Get("Tags")
                    //State = reader.GetString("State")
                };
            }

            CloseConnection();
        }

        public int Create(TaskDTO task)
        {
            var cmdText = @"INSERT Task (Title, Description, AssignedToId, State, Tags)
                            VALUES (@Title, @Description, @AssignedToId, @State, @Tags);
                            SELECT SCOPE_IDENTITY()";

            using var command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@AssignedToId", task.AssignedToId);
            command.Parameters.AddWithValue("@State", task.State);
            command.Parameters.AddWithValue("@Tags", task.Tags);

            OpenConnection();

            var id = command.ExecuteScalar();

            CloseConnection();

            return (int)id;
        }

        public void Delete(int taskId)
        {
            var cmdText = @"DELETE Task WHERE Id = @Id";

            using var command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@Id", taskId);

            OpenConnection();

            command.ExecuteNonQuery();

            CloseConnection();
        }

        public TaskDetailsDTO FindById(int id)
        {
            var cmdText = @"SELECT c.Title, c.Description, c.AssignedToId, c.State, c.Tags
                            FROM TaskDetails AS c
                            LEFT JOIN Tasks AS t ON c.Id = t.Id
                            WHERE c.Id = @Id";

            using var command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@Id", id);

            OpenConnection();

            using var reader = command.ExecuteReader();

            var taskDetails = reader.Read()
                ? new TaskDetailsDTO
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title"),
                    Description = reader.GetString("Description"),
                    AssignedToId = reader.GetInt32("AssignedToId"),
                    AssignedToName = reader.GetString("AssignedToName"),
                    AssignedToEmail = reader.GetString("AssignedToEmail")
                    //Tags = reader.Get("Tags")
                    //State = reader.GetString("State")
                }
                : null;

            CloseConnection();

            return taskDetails;
        }

        public void Update(TaskDTO task)
        {
            var cmdText = @"UPDATE Tasks SET
                            Title = @Title,
                            Description = @Description,
                            AssignedToId = @AssignedToId,
                            State = @State
                            Tags = @Tags
                            WHERE Id = @Id";

            using var command = new SqlCommand(cmdText, _connection);

            command.Parameters.AddWithValue("@Id", task.Id);
            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@AssignedToId", task.AssignedToId);
            command.Parameters.AddWithValue("@State", task.State);
            command.Parameters.AddWithValue("@Tags", task.Tags);

            OpenConnection();

            command.ExecuteNonQuery();

            CloseConnection();
        }

        private void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        private void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    } */

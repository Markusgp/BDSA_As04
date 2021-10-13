using System;
using Xunit;
using System.Data.SqlClient;
using Assignment4.Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Data.SQLite;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests
    {
        private readonly KanbanContext _context;
        private readonly TaskRepository repo;

        public TaskRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            var context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated(); //Error here
            
            var entity = new Task
            {
                Title = "Documenting database",
                Description = "Creating documentation for our database",
                State = State.New
            };

            var user = new User
            {
                Id = 0,
                Name = "Test User",
                Email = "...",
                Tasks = new List<Task>()
            };

            context.Tasks.Add(entity);
            context.Users.Add(user);
            context.SaveChanges();

            _context = context;
            repo = new TaskRepository(_context);
           
        }

        [Fact]
        public void TaskRepo_creates_task_at_id_2()
        {
            var entity = new TaskCreateDTO
            {   
                Title = "Repo testing",
                AssignedToId = 0,
                Description = "Testing our three repositories",
                Tags = new List<string>()
            };

            Assert.NotNull(entity);
            Assert.NotNull(repo);

            var (res,id) = repo.Create(entity);

            Assert.Equal(Response.Created,res);
            Assert.Equal(2,id);
        }

        [Fact]
        public void TaskRepo_reads_task_at_id_0()
        {

        }
    }
}

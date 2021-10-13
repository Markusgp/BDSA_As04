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
    public class TaskRepositoryTests : IDisposable
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

            context.Tasks.Add(entity);
            context.SaveChanges();

            _context = context;
            repo = new TaskRepository(_context);
           
        }

        [Fact]
        public void TaskRepo_creates_task_at_id_1()
        {
            var entity = new TaskCreateDTO
            {   
                Title = "Repo testing",
                //Description = "Testing our three repositories",
                State = State.New,
                //Tags = {"testing","important"}
            };

            Assert.NotNull(entity);
            Assert.NotNull(repo);

            var (res,id) = repo.Create(entity);

            Assert.Equal(Response.Created,res);
            Assert.Equal(1,id);
        }

        [Fact]
        public void TaskRepo_reads_task_at_id_0()
        {

        }


        public void Dispose()
        {
            _context.Dispose();
        }

    }
}

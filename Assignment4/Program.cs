using System;
using System.IO;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Assignment4.Entities;
using Assignment4.Core;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace Assignment4
{
    class Program
    {
        static void Main(string[] args)
        {
            using var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            using var db = new KanbanContext(builder.Options);
            db.Database.EnsureCreated();
        }

        static int Create(KanbanContext db, string title)
        {
            Console.WriteLine("Creating new task");
            var t = db.Add(new Task {Title = title, State = State.New});
            db.SaveChanges();
            
            return t.Entity.Id;
        }

        static ICollection<Task> Read(KanbanContext db) 
        {
            Console.WriteLine("Getting all tasks");
            return db.Tasks.ToList();

        }
    }
}
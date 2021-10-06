﻿using System;
using System.IO;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Assignment4.Entities;

namespace Assignment4
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = LoadConfiguration();
            var connectionString = configuration.GetConnectionString("Kanban");

            Console.Write("Input title of task: ");
            var title = Console.ReadLine();
            var cmdText = "SELECT * FROM Tasks WHERE Title LIKE '%' + @title + '%'";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(cmdText, connection);

            command.Parameters.AddWithValue("@title", title);

            connection.Open();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                 var task = new
                 {
                     Title = reader.GetString("Title"),
                     State = reader.GetString("State")
                 };

                 Console.WriteLine(task);
             }

            var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>().UseSqlServer(connectionString);
            using var context = new KanbanContext(optionsBuilder.Options);

            var testTask = new Task
            {
                Title = "Testing",
                State = State.New
            };

            context.Tasks.Add(testTask);
            context.SaveChanges();

            //KanbanContextFactory.Seed(context);

            /* var chars = from c in context.Tasks
                        where c.Title.Contains("a")
                        select new
                        {
                            c.GivenName,
                            c.Surname,
                            c.City.Name,
                            Powers = string.Join(", ", c.Powers.Select(p => p.Name))
                        };

            foreach (var c in chars)
            {
                Console.WriteLine(c);
            } */


            static IConfiguration LoadConfiguration()
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddUserSecrets<Program>();

                return builder.Build();
            }
        }
    }
}
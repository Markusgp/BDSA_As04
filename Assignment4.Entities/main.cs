using System;
using System.IO;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Assignment4.Entities
{
    class main
    {
        static void Main(string[] args)
        {
        
            using (var db = new KanbanContext())
            {
                Console.WriteLine($"Database path: {db.DbPath}.");
                
                switch (args[0]) {
                    case "1":
                        GetAll(db);
                        break;
                    case "2":
                        CreateNew(db, args[1]);
                        break;
                }
                
            }
        }

        static void GetAll(KanbanContext db) 
        {
            Console.WriteLine("Getting all tasks");
                var task = db.Tasks
                    .OrderBy(t => t.Title);
                task.ToList().ForEach(t => Console.WriteLine(t.Title));

            db.SaveChanges();
        }

        static void CreateNew(KanbanContext db, string title)
        {
            Console.WriteLine("Creating new task");
            var t = db.Add(new Task {Title = title, State = State.New});
            Console.WriteLine(t.Entity.Id);

            db.SaveChanges();
        }
    }
}
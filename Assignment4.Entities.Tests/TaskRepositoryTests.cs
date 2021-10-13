/* using System;
using Xunit;
using System.Data.SqlClient;
using Assignment4;
using Microsoft.Extensions.Configuration;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests
    {
        [Fact]
        public void Repo_returns_all()
        {
            var connection = new SqlConnection("Kanban");
            var repo = new TaskRepository(connection);
        }

    }
}
 */
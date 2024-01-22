using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TodoApp.Models;
using System.Security.AccessControl;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;


namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoAppController : ControllerBase
    {

        private IConfiguration _configuration;

        public TodoAppController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetTodos")]
        public List<Todo> GetTodos()
        {
            string query = "select * from dbo.todos";
            DataTable table = new DataTable();

            string? sqlDatasource = _configuration.GetConnectionString("TodoApp");

            if (sqlDatasource == null)
            {
                throw new InvalidOperationException("Missing connection string.");
            }

            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myCon.Close();
            }
            return table.AsEnumerable().Select(row => new Todo
            {
                id = row.Field<long>("id"),
                description = row.Field<string>("description"),
                isChecked = row.Field<bool>("isChecked")
            }).ToList();
        }

        [HttpPost]
        [Route("AddTodo")]
        public Todo AddTodo([FromForm] string description)
        {
            string query = "INSERT INTO dbo.todos (description, isChecked) OUTPUT INSERTED.id VALUES (@description, @isChecked)";

            string? sqlDatasource = _configuration.GetConnectionString("TodoApp");

            if (sqlDatasource == null)
            {
                throw new InvalidOperationException("Missing connection string.");
            }

            long newId;

            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@description", description);
                    myCommand.Parameters.AddWithValue("@isChecked", false);
                    newId = (long)myCommand.ExecuteScalar();
                }
                myCon.Close();
            }

            return new Todo { id = newId, description = description, isChecked = false };
        }

        [HttpDelete]
        [Route("DeleteTodo")]
        public JsonResult DeleteTodo(int id)
        {
            string query = "DELETE FROM dbo.todos WHERE id=@id";

            string? sqlDatasource = _configuration.GetConnectionString("TodoApp");

            if (sqlDatasource == null)
            {
                throw new InvalidOperationException("Missing connection string.");
            }

            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Deleted successfully");
        }
    }
}

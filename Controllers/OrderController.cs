using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MicroBusiness.Models;

using System.Data.SqlClient;
using Dapper;

namespace Order.Controllers;

public class OrderController : Controller{

    private readonly string connectionString = "Server=localhost;Database=OrderDB;User ID=sa;Password=Pa$$w0rd;";

    
    
    // static IList<Customer> customerList = new List<Customer>();
    public IActionResult Index(string searchString){
        using(var connection = new SqlConnection(connectionString)){
            connection.Open();

            string commandSQL = "Select * from Customer";

            var customer = connection.Query<Customer>(commandSQL);

            if(!String.IsNullOrEmpty(searchString)){
                customer = customer.Where(c => c.FirstName.Contains(searchString) || c.LastName.Contains(searchString));    
            }
            connection.Close();
            
            return View(customer.ToList());
        }
    }


}
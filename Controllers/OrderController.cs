using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MicroBusiness.Models;

using System.Data.SqlClient;
using Dapper;

namespace Order.Controllers;

public class OrderController : Controller{

    private readonly string connectionString = "Server=localhost;Database=OrderDB;User ID=sa;Password=Pa$$w0rd;";
    
    public IActionResult Index(string orderList){
        
        string dbItem = "";
        
        dbItem = checkOrderList(orderList);

        if(!string.IsNullOrEmpty(dbItem)){
            return RedirectToAction(dbItem);
        } else {
            return RedirectToAction("Customer");
        }
    }

    public ActionResult Customer(string searchString,string sortOrder){
       
        using(var connection = new SqlConnection(connectionString)){
            connection.Open();
            
            string commandSQL = "Select * from Customer";

            var customer = connection.Query<Customer>(commandSQL);

            ViewData["FirstNameSortParm"] = sortOrder == "first_name" ? "first_name_desc" : "first_name";

            if(!string.IsNullOrEmpty(searchString)){
                commandSQL = searchItem(searchString);
                customer = connection.Query<Customer>(commandSQL,new {searchName = searchString});
            }

            if(!string.IsNullOrEmpty(sortOrder)){
                commandSQL = sortItem(sortOrder);
                customer = connection.Query<Customer>(commandSQL);
            }

            connection.Close();

            return View(customer.ToList());
        }
    }

    public ActionResult OrderItem(){
        using(var connection = new SqlConnection(connectionString)){
            connection.Open();

            string commandSQL = "Select * from OrderItem";

            var orderItem =  connection.Query<OrderItem>(commandSQL);

            connection.Close();

            return View(orderItem.ToList());   
        }
    }

    private string checkOrderList(string orderList){
        

        if(orderList == "Customer"){
            return "Customer";
        } else if (orderList == "Order"){
            return "Order";
        } else if (orderList == "OrderItem"){
            return "OrderItem";
        } else if (orderList == "Product"){
            return "Product";
        } else if (orderList == "Supplier"){
            return "Supplier";
        } else {
            return "";
        }
    }

    private string sortItem(string sortOrder){

        switch(sortOrder){
            case "first_name_desc":
                return "select * from Customer order by FirstName ASC";
            case "first_name":
                return "select * from Customer order by FirstName DESC";
        }
        return "";
    }

    private string searchItem(String searchString){
        
        string sqlCommand = "";
        
        Console.WriteLine(searchString);

        sqlCommand = "SELECT * from Customer where FirstName LIKE @searchName OR LastName LIKE @searchName";

        return sqlCommand;
    }
}
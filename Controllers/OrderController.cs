using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MicroBusiness.Models;

using System.Data.SqlClient;
using Dapper;

namespace Order.Controllers;

public class OrderController : Controller{

    private readonly string connectionString = "Server=localhost;Database=OrderDB;User ID=sa;Password=Pa$$w0rd;";
    
    string dbItem = "";
    public IActionResult Index(string orderList){
        
        dbItem = checkOrderList(orderList);

        if(!string.IsNullOrEmpty(dbItem)){
            return RedirectToAction(dbItem);
        } else {
            return RedirectToAction("Customer");
        }
    }

    public string checkOrderList(string orderList){
        

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

    public ActionResult Customer(string searchString,string sortOrder){
       
        using(var connection = new SqlConnection(connectionString)){
            connection.Open();
            
            string commandSQL = "Select * from Customer";

            var customer = connection.Query<Customer>(commandSQL);
            
            ViewData["FirstNameSortParm"] = sortOrder == "first_name" ? "first_name_desc" : "first_name";

            if(!String.IsNullOrEmpty(searchString)){
                customer = customer.Where(c => c.FirstName.Contains(searchString) || c.LastName.Contains(searchString));    
            }

            switch (sortOrder){
                case "first_name_desc":
                    customer = customer.OrderByDescending(c => c.FirstName);
                    break;
                case "first_name":
                    customer = customer.OrderBy(c => c.FirstName);
                    break;
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
    


}
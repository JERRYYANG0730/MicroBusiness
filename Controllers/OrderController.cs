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
        using(var connection = new SqlConnection(connectionString)){

            connection.Open();

            orderList = "Customer";

            dbItem = checkOrderList(orderList);

            connection.Close();

            return RedirectToAction(dbItem);
            
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

    public ActionResult Customer(string searchString,string sortOrder, string orderList){
       
        using(var connection = new SqlConnection(connectionString)){
            connection.Open();

            dbItem = "Customer";
            
            string commandSQL = $@"Select * from {dbItem}";

            dbItem = checkOrderList(orderList);

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

            return RedirectToAction(dbItem); 
           
        }
    }


    public ActionResult OrderItem(){
        using(var connection = new SqlConnection(connectionString)){
            connection.Open();
            dbItem = "OrderItem";

            string commandSQL = $@"Select * from {dbItem}";

            dbItem = "";

            var orderItem =  connection.Query<OrderItem>(commandSQL);

            connection.Close();

            return View(orderItem.ToList());

            // if (!string.IsNullOrEmpty(dbItem)){
            //     return RedirectToAction(dbItem);
            // } else {
            //     return View(orderItem.ToList());
            // }     

        }
    }
    


}
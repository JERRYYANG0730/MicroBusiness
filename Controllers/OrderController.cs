using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MicroBusiness.Models;

namespace Order.Controllers;

public class OrderController : Controller{
    public IActionResult Index(){
        return View("Index");
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellersService;

        public SellersController(SellerService sellerService)
        {
            _sellersService = sellerService;
        }
        public IActionResult Index()
        {
            var list = _sellersService.FinAll();
            return View(list);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] //anotação para indicar este método é post
        [ValidateAntiForgeryToken] //anotação para evitar ataques csrf
        public  IActionResult Create(Seller seller)
        {
            _sellersService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }
    }
}
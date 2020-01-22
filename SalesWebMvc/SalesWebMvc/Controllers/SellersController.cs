using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services.Exception;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellersService;
        private readonly DepartmentService _departmentService;
        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellersService = sellerService;
            _departmentService = departmentService;
        }
        public IActionResult Index()
        {
            var list = _sellersService.FinAll();
            return View(list);
        }
        public IActionResult Create()
        {
            var departments = _departmentService.FinAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost] //anotação para indicar este método é post
        [ValidateAntiForgeryToken] //anotação para evitar ataques csrf
        public  IActionResult Create(Seller seller)
        {
            _sellersService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var obj = _sellersService.FindById(id.Value);//devemos colocar .Value por se tratar de um valor opcional

            if(obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellersService.Remove(id);
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellersService.FindById(id.Value);//devemos colocar .Value por se tratar de um valor opcional

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var obj = _sellersService.FindById(id.Value);

            if(obj == null)
            {
                return NotFound();
            }
            List<Department> departments = _departmentService.FinAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller) 
        {
            if(id != seller.Id) 
            {
                return BadRequest();
            }

            try
            {
                _sellersService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch(NotFoundException)
            {
                return NotFound();   
            }
            catch (DbConcurrencyException)
            {
                return BadRequest();
            }
        }
    }
}
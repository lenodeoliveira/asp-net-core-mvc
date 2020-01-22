using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services.Exception;
using System.Diagnostics;

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
        public async Task<IActionResult> Index()
        {
            var list = await _sellersService.FinAllAsync();
            return View(list);
        }
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FinAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost] //anotação para indicar este método é post
        [ValidateAntiForgeryToken] //anotação para evitar ataques csrf
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var department = await _departmentService.FinAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = department };
                return View(viewModel);
            }
            await _sellersService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellersService.FindByIdAsync(id.Value);//devemos colocar .Value por se tratar de um valor opcional

            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellersService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch(IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });

            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellersService.FindByIdAsync(id.Value);//devemos colocar .Value por se tratar de um valor opcional

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellersService.FindByIdAsync(id.Value);

            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            List<Department> departments = await _departmentService.FinAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller) 
        {
            if (!ModelState.IsValid)
            {
                var department = await _departmentService.FinAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = department };
                return View(viewModel);
            }
            if (id != seller.Id) 
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismacth" });
            }

            try
            {
                await _sellersService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch(ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
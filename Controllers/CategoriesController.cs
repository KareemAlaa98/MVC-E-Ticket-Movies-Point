using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies_Point.Data;
using Movies_Point.Models;
using Movies_Point.Repository;
using Movies_Point.IRepository;
using Movies_Point.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Movies_Point.Controllers
{
    public class CategoriesController : Controller
    {
        ICategoryRepository categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            List<Category> categories = categoryRepository.ReadAll();
            return View(categories);
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            var category = new CategoryViewModel();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public IActionResult SaveNew(CategoryViewModel categoryVM)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Create(categoryVM);
                return RedirectToAction("Index");
            }
            return View("create", categoryVM);
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult Edit(int id)
        {
            var category = categoryRepository.ReadById(id);
            
            if (category == null) { RedirectToAction("Index"); }
            
            var categoryVM = new CategoryViewModel();
            categoryVM.Id = category.Id;
            categoryVM.Name = category.Name;

            return View(categoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public IActionResult SaveEdit(CategoryViewModel categoryVM)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Update(categoryVM);
                return RedirectToAction("Index");
            }
            return View("Edit", categoryVM);
        }


        [Authorize(Roles ="Admin")]
        public IActionResult Delete(int id)
        {
            categoryRepository.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var categoryMovies = categoryRepository.getCategoryMovies(id);


            ViewData["categoryName"] = categoryRepository.ReadById(id).Name;
            return View(categoryMovies);
        }
    }
}

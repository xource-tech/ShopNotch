﻿using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Logic;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopNotch.Models;
using ShopNotch.Models.Classes;

namespace ShopNotch.Controllers.Notch
{
	[Area("Notch")]
	public class CategoriesController : Controller
    {
        private ICategoryLogic _categoryLogic;
        private Mapper _mapper;

        public CategoriesController(ICategoryLogic logic)
        {
	        _categoryLogic = logic;
			_mapper = new Mapper();
        }

        public IActionResult Index()
        {
			CategoryViewModel model = new CategoryViewModel
			{
				Categories = new List<CategoryModel>(),
				Tree = _categoryLogic.GetCategoryTree()
			};

			var categories = _categoryLogic.GetAll();

			foreach (var category in categories)
			{
				model.Categories.Add(_mapper.GetCategoryModel(category));
			}

			return View( model );
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _categoryLogic.GetById((int) id);
            if (category == null)
            {
                return NotFound();
            }
			

            CategoryViewModel model = new CategoryViewModel
            {
	            Category = _mapper.GetCategoryModel(category)
            };

			return View(model);
        }

        public IActionResult Create()
        {
	        var categories = _categoryLogic.GetAll();

	        CategoryViewModel model = new CategoryViewModel()
	        {
		        Categories = new List<CategoryModel>(),
		        CategoryNames = GetAllNames(categories)
	        };

	        foreach (var category in categories)
	        { 
				model.Categories.Add(_mapper.GetCategoryModel(category));
	        }

			return View(model);
        }

		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryViewModel model)
        {
	        var category = new Category
	        {
		        Id = 0,
		        Name = model.Name,
				ParentId = model.SelectedParent
	        };

			if (ModelState.IsValid)
            {
	            _categoryLogic.Add(category);
				
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _categoryLogic.GetById((int) id);
            if (category == null)
            {
                return NotFound();
            }

            CategoryViewModel model = new CategoryViewModel
            {
	            Category = _mapper.GetCategoryModel(category)
            };

            model.CategoryNames = category.Parent != null ? GetAllNames(_categoryLogic.GetAll(), category.Parent) : GetAllNames(_categoryLogic.GetAll());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CategoryViewModel model)
        {
            if (id != model.Category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
	            if (model.SelectedParent != null)
	            {
		            model.Category.ParentId = (int) model.SelectedParent;
	            }
                try
                {
                    _categoryLogic.Update(_mapper.GetCategoryFromModel(model.Category));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(model.Category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _categoryLogic.GetById((int) id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
	        var category = _categoryLogic.GetById(id);
            _categoryLogic.Delete(category);

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
	        return (_categoryLogic.GetById(id) != null);
        }

        private List<SelectListItem> GetAllNames(IEnumerable<Category> categories)
        {
	        List<SelectListItem> list = new List<SelectListItem>();

	        foreach (Category category in categories)
	        {
		        list.Add(new SelectListItem(category.Name, category.Id.ToString()));
	        }

	        return list;
        }
        private List<SelectListItem> GetAllNames(IEnumerable<Category> categories, Category selected)
        {
	        List<SelectListItem> list = new List<SelectListItem>();

	        foreach (Category category in categories)
	        {
		        if (category.Id == selected.Id)
		        {
			        list.Add(new SelectListItem(category.Name, category.Id.ToString(), true));
		        }
		        else
		        {
			        list.Add(new SelectListItem(category.Name, category.Id.ToString()));
		        }
	        }

	        return list;
        }

	}
}

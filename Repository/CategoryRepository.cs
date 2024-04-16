using Movies_Point.Data;
using Movies_Point.IRepository;
using Movies_Point.Models;
using Microsoft.EntityFrameworkCore;
using Movies_Point.ViewModels;

namespace Movies_Point.Repository
{
    public class CategoryRepository : ICategoryRepository
    {        
        //ApplicationDbContext context = new ApplicationDbContext();
        ApplicationDbContext context;
        public CategoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Category> ReadAll()
        {
           return context.Categories.ToList();
        }
            
        public List<Movie> getCategoryMovies(int id)
        {
           return context.Movies.Include("Category").Include("Cinema").Where(e => e.CategoryId == id).ToList();
        }

        public Category ReadById(int id)
        {
            return context.Categories.Find(id);
        }

        public void Create(CategoryViewModel categoryVM)
        {
            var category = new Category();
            category.Id = categoryVM.Id;
            category.Name = categoryVM.Name;
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var category = ReadById(id);
            if(category != null)
            {
                context.Categories.Remove(category);
                context.SaveChanges();
            }
        }


        public void Update(CategoryViewModel categoryVM)
        {
            var dbCategory = ReadById(categoryVM.Id);
            if (dbCategory != null)
            {
                dbCategory.Id = categoryVM.Id;
                dbCategory.Name = categoryVM.Name;
                context.SaveChanges();
            }
        }
    }
}

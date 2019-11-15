using InternetShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetShop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products()
        {
            using (var context = new Context())
            {
                    ViewBag.ProductInfoList = (from o in context.Products.ToList()
                                               join c in context.Categories.ToList() on o.CategoryId equals c.Id into gc
                                               from ct in gc.DefaultIfEmpty()
                                               select new ProductInfo()
                                               {
                                                   Id = o.Id,
                                                   Name = o.Name,
                                                   Price = o.Price,
                                                   CategoryId = o.CategoryId,
                                                   CategoryName = ct?.Name ?? String.Empty
                                               }).ToList();
            }
            return View();
        }

        public ActionResult Product(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            using (var context = new Context())
            {
                Product product = context.Products.FirstOrDefault(p => p.Id == id);
                if (product == null)
                {
                    return HttpNotFound();
                }

                Category category = context.Categories.FirstOrDefault(p => p.Id == product.CategoryId);

                string categoryName = "";
                if (category != null)
                {
                    categoryName = category.Name;
                }

                ViewBag.ProductInfo = new ProductInfo{
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    CategoryName = categoryName
                };

            }

            return View();
        }

        public ActionResult AddProduct()
        {
            ViewBag.Message = "";

            using (var context = new Context())
            {
                ViewBag.Categories = context.Categories.OrderBy(x=>x.Name).ToList();
            }

            return View();

        }

        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            if (product == null)
            {
                return HttpNotFound();
            }

            using (var context = new Context())
            {
                ViewBag.Categories = context.Categories.OrderBy(x => x.Name).ToList();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "";
                return View(product);
            }

            using (var context = new Context())
            {
                var resultSeek = context.Products.Any(x => x.Name == product.Name);
                if (resultSeek)
                {
                    ViewBag.Message = "Наименование \"" + product.Name + "\" уже есть в базе данных. Повторите ввод!";
                    return View(product);
                }

                context.Products.Add(product);
                context.SaveChanges();
            }
            return RedirectToAction("Products");
        }

        public ActionResult EditProduct()
        {
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult EditProduct(int? id)
        {
            Product product = null;

            if (id == null)
            {
                return HttpNotFound();
            }

            using (var context = new Context())
            {
                ViewBag.Categories = context.Categories.OrderBy(x => x.Name).ToList();
            }

            using (var context = new Context())
            {
                product = context.Products.FirstOrDefault(p => p.Id == id);
                if (product == null)
                {
                    return HttpNotFound();
                }
            }

            ViewBag.Message = "";

            return View(product);
        }

        [HttpPost]
        public ActionResult EditProduct(Product product)
        {
            Product prod;
            if (product == null)
            {
                return HttpNotFound();
            }

            using (var context = new Context())
            {
                ViewBag.Categories = context.Categories.OrderBy(x => x.Name).ToList();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "";
                return View(product);
            }

            using (var context = new Context())
            {

                prod = context.Products.Find(product.Id);
                if (prod != null)
                {
                    if (prod.Name != product.Name)
                    {
                        var resultSeek = context.Products.Any(x => x.Name == product.Name);
                        if (resultSeek)
                        {
                            ViewBag.Message = "Наименование \"" + product.Name + "\" уже есть в базе данных. Повторите ввод!";
                            return View(product);
                        }
                    }
                    prod.Name = product.Name;
                    prod.Price = product.Price;
                    prod.CategoryId = product.CategoryId;
                    context.SaveChanges();
                }
            }

            return RedirectToAction("Products");
        }

        public ActionResult DeleteProduct()
        {
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult DeleteProduct(int? id)
        {
            Product product = null;

            if (id == null)
            {
                return HttpNotFound();
            }

            using (var context = new Context())
            {
                ViewBag.Categories = context.Categories.OrderBy(x => x.Name).ToList();
            }

            using (var context = new Context())
            {
                product = context.Products.FirstOrDefault(p => p.Id == id);
                if (product == null)
                {
                    return HttpNotFound();
                }
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult DeleteProduct(Product product)
        {
            Product prod;
            if (product == null)
            {
                return HttpNotFound();
            }

            using (var context = new Context())
            {
                ViewBag.Categories = context.Categories.OrderBy(x => x.Name).ToList();
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            using (var context = new Context())
            {

                prod = context.Products.Find(product.Id);
                if (prod != null)
                {
                    context.Products.Remove(prod);
                    context.SaveChanges();
                }
            }

            return RedirectToAction("Products");
        }

        [HttpGet]
        public ActionResult LoginUser()
        {
            ViewBag.Login = "";
            ViewBag.Password = "";
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public ActionResult LoginUser(string login, string password)
        {
            bool resultSeek;

            ViewBag.Login = login;
            ViewBag.Password = password;

            if (login == "")
            {
                ViewBag.Message = "Введите логин!";
                return View();
            }
            if (password == "")
            {
                ViewBag.Message = "Введите пароль!";
                return View();
            }
            using (var context = new Context())
            {
                resultSeek = context.Users.Any(x => x.Login == login);
                if (!resultSeek)
                {
                    ViewBag.Message = "Логин \"" + login + "\" не найден в базе данных. Повторите ввод!";
                    return View();
                }
                resultSeek = context.Users.Any(x => x.Login == login && x.Password == password);
                if (!resultSeek)
                {
                    ViewBag.Message = "Пароль введен неправильно. Повторите ввод!";
                    return View();
                }
            }

            Session["userMain"] = login;

            return RedirectToAction("Products");

        }

        public ActionResult LogoutUser()
        {

            Session["userMain"] = "";

            return RedirectToAction("Products");

        }

        public ActionResult RegistrationUser()
        {
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public ActionResult RegistrationUser(User user)
        {
            if (user == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "";
                return View(user);
            }

            using (var context = new Context())
            {
                var resultSeek = context.Users.Any(x => x.Login == user.Login);
                if (resultSeek)
                {
                    ViewBag.Message = "Логин \"" + user.Login + "\" уже есть в базе данных. Повторите ввод!";
                    return View(user);
                }
                context.Users.Add(user);
                context.SaveChanges();

            }

            Session["userMain"] = user.Login;

            return RedirectToAction("Products");

        }

        public ActionResult Categories()
        {

            using (var context = new Context())
            {
                ViewBag.Categories = context.Categories.ToList();


            }

            return View();
        }

        public ActionResult Category(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            using (var context = new Context())
            {
                var category = context.Categories.FirstOrDefault(p => p.Id == id);
                if (category == null)
                {
                    return HttpNotFound();
                }

                ViewBag.Category = category;
            }

            return View();
        }

        public ActionResult AddCategory()
        {
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            if (category == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "";
                return View(category);
            }

            using (var context = new Context())
            {
                var resultSeek = context.Categories.Any(x => x.Name == category.Name);
                if (resultSeek)
                {
                    ViewBag.Message = "Наименование \"" + category.Name + "\" уже есть в базе данных. Повторите ввод!";
                    return View(category);
                }

                context.Categories.Add(category);
                context.SaveChanges();
            }

            return RedirectToAction("Categories");
        }

        public ActionResult EditCategory()
        {
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult EditCategory(int? id)
        {
            Category category = null;

            if (id == null)
            {
                return HttpNotFound();
            }

            using (var context = new Context())
            {
                category = context.Categories.FirstOrDefault(p => p.Id == id);
                if (category == null)
                {
                    return HttpNotFound();
                }
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult EditCategory(Category category)
        {
            Category ct;
            if (category == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            using (var context = new Context())
            {

                ct = context.Categories.Find(category.Id);
                if (ct != null)
                {
                    ct.Name = category.Name;
                    context.SaveChanges();
                }
            }

            return RedirectToAction("Categories");
        }

        public ActionResult DeleteCategory()
        {
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult DeleteCategory(int? id)
        {
            Category category = null;

            if (id == null)
            {
                return HttpNotFound();
            }

            using (var context = new Context())
            {
                category = context.Categories.FirstOrDefault(p => p.Id == id);
                if (category == null)
                {
                    return HttpNotFound();
                }
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult DeleteCategory(Category category)
        {
            Category ct;
            if (category == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            using (var context = new Context())
            {

                ct = context.Categories.Find(category.Id);
                if (ct != null)
                {
                    context.Categories.Remove(ct);
                    context.SaveChanges();
                }
            }

            return RedirectToAction("Categories");
        }

        [HttpGet]
        public ActionResult FormFilter()
        {
            ViewBag.categoryId = 0;
            ViewBag.countRecords = 3;
            ViewBag.Message = "";

            using (var context = new Context())
            {
                ViewBag.Categories = context.Categories.OrderBy(x => x.Name).ToList();
            }

            return View();
        }

        [HttpPost]
        public ActionResult FormFilter(int categoryId, int countRecords)
        {
            List<ProductInfo> tovars;
            ViewBag.categoryId = categoryId;
            ViewBag.countRecords = countRecords;

            using (var context = new Context())
            {
                ViewBag.Categories = context.Categories.OrderBy(x => x.Name).ToList();
            }

            if (countRecords <= 0)
            {
                ViewBag.Message = "Введите число больше 0!";
                return View();
            }

            using (var context = new Context())
            {
                if (categoryId == 0)  // все записи
                {
                    tovars = (from o in context.Products.ToList()
                              join c in context.Categories.ToList() on o.CategoryId equals c.Id into gc
                              from ct in gc.DefaultIfEmpty()
                              select new ProductInfo()
                              {
                                  Id = o.Id,
                                  Name = o.Name,
                                  Price = o.Price,
                                  CategoryId = o.CategoryId,
                                  CategoryName = ct?.Name ?? String.Empty
                              }).ToList();
                    if (tovars.Count == 0)
                    {
                        ViewBag.Message = "Не найдено записей!";
                        return View();
                    }
                }
                else  // записи по категории categoryId
                {
                    tovars = (from o in context.Products.ToList()
                                  join c in context.Categories.ToList() on o.CategoryId equals c.Id into gc
                                  from ct in gc.DefaultIfEmpty()
                                  select new ProductInfo()
                                  {
                                      Id = o.Id,
                                      Name = o.Name,
                                      Price = o.Price,
                                      CategoryId = o.CategoryId,
                                      CategoryName = ct?.Name ?? String.Empty
                                  }).Where(x => x.CategoryId == categoryId).ToList();

                    var category = context.Categories.Find(categoryId);
                    var categoryName = "";
                    if (category != null)
                    {
                        categoryName = category.Name;
                    }
                    if (tovars.Count == 0)
                    {
                        ViewBag.Message = "Не найдено записей по категории '" + categoryName + "'!";
                        return View();
                    }
                }

            }

            return RedirectToAction("ProductsFilter", "Home", new { categoryId = categoryId, pageNumber = 1, countRecords = countRecords });
           
        }

        public ActionResult ProductsFilter(int categoryId, int pageNumber, int countRecords)
        {
            List<ProductInfo> tovars;
            List<int> listPages;
            Category category;
            string categoryName;
            int recordsCount;

            if (pageNumber < 0)
            {
                pageNumber = 1;
            }

            if (countRecords <= 0)
            {
                countRecords = 1;
            }

            ViewBag.categoryId = categoryId;
            ViewBag.pageNumber = pageNumber;
            ViewBag.countRecords = countRecords;
            categoryName = "";

            using (var context = new Context())
            {
                if (categoryId == 0)  // все записи
                {
                    tovars = (from o in context.Products.ToList()
                                  join c in context.Categories.ToList() on o.CategoryId equals c.Id into gc
                                  from ct in gc.DefaultIfEmpty()
                                  select new ProductInfo()
                                  {
                                      Id = o.Id,
                                      Name = o.Name,
                                      Price = o.Price,
                                      CategoryId = o.CategoryId,
                                      CategoryName = ct?.Name ?? String.Empty
                                  }).ToList();
                    categoryName = null;
                }
                else  // записи по категории categoryId
                {
                    tovars = (from o in context.Products.ToList()
                                  join c in context.Categories.ToList() on o.CategoryId equals c.Id into gc
                                  from ct in gc.DefaultIfEmpty()
                                  select new ProductInfo()
                                  {
                                      Id = o.Id,
                                      Name = o.Name,
                                      Price = o.Price,
                                      CategoryId = o.CategoryId,
                                      CategoryName = ct?.Name ?? String.Empty
                                  }).Where(x => x.CategoryId == categoryId).ToList();

                    category = context.Categories.Find(categoryId);
                    categoryName = "";
                    if (category != null)
                    {
                        categoryName = category.Name;
                    }
                }

                if (tovars.Count == 0)
                {
                    return RedirectToAction("FormFilter", "Home", new { categoryId = categoryId, countRecords = countRecords });
                }

                recordsCount = tovars.Count;

                int countPages = (int)Math.Ceiling((decimal)recordsCount / countRecords);

                 ViewBag.categoryName = categoryName;
                 ViewBag.ProductInfoList = tovars.Skip((pageNumber - 1) * countRecords).Take(countRecords);
                 listPages = new List<int>();

                 for (int i = 0; i < countPages; i++)
                 {
                     listPages.Add(i+1);
                 }

                if ((pageNumber == 1) || ((pageNumber - 1) > countPages))
                {
                    ViewBag.pagePreviousNumber = null;
                }
                else
                {
                    ViewBag.pagePreviousNumber = pageNumber - 1;
                }

                if ((pageNumber + 1) > countPages)
                {
                    ViewBag.pageNextNumber = null;
                }
                else
                {
                    ViewBag.pageNextNumber = pageNumber + 1;
                }

                ViewBag.listPages = listPages;
            }

            return View();
        }

    }
}
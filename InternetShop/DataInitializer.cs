using InternetShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InternetShop
{
    public class DataInitializer : CreateDatabaseIfNotExists<Context>
    {
        protected override void Seed(Context context)
        {
            // Создание уникального индекса по наименованию в таблице Categories
            context.Database.ExecuteSqlCommand("create unique index ix_name on Categories(Name)");

            // Создание уникального индекса по наименованию в таблице Products
            context.Database.ExecuteSqlCommand("create unique index ix_name on Products(Name)");

            // Создание индекса по наименованию в таблице Products
            context.Database.ExecuteSqlCommand("create index ix_categoryId on Products(CategoryId)");

            // Создание уникального индекса по наименованию в таблице Users
            context.Database.ExecuteSqlCommand("create unique index ix_login on Users(Login)");


            // добавление записей по умолчанию в новой базе данных
            context.Categories.AddRange(new List<Category>{
                new Category
                {
                    Name = "Фрукты"
                },
                new Category
                {
                    Name = "Овощи"
                },
                new Category
                {
                    Name = "Напитки"
                },
                new Category
                {
                    Name = "Торты"
                },
                new Category
                {
                    Name = "Консервы"
                }
            });
            context.Products.AddRange(new List<Product>{
                new Product
                {
                    Name = "Яблоки",
                    Price = 400,
                    CategoryId = 1
                },
                new Product
                {
                    Name = "Сливы",
                    Price = 550,
                    CategoryId = 1
                },
                new Product
                {
                    Name = "Апельсины",
                    Price = 415,
                    CategoryId = 1
                },
                new Product
                {
                    Name = "Картофель",
                    Price = 99,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Свекла",
                    Price = 235,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Помидоры",
                    Price = 450,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Огурцы",
                    Price = 380,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Морковь",
                    Price = 150,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Лимонад",
                    Price = 450,
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Вода",
                    Price = 120,
                    CategoryId = 3
                }
            });
            context.Users.AddRange(new List<User>{
                new User
                {
                    Login = "Admin",
                    Password = "123456"
                },
                new User
                {
                    Login = "User1",
                    Password = "100001"
                },
                new User
                {
                    Login = "User2",
                    Password = "100002"
                }
            });
            context.SaveChanges();
        }
    }
}
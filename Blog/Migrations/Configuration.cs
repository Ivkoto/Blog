namespace Blog.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<Blog.Models.BlogDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Blog.Models.BlogDbContext";
        }

        protected override void Seed(Blog.Models.BlogDbContext context)
        {
            if (!context.Roles.Any())
            {
                this.CreateRole(context, "Admin");
                this.CreateRole(context, "User");
            }

            if (!context.Users.Any())
            {
                this.CreateUser(context, "admin@admin.com", "Admin", "123");
                this.SetRoleToUser(context, "admin@admin.com", "Admin");
            }            
        }
        

        private void CreateRole(BlogDbContext context, string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var result = roleManager.Create(new IdentityRole(roleName));

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        private void CreateUser(BlogDbContext context, string email, string fullName, string password)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var admin = new ApplicationUser
            {
                UserName = email,
                FullName = fullName,
                Email = email,
            };

            userManager.PasswordValidator = new PasswordValidator
            {
                RequireDigit = false,
                RequiredLength = 1,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false,
            };

            var result = userManager.Create(admin, password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        private void SetRoleToUser(BlogDbContext context, string email, string role)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var user = context.Users.Where(u => u.Email == email).First();

            var result = userManager.AddToRole(user.Id, role);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

    }
}

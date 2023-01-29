using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using WebUI.Models;
using WebUI.Models.Shared;

[assembly: OwinStartupAttribute(typeof(WebUI.Startup))]
namespace WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app); 
            //InitializeRolesandUsers();
		//bharath
		//Bharath Gone55

        }

        // In this method we will create default User roles and Admin user for login    
        private void InitializeRolesandUsers()
        {
            CreateRoles();
            CreateAdmin();
            ExamCentreMasterDB.Get().ForEach(examCentre =>
            {
                CreateAtUser(examCentre.ExamCentreCode);
            });
        }

        public void CreateRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if(!roleManager.RoleExists("AtUser"))
                roleManager.Create(new IdentityRole("AtUser"));
            if (!roleManager.RoleExists("AtAdmin"))
                roleManager.Create(new IdentityRole("AtAdmin"));
        }
        public bool CreateAdmin()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            if (UserManager.FindByName("Admin") == null)
            {
                var user = new ApplicationUser { UserName = "Admin", Email = $"admin@braou.ac.in" };
                var result = UserManager.Create(user, $"Admin@2022");
                if (result.Succeeded)
                {
                    result = UserManager.AddToRole(user.Id, "AtAdmin");
                }
                return result.Succeeded;
            }
            return true;
        }

        public bool CreateAtUser(string examcentrecode)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            if (UserManager.FindByName(examcentrecode) == null)
            {
                var user = new ApplicationUser { UserName = examcentrecode, Email = $"{examcentrecode}@braou.ac.in" };
                var result = UserManager.Create(user, $"{examcentrecode}@braou");
                if (result.Succeeded)
                {
                    result = UserManager.AddToRole(user.Id, "AtUser");
                }
                return result.Succeeded;
            }
            return true;
        }
    }
}

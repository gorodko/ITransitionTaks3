using ItransitionTask3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ItransitionTask3.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationUserManager _userManager;

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }

        

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: Users/Index
        [Authorize]
        public ActionResult Index()
        {
            ApplicationUserManager userManager = HttpContext.GetOwinContext()
                                            .GetUserManager<ApplicationUserManager>();

            return View(userManager.Users.ToList());
        }


        // POST Users/Delete
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Delete(string[] ids)
        {
            if (ids == null) return Json(new { success = true });
            ApplicationUserManager userManager = HttpContext.GetOwinContext()
                                           .GetUserManager<ApplicationUserManager>();
            foreach (string id in ids)
            {
                var user = await userManager.FindByIdAsync(id);
                userManager.UpdateSecurityStamp(user.Id);
                userManager.Delete(user);
            }
            return Json(new { success = true });
        }
        // POST Users/Block
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Block(string[] ids)
        {
            if(ids==null) return Json(new { success = true });
            ApplicationUserManager userManager = HttpContext.GetOwinContext()
                                           .GetUserManager<ApplicationUserManager>();
            foreach (string id in ids)
            {
                var user = await userManager.FindByIdAsync(id);
                user.LockoutEnabled = true;
                user.LockoutEndDateUtc = System.DateTime.Now.AddYears(1);
                user.Status = "unactive"; 
                userManager.UpdateSecurityStamp(user.Id);
                userManager.Update(user);
            }
            return Json(new { success = true });
        }
        // POST Users/Unblock
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Unblock(string[] ids)
        {
            ApplicationUserManager userManager = HttpContext.GetOwinContext()
                               .GetUserManager<ApplicationUserManager>();
            foreach (string id in ids)
            {
                var user = await userManager.FindByIdAsync(id);
                if (user.LockoutEnabled == false) continue;
                user.LockoutEnabled = false;
                user.LockoutEndDateUtc = System.DateTime.Now.AddYears(-1);
                user.Status = "active";
                userManager.UpdateSecurityStamp(user.Id);
                userManager.Update(user);
            }
            return Json(new { success = true });
        }
    }
}
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
   
    public class AuthorizationController : Controller
    {

        public AuthorizationController()
        { 
        
        }

        // GET: Authorization
        public ActionResult Index(string Role)
        {
            //Claim是什么，可以理解为你的身份证的中的名字，性别等等的每一条信息，
            //然后Claim组成一个ClaimIdentity 就是组成一个身份证
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name,value:"张三"),
                new Claim(type:"Address",value:"北京市"),
                new Claim(ClaimTypes.Role,Role)
            };

            var identity = new ClaimsIdentity(claim,authenticationType:"ZSIdentity");
            HttpContext.SignInAsync(principal:new ClaimsPrincipal(identity));
            return View();
        }

        // GET: Authorization/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Authorization/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Authorization/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Authorization/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Authorization/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Authorization/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Authorization/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

using CareoProject.Data_Access_Layer;
using CareoProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CareoProject.Areas.Manage.Controllers
{ 
    [Area("Manage")]
    public class CustomerController : Controller
    {
    private AppDbContext _context { get; }
    private IWebHostEnvironment _env { get; }
    public CustomerController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
            _env = env;
    }
        public IActionResult Index()
        {
            return View(_context.Customers.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);
            if (customer.ImageUrl != null)
            {
                if (customer.ImageUrl.ContentType != "image/jpeg" && customer.ImageUrl.ContentType != "image/png" && customer.ImageUrl.ContentType != "image/wepb")
                {
                    ModelState.AddModelError("", "Faylin tipi png ve ya jpeg olmalidir");
                    return View(customer);
                }
                if(customer.ImageUrl.Length/1024 > 3000)
                {
                    ModelState.AddModelError("", "Faylin olcusu max 3mb ola biler");
                    return View(customer);
                }
                string filename = customer.ImageUrl.FileName;
                if(filename.Length > 64)
                {
                    filename.Substring(filename.Length - 64, 64);

                }
                string newFileName = Guid.NewGuid().ToString() + filename;
                string path = Path.Combine(_env.WebRootPath, "assets", "images", newFileName);
                using (FileStream fs = new FileStream(path,FileMode.Create))
                {
                    customer.ImageUrl.CopyTo(fs);
                }
                customer.Image = newFileName;
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            Customer customer = _context.Customers.Find(id);
            if (customer == null) return BadRequest();
            if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "assets", "images", customer.Image)))
                System.IO.File.Delete(Path.Combine(_env.WebRootPath, "assets", "images", customer.Image));
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
           Customer customer = _context.Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null) return BadRequest();
            return View(customer);
        }
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            Customer customer1 = _context.Customers.FirstOrDefault(c => c.Id == customer.Id);
            if (customer1 == null) return NotFound();
            customer1.SpecialtyName = customer.SpecialtyName;
            customer1.Fullname = customer.Fullname;
            customer1.Description = customer.Description;
            customer1.ImageUrl = customer.ImageUrl;
            if (customer.ImageUrl != null)
            {
                if (customer.ImageUrl.ContentType != "image/jpeg" && customer.ImageUrl.ContentType != "image/png" && customer.ImageUrl.ContentType != "image/wepb")
                {
                    ModelState.AddModelError("", "Faylin tipi png ve ya jpeg olmalidir");
                    return View(customer);
                }
                if (customer.ImageUrl.Length / 1024 > 3000)
                {
                    ModelState.AddModelError("", "Faylin olcusu max 3mb ola biler");
                    return View(customer);
                }
                string filename = customer.ImageUrl.FileName;
                if (filename.Length > 64)
                {
                    filename.Substring(filename.Length - 64, 64);

                }
                if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "assets", "images", customer1.Image)))
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, "assets", "images", customer1.Image));
                string newFileName = Guid.NewGuid().ToString() + filename;
                string path = Path.Combine(_env.WebRootPath, "assets", "images", newFileName);
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    customer.ImageUrl.CopyTo(fs);
                }
                customer1.Image = newFileName;
            }
                _context.SaveChanges();
              return RedirectToAction(nameof(Index));
        }
    }
}

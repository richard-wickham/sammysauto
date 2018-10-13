using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SammysAuto.Data;
using SammysAuto.Models;
using SammysAuto.Utility;

namespace SammysAuto.Controllers
{
    [Authorize (Roles = SD.AdminEndUser)]
    public class UsersController : Controller
    {

        private ApplicationDbContext _db;

        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(string option=null, string search = null)
        {
            var users = _db.Users.ToList();

            if(option == "email" && search != null)
            {
                users = _db.Users.Where(u => u.Email.ToLower().Contains(search.ToLower())).ToList();
            }
            else
            {
                if (option == "name" && search != null)
                {
                    users = _db.Users.Where(u => u.FirstName.ToLower().Contains(search.ToLower())
                            || u.LastName.ToLower().Contains(search.ToLower())).ToList();
                }
                else
                {
                    if (option == "phone" && search != null)
                    {
                        users = _db.Users.Where(u => u.PhoneNumber.ToLower().Contains(search.ToLower())).ToList();
                    }
                }
            }
            return View(users);
        }

        // GET Details
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationUser user = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET Edit
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationUser user = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // Post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", user);
            }
            else
            {
                var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Id == user.Id);
                if (userInDb == null)
                {
                    return NotFound();
                }
                else
                { 
                    userInDb.FirstName = user.FirstName;
                    userInDb.LastName = user.LastName;
                    userInDb.PhoneNumber = user.PhoneNumber;
                    userInDb.Address = user.Address;
                    userInDb.City = user.City;
                    userInDb.PostalCode = user.PostalCode;

                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }             
            }
        }


        // GET Delete
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationUser user = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        //Post Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (string id)
        {
            // gets the user object to be deleted
            var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);

            // gets all the cars that belong to the user
            var cars = _db.Cars.Where(x => x.UserId == userInDb.Id);

            // load the cars into a list 
            List<Car> listCar = cars.ToList();

            // iterate through each car to remove the services for that car
            foreach (var car in listCar)
            {
                var services = _db.Services.Where(x => x.CarId == car.Id);

                // removes all the services for the car
                _db.Services.RemoveRange(services);
            }

            // removes all the cars associated to this user
            _db.Cars.RemoveRange(cars);

            // finally removes the user
            _db.Users.Remove(userInDb);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
        }
    }
}
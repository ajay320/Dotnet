using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Task10Field.Data;
using Task10Field.Models;
using Task10Field.Services;

namespace Task10Field.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext _context;
        public IWebHostEnvironment _environment;
        public EmailSender _email;
        public HomeController(ApplicationDbContext context,IWebHostEnvironment environment,EmailSender email) { 
        
        _context = context;
            _environment = environment;
            _email = email;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult ShowData()
        {
          var data=  _context.Employee.ToList();
            return View(data);
        }

        [HttpPost]


        public async Task<IActionResult> Register(IFormCollection form,IFormFile AdharCard)
        {
            string folderName = Path.Combine(_environment.WebRootPath,"Images");
            string fileName = AdharCard.FileName;
            string path=Path.Combine(folderName,fileName);
            var stream = new FileStream(path,FileMode.Create);
             await AdharCard.CopyToAsync(stream);

            Employee model = new Employee()
            {
                FullName = form["FullName"],
                JoiningDate = DateOnly.TryParse(form["JoiningDate"], out DateOnly joiningDate) ? joiningDate : DateOnly.MinValue,
                Email = form["Email"],
                Gender = form["Gender"],
                Designation = form["Designation"],
                Address = form["Address"],
                Salary = int.Parse(form["Salary"]),
                EmployeeCode =int.Parse( form["EmployeeCode"]),
                AdharCard=fileName
            };
            var data = _context.Employee.Where(x => x.Email == model.Email).FirstOrDefault();
            if (data == null)
            {
                _context.Employee.Add(model);
                _context.SaveChanges();
                TempData["msg"] = "Your Form is Submited";
                string sendto = form["email"];
                string subject = "Registration Successfully";
                string message = "Dear user ,Your Registration in DigiCoder has been successfully compleated";
                await _email.SendEmailAsync(sendto, subject, message);
                return RedirectToAction("ShowData");
            }
            else
            {
                TempData["error"] = "This Email is Already added";
            }
            return View(model);
          
       
        }


    }
}

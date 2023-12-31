using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MVCAPP.Models;

namespace MVCAPP.Controllers
{
    public class UserLoginController : Controller
    {

       private static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("https://supportsystemapi.azurewebsites.net/api/"),
        };

    // private static HttpClient sharedClient = new()
    // {
    //     BaseAddress = new Uri("http://localhost:5173/api/"),
    // };


     


       

        public IActionResult Login()
        {
            return View();
        }

       

    /// How to Secure Passwords with BCrypt.NET
    /// [
    ///  var passwordHash = BCrypt.HashPassword("Password123!");
    /// ]
    /// https://code-maze.com/dotnet-secure-passwords-bcrypt/
    /// Acccessed[1 September 2023]

        [HttpPost]

       [HttpPost]

        public async Task<IActionResult> Login(UserLogin cred)
        {             

            try
            {
                HttpResponseMessage response = await sharedClient.PostAsJsonAsync("users/Login",cred);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("POST request successful");
                    try
                    {
                        var user = await response.Content.ReadFromJsonAsync<UserInfo>();
                        if(user.UserId==0)
                        {
                            response = await sharedClient.PostAsJsonAsync("users/Login",cred);
                            var dev = await response.Content.ReadFromJsonAsync<TeamDev>();
                            HttpContext.Session.SetInt32("DevId", dev.DevId);
                            HttpContext.Session.SetString("Name", dev.Name+" "+dev.Surname);
                            HttpContext.Session.SetString("Email", dev.Email);

                            HttpContext.Session.SetString("Role", "Staff");
                            return RedirectToAction("Index", "Admin");   
                        }
                        HttpContext.Session.SetInt32("UserId", user.UserId);
                        HttpContext.Session.SetString("Name", user.Name);
                        HttpContext.Session.SetString("Email", user.Email);

                        HttpContext.Session.SetString("Role", "Student");

                        return RedirectToAction("ViewTicket", "Ticket");

                        
                    }
                    //this will catch if they return a dev team object instead, 
                    catch(JsonException jsonEx)
                    {
                        var user = await response.Content.ReadFromJsonAsync<TeamDev>();
                        HttpContext.Session.SetInt32("DevId", user.DevId);
                        HttpContext.Session.SetString("Name", user.Name+" "+user.Surname);
                        HttpContext.Session.SetString("Email", user.Email);

                        HttpContext.Session.SetString("Role", "Staff");
                        return RedirectToAction("Index", "Admin");
                    }
                }
                else
                { 
                    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                    ModelState.AddModelError("Password", "Invalid username or password.");
                    return View();
                
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("Password", "An error occurred please try again.");
                Console.WriteLine($"Request error: {ex.Message}");
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "UserLogin");
        }

    }
}

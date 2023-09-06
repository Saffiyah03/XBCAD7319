using api.email;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Resources;
using System.Text.RegularExpressions;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController:ControllerBase
    {
        
        private readonly StudentSupportXbcadContext _context;

        public UsersController(StudentSupportXbcadContext context)
        {
            _context = context;
            
        }

        //TODO:end point that adds a user when given the details make sure email doesnt already exist in Userlogin table if it does not then add the user and thier login to the respective tables, use bcrypt to check the hashes 
        [HttpPost("add")]
        public async Task<IActionResult> addUser([FromBody]UserInfo user)
        {
            //check if the email already exists in the userlogin table
            var email = _context.UserLogin.Where(x=>x.Email==user.Email).FirstOrDefault();
            if(email!=null)
            {
                return BadRequest("Email already exists");
            }
            else
            {
                //add the user to the userlogin table
                _context.UserInfo.Add(user);
                _context.SaveChanges();
                return Ok("User added");
            }
        }


        /// How to Secure Passwords with BCrypt.NET
        /// [
        ///  var passwordHash = BCrypt.HashPassword("Password123!");
        /// ]
        /// https://code-maze.com/dotnet-secure-passwords-bcrypt/
        /// Acccessed[1 September 2023]
        [HttpPost("addlogin")]
        public async Task<IActionResult> addCredentials([FromBody]UserLogin user)
        {
            //check if the email already exists in the userlogin table
            var email = _context.UserLogin.Where(x=>x.Email==user.Email).FirstOrDefault();
            if(email!=null)
            {
                return BadRequest("Email already exists");
            }
            else
            {
                //hash the password
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                //add the user to the userlogin table
                _context.UserLogin.Add(user);
                _context.SaveChanges();
                return Ok("User added");
            }
        }

        //TODO:login:end point that will get the email and encrypted pasword and if successfull return the user object from db but check if the email exists in the dev table first, if it exists then return the devteam object
        [HttpGet("Login")]
        public async  Task<IActionResult> login(string? email,string?password)
        {
           //check if the user exists in the devteam table
            var dev = _context.DevTeam.Where(x=>x.Email==email).FirstOrDefault();
            if(dev!=null)
            {
                //return the devteam object
                return Ok(dev);
            }
            else
            {
                //check if the user exists in the userlogin table
                var user = _context.UserLogin.Where(x=>x.Email==email).FirstOrDefault();
                if(user!=null)
                {
                    //check if the password matches the hash
                    if(BCrypt.Net.BCrypt.Verify(password,user.Password))
                    {
                        //return the user object
                        return Ok(user);
                    }
                    else
                    {
                        return BadRequest("Incorrect password");
                    }
                }
                else
                {
                    return BadRequest("User does not exist");
                }
            }
        }

        //TODO: delete user when given the user object and delete thier object from the userlogin table by using the email
        [HttpDelete("remove")]
        public async Task<IActionResult> removeUser(string? userID)
        {
            //get the users email from the userlogin table
            var user = _context.UserLogin.Where(x=>x.UserId==userID).FirstOrDefault();
            string email = user.Email;

            //remove the user from the userlogin table
            _context.UserLogin.Remove(user);
            _context.SaveChanges();
            return Ok("User removed");
        }

    }

}
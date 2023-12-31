using api.email;
using api.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController:ControllerBase
    {
        private readonly XbcadDb2Context _context;

        public CategoryController(XbcadDb2Context context)
        {
            _context = context;           
        }

        //get the category id using the category name
        [HttpGet("getcategoryid")]
        public async Task<IActionResult> getCategoryId(string categoryName)
        {
            var category = _context.Categories.Where(x=>x.CategoryName==categoryName).FirstOrDefault();
            return Ok(category.CategoryId);
        }

        //get all the category names
        [HttpGet("getcategoryNames")]
        public async Task<IActionResult> getCategoryNames()
        {
            var category = _context.Categories.Select(x=>x.CategoryName).ToList();
            return Ok(category);
        }
        
        

    }


}


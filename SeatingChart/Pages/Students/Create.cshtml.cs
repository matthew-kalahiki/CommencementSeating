using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SeatingChart.Data;
using SeatingChart.Models;

namespace SeatingChart.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly SeatingChart.Data.ChartContext _context;

        public CreateModel(SeatingChart.Data.ChartContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int? ChartNum {get;set;}

        public IActionResult OnGet(int? chartNum)
        {
            ChartNum = chartNum;
            return Page();
        }

        [BindProperty]
        public String namesInput { get; set; } = default!;

        [BindProperty]
        public Student Student { get; set; } = default!;

        [BindProperty]
        public int IsGrad {get;set;}

        public async Task<IActionResult> OnPostAsync()
        {
            // Check if namesInput is not empty
            if (!string.IsNullOrWhiteSpace(namesInput))
            {
                // Split the input into an array of names 
                string[] namesArray = namesInput.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                foreach (var name in namesArray)
                {
                    var nameParts = name.Trim().Split(' ');

                    var newStudent = new Student
                    {
                        FirstName = nameParts[0],

                        MiddleName = nameParts.Length >= 3
                            ? nameParts.Skip(1).Take(nameParts.Length - 2).Aggregate((x, y) => x + " " + y)
                            : "",

                        LastName = nameParts.Length >= 2
                            ? nameParts.Last()
                            : "",
                        isGrad = IsGrad == 1 ? true : false
                    };
                    Console.WriteLine(IsGrad);
                    
                    _context.Students.Add(newStudent);
                }

                await _context.SaveChangesAsync();
                return RedirectToPage("./Index", new {chartNum = ChartNum.ToString()});
            }

            // Handle case where namesInput is empty
            ModelState.AddModelError("namesInput", "Please provide a list of names.");
            return Page();
        }
    }
}

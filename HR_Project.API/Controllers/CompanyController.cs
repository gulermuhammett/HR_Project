using HR_Project.Entities.Entities;
using HR_Project.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HR_Project.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IGenericService<Company> companyService;

        public CompanyController(IGenericService<Company> companyService)
        {
            this.companyService = companyService;
        }

        //services

        [HttpGet]
        public IActionResult GetAllCompanies()
        {
            return Ok(companyService.GetAll());
        }

        [HttpGet]
        public IActionResult GetAllActive()
        {
            return Ok(companyService.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult GetCompanyByID(int id)
        {
            return Ok(companyService.GetByID(id));
        }

        [HttpPost]
        public IActionResult CreateCompany([FromBody] Company company)
        {
            companyService.Add(company);
            return CreatedAtAction("GetCompanyByID", new { id = company.ID }, company);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCompany(int id, [FromBody] Company company)
        {
            if (id != company.ID)
                return BadRequest("IDs do not match. Please check and try again!");

            try
            {
                companyService.Update(company);
                return Ok(company);
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!companyService.Any(x => x.ID == id))
                {
                    return NotFound("There is no such user!");
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCompany(int id)
        {
            var company = companyService.GetByID(id);
            if (company == null)
                return NotFound();
            try
            {
                companyService.Remove(company);
                return Ok("Company silindi");
            }
            catch (DeletedRowInaccessibleException)
            {

                return BadRequest();
            }
        }

    }
}

using HR_Project.Entities.Entities;
using HR_Project.Repositories.Migrations;
using HR_Project.Services.Abstract;
using HR_Project.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Identity;
using System.Data;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HR_Project.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericService<User> serviceUser;
        private readonly IGenericService<Department> serviceDepartment;
        private readonly IGenericService<Job> serviceJob;
        private readonly IGenericService<Advance> serviceAdvance;

        public UserController(IGenericService<User> serviceUser, IGenericService<Department> serviceDepartment, IGenericService<Job> serviceJob, IGenericService<Advance> serviceAdvance)
        {
            this.serviceUser = serviceUser;
            this.serviceDepartment = serviceDepartment;
            this.serviceJob = serviceJob;
            this.serviceAdvance = serviceAdvance;
        }

        // GET: api/Login
        [HttpGet]
        public IActionResult Login(string email, string password)
        {
            if (serviceUser.Any(x => x.Email == email))
            {
                User loggeduser = serviceUser.GetByDefault(x => x.Email == email && x.Password == password);
                if (loggeduser != null)
                    return Ok(loggeduser);
                else
                    return BadRequest("Username or Password Incorrect!");
            }
            return NotFound("Username or Password Incorrect!");
        }
        //GET: api/User/GetAllUsers
        [HttpGet]
        public IActionResult GetAllJob()
        {
            return Ok(serviceJob.GetAll());
        }

        //GET: api/User/GetAllUsers
        [HttpGet]
        public IActionResult GetAllDepartment()
        {
            return Ok(serviceDepartment.GetAll());
        }


        //GET: api/User/GetAllUsers
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(serviceUser.GetAll());
        }

        [HttpGet]
        public IActionResult GetAllUserInclude()
        {
            return Ok(serviceUser.GetAll(t0 => t0.Job, t1 => t1.Department, t2 => t2.Company));
        }
        //GET: api/User/GetAllActive
        [HttpGet]

        public IActionResult GetAllActive()
        {
            return Ok(serviceUser.GetActive());
        }

        //GET: api/User/GetUserByID
        [HttpGet("{id}")]

        public IActionResult GetUserByID(int id)
        {
            return Ok(serviceUser.GetByID(id));
        }

        [HttpGet("{id}")]
        public IActionResult GetUserByIdInclude(int id)
        {
            return Ok(serviceUser.GetByID(id, t0 => t0.Job, t1 => t1.Department, t2 => t2.Company));
        }

        //GET: api/User/GetUserByName
        [HttpGet("{name}")]

        public IActionResult GetUserByName(string name)
        {
            return Ok(serviceUser.GetDefault(x => x.FirstName.ToLower().Contains(name.ToLower()) == true).ToList());
        }
        [HttpPost]

        //POST: api/User/CreateUser
        public IActionResult CreateUser([FromBody] User user)
        {
            //if (user.FirstName2 == null)
            //    user.FirstName2 = "";
            //user.Email = (user.FirstName.ToLower() + user.FirstName2.ToLower() + "." + user.LastName.ToLower() + "@bilgeadamboost.com").Replace("ü", "u").Replace("ö", "o").Replace("ı", "i").Replace("ş", "s").Replace("ç", "c").Replace("ğ", "g");
            //user.Password = Password.GeneratePassword();
            serviceUser.Add(user);

            return CreatedAtAction("GetUserByID", new { id = user.ID }, user);
        }

        //PUT: api/User/UpdateUser
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.ID)
                return BadRequest("IDs do not match. Please check and try again!");

            try
            {
                serviceUser.Update(user);
                return Ok(user);
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!serviceUser.Any(x => x.ID == id))
                {
                    return NotFound("There is no such user!");
                }
            }
            return NoContent();

            #region MyRegion
            //else if(!serviceUser.Any(x => x.ID == id))
            //{
            //    return NotFound("There is no such user!");
            //}
            //else
            //{
            //    serviceUser.Update(user);
            //    return Ok(user);
            //}
            #endregion

        }
        //Delete: api/User/DeleteUser
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = serviceUser.GetByID(id);
            if (user == null)
                return NotFound();
            try
            {
                serviceUser.Remove(user);
                return Ok("User silindi");
            }
            catch (DeletedRowInaccessibleException)
            {

                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult ActivateUser(int id)
        {
            var user = serviceUser.GetByID(id);
            if (user == null)
                return NotFound();
            try
            {
                serviceUser.Activate(id);
                return Ok(user);
            }
            catch (DeletedRowInaccessibleException)
            {

                return BadRequest();
            }
        }
        
        [HttpGet("{id}")]
        public IActionResult GetAllAdvances(int id)
        {
            return Ok(serviceAdvance.GetAll(x => x.UserID == id, t0 => t0.User).OrderByDescending(x => x.RequestDate));
        }

        [HttpGet("{id}")]
        public IActionResult GetAllCompanyAdvances(int id)
        {
            return Ok(serviceAdvance.GetAll(x => x.User.CompanyID == id, t0 => t0.User).OrderByDescending(x => x.RequestDate));
        }
        [HttpGet("{id}")]
        public IActionResult GetAllPendingAdvances(int id)
        {
            return Ok(serviceAdvance.GetAll(x => x.UserID == id, y => y.Status == Entities.Enums.Status.Pending, t0 => t0.User).OrderByDescending(x => x.RequestDate));
        }

        [HttpGet("{id}")]
        public IActionResult GetAllCanceledAdvances(int id)
        {
            return Ok(serviceAdvance.GetAll(x => x.UserID == id, y => y.Status == Entities.Enums.Status.Canceled).OrderByDescending(x => x.RequestDate));
        }

        [HttpGet("{id}")]
        public IActionResult GetAllConfirmedAdvances(int id)
        {
            return Ok(serviceAdvance.GetAll(x => x.UserID == id, y => y.Status == Entities.Enums.Status.Confirmed));
        }

        [HttpPost]
        public IActionResult CreateAdvance([FromBody] Advance advance)
        {
            advance.IsActive = true;
            advance.Status = Entities.Enums.Status.Pending;

            serviceAdvance.Add(advance);

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult DeleteAdvance(int id)
        {
            var advance = serviceAdvance.GetByID(id);
            if (advance == null)
                return NotFound();
            try
            {
                serviceAdvance.Delete(id);
                return Ok("Deleted Advance");
            }
            catch (DeletedRowInaccessibleException)
            {

                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult HoldAdvance(int id) //Avans talebini beklemeye al
        {
            var advance = serviceAdvance.GetByID(id);
            if (advance == null)
                return NotFound();
            else
            {
                advance.Status = Entities.Enums.Status.Pending;
                advance.TransactionDate = DateTime.Now;
                serviceAdvance.Update(advance);
                return Ok("Advance is pending");
            }
        }

        [HttpGet("{id}")]
        public IActionResult CancelAdvance(int id) //Avans talebini iptal et
        {
            var advance = serviceAdvance.GetByID(id);
            if (advance == null)
                return NotFound();
            else
            {
                advance.Status = Entities.Enums.Status.Canceled;
                advance.TransactionDate = DateTime.Now;
                serviceAdvance.Update(advance);
                return Ok("Advance is canceled");
            }
        }

        [HttpGet("{id}")]
        public IActionResult ApproveAdvance(int id) //Avans talebini onayla
        {
            var advance = serviceAdvance.GetByID(id);
            if (advance == null)
                return NotFound();
            else{
                advance.Status = Entities.Enums.Status.Confirmed;
                advance.TransactionDate = DateTime.Now;
                serviceAdvance.Update(advance);
                return Ok("Advance is confirmed");
            }
            
        }
    }
}

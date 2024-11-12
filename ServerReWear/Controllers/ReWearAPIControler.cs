using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerReWear.DTO;
using ServerReWear.Models;


namespace ServerReWear.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReWearAPIControler : ControllerBase
    {
        //a variable to hold a reference to the db context!
        private ShiriDBContext context;
        //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)
        private IWebHostEnvironment webHostEnvironment;
        //Use dependency injection to get the db context and web host into the constructor
        public ReWearAPIControler(ShiriDBContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.webHostEnvironment = env;
        }


        // הגדרת פעולת התחברות
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginInfo loginInfo)
        {
            try
            {

                //LogOut any user that is already logged in
                HttpContext.Session.Clear();

                // קבלת פרטי המשתמש ממסד הנתונים
                Models.User user = context.Users.FirstOrDefault(u => u.UserName == loginInfo.Username);

                // בדיקה האם המשתמש קיים
                if (user == null)
                {
                    return NotFound();
                }
                if (user.Password != loginInfo.Password)
                {
                    return Unauthorized();
                }
                // HttpContext.Session its an object that allows you to store temporary data for the current user.
                HttpContext.Session.SetString("LoggedInUser", user.UserName);

                // create a new DTO.User object based on the existing user object.
                DTO.UserDTO DTO_User = new DTO.UserDTO(user);
                // החזרת פרטי המשתמש
                return Ok(DTO_User);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] DTO.UserDTO userDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.User modelsUser = new User()
                {
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    Password = userDto.Password,
                    Phone= userDto.Phone
                };

                context.Users.Add(modelsUser);
                context.SaveChanges();

                //User was added!
                DTO.UserDTO dtoUser = new DTO.UserDTO(modelsUser);
                return Ok(dtoUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

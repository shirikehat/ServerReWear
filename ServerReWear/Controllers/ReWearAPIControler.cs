using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerReWear.DTO;
using ServerReWear.Models;
using System.Text.RegularExpressions;


namespace ServerReWear.Controllers
{
    [Route("api")]
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
                Models.User? user = context.GetUser(loginInfo.Username);

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
                DTO_User.ProfileImagePath = GetProfileImageVirtualPath(DTO_User.Id);
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
                Models.User modelsUser = userDto.GetModel();

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


        [HttpPost("update")]
        public IActionResult UpdateUser([FromBody] DTO.UserDTO userDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.User modelsUser = userDto.GetModel();

                context.Users.Update(modelsUser);
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

        [HttpPost("UploadProfileImage")]
        public async Task<IActionResult> UploadProfileImageAsync(IFormFile file)
        {
            //Check if who is logged in
            string? userName = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("User is not logged in");
            }

            //Get model user class from DB with matching username. 
            Models.User? user = context.GetUser(userName);
            //Clear the tracking of all objects to avoid double tracking
            context.ChangeTracker.Clear();

            if (user == null)
            {
                return Unauthorized("User is not found in the database");
            }


            //Read all files sent
            long imagesSize = 0;

            if (file.Length > 0)
            {
                //Check the file extention!
                string[] allowedExtentions = { ".png", ".jpg" };
                string extention = "";
                if (file.FileName.LastIndexOf(".") > 0)
                {
                    extention = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
                }
                if (!allowedExtentions.Where(e => e == extention).Any())
                {
                    //Extention is not supported
                    return BadRequest("File sent with non supported extention");
                }

                //Build path in the web root (better to a specific folder under the web root
                string filePath = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{user.UserId}{extention}";

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);

                    if (IsImage(stream))
                    {
                        imagesSize += stream.Length;
                    }
                    else
                    {
                        //Delete the file if it is not supported!
                        System.IO.File.Delete(filePath);
                    }

                }

            }

            DTO.UserDTO dtoUser = new DTO.UserDTO(user);
            dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
            return Ok(dtoUser);
        }

        //Helper functions

        //this function gets a file stream and check if it is an image
        private static bool IsImage(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            List<string> jpg = new List<string> { "FF", "D8" };
            List<string> bmp = new List<string> { "42", "4D" };
            List<string> gif = new List<string> { "47", "49", "46" };
            List<string> png = new List<string> { "89", "50", "4E", "47", "0D", "0A", "1A", "0A" };
            List<List<string>> imgTypes = new List<List<string>> { jpg, bmp, gif, png };

            List<string> bytesIterated = new List<string>();

            for (int i = 0; i < 8; i++)
            {
                string bit = stream.ReadByte().ToString("X2");
                bytesIterated.Add(bit);

                bool isImage = imgTypes.Any(img => !img.Except(bytesIterated).Any());
                if (isImage)
                {
                    return true;
                }
            }

            return false;
        }
        //this function check which product image exist and return the virtual path of it.
        //if it does not exist it returns the default product image virtual path
        private string GetProductImageVirtualPath(int productId)
        {
            string virtualPath = $"/productImages/{productId}";
            string path = $"{this.webHostEnvironment.WebRootPath}\\productImages\\{productId}.png";
            if (System.IO.File.Exists(path))
            {
                virtualPath += ".png";
            }
            else
            {
                path = $"{this.webHostEnvironment.WebRootPath}\\productImages\\{productId}.jpg";
                if (System.IO.File.Exists(path))
                {
                    virtualPath += ".jpg";
                }
                else
                {
                    virtualPath = $"/productImages/product.png";
                }
            }

            return virtualPath;
        }

        //this function check which profile image exist and return the virtual path of it.
        //if it does not exist it returns the default profile image virtual path
        private string GetProfileImageVirtualPath(int userId)
        {
            string virtualPath = $"/profileImages/{userId}";
            string path = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{userId}.png";
            if (System.IO.File.Exists(path))
            {
                virtualPath += ".png";
            }
            else
            {
                path = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{userId}.jpg";
                if (System.IO.File.Exists(path))
                {
                    virtualPath += ".jpg";
                }
                else
                {
                    virtualPath = $"/profileImages/default.png";
                }
            }

            return virtualPath;
        }



        [HttpPost("GetProducts")]
        public IActionResult GetProducts([FromBody] UserDTO theUser)
        {
            try
            {
                //Check if who is logged in
                string? userName = HttpContext.Session.GetString("LoggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in");
                }

                User? u = context.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (u == null)
                {
                    return Unauthorized("User is not logged in");
                }

                List<Product> products = context.Products.Include(p => p.User).Where(p => p.UserId == theUser.Id && p.StatusId==1).ToList();

                List<ProductDTO> dtoProducts = new List<ProductDTO>();
                foreach (var product in products)
                {
                    ProductDTO p = new ProductDTO(product, this.webHostEnvironment.WebRootPath);
                    if (p.User != null)
                        p.User.ProfileImagePath = GetProfileImageVirtualPath(p.UserId);
                    dtoProducts.Add(p);
                }

                return Ok(dtoProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            try
            {
                //Check if who is logged in
                string? userName = HttpContext.Session.GetString("LoggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in");
                }

                User? u = context.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (u == null)
                {
                    return Unauthorized("User is not logged in");
                }

                List<Product> products = context.Products.Include(p => p.User).Where(p => p.StatusId == 1).ToList();

                List<ProductDTO> dtoProducts = new List<ProductDTO>();
                foreach (var product in products)
                {
                    ProductDTO p = new ProductDTO(product, this.webHostEnvironment.WebRootPath);
                    if (p.User != null)
                        p.User.ProfileImagePath = GetProfileImageVirtualPath(p.UserId);
                    dtoProducts.Add(p);
                }

                return Ok(dtoProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetBasicData")]
        public IActionResult GetBasicData()
        {
            try
            {


                List<Models.Status> statuses = context.Statuses.ToList();
                List<Models.Type> types = context.Types.ToList();
                BasicData basicData = new BasicData(statuses, types);

                return Ok(basicData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("AddToCart")]
        public IActionResult AddToCart([FromBody] int productCode)
        {
            try
            {
                //Check if who is logged in
                string? userName = HttpContext.Session.GetString("LoggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in");
                }

                User? u = context.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (u == null)
                {
                    return Unauthorized("User is not logged in");
                }

                Cart c = new Cart()
                {
                    UserId = u.UserId,
                    ProductCode = productCode
                };

                context.Carts.Add(c);
                context.SaveChanges();

                CartDTO dto = new CartDTO(c, this.webHostEnvironment.WebRootPath);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddToWishlist")]
        public IActionResult AddToWishlist([FromBody] int productCode)
        {
            try
            {
                //Check if who is logged in
                string? userName = HttpContext.Session.GetString("LoggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in");
                }

                User? u = context.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (u == null)
                {
                    return Unauthorized("User is not logged in");
                }

                WishList w = new WishList()
                {
                    UserId = u.UserId,
                    ProductCode = productCode
                };

                context.WishLists.Add(w);
                context.SaveChanges();
                WishlistDTO dto = new WishlistDTO(w, this.webHostEnvironment.WebRootPath);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }




        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                //Check if who is logged in
                string? userName = HttpContext.Session.GetString("LoggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in");
                }

                User? u = context.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (u == null)
                {
                    return Unauthorized("User is not logged in");
                }

                List<User> users = context.Users.ToList();

                List<UserDTO> dtoUsers = new List<UserDTO>();
                foreach (var user in users)
                {
                    UserDTO us = new UserDTO(user);
                    us.ProfileImagePath = GetProfileImageVirtualPath(us.Id);
                    dtoUsers.Add(us);
                }

                return Ok(dtoUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetCart")]
        public IActionResult GetCart()
        {
            try
            {
                //Check if who is logged in
                string? userName = HttpContext.Session.GetString("LoggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in");
                }

                User? u = context.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (u == null)
                {
                    return Unauthorized("User is not logged in");
                }

                List<Cart> carts = context.Carts.Include(p => p.ProductCodeNavigation).Where(p => p.UserId == u.UserId && p.ProductCodeNavigation.StatusId== 1).ToList();

                List<CartDTO> dtoCarts = new List<CartDTO>();
                foreach (var cart in carts)
                {
                    CartDTO c = new CartDTO(cart, this.webHostEnvironment.WebRootPath);
                    dtoCarts.Add(c);
                }

                return Ok(dtoCarts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpGet("GetWishlist")]
        public IActionResult GetWishlist()
        {
            try
            {
                //Check if who is logged in
                string? userName = HttpContext.Session.GetString("LoggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in");
                }

                User? u = context.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (u == null)
                {
                    return Unauthorized("User is not logged in");
                }

                List<WishList> wishlists = context.WishLists.Include(p => p.ProductCodeNavigation).Where(p => p.UserId == u.UserId && p.ProductCodeNavigation.StatusId == 1).ToList();

                List<WishlistDTO> dtoWishlists = new List<WishlistDTO>();
                foreach (var wishlist in wishlists)
                {
                    WishlistDTO w = new WishlistDTO(wishlist, this.webHostEnvironment.WebRootPath);
                    dtoWishlists.Add(w);
                }

                return Ok(dtoWishlists);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetOrders")]
        public IActionResult GetOrders()
        {
            try
            {
                //Check if who is logged in
                string? userName = HttpContext.Session.GetString("LoggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in");
                }

                User? u = context.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (u == null)
                {
                    return Unauthorized("User is not logged in");
                }

                List<OrdersFrom> ordersFroms = context.OrdersFroms.Include(p => p.ProductCodeNavigation).Where(p => p.UserId == u.UserId).ToList();

                List<OrderDTO> dtoOrders = new List<OrderDTO>();
                foreach (var order in ordersFroms)
                {
                    OrderDTO o = new OrderDTO(order, this.webHostEnvironment.WebRootPath);
                    dtoOrders.Add(o);
                }

                return Ok(dtoOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Block")]
        public IActionResult Block([FromBody] DTO.UserDTO u)
        {
            try
            {
                //Create model user class
                Models.User user = context.GetUser1(u.Id);
                user.IsBlocked = u.IsBlocked;
                context.Entry(user).State = EntityState.Modified;

                context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("RemoveCart")]
        public IActionResult RemoveCart([FromQuery] int cartId)
        {
            try
            {
                //Create model user class
                context.DeleteCart(cartId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet("RemoveWishlist")]
        public IActionResult RemoveWishlist([FromQuery] int wishlistId)
        {
            try
            {
                //Create model user class
                context.DeleteWishlist(wishlistId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost("PostProduct")]
        public async Task<IActionResult> PostProduct([FromBody] DTO.ProductDTO product_dto)
        {
            try
            {
                if (product_dto == null)
                {
                    return BadRequest("Invalid user data.");
                }

                //Check if who is logged in
                string? userName = HttpContext.Session.GetString("LoggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in");
                }

                User? u = context.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (u == null)
                {
                    return Unauthorized("User is not logged in");
                }

                if (u.UserId != product_dto.UserId)
                {
                    return Unauthorized($"User with id: {u.UserId} is trying to post product for user {product_dto.UserId}");
                }

                // יצירת מוצר בהתבסס על הקלט מהמשתמש
                product_dto.StatusId = 1;
                Models.Product modelproduct = new Models.Product
                {
                    UserId = product_dto.UserId,
                    Description = product_dto.Description,
                    Size = product_dto.Size,
                    Price = product_dto.Price,
                    StatusId = product_dto.StatusId,
                    Store = product_dto.Store,
                    TypeId = product_dto.TypeId,

                };
                // הוספת המשתמש למסד הנתונים
                context.Products.Add(modelproduct);
                await context.SaveChangesAsync(); // שמירת השינויים במסד הנתונים
                return Ok(modelproduct.ProductCode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }




        [HttpPost("AddType")]
        public async Task<IActionResult> AddType([FromBody] DTO.PrType type_dto)
        {
            try
            {
                if (type_dto == null)
                {
                    return BadRequest("Invalid user data.");
                }

                //Check if who is logged in
                string? userName = HttpContext.Session.GetString("LoggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in");
                }

                User? u = context.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (u == null)
                {
                    return Unauthorized("User is not logged in");
                }



                // יצירת  בהתבסס על הקלט מהמשתמש

                Models.Type modeltype = new Models.Type
                {
                    Name = type_dto.Name

                };
                // הוספת  למסד הנתונים
                context.Types.Add(modeltype);
                await context.SaveChangesAsync(); // שמירת השינויים במסד הנתונים
                return Ok(modeltype.TypeCode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("BuyProduct")]
        public async Task<IActionResult> BuyProduct([FromBody] DTO.OrderDTO order_dto)
        {
            try
            {
                if (order_dto == null)
                {
                    return BadRequest("Invalid user data.");
                }

                //Check if who is logged in
                string? userName = HttpContext.Session.GetString("LoggedInUser");
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not logged in");
                }

                User? u = context.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (u == null)
                {
                    return Unauthorized("User is not logged in");
                }
                // יצירת הזמנה בהתבסס על הקלט מהמשתמש
                Models.OrdersFrom modelorder = order_dto.GetModel();

                Product? p = context.Products.Where(u => u.ProductCode == modelorder.ProductCode).FirstOrDefault();
                if (p != null)
                {
                    p.StatusId = 2; //Bought
                }

                context.Products.Update(p);
                // הוספת המשתמש למסד הנתונים
                context.OrdersFroms.Add(modelorder);
                await context.SaveChangesAsync(); // שמירת השינויים במסד הנתונים
                return Ok(modelorder.ProductCode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }





    } 
}


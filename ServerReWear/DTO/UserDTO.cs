namespace ServerReWear.DTO
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public UserDTO() { }
        public UserDTO(Models.User modeluser)
        {
            this.UserName = modeluser.UserName;
            this.Password = modeluser.Password;
            this.Phone = modeluser.Phone;
            this.Email = modeluser.Email;
        }
    }
}

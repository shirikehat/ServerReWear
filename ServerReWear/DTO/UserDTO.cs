﻿namespace ServerReWear.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsManager { get; set; }
        public bool IsBlocked { get; set; }
        public string ProfileImagePath { get; set; } = "";

        public UserDTO() { }
        public UserDTO(Models.User modeluser)
        {
            this.Id = modeluser.UserId;
            this.UserName = modeluser.UserName;
            this.Password = modeluser.Password;
            this.Phone = modeluser.Phone;
            this.Email = modeluser.Email;
            this.IsManager = modeluser.IsManager;
            this.IsBlocked = modeluser.IsBlocked;
        }

        public Models.User GetModel()
        {
            return new Models.User()
            {
                UserId = this.Id,
                UserName = this.UserName,
                Password = this.Password,
                Phone = this.Phone,
                Email = this.Email,
                IsManager= this.IsManager,  
            };
        }

    }
}

using Market.Entities;

namespace Market.Models
{
    public class UserInfo
    {
        public string Id { get; set; }

        public string? Avatar { get; set; }

        public string? Intro { get; set; }

        public string? NickName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string StudentId { get; set; }

        public int Status { get; set; }

        public long UpdateTime { get; set; }

        public long CreateTime { get; set; }

        public string? Address { get; set; }
    
        public static UserInfo FromUser(User user) {
            return new UserInfo
            {
                Id = user.Id,
                Avatar = user.Avatar,
                Intro = user.Intro,
                NickName = user.NickName,
                Username = user.Username,
                Email = user.Email,
                StudentId = user.StudentId,
                Status = user.Status,
                UpdateTime = user.UpdateTime,
                CreateTime = user.CreateTime,
                Address = user.Address
            };
        }
    }
}
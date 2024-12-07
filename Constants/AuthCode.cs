namespace Market.Constants
{
    public partial class AuthCode
    {
        public static readonly AuthCode UserLogin = new AuthCode(1001001, "登录错误");
        public static readonly AuthCode UserLoginNotExist = new AuthCode(1001002, "用户不存在");
        public static readonly AuthCode UserLoginExist = new AuthCode(1001003, "用户已存在");
        public static readonly AuthCode UserLoginPasswordError = new AuthCode(1001004, "密码错误");
        public static readonly AuthCode UserLoginWeChatExist = new AuthCode(1001005, "微信用户已存在");
        public static readonly AuthCode UserPermission = new AuthCode(1001006, "访问权限异常");
        public static readonly AuthCode UserPermissionUnauthenticated = new AuthCode(1001007, "请登录");
        public static readonly AuthCode UserPermissionExpired = new AuthCode(1001008, "请重新登录");
        public static readonly AuthCode UserPermissionUnauthorized = new AuthCode(1001009, "权限不足");
        public int Code { get; }
        public string Name { get; set; }
        private AuthCode(int code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
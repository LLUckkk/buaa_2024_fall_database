namespace Market.Constants
{
    public partial class ResultCode
    {
        public static readonly ResultCode Success = new ResultCode(1, "操作成功");
        public static readonly ResultCode Fail = new ResultCode(0, "操作失败");
        public static readonly ResultCode NotFindError = new ResultCode(10001, "未查询到信息");
        public static readonly ResultCode SaveError = new ResultCode(10002, "保存信息失败");
        public static readonly ResultCode UpdateError = new ResultCode(10003, "更新信息失败");
        public static readonly ResultCode DeleteError = new ResultCode(100012, "删除信息失败");
        public static readonly ResultCode ValidateError = new ResultCode(10004, "数据检验失败");
        public static readonly ResultCode StatusHasValid = new ResultCode(10005, "状态已经被启用");
        public static readonly ResultCode StatusHasInvalid = new ResultCode(10006, "状态已经被禁用");
        public static readonly ResultCode SystemError = new ResultCode(10007, "系统异常");
        public static readonly ResultCode BusinessError = new ResultCode(10008, "业务异常");
        public static readonly ResultCode TransferStatusError = new ResultCode(10010, "当前状态不正确，请勿重复提交");
        public static readonly ResultCode NotGrant = new ResultCode(10011, "没有操作该功能的权限，请联系管理员");
        public int Code { get; }
        public string Name { get; set; }
        private ResultCode(int code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
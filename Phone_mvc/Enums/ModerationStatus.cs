using System.ComponentModel;

namespace Phone_mvc.Enums
{
    public enum ModerationStatus
    {
        [Description("Đã duyệt")]
        Approved = 0,
        [Description("Từ chối")]
        Rejected = 1,
    }
}

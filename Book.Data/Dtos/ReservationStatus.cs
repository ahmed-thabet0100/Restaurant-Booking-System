using System.Runtime.Serialization;

namespace Restaurant.Data.Dtos
{
    public enum ReservationStatus
    {
        [EnumMember(Value = "pending")]
        Pending,
        [EnumMember(Value = "Approved")]
        Approved,
        [EnumMember(Value = "Rejected")]
        Rejected,
        [EnumMember(Value = "Completed")]
        Completed,
        [EnumMember(Value = "Cancelled")]
        Cancelled
    }
}

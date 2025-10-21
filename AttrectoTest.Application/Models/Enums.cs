using System.Runtime.Serialization;


namespace AttrectoTest.Application.Models;

public enum ListSort
{

    [EnumMember(Value = @"createdAt_desc")]
    CreatedAtDesc = 0,

    [EnumMember(Value = @"createdAt_asc")]
    CreatedAtAsc = 1,

    [EnumMember(Value = @"likes_desc")]
    LikesDesc = 2,

    [EnumMember(Value = @"likes_asc")]
    LikesAsc = 3,

}

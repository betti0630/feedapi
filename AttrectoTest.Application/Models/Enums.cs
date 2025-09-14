using System.Runtime.Serialization;


namespace AttrectoTest.Application.Models;

public enum ListSort
{

    [EnumMember(Value = @"createdAt_desc")]
    CreatedAt_desc = 0,

    [EnumMember(Value = @"createdAt_asc")]
    CreatedAt_asc = 1,

    [EnumMember(Value = @"likes_desc")]
    Likes_desc = 2,

    [EnumMember(Value = @"likes_asc")]
    Likes_asc = 3,

}

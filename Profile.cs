using AutoMapper;
using BackEnd.Model.Auth;

namespace BackEnd.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<DbUser, UserModel>();
        CreateMap<UserModel, DbUser>();
    }
}

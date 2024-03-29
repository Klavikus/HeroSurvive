using System;
using CodeBase.GameCore.Domain.Data;
using CodeBase.GameCore.Infrastructure.Services;

namespace CodeBase.GameCore.Domain.Services
{
    public interface IAuthorizeService : IService
    {
        event Action Authorized;
        event Action AuthorizeError;
        event Action<UserData> UserDataUpdated;
        public bool IsAuthorized { get; }
        void Authorize();
        UserData GetUserData();
    }
}
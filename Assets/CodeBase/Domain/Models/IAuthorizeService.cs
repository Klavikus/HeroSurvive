﻿using System;
using CodeBase.Infrastructure.Services;

namespace CodeBase.Domain.Models
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
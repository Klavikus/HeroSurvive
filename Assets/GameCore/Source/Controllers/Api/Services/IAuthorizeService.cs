﻿using System;
using GameCore.Source.Domain.Data;

namespace GameCore.Source.Controllers.Api.Services
{
    public interface IAuthorizeService
    {
        event Action Authorized;
        event Action AuthorizeError;
        event Action<UserData> UserDataUpdated;
        public bool IsAuthorized { get; }
        void Authorize();
        UserData GetUserData();
    }
}
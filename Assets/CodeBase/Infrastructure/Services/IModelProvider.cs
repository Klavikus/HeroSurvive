﻿using CodeBase.MVVM.Models;

namespace CodeBase.Infrastructure.Services
{
    public interface IModelProvider : IService
    {
        public GameLoopModel GameLoopModel { get; }
    }
}
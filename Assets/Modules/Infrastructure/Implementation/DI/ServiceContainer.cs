using System;
using System.Collections.Generic;
using System.Data;

namespace Modules.Infrastructure.Implementation.DI
{
    public class ServiceContainer
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        
        private bool _isRegisterLocked;

        public void RegisterAsSingle<TService>(TService implementation) where TService : class
        {
            if (_isRegisterLocked)
                throw new EvaluateException(
                    $"Service registration was locked, but trying to register service - {typeof(TService)}");

            _services.Add(typeof(TService), implementation);
        }

        public TService Single<TService>() where TService : class => 
            _services[typeof(TService)] as TService;

        public void LockRegister() => _isRegisterLocked = true;
    }
}
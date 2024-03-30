using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Source.Controllers.Api;

namespace GameCore.Source.Controllers.Core.StateMachines
{
    public abstract class EntityState : IEntityState
    {
        private List<Transition> _transitions = new();

        public void SetTransitions(params Transition[] transitions) =>
            _transitions = transitions.ToList();

        public void Update()
        {
            OnUpdate();
            foreach (Transition transition in _transitions)
                transition.Update();
        }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            foreach (Transition transition in _transitions)
                transition?.Dispose();
        }
    }
}
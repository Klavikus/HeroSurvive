using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeBase.Domain
{
    abstract class EntityState : IEntityState
    {
        private List<Transition> _transitions;

        public event Action<IEntityState> NeedChangeState;

        public void SetTransitions(params Transition[] transitions) => _transitions = transitions.ToList();

        public virtual void Enter() => SubscribeOnTransitions();

        public virtual void Exit() => UnsubscribeOnTransitions();

        public virtual void Update()
        {
        }

        private void SubscribeOnTransitions()
        {
            if (_transitions == null)
                return;

            foreach (Transition transition in _transitions)
                transition.NeedChangeState += ChangeState;
        }

        private void UnsubscribeOnTransitions()
        {
            if (_transitions == null)
                return;

            foreach (Transition transition in _transitions)
                transition.NeedChangeState -= ChangeState;
        }

        private void ChangeState(IEntityState nextEntityState) => NeedChangeState?.Invoke(nextEntityState);

        public void Dispose()
        {
            UnsubscribeOnTransitions();
            GC.SuppressFinalize(this);
        }
    }
}
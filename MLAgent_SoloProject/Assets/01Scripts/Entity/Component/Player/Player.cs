using MLAgent.Entities.Components;

namespace MLAgent.Entities
{
    public class Player : Entity
    {
        private EntityMover _entityMover;

        protected override void AfterInitComponents()
        {
            base.AfterInitComponents();
            _entityMover = GetCompo<EntityMover>();
        }

        protected override void DisposeComponents()
        {
            base.DisposeComponents();
        }
    }

}

using UnityEngine;

namespace MLAgent.Entities.Components
{
    public class EntityMover : MonoBehaviour, IEntityComponent
    {
        private Vector2 _movement;
        public Vector2 Velocity => _rbCompo.velocity;
        public bool CanManualMove { get; set; } = true; //키보드로 움직임 가능

        private Rigidbody _rbCompo;
        private Entity _entity;


        private Collider _collider;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _rbCompo = _entity.GetComponent<Rigidbody>();
            _collider = _entity.GetComponent<Collider>();
        }


        public void AddForceToEntity(Vector3 force, ForceMode mode = ForceMode.Impulse)
        {
            _rbCompo.AddForce(force, mode);
        }

        public void StopImmediately()
        {
            _rbCompo.velocity = Vector2.zero;
            _movement = Vector2.zero;
        }

        public void SetMovement(Vector2 movement) => _movement = movement;

        private void FixedUpdate()
        {
            MoveCharacter();
        }

        private void MoveCharacter()
        {
            if (CanManualMove)
                _rbCompo.velocity = _movement;
        }
    }
}

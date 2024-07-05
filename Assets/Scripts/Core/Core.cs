using Core.CoreComponents;
using Generics;
using UnityEngine;

namespace Core
{
    public class Core : MonoBehaviour
    {
        public Movement Movement
        {
            get => GenericNotImplementedError<Movement>.TryGet(movement, transform.parent.name);
            private set => movement = value;
        }
        public CollisionSenses CollisionSenses
        {
            get => GenericNotImplementedError<CollisionSenses>.TryGet(collisionSenses, transform.parent.name);
            private set => collisionSenses = value;
        }
 

        private Movement movement;
        private CollisionSenses collisionSenses;
  

        private void Awake()
        {
            Movement = GetComponentInChildren<Movement>();
            CollisionSenses = GetComponentInChildren<CollisionSenses>();
      
        }

        public void LogicUpdate()
        {
            Movement.LogicUpdate();
       
        }

    }
}
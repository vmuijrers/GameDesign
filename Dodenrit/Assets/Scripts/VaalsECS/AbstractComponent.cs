using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VaalsECS
{
    [RequireComponent(typeof(Entity))]
    public abstract class AbstractComponent : MonoBehaviour
    {
        private Entity entity;
        public Entity Entity
        {
            get { return entity; }
            private set { entity = value; }
        }
        protected virtual void Awake()
        {
            //Debug.Log("Awake! " + this.name);
            entity = GetComponent<Entity>();
        }

        protected virtual void OnEnable()
        {
            //Debug.Log("Registering a component! " + this.name);
            if(entity == null)
            {
                entity = GetComponent<Entity>();
            }
                
            entity.RegisterComponent(this, GetType());
        }
        protected virtual void OnDisable()
        {
            entity.UnRegisterComponent(this, GetType());
        }
    }
}


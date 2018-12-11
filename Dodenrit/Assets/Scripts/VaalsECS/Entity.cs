using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VaalsECS
{
    public class Entity : MonoBehaviour
    {

        public Dictionary<System.Type, object> components = new Dictionary<System.Type, object>();
        // Use this for initialization
        void Start()
        { 

            //EntityManager.RegisterEntity(this);
        }

        public bool HasActiveComponent<T>()
        {
            if (components.ContainsKey(typeof(T)))
            {
                return true;
            }
            return false;
        }
        public bool HasActiveComponent(System.Type obj)
        {
            if (components.ContainsKey(obj))
            {
                return true;
            }
            return false;
        }

        public T GetCachedComponent<T>()
        {
            if (components.ContainsKey(typeof(T)))
            {
                return (T)components[typeof(T)];
            }
            return default(T);
        }
        public T GetCachedComponent<T>(T obj)
        {
            if (components.ContainsKey(obj.GetType()))
            {
                return (T)components[obj.GetType()];
            }
            return default(T);
        }
        public void RegisterComponent<T>(T obj, System.Type runtimeType, bool raiseEvent = false)
        {
            if (!components.ContainsKey(runtimeType /*obj.GetType()*/))
            {
                //Debug.Log("Registered a component on Entity: " +obj.GetType().Name + " " + name);
                components.Add(runtimeType/*obj.GetType()*/, obj);

                if (raiseEvent)
                {
                    Message<T>.SendMessage(MessageEnum.ON_ADD_ABSTRACT_COMPONENT, obj);
                }
            }
            //Todo: Register per component to entitymanager
            EntityManager.RegisterEntityWithComponent(this, runtimeType/*typeof(T)obj.GetType()*/);
        }


        public void UnRegisterComponent<T>(T obj, System.Type runTimeType, bool raiseEvent = false)
        {

            //Todo: UnRegister per component to entitymanager
            if (raiseEvent)
            {
                Message<T>.SendMessage(MessageEnum.ON_REMOVE_ABSTRACT_COMPONENT, obj);
            }
            EntityManager.UnRegisterEntityWithComponent(this, runTimeType/*typeof(T) obj.GetType()*/);
            if (components.ContainsKey(runTimeType /*typeof(T)obj.GetType()*/))
            {
                components.Remove(runTimeType/* typeof(T) obj.GetType()*/);
            }

        }
    }

}

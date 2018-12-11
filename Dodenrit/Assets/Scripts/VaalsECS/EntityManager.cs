using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VaalsECS
{
    public class EntityManager
    {
        private static Dictionary<System.Type, List<Entity>> entityDictionary = new Dictionary<System.Type, List<Entity>>();

        public static List<Entity> GetEntities(System.Type[] exclude = null, params System.Type[] types)
        {
            if(types.Length == 1 && exclude == null && entityDictionary.ContainsKey(types[0])) { return entityDictionary[types[0]]; }
            //foreach(KeyValuePair<System.Type, List<Entity>> kvp in entityDictionary){
            //    Debug.Log("Dicitionary keys: " + kvp.Key.ToString());
            //}

            //foreach(System.Type t in types)
            //{
            //    Debug.Log("Looking for: " + t);
            //}
            HashSet<Entity> entitySet = new HashSet<Entity>();

            //Find all wanted entities
            foreach (System.Type mb in types)
            {
                if (entityDictionary.ContainsKey(mb))
                {
                    if (entitySet.Count == 0)
                    {
                        entitySet.UnionWith(entityDictionary[mb]);
                    }
                    else
                    {
                        entitySet.IntersectWith(entityDictionary[mb]);
                    }
                }
            }

            if(exclude != null)
            {
                //Remove all unwanted entities
                foreach (System.Type t in exclude)
                {
                    if (entityDictionary.ContainsKey(t))
                    {
                        entitySet.ExceptWith(entityDictionary[t]);
                    }

                }
            }


            return entitySet.ToList();
        }

        public static void RegisterEntityWithComponent(Entity e,System.Type component)
        {
            if (entityDictionary.ContainsKey(component))
            {
                //Debug.Log("Registered an Entity in Manager To List: " + component.ToString() + " " + e.name);
                entityDictionary[component].Add(e);
            }
            else
            {
                //Debug.Log("Registered an Entity in Manager To List: " + component.ToString() + " " + e.name);
                entityDictionary.Add(component, new List<Entity>() { e });
            }
        }
        public static void UnRegisterEntityWithComponent(Entity e, System.Type component)
        {
            if (entityDictionary.ContainsKey(component))
            {
                entityDictionary[component].Remove(e);
            }
        }

        public static void RegisterEntity(Entity e)
        {
            foreach (KeyValuePair<System.Type, object> vk in e.components)
            {
                Debug.Log("Registered an Entity in Manager To List: " + vk.Key.ToString() + " " + e.name);
                if (entityDictionary.ContainsKey(vk.Key))
                {
                    entityDictionary[vk.Key].Add(e);
                }
                else
                {
                    entityDictionary.Add(vk.Key, new List<Entity>() { e });
                }
            }
        }

        public static void UnRegisterEntity(Entity e)
        {
            foreach (KeyValuePair<System.Type, List<Entity>> vk in entityDictionary)
            {
                if (entityDictionary[vk.Key].Contains(e))
                {
                    entityDictionary[vk.Key].Remove(e);
                }
            }
        }
    }

}

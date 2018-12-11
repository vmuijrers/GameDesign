using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace VaalsECS
{
    public class Context : MonoBehaviour
    {

        private System.Action OnUpdateAction;


        void OnEnable()
        {
           

        }

        void OnDisabled()
        {
         
        }

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (OnUpdateAction != null)
            {
                OnUpdateAction.Invoke();
            }

        }
    }

}




using System.Collections;
using UnityEngine;

namespace GasRepairsAndProbablyExpensiveSnacks
{
    public class StaticCoroutine : MonoBehaviour
    {
        // A custom class that allows execution of a Coroutine within a static method, something you're normally not allowed to do

        // this
        private static StaticCoroutine instance;

        // when destroyed (as Unity doesn't stop coroutines until they are physically destroyed)
        private void OnDestroy()
        { 
            instance.StopAllCoroutines(); 
        }

        // not applicable for KSP but left for reference
        private void OnApplicationQuit()
        { 
            instance.StopAllCoroutines(); 
        }

        // Coroutine creator, searches for existing, then creates new static instance if none found
        private static StaticCoroutine Build()
        {
            if (instance != null)
            { 
                return instance; 
            }

            instance = (StaticCoroutine)FindObjectOfType(typeof(StaticCoroutine));

            if (instance != null)
            { 
                return instance; 
            }

            GameObject instanceObject = new GameObject("StaticCoroutine");
            instanceObject.AddComponent<StaticCoroutine>();
            instance = instanceObject.GetComponent<StaticCoroutine>();

            if (instance != null)
            { 
                return instance; 
            }

            Debug.LogError("Build did not generate a replacement instance. Method Failed!");

            return null;
        }

        // start the coroutine
        public static void Start(string methodName)
        { 
            Build().StartCoroutine(methodName); 
        }

        public static void Start(string methodName, object value)
        { 
            Build().StartCoroutine(methodName, value); 
        }

        public static void Start(IEnumerator routine)
        { 
            Build().StartCoroutine(routine); 
        }
       





    }
}

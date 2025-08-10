using System.Collections;
using UnityEngine;

namespace BaseGame.Utils
{
    public sealed class Coroutines : MonoBehaviour
    {
        private static Coroutines Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("[Coroutines]");
                    _instance = go.AddComponent<Coroutines>();
                    DontDestroyOnLoad(go);
                }

                return _instance;
            }
        }

        private static Coroutines _instance;

        public static Coroutine StartRoutine(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }

        public static void StopRoutine(IEnumerator routine)
        {
            Instance.StopCoroutine(routine);
        }

        public static void StopAllRoutines()
        {
            Instance.StopAllCoroutines();
        }
    }
}

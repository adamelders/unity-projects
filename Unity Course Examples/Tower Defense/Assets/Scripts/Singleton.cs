using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    private static T instance;

    public static T Instance {
        get {
            T gameObject = FindObjectOfType<T>();
            if (instance == null)
                instance = gameObject;
            else if (instance != gameObject)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            return instance;
        }
    }

}

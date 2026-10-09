using UnityEngine;

public class SingletonProtected<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	private static object _lock = new object();

	private static bool isDestroyed = false;

	private static bool isDyplicate = false;

	protected static T Instance
	{
		get
		{
			if (isDestroyed)
			{
				Debug.LogWarning(string.Concat("[Singleton] Instance '", typeof(T), "' already destroyed on application quit. Won't create again - returning null."));
				return null;
			}
			lock (_lock)
			{
				if ((Object)_instance == (Object)null)
				{
					_instance = (T)Object.FindObjectOfType(typeof(T));
					if (Object.FindObjectsOfType(typeof(T)).Length > 1)
					{
						Debug.LogWarning("[Singleton] Something went really wrong - there should never be more than 1 singleton! Reopenning the scene might fix it.");
						return _instance;
					}
					if ((Object)_instance == (Object)null)
					{
						GameObject gameObject = new GameObject();
						_instance = gameObject.AddComponent<T>();
						gameObject.name = typeof(T).Name;
					}
				}
				return _instance;
			}
		}
	}

	protected virtual void OnDestroy()
	{
		if (!isDyplicate)
		{
			isDestroyed = true;
			_instance = null;
		}
		isDyplicate = false;
	}

	protected virtual void Awake()
	{
		CheckInstance();
	}

	protected bool CheckInstance()
	{
		if (Object.FindObjectsOfType(typeof(T)).Length > 1)
		{
			Object.Destroy(base.gameObject);
			isDyplicate = true;
			return false;
		}
		isDestroyed = false;
		_instance = GetComponent<T>();
		Object.DontDestroyOnLoad(base.gameObject);
		return true;
	}
}

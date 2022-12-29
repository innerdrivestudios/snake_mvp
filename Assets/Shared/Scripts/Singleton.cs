using UnityEngine;

/**
 * Basic class to create a quick singleton ;)
 */
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	public static T Instance { get; private set; }

	virtual protected void Awake()
	{
		if (Instance == null)
		{
			Instance = this as T;
		}
		else
		{
			Debug.LogError("Singleton created twice!");
			Destroy(gameObject);
		}
	}
}

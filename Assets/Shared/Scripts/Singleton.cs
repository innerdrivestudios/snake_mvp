using UnityEngine;

/**
 * Basic class to create a quick singleton ;)
 */
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	public static T instance { get; private set; }

	virtual protected void Awake()
	{
		if (instance == null)
		{
			instance = this as T;
		}
		else
		{
			Debug.LogError("Singleton created twice!");
			Destroy(gameObject);
		}
	}

	virtual protected void OnDestroy()
	{
		instance = null;
	}
}

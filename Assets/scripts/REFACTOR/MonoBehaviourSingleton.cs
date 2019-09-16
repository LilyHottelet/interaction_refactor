using UnityEngine;

public class MonoBehaviourSingleton<_INSTANCE_> : MonoBehaviour
		where _INSTANCE_ : MonoBehaviour
{
	public static bool HasInstance { get => instance != null; }

	public static _INSTANCE_ Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<_INSTANCE_>();
			}
			return instance;
		}
	}

	private static _INSTANCE_ instance;

	public virtual void Awake()
	{
		if (instance == null)
		{
			instance = this as _INSTANCE_;
			if (Application.isPlaying)
			{
				DontDestroyOnLoad(gameObject);
			}
		}
		else
		{
			if (Application.isPlaying)
			{
				Destroy(gameObject);
			}
			else
			{
				DestroyImmediate(gameObject);
			}
		}
	}
}

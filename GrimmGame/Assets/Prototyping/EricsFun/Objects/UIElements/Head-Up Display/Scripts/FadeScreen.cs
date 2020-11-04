using UnityEngine;

public class FadeScreen : MonoBehaviour
{
	private Animator animator;

	public static FadeScreen instance;

	private void Awake()
	{
		// Assure that the object can be destroyed if has to be created again later.
		if (instance != null && instance != this)
			Destroy(this.gameObject);
		else
			instance = this;
	}

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void FadeOut()
	{
		animator.SetTrigger("FadeOut");
	}

	public void FadeIn()
	{
		animator.SetTrigger("FadeIn");
	}
}

using UnityEngine;

namespace GameLab
{
	public class BetterMonoBehaviour : MonoBehaviour
	{
		public Transform CachedTransform { get; private set; }
		public RectTransform CachedRectTransform { get; private set; }
		public Bounds CachedBounds { get; private set; }

		protected virtual void Awake()
		{
			CachedTransform = transform;
			CachedRectTransform = CachedTransform as RectTransform;
			CachedBounds = gameObject.CalculateBounds(true);
		}

		protected virtual void LateUpdate()
		{
			if(!CachedTransform.hasChanged)
			{
				return;
			}

			CachedBounds = CachedTransform.CalculateBounds(true);
		}
	}
}
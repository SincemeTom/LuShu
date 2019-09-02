//using DG.Tweening;
using System;
using FairyGUI;
using UnityEngine;

namespace FairyGame
{
	public static class TweenUtils
	{
//		public static Tweener TweenFloat(float start, float end, float duration, Action<object,float> OnUpdate, Action<object> onComplete, object self)
//		{
//			Tweener tweener = DOTween.To(() => start, x =>
//			{
//				if (OnUpdate != null)
//				{
//					try
//					{
//						if (self != null)
//							OnUpdate(self, x);
//					}
//					catch (Exception e)
//					{
//						Debug.LogError(e);
//					}
//				}
//			}, end, duration);
//
//			tweener.OnComplete(() =>
//			{
//				if (OnUpdate != null)
//				{
//					OnUpdate = null;
//				}
//
//				if (onComplete != null)
//				{
//					onComplete(self);
//					onComplete = null;
//				}
//
//				if (self!=null)
//				{
//					self = null;
//				}
//			});
//
//			tweener.OnKill(() =>
//			{
//				if (OnUpdate != null)
//					OnUpdate = null;
//
//				if (onComplete != null)
//					onComplete = null;
//				if (self !=null)
//					self = null;
//			});
//
//			if (self != null)
//				tweener.SetTarget(self);
//
//			return tweener;
//		}
//
//		public static Tweener TweenVector2(Vector2 start, Vector2 end, float duration, Action<object,Vector2> OnUpdate, Action<object> onComplete, object self)
//		{
//			Tweener tweener = DOTween.To(() => start, x =>
//			{
//				if (OnUpdate != null)
//				{
//					try
//					{
//						if (self != null)
//							OnUpdate(self, x);
//					}
//					catch (Exception e)
//					{
//						Debug.LogError(e);
//					}
//				}
//			}, end, duration);
//
//			tweener.OnComplete(() =>
//			{
//				if (OnUpdate != null)
//					OnUpdate = null;
//
//				if (onComplete != null)
//				{
//					onComplete(self);
//					onComplete = null;
//				}
//
//				if (self !=null)
//					self = null;
//			});
//
//			tweener.OnKill(() =>
//			{
//				if (OnUpdate != null)
//					OnUpdate = null;
//
//				if (onComplete != null)
//					onComplete = null;
//
//				if (self !=null)
//					self = null;
//			});
//
//			if (self != null)
//				tweener.SetTarget(self);
//
//			return tweener;
//		}
//
//		public static Tweener TweenVector3(Vector3 start, Vector3 end, float duration, Action<object,Vector3> OnUpdate, Action<object> onComplete, object self)
//		{
//			Tweener tweener = DOTween.To(() => start, x =>
//			{
//				if (OnUpdate != null)
//				{
//					try
//					{
//						if (self != null)
//							OnUpdate(self, x);
//					}
//					catch (Exception e)
//					{
//						Debug.LogError(e);
//					}
//				}
//			}, end, duration);
//
//			tweener.OnComplete(() =>
//			{
//				if (OnUpdate != null)
//					OnUpdate = null;
//				if (onComplete != null)
//				{
//					onComplete(self);
//					onComplete = null;
//				}
//
//				if (self !=null)
//					self = null;
//			});
//
//			tweener.OnKill(() =>
//			{
//				if (OnUpdate != null)
//					OnUpdate = null;
//
//				if (onComplete != null)
//					onComplete = null;
//
//				if (self !=null)
//					self = null;
//			});
//
//			if (self != null)
//				tweener.SetTarget(self);
//
//			return tweener;
//		}

		public static void SetEase(GTweener tweener, EaseType ease)
		{
			tweener.SetEase(ease);
		}
		
		public static void OnComplete(GTweener tweener, Action func)
		{
			tweener.OnComplete(() =>
			{
				try
				{
					func();
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}
				func = null;
			});
		}
		
		public static void OnComplete(GTweener tweener, Action<object> func)
		{
			OnComplete(tweener, func, null);
		}

		public static void OnComplete(GTweener tweener, Action<object> func, object self)
		{
			tweener.OnComplete(() =>
			{
				try
				{
					if (self != null)
						func(self);
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}
				func = null;
				if (self!=null)
					self = null;
			});
		}

//		public static void SetDelay(Tweener tweener, float delay)
//		{
//			tweener.SetDelay(delay);
//		}
//
//		public static void SetLoops(Tweener tweener, int loops)
//		{
//			tweener.SetLoops(loops);
//		}
//
//		public static void SetLoops(Tweener tweener, int loops, bool yoyo)
//		{
//			tweener.SetLoops(loops, yoyo ? LoopType.Yoyo : LoopType.Restart);
//		}
//
//		public static void SetTarget(Tweener tweener, object target)
//		{
//			tweener.SetTarget(target);
//		}
	}
}

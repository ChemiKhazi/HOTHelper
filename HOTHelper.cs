using System;
using Holoville.HOTween;
using Holoville.HOTween.Core;

namespace kontrabida.hothelper
{
	public class HOTweenHelper
	{
		/// <summary>
		/// Target object of this tween
		/// </summary>
		public object Target { get; protected set; }
		/// <summary>
		/// The actual wrapped HOTween object
		/// </summary>
		public Tweener Tween { get; protected set; }
		/// <summary>
		/// The tweening parameters built by this class
		/// </summary>
		public TweenParms Params { get; protected set; }
		/// <summary>
		/// The time value that will be passed to HOTween
		/// </summary>
		public float? Time { get; protected set; } 

		/// <summary>
		/// Indicates if this HOTween has all the data needed to play
		/// </summary>
		public bool CanRun
		{
			get
			{
				if (Params == null)
					return false;
				if (!Time.HasValue)
					return false;
				return true;
			}
		}

		/// <summary>
		/// Accessor for creating the Params and then using it immediately
		/// </summary>
		protected TweenParms ParamUtil
		{
			get
			{
				if (Params == null)
					Params = new TweenParms();
				return Params;
			}
		}

		public HOTweenHelper(object target)
		{
			Target = target;
		}

		/// <summary>
		/// The time this tween will take.
		/// </summary>
		/// <param name="duration"></param>
		/// <returns></returns>
		public HOTweenHelper Duration(float duration)
		{
			Time = duration;
			return this;
		}

		/// <summary>
		/// Sets the time of the tween and sets SpeedBased to true
		/// </summary>
		/// <param name="speed"></param>
		/// <returns></returns>
		public HOTweenHelper Speed(float speed)
		{
			Time = speed;
			ParamUtil.SpeedBased(true);
			return this;
		}

		/// <summary>
		/// The easing function for this tween
		/// </summary>
		/// <param name="easing"></param>
		/// <returns></returns>
		public HOTweenHelper Ease(EaseType easing)
		{
			ParamUtil.Ease(easing);
			return this;
		}

		/// <summary>
		/// Add a single property to the Tween
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <param name="isRelative"></param>
		/// <returns></returns>
		public HOTweenHelper Prop(string propertyName, object value, bool isRelative = false)
		{
			ParamUtil.Prop(propertyName, value, isRelative);
			return this;
		}

		/// <summary>
		/// Key/pair values for properties to tween on the target object, can take more than one key/pair
		/// </summary>
		/// <param name="properties">Must be in the format: "propName1", valueOfProp1, "propName2", valueOfProp2...</param>
		/// <returns></returns>
		public HOTweenHelper Props(params object[] properties)
		{
			if ((properties.Length%2) != 0)
				throw new Exception("Property Key/Value pair count does not match");
			for (int i = 0; i < properties.Length; i += 2)
			{
				string propName = (string)properties[i];
				var propVal = properties[i + 1];
				ParamUtil.Prop(propName, propVal);
			}
			return this;
		}

		public HOTweenHelper Delay(float delay)
		{
			ParamUtil.Delay(delay);
			return this;
		}

		/// <summary>
		/// Set a callback for this tween
		/// </summary>
		/// <param name="callback"></param>
		/// <returns></returns>
		public HOTweenHelper OnComplete(TweenDelegate.TweenCallback callback)
		{
			ParamUtil.OnComplete(callback);
			return this;
		}

		/// <summary>
		/// Set a callback with parameters for this tween
		/// </summary>
		/// <param name="callback"></param>
		/// <returns></returns>
		public HOTweenHelper OnComplete(TweenDelegate.TweenCallbackWParms callback)
		{
			ParamUtil.OnComplete(callback);
			return this;
		}

		/// <summary>
		/// Creates the tween, which starts it. Call this at the end of the sequence.
		/// </summary>
		/// <returns></returns>
		public Tweener Start()
		{
			if (Params == null)
				return null;
			if (!Time.HasValue)
				return null;
			Tween = HOTween.To(Target, Time.Value, Params);
			return Tween;
		}
	}

	public static class HOTHelperExtension
	{
		/// <summary>
		/// Create a tween on this object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="target"></param>
		/// <returns></returns>
		public static HOTweenHelper Tween<T>(this T target)
		{
			return new HOTweenHelper(target);
		}
	}

}
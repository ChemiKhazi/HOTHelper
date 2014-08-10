using Holoville.HOTween;
using Holoville.HOTween.Core;
using System.Collections.Generic;

namespace kontrabida.hothelper
{
	public class HOTSequence
	{
		protected class TimeTweeen
		{
			public float time;
			public IHOTweenComponent tween;

			public TimeTweeen(float time, IHOTweenComponent tween)
			{
				this.time = time;
				this.tween = tween;
			}
		}

		public Sequence Sequence { get; protected set; }
		public SequenceParms Params { get; protected set; }

		public bool IsReady { get { return Sequence != null; }}

		protected SequenceParms ParamUtil
		{
			get
			{
				if (Params == null)
					Params = new SequenceParms();
				return Params;
			}
		}

		protected List<TimeTweeen> insertTweens = new List<TimeTweeen>();
		protected List<IHOTweenComponent> appendTweens = new List<IHOTweenComponent>(); 

		public HOTSequence OnComplete(TweenDelegate.TweenCallback callback)
		{
			ParamUtil.OnComplete(callback);
			return this;
		}

		public HOTSequence OnComplete(TweenDelegate.TweenCallbackWParms callback)
		{
			ParamUtil.OnComplete(callback);
			return this;
		}

		public HOTSequence Insert(float time, HOTWeenHelper tween)
		{
			insertTweens.Add(new TimeTweeen(time, tween.Start()));
			return this;
		}

		public HOTSequence Append(HOTWeenHelper tween)
		{
			appendTweens.Add(tween.Start());
			return this;
		}

		public Sequence BuildSequence()
		{
			if (!IsReady)
			{
				Sequence = new Sequence(Params);
				appendTweens.ForEach(tween => Sequence.Append(tween));
				insertTweens.ForEach(tween => Sequence.Insert(tween.time, tween.tween));
			}
			return Sequence;
		}

		public Sequence Play()
		{
			BuildSequence().Play();
			return Sequence;
		}
	}
}
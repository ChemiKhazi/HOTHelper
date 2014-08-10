using Holoville.HOTween;
using Holoville.HOTween.Core;
using System.Collections.Generic;

namespace kontrabida.hothelper
{
	public class HOTSequence
	{
		protected class SequenceOp
		{
			public enum OpType
			{
				Append,
				Insert,
				Prepend
			}

			public OpType type;
			public float time;
			public IHOTweenComponent tween;

			public SequenceOp(float time, IHOTweenComponent tween)
			{
				this.type = OpType.Insert;
				this.time = time;
				this.tween = tween;
			}

			public SequenceOp(OpType type, IHOTweenComponent tween)
			{
				this.time = -1f;
				this.type = type;
				this.tween = tween;
			}
		}

		public Sequence Sequence { get; protected set; }
		public SequenceParms Params { get; protected set; }

		public bool IsReady
		{
			get { return Sequence != null; }
		}

		protected SequenceParms ParamUtil
		{
			get
			{
				if (Params == null)
					Params = new SequenceParms();
				return Params;
			}
		}

		protected List<SequenceOp> sequenceOps = new List<SequenceOp>();
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

		public HOTSequence Insert(float time, HOTweenHelper tween)
		{
			sequenceOps.Add(new SequenceOp(time, tween.Start()));
			return this;
		}

		public HOTSequence Insert(float time, IHOTweenComponent tween)
		{
			sequenceOps.Add(new SequenceOp(time, tween));
			return this;
		}

		public HOTSequence Append(HOTweenHelper tween)
		{
			sequenceOps.Add(new SequenceOp(SequenceOp.OpType.Append, tween.Start()));
			return this;
		}

		public HOTSequence Append(IHOTweenComponent tween)
		{
			sequenceOps.Add(new SequenceOp(SequenceOp.OpType.Append, tween));
			return this;
		}
		
		public HOTSequence Prepend(HOTweenHelper tween)
		{
			sequenceOps.Add(new SequenceOp(SequenceOp.OpType.Prepend, tween.Start()));
			return this;
		}

		public HOTSequence Prepend(IHOTweenComponent tween)
		{
			sequenceOps.Add(new SequenceOp(SequenceOp.OpType.Prepend, tween));
			return this;
		}

		public Sequence BuildSequence()
		{
			if (!IsReady)
			{
				Sequence = new Sequence(Params);
				foreach (var sequenceOp in sequenceOps)
				{
					switch (sequenceOp.type)
					{
						case SequenceOp.OpType.Append:
							Sequence.Append(sequenceOp.tween);
							break;
						case SequenceOp.OpType.Insert:
							Sequence.Insert(sequenceOp.time, sequenceOp.tween);
							break;
						case SequenceOp.OpType.Prepend:
							Sequence.Prepend(sequenceOp.tween);
							break;
					}
				}
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
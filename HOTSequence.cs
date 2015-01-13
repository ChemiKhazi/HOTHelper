using Holoville.HOTween;
using Holoville.HOTween.Core;
using System.Collections.Generic;

namespace kontrabida.hothelper
{
	public class HOTSequence
	{
		public static HOTSequence New
		{
			get
			{
				return new HOTSequence();
			}
		}

		protected class SequenceOp
		{
			public enum OpType
			{
				Append,
				Insert,
				Prepend,
				AppendCallback,
				InsertCallback,
				AppendInterval,
				PrependInterval
			}

			public OpType type;
			public float time;
			public object opObject;

			public SequenceOp(float time, IHOTweenComponent tween)
			{
				this.type = OpType.Insert;
				this.time = time;
				this.opObject = tween;
			}

			public SequenceOp(OpType type, IHOTweenComponent tween)
			{
				this.time = -1f;
				this.type = type;
				this.opObject = tween;
			}

			public SequenceOp(TweenDelegate.TweenCallback callback, float time)
			{
				this.type = OpType.InsertCallback;
				this.time = time;
				this.opObject = callback;
			}

			public SequenceOp(TweenDelegate.TweenCallback callback)
			{
				this.time = -1f;
				this.type = OpType.AppendCallback;
				this.opObject = callback;
			}

			public SequenceOp(OpType type, float interval)
			{
				this.type = type;
				this.time = interval;
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

		public HOTSequence AutoKill(bool kill = false)
		{
			ParamUtil.AutoKill(kill);
			return this;
		}

		public HOTSequence Loops(int loops, LoopType loop_type = LoopType.Restart)
		{
			ParamUtil.Loops(loops, loop_type);
			return this;
		}

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

		public HOTSequence InsertCallback(float time, TweenDelegate.TweenCallback callback)
		{
			sequenceOps.Add(new SequenceOp(callback, time));
			return this;
		}

		public HOTSequence AppendCallback(TweenDelegate.TweenCallback callback)
		{
			sequenceOps.Add(new SequenceOp(callback));
			return this;
		}

		public HOTSequence AppendInterval(float interval)
		{
			sequenceOps.Add(new SequenceOp(SequenceOp.OpType.AppendInterval, interval));
			return this;
		}

		public HOTSequence PrependInterval(float interval)
		{
			sequenceOps.Add(new SequenceOp(SequenceOp.OpType.PrependInterval, interval));
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
							Sequence.Append((IHOTweenComponent) sequenceOp.opObject);
							break;
						case SequenceOp.OpType.Insert:
							Sequence.Insert(sequenceOp.time, (IHOTweenComponent) sequenceOp.opObject);
							break;
						case SequenceOp.OpType.Prepend:
							Sequence.Prepend((IHOTweenComponent) sequenceOp.opObject);
							break;
						case SequenceOp.OpType.AppendCallback:
							Sequence.AppendCallback((TweenDelegate.TweenCallback) sequenceOp.opObject);
							break;
						case SequenceOp.OpType.InsertCallback:
							Sequence.InsertCallback(sequenceOp.time, (TweenDelegate.TweenCallback) sequenceOp.opObject);
							break;
						case SequenceOp.OpType.AppendInterval:
							Sequence.AppendInterval(sequenceOp.time);
							break;
						case SequenceOp.OpType.PrependInterval:
							Sequence.PrependInterval(sequenceOp.time);
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
HOTHelper
=========

Syntatic sugar to make working with the Unity3D Tweening library, [HOTween](http://hotween.demigiant.com/index.html), better.

Status
------

As of now, this helper class does not have feature parity with HOTween. I'm implementing helpers as I need them.

If you'd like to help implement HOTween features, please clone the repo and submit a pull request!

Installation
------------
1. Copy `HOTHelper.cs` to your Unity project
2. If you use Sequences, copy `HOTSequences.cs` as well.

Usage
-----

At the `using` section for code you'd like to use HOTHelper in, include:
```
using kontrabida.hothelper;
```

### Tweening

HOTHelper contains an extension method to create a new HOTWeen on any code object.

```C#
// To create tweens on object, call .Tween() on the object.
// This creates a HOTWeenHelper instance with the previous object as the target

HOTWeenHelper tweenTransform = someGameObject.Transform.Tween();

HOTweenHelper tweenSprite = someSpriteRenderer.Tween();

// You can also just create HOTWeenHelper by yourself
HOTWeenHelper myTween = new HOTweenHelper(someobject.Transform);
```

You then chain calls on the `HOTweenHelper` instance to set the TweenParms for the Tweener.

```C#
// To tween the position of the transform by speed
tweenTransform.Prop("position", Vector3.one).Speed(5f);

// Tween the color of the sprite by tween time
tweenSprite.Prop("color", Color.red).Duration(5f);
```

Unlike calling `HOTween.To()`, HOTweenHelper doesn't automatically start the tween. The `.Start()` function should be called to play the tween.

Since you can easily chain calls together, this makes setting up HOTweens in code easier than creating `TweenParms` before calling `HOTween.To`.

Note how the OnComplete call is at the end of the chain, making reading the code flow easier.

```C#
someGameObject.Transform.Tween()
                        .Prop("position", Vector3.one)
                        .Prop("localScale", new Vector3(0.5f, 0.5f, 0.5f))
                        .Speed(5f)
                        .OnComplete(()=>Debug.log("Move complete"))
                        .Start();
```

### Sequences

Sequences do not have extension functions to create them. Instantiate it yourself like a HOTween Sequence.

```C#
HOTSequence sequence = new HOTSequence();
```

You then do Insert/Append/Prepend like with regular sequences. These can be done with `HOTWeen.To` or with `HOTweenHelper`

```C#
// Create a tween with HOTween.To
var tween1 = HOTween.To(...);

// When creating with HOTweenHelper, do not call .Start()
// at the end of the chain
var tween2 = gameObj.Tween()
                    .Prop("position", Vector3.one)
                    .Speed(5f);

// Create the HOTSequence instance and insert/append
HOTSequence sequence = new HOTSequence()
                            .Insert(0, tween1)
                            .Append(tween2);

// Calling .BuildSequence() returns the generated Sequence instance
Sequence generatedSequence = sequence.BuildSequence();

// You can also call .Play() on the HOTSequence
// to return the generated HOTween sequence and play it
Sequence alsoGeneratedSequence = new HOTSequence()
                                    .Insert(0, tween1)
                                    .Append(tween2)
                                    .Play()
```

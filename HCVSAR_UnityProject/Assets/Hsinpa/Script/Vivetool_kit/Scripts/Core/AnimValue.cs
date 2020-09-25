// ========================================================================== //
//
//  class AnimValue
//  -----
//  Purpose: Animate any value with a coroutine
//
//  Usage: This is the base class of all value animators. It can be used by
//      subclassing, or by simply specify the value type and required functions.
//
//  Subclassing Example:
//      public class FloatAnimator : ValueAnimator<float>
//      {
//          public FloatAnimator(MonoBehaviour monoBehaviour, GetValue getValue, SetValue setValue, float defaultDuration = 0)
//              : base(monoBehaviour, getValue, setValue, (a, b, t) => { return a + (b-a)*t; }, defaultDuration) {}
//      }
//
//  Direct Use Example:
//    In Awake(): opacityFader = new ValueAnimator<float>(this, () => { return this.opacity; }, (val) => { this.opacity = val; }, (a, b, t) => { return a + (b-a)*t; }, 0.2f);
//    To use: opacityFader.FadeTo(0.5f);
//
//  Note: This class uses MonoBehaviour's Coroutine feature to fade the value,
//      so it must be instantiated with a reference to a MonoBehaviour instance.
//
//  Created: 2016-11-03
//  Updated: 2016-11-08
//
//  Copyright 2016 Yu-hsien Chang
// 
// ========================================================================== //
using UnityEngine;
using System;
using System.Collections;

namespace Htc.ViveToolkit
{
    /// <summary>
    /// Animate any value with a coroutine
    /// </summary>
    /// <typeparam name="T">The type of the value to be animated</typeparam>
    public class AnimValue<T>
    {
        public const float useDefaultDuration = -1;

        public delegate T GetValue();
        public delegate void SetValue(T value);
        public delegate T InterpolateValue(T start, T end, float t);

        public T value
        {
            get { return getValue(); }
            set { setValue(value); }
        }

        public bool isInTransition { get { return runningCoroutine != null; } }
        public T target { get; private set; }

        private MonoBehaviour monoBehaviour;

        private GetValue getValue;
        private SetValue setValue;
        private InterpolateValue interpValue;

        private float defaultDuration;

        private IEnumerator runningCoroutine = null;
        private Action<T> onFinishCallback;

        public AnimValue(MonoBehaviour monoBehaviour, GetValue getValue, SetValue setValue, InterpolateValue interpValue, float defaultDuration = 0)
        {
            this.monoBehaviour = monoBehaviour;
            this.getValue = getValue;
            this.setValue = setValue;
            this.interpValue = interpValue;
            this.defaultDuration = defaultDuration;
        }

        /// <summary>
        /// Fades the value
        /// </summary>
        /// <param name="value">Target value to fade to.</param>
        /// <param name="duration">Duration of the fade in seconds. Negative value indicates using default.</param>
        /// <param name="onFinishCallback">Callback function when the fading is finished.</param>
        public void FadeTo(T value, float duration = -1, Action<T> onFinishCallback = null)
        {
            Stop();

            target = value;
            if (duration < 0)
                duration = defaultDuration;

            if (monoBehaviour && monoBehaviour.gameObject.activeInHierarchy && duration > 0)
            {
                this.onFinishCallback = onFinishCallback;
                runningCoroutine = FadeToCoroutine(duration);
                monoBehaviour.StartCoroutine(runningCoroutine);
            }
            else
            {
                OnFinish();
            }
        }

        /// <summary>
        /// Stop the value fading immediately and leave the value as is.
        /// Note that the OnFinishCallback provided will not be called and will forever be lost.
        /// </summary>
        public void Stop()
        {
            if (runningCoroutine != null)
            {
                monoBehaviour.StopCoroutine(runningCoroutine);
                runningCoroutine = null;
            }
        }

        /// <summary>
        /// Stop the value fading immediately, set the value to the target value,
        /// and invoke the OnFinishCallback provided.
        /// </summary>
        public void Finish()
        {
            if (runningCoroutine != null)
            {
                Stop();
                OnFinish();
            }
        }

        private IEnumerator FadeToCoroutine(float duration)
        {
            T start = value;
            float time = 0;

            while (time < duration)
            {
                value = interpValue(start, target, time / duration);

                time += Time.deltaTime;
                yield return null;
            }

            runningCoroutine = null;

            OnFinish();
        }

        private void OnFinish()
        {
            value = target;
            if (onFinishCallback != null)
            {
                onFinishCallback(target);
                onFinishCallback = null;
            }
        }
    }
}
using System;
using System.Collections;
using UnityEngine;
using System.Dynamic;
using static UnityEngine.Rendering.DebugUI;

namespace Utils
{

    [Serializable]
    public struct Tween<T>
    {
        public AnimationCurve curve;
        public float t;
        public float duration;
        public T value;
        public T startValue;
        public T endValue;
        public bool isActive;
        public bool isReversing;
        public bool isLooping;

        public bool SetActive(bool state) => isActive = state;
        public void Reset()
        {
            value = startValue;
            t = 0;
            SetActive(false);
            isReversing = false;
        }

        public void Reverse()
        {
            value = endValue;
            t = 0;
            isReversing = true;
        }
    }

    public static class TweenUtil
    {
        public static bool Update(float delta, ref Tween<float> tween)
        {
            if (!tween.isActive) return false;

            if(tween.isLooping && tween.t + delta > tween.duration)
            {
                tween.isReversing = !tween.isReversing;
                tween.t = 0;
            }

            tween.t = Mathf.Clamp(tween.t + delta, 0, tween.duration);
            float alpha = tween.curve.Evaluate(tween.t / tween.duration);
            if (tween.isReversing) alpha = 1 - alpha;

            tween.value = (1 - alpha) * tween.startValue + alpha * tween.endValue;
            return true;
        }

        public static bool Update(float delta, ref Tween<Vector2> tween)
        {
            if (!tween.isActive) return false;

            if (tween.isLooping && tween.t + delta > tween.duration)
            {
                tween.isReversing = !tween.isReversing;
                tween.t = 0;
            }

            tween.t = Mathf.Clamp(tween.t + delta, 0, tween.duration);
            float alpha = tween.curve.Evaluate(tween.t / tween.duration);
            if (tween.isReversing) alpha = 1 - alpha;

            tween.value = (1 - alpha) * tween.startValue + alpha * tween.endValue;
            return true;
        }

        public static bool Update(float delta, ref Tween<Vector3> tween)
        {
            if (!tween.isActive) return false;

            if (tween.isLooping && tween.t + delta > tween.duration)
            {
                tween.isReversing = !tween.isReversing;
                tween.t = 0;
            }

            tween.t = Mathf.Clamp(tween.t + delta, 0, tween.duration);
            float alpha = tween.curve.Evaluate(tween.t / tween.duration);
            if (tween.isReversing) alpha = 1 - alpha;

            tween.value = (1 - alpha) * tween.startValue + alpha * tween.endValue;
            return true;
        }
    }
}
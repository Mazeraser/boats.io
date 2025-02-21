using System;
using DG.Tweening;
using UnityEngine;

namespace Codebase.Infrastructure.Animator
{
    public class CircleAnimator : MonoBehaviour, IAnimator
    {
        private Vector3 _regularScale;
        [SerializeField] 
        private float _timeToTransition;

        private Sequence _animations;

        public void Death(TweenCallback callback = null)
        {
            _animations.Append(transform.DOScale(new Vector3(0, 0, 0), _timeToTransition).OnComplete(callback));
        }

        public void Born()
        {
            gameObject.SetActive(true);
            _animations.Append(transform.DOScale(_regularScale,_timeToTransition));
        }

        private void Start()
        {
            _regularScale = transform.localScale;
            transform.localScale = new Vector3(0, 0, 0);
            _animations = DOTween.Sequence();
            Born();
        }

        private void OnDestroy()
        {
            _animations.Kill();
        }
    }
}
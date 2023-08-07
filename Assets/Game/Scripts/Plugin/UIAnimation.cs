using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static UIAnimation.MoveAnimation;

public class UIAnimation : MonoBehaviour
{
    [System.Serializable]
    public class Animation
    {
        public bool enable = false;
        public Ease easeType;
        public float duration = 0.5f;
        public float delayTime = 0f;
        public float value = 0f;
    }

    [System.Serializable]
    public class MoveAnimation : Animation
    {
        public enum Direction { Top, Bottom, Left, Right }
        public Direction direction;
    }

    [System.Serializable]
    public class ScaleAnimation : Animation
    {
        public Vector3 startScale = Vector3.one;
    }

    public RectTransform container = null;

    public MoveAnimation moveBehavior = null;
    public ScaleAnimation scaleBehavior = null;
    public Animation fadeBehavior = null;

    private CanvasGroup canvasGr = null;

    private Vector3 defaultAnchorPos;

    private void Awake()
    {
        canvasGr = GetComponent<CanvasGroup>();
        defaultAnchorPos = container.anchoredPosition3D;
    }

    public void DoAnimation()
    {
        if (moveBehavior.enable)
        {
            Vector3 startPos = GetStartPos(moveBehavior.direction);
            container.anchoredPosition3D = startPos;
            container.DOAnchorPos3D(defaultAnchorPos, moveBehavior.duration).SetDelay(moveBehavior.delayTime).SetEase(moveBehavior.easeType);
        }

        if (scaleBehavior.enable)
        {
            container.localScale = scaleBehavior.startScale;
            container.DOScale(Vector3.one, scaleBehavior.duration).SetDelay(scaleBehavior.delayTime).SetEase(scaleBehavior.easeType);
        }

        if (fadeBehavior.enable)
        {
            canvasGr.alpha = fadeBehavior.value;
            canvasGr.DOFade(1, fadeBehavior.duration).SetDelay(fadeBehavior.delayTime).SetEase(fadeBehavior.easeType);
        }
    }

    private Vector3 GetStartPos(Direction direction)
    {
        switch (direction)
        {
            case Direction.Top:
                return new Vector3(0, (Screen.width / 2) + (container.sizeDelta.y / 2), 0);
            case Direction.Bottom:
                return new Vector3(0, ((Screen.width / 2) + (container.sizeDelta.y / 2)) * -1, 0);
            case Direction.Left:
                return new Vector3(((Screen.width / 2) + (container.sizeDelta.x / 2)) * -1, 0, 0);
            case Direction.Right:
                return new Vector3((Screen.width / 2) + (container.sizeDelta.x / 2), 0, 0);
        }
        return Vector3.zero;
    }
}

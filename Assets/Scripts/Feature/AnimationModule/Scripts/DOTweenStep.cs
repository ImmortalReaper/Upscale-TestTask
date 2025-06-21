using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.AnimationModule.Scripts
{
    [Serializable]
    public class DOTweenStep
    {
        //[Header("Step Operation")]
        public StepOperation operation = StepOperation.Append;
    
        //[Header("Tween Settings")]
        public TweenType tweenType = TweenType.Move;
        public float duration = 1f;
        public float delay = 0f;
        public Ease easeType = Ease.OutQuad;
        public AnimationCurve customCurve;
        public bool useCustomCurve = false;
    
        //[Header("Transform Values")]
        public Vector3 targetVector = Vector3.zero;
        public float targetFloat = 1f;
        public Color targetColor = Color.white;
    
        //[Header("Special Settings")]
        [Range(0, 10)]
        public int punchVibrato = 10;
        [Range(0, 1)]
        public float punchElasticity = 1f;
    
        [Range(0, 10)]
        public int shakeVibrato = 10;
        [Range(0, 90)]
        public float shakeRandomness = 90f;
        public bool shakeFadeOut = true;
    
        //[Header("Loop Settings")]
        public bool enableLoop = false;
        public int loopCount = -1;
        public LoopType loopType = LoopType.Restart;
    
        public Action onStepStart;
        public Action onStepComplete;
        public Action onStepUpdate;
    
        //[Header("Target Override")]
        public bool useCustomTarget = false;
        public Transform customTarget;
        public SpriteRenderer spriteRenderer;
        public CanvasGroup canvasGroup;
        public Image uiImage;
        public TextMeshProUGUI uiText;
        public TextMesh textMesh;
    }
}

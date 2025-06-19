using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DOTweenSequenceAnimator : MonoBehaviour
{
    //[Header("Sequence Settings")]
    [SerializeField] bool playOnStart = true;
    [SerializeField] bool autoKill = true;
    [SerializeField] bool sequenceLoop = false;
    [SerializeField] int sequenceLoopCount = -1;
    [SerializeField] LoopType sequenceLoopType = LoopType.Restart;
    
    //[Header("Animation Steps")]
    [SerializeField] List<DOTweenStep> tweenSteps = new List<DOTweenStep>();
    
    //[Header("Control Buttons")]
    [Space(10)]
    [SerializeField] bool playSequence = false;
    [SerializeField] bool pauseSequence = false;
    [SerializeField] bool resumeSequence = false;
    [SerializeField] bool restartSequence = false;
    [SerializeField] bool killSequence = false;
    [SerializeField] bool resetToStart = false;
    
    //[Header("Preview Helpers")]
    [SerializeField] bool previewFirstStep = false;
    [SerializeField] bool previewLastStep = false;
    
    //[Header("Debug")]
    [SerializeField]  bool showDebugLogs = false;
    
    // Runtime variables
    private Sequence currentSequence;
    private Vector3 initialPosition;
    private Vector3 initialLocalPosition;
    private Vector2 initialAnchoredPosition; // Added for UI
    private Vector3 initialRotation;
    private Vector3 initialLocalRotation;
    private Vector3 initialScale;
    private Color initialColor;
    private float initialAlpha;
    
    // Events
    public Action OnSequenceStart;
    public Action OnSequenceComplete;
    public Action OnSequencePause;
    public Action OnSequencePlay;
    public Action OnSequenceKill;
    public Action OnSequenceLoop;
    
    // Components cache
    private SpriteRenderer spriteRenderer;
    private CanvasGroup canvasGroup;
    private Image uiImage;
    private TextMeshProUGUI uiText;
    private RectTransform rectTransform;
    
    void Start()
    {
        StoreInitialValues();
        CacheComponents();
        
        if (playOnStart)
        {
            PlaySequence();
        }
    }
    
    void OnValidate()
    {
        if (!Application.isPlaying) return;
        
        if (playSequence)
        {
            playSequence = false;
            PlaySequence();
        }
        
        if (pauseSequence)
        {
            pauseSequence = false;
            PauseSequence();
        }
        
        if (resumeSequence)
        {
            resumeSequence = false;
            ResumeSequence();
        }
        
        if (restartSequence)
        {
            restartSequence = false;
            RestartSequence();
        }
        
        if (killSequence)
        {
            killSequence = false;
            KillSequence();
        }
        
        if (resetToStart)
        {
            resetToStart = false;
            ResetToStart();
        }
        
        if (previewFirstStep)
        {
            previewFirstStep = false;
            PreviewFirstStep();
        }
        
        if (previewLastStep)
        {
            previewLastStep = false;
            PreviewLastStep();
        }
    }
    
    void StoreInitialValues()
    {
        initialPosition = transform.position;
        initialLocalPosition = transform.localPosition;
        initialRotation = transform.eulerAngles;
        initialLocalRotation = transform.localEulerAngles;
        initialScale = transform.localScale;
        
        // Store initial anchored position for UI elements
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            initialAnchoredPosition = rectTransform.anchoredPosition;
        }
        
        // Store initial color/alpha values
        if (GetComponent<SpriteRenderer>())
            initialColor = GetComponent<SpriteRenderer>().color;
        else if (GetComponent<Image>())
            initialColor = GetComponent<Image>().color;
        else if (GetComponent<TextMeshProUGUI>())
            initialColor = GetComponent<TextMeshProUGUI>().color;
        
        if (GetComponent<CanvasGroup>())
            initialAlpha = GetComponent<CanvasGroup>().alpha;
    }
    
    void CacheComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        canvasGroup = GetComponent<CanvasGroup>();
        uiImage = GetComponent<Image>();
        uiText = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }
    
    [ContextMenu("Play Sequence")]
    public void PlaySequence()
    {
        KillSequence(); // Kill any existing sequence
        
        if (tweenSteps.Count == 0)
        {
            if (showDebugLogs)
                Debug.LogWarning("No tween steps defined!", this);
            return;
        }
        
        currentSequence = DOTween.Sequence();
        currentSequence.SetAutoKill(autoKill);
        
        if (showDebugLogs)
            Debug.Log("Starting DOTween Sequence", this);
        
        for (int i = 0; i < tweenSteps.Count; i++)
        {
            var step = tweenSteps[i];
            Tween tween = CreateTween(step);
            
            if (tween != null)
            {
                if (step.operation == StepOperation.Append || i == 0)
                {
                    currentSequence.Append(tween);
                    if (showDebugLogs) Debug.Log($"Appending step {i}: {step.tweenType}", this);
                }
                else if (step.operation == StepOperation.Join)
                {
                    currentSequence.Join(tween);
                    if (showDebugLogs) Debug.Log($"Joining step {i}: {step.tweenType}", this);
                }
            }
        }
        
        // Set sequence loop
        if (sequenceLoop)
        {
            currentSequence.SetLoops(sequenceLoopCount, sequenceLoopType);
        }
        
        // Set sequence callbacks
        currentSequence.OnStart(() => {
            OnSequenceStart?.Invoke();
            if (showDebugLogs) Debug.Log("Sequence Started", this);
        });
        
        currentSequence.OnComplete(() => {
            OnSequenceComplete?.Invoke();
            if (showDebugLogs) Debug.Log("Sequence Completed", this);
        });
        
        currentSequence.OnPause(() => {
            OnSequencePause?.Invoke();
            if (showDebugLogs) Debug.Log("Sequence Paused", this);
        });
        
        currentSequence.OnPlay(() => {
            OnSequencePlay?.Invoke();
            if (showDebugLogs) Debug.Log("Sequence Resumed", this);
        });
        
        currentSequence.OnKill(() => {
            OnSequenceKill?.Invoke();
            if (showDebugLogs) Debug.Log("Sequence Killed", this);
        });
        
        if (sequenceLoop)
        {
            currentSequence.OnStepComplete(() => {
                OnSequenceLoop?.Invoke();
                if (showDebugLogs) Debug.Log("Sequence Loop", this);
            });
        }
        
        currentSequence.Play();
    }
    
    Tween CreateTween(DOTweenStep step)
    {
        Transform targetTransform = step.useCustomTarget && step.customTarget != null ? step.customTarget : transform;
        RectTransform targetRectTransform = step.useCustomTarget && step.customTarget != null ? 
            step.customTarget.GetComponent<RectTransform>() : rectTransform;
        
        Tween tween = null;
        
        switch (step.tweenType)
        {
            case TweenType.Move:
                tween = targetTransform.DOMove(step.targetVector, step.duration);
                break;
                
            case TweenType.MoveLocal:
                tween = targetTransform.DOLocalMove(step.targetVector, step.duration);
                break;
                
            case TweenType.MoveAnchor:
                if (targetRectTransform != null)
                    tween = targetRectTransform.DOAnchorPos(step.targetVector, step.duration);
                else if (showDebugLogs)
                    Debug.LogWarning("MoveAnchor requires RectTransform component!", this);
                break;
                
            case TweenType.Rotate:
                tween = targetTransform.DORotate(step.targetVector, step.duration);
                break;
                
            case TweenType.RotateLocal:
                tween = targetTransform.DOLocalRotate(step.targetVector, step.duration);
                break;
                
            case TweenType.Scale:
                tween = targetTransform.DOScale(step.targetVector, step.duration);
                break;
                
            case TweenType.Fade:
                tween = CreateFadeTween(step, targetTransform);
                break;
                
            case TweenType.Color:
                tween = CreateColorTween(step, targetTransform);
                break;
                
            case TweenType.Punch:
                tween = targetTransform.DOPunchScale(step.targetVector, step.duration, step.punchVibrato, step.punchElasticity);
                break;
                
            case TweenType.Shake:
                tween = targetTransform.DOShakePosition(step.duration, step.targetVector, step.shakeVibrato, step.shakeRandomness, step.shakeFadeOut);
                break;
                
            case TweenType.ShakeAnchor:
                if (targetRectTransform != null)
                    tween = targetRectTransform.DOShakeAnchorPos(step.duration, step.targetVector, step.shakeVibrato, step.shakeRandomness, step.shakeFadeOut);
                else if (showDebugLogs)
                        Debug.LogWarning("ShakeAnchor requires RectTransform component!", this);
                break;
        }
        
        if (tween != null)
        {
            // Apply delay
            if (step.delay > 0)
                tween.SetDelay(step.delay);
            
            // Apply easing
            if (step.useCustomCurve && step.customCurve != null)
                tween.SetEase(step.customCurve);
            else
                tween.SetEase(step.easeType);
            
            // Apply loop
            if (step.enableLoop)
                tween.SetLoops(step.loopCount, step.loopType);
            
            // Set callbacks
            tween.OnStart(() => {
                step.onStepStart?.Invoke();
                if (showDebugLogs) Debug.Log($"Step Started: {step.tweenType}", this);
            });
            
            tween.OnComplete(() => {
                step.onStepComplete?.Invoke();
                if (showDebugLogs) Debug.Log($"Step Completed: {step.tweenType}", this);
            });
            
            tween.OnUpdate(() => {
                step.onStepUpdate?.Invoke();
            });
        }
        
        return tween;
    }
    
    Tween CreateFadeTween(DOTweenStep step, Transform target)
    {
        if (step.canvasGroup != null)
            return step.canvasGroup.DOFade(step.targetFloat, step.duration);
        else if (canvasGroup != null)
            return canvasGroup.DOFade(step.targetFloat, step.duration);
        else if (step.spriteRenderer != null)
            return step.spriteRenderer.DOFade(step.targetFloat, step.duration);
        else if (spriteRenderer != null)
            return spriteRenderer.DOFade(step.targetFloat, step.duration);
        else if (step.uiImage != null)
            return step.uiImage.DOFade(step.targetFloat, step.duration);
        else if (uiImage != null)
            return uiImage.DOFade(step.targetFloat, step.duration);
        else if (step.uiText != null)
            return step.uiText.DOFade(step.targetFloat, step.duration);
        else if (uiText != null)
            return uiText.DOFade(step.targetFloat, step.duration);
        
        return null;
    }
    
    Tween CreateColorTween(DOTweenStep step, Transform target)
    {
        if (step.spriteRenderer != null)
            return step.spriteRenderer.DOColor(step.targetColor, step.duration);
        else if (spriteRenderer != null)
            return spriteRenderer.DOColor(step.targetColor, step.duration);
        else if (step.uiImage != null)
            return step.uiImage.DOColor(step.targetColor, step.duration);
        else if (uiImage != null)
            return uiImage.DOColor(step.targetColor, step.duration);
        else if (step.uiText != null)
            return step.uiText.DOColor(step.targetColor, step.duration);
        else if (uiText != null)
            return uiText.DOColor(step.targetColor, step.duration);
        
        return null;
    }
    
    [ContextMenu("Pause Sequence")]
    public void PauseSequence()
    {
        currentSequence?.Pause();
    }
    
    [ContextMenu("Resume Sequence")]
    public void ResumeSequence()
    {
        currentSequence?.Play();
    }
    
    [ContextMenu("Restart Sequence")]
    public void RestartSequence()
    {
        currentSequence?.Restart();
    }
    
    [ContextMenu("Kill Sequence")]
    public void KillSequence()
    {
        currentSequence?.Kill();
        currentSequence = null;
    }
    
    [ContextMenu("Reset To Start")]
    public void ResetToStart()
    {
        KillSequence();
        transform.position = initialPosition;
        transform.localPosition = initialLocalPosition;
        transform.eulerAngles = initialRotation;
        transform.localEulerAngles = initialLocalRotation;
        transform.localScale = initialScale;
        
        // Reset anchored position for UI elements
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = initialAnchoredPosition;
        }
        
        // Reset colors/alpha
        if (spriteRenderer != null)
            spriteRenderer.color = initialColor;
        if (uiImage != null)
            uiImage.color = initialColor;
        if (uiText != null)
            uiText.color = initialColor;
        if (canvasGroup != null)
            canvasGroup.alpha = initialAlpha;
    }
    
    // Public API
    public bool IsActive => currentSequence != null && currentSequence.IsActive();
    public bool IsPlaying => currentSequence != null && currentSequence.IsPlaying();
    public bool IsPaused => currentSequence != null && !currentSequence.IsPlaying() && !currentSequence.IsComplete();
    public float ElapsedTime => currentSequence?.Elapsed() ?? 0f;
    public float Duration => currentSequence?.Duration() ?? 0f;
    public float Progress => Duration > 0 ? ElapsedTime / Duration : 0f;
    
    public void SetTimeScale(float timeScale)
    {
        currentSequence.timeScale = timeScale;
    }
    
    public void Goto(float time, bool andPlay = false)
    {
        currentSequence?.Goto(time, andPlay);
    }
    
    [ContextMenu("Preview First Step")]
    public void PreviewFirstStep()
    {
        if (tweenSteps.Count == 0) return;
        
        KillSequence();
        ResetToStart();
        
        var firstStep = tweenSteps[0];
        ApplyStepValues(firstStep);
    }
    
    [ContextMenu("Preview Last Step")]
    public void PreviewLastStep()
    {
        if (tweenSteps.Count == 0) return;
        
        KillSequence();
        ResetToStart();
        
        // Apply all step values in sequence to get final state
        foreach (var step in tweenSteps)
        {
            ApplyStepValues(step);
        }
    }
    
    void ApplyStepValues(DOTweenStep step)
    {
        Transform targetTransform = step.useCustomTarget && step.customTarget != null ? step.customTarget : transform;
        RectTransform targetRectTransform = step.useCustomTarget && step.customTarget != null ? 
            step.customTarget.GetComponent<RectTransform>() : rectTransform;
        
        switch (step.tweenType)
        {
            case TweenType.Move:
                targetTransform.position = step.targetVector;
                break;
                
            case TweenType.MoveLocal:
                targetTransform.localPosition = step.targetVector;
                break;
                
            case TweenType.MoveAnchor:
                if (targetRectTransform != null)
                    targetRectTransform.anchoredPosition = step.targetVector;
                break;
                
            case TweenType.Rotate:
                targetTransform.eulerAngles = step.targetVector;
                break;
                
            case TweenType.RotateLocal:
                targetTransform.localEulerAngles = step.targetVector;
                break;
                
            case TweenType.Scale:
                targetTransform.localScale = step.targetVector;
                break;
                
            case TweenType.Color:
                ApplyColorValue(step, targetTransform);
                break;
                
            case TweenType.Fade:
                ApplyFadeValue(step, targetTransform);
                break;
        }
    }
    
    void ApplyColorValue(DOTweenStep step, Transform target)
    {
        if (step.spriteRenderer != null)
            step.spriteRenderer.color = step.targetColor;
        else if (spriteRenderer != null)
            spriteRenderer.color = step.targetColor;
        else if (step.uiImage != null)
            step.uiImage.color = step.targetColor;
        else if (uiImage != null)
            uiImage.color = step.targetColor;
        else if (step.uiText != null)
            step.uiText.color = step.targetColor;
        else if (uiText != null)
            uiText.color = step.targetColor;
        else if (step.textMesh != null)
            step.textMesh.color = step.targetColor;
    }
    
    void ApplyFadeValue(DOTweenStep step, Transform target)
    {
        if (step.canvasGroup != null)
            step.canvasGroup.alpha = step.targetFloat;
        else if (canvasGroup != null)
            canvasGroup.alpha = step.targetFloat;
        else if (step.spriteRenderer != null)
        {
            var color = step.spriteRenderer.color;
            color.a = step.targetFloat;
            step.spriteRenderer.color = color;
        }
        else if (spriteRenderer != null)
        {
            var color = spriteRenderer.color;
            color.a = step.targetFloat;
            spriteRenderer.color = color;
        }
        else if (step.uiImage != null)
        {
            var color = step.uiImage.color;
            color.a = step.targetFloat;
            step.uiImage.color = color;
        }
        else if (uiImage != null)
        {
            var color = uiImage.color;
            color.a = step.targetFloat;
            uiImage.color = color;
        }
        else if (step.uiText != null)
        {
            var color = step.uiText.color;
            color.a = step.targetFloat;
            step.uiText.color = color;
        }
        else if (uiText != null)
        {
            var color = uiText.color;
            color.a = step.targetFloat;
            uiText.color = color;
        }
    }
    
    // Utility methods for complex sequences
    public void PlayStepsRange(int startIndex, int endIndex)
    {
        if (startIndex < 0 || endIndex >= tweenSteps.Count || startIndex > endIndex)
        {
            if (showDebugLogs)
                Debug.LogWarning($"Invalid range: {startIndex}-{endIndex}", this);
            return;
        }
        
        KillSequence();
        
        currentSequence = DOTween.Sequence();
        currentSequence.SetAutoKill(autoKill);
        
        for (int i = startIndex; i <= endIndex; i++)
        {
            var step = tweenSteps[i];
            Tween tween = CreateTween(step);
            
            if (tween != null)
            {
                if (step.operation == StepOperation.Append || i == startIndex)
                {
                    currentSequence.Append(tween);
                }
                else if (step.operation == StepOperation.Join)
                {
                    currentSequence.Join(tween);
                }
            }
        }
        
        currentSequence.Play();
    }
    
    public int GetJoinGroupCount()
    {
        int groups = 0;
        bool inJoinGroup = false;
        
        for (int i = 0; i < tweenSteps.Count; i++)
        {
            var step = tweenSteps[i];
            
            if (step.operation == StepOperation.Append || i == 0)
            {
                groups++;
                inJoinGroup = false;
            }
            else if (step.operation == StepOperation.Join && !inJoinGroup)
            {
                inJoinGroup = true;
            }
        }
        
        return groups;
    }
    
    void OnDestroy()
    {
        KillSequence();
    }
}

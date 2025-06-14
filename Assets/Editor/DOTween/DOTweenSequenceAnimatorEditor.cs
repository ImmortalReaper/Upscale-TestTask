using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DOTweenSequenceAnimator))]
public class DOTweenSequenceAnimatorEditor : Editor
{
    private SerializedProperty playOnStart;
    private SerializedProperty autoKill;
    private SerializedProperty sequenceLoop;
    private SerializedProperty sequenceLoopCount;
    private SerializedProperty sequenceLoopType;
    private SerializedProperty tweenSteps;
    private SerializedProperty showDebugLogs;
    
    // Control buttons
    private SerializedProperty playSequence;
    private SerializedProperty pauseSequence;
    private SerializedProperty resumeSequence;
    private SerializedProperty restartSequence;
    private SerializedProperty killSequence;
    private SerializedProperty resetToStart;
    private SerializedProperty previewFirstStep;
    private SerializedProperty previewLastStep;
    
    // Events
    private SerializedProperty OnSequenceStart;
    private SerializedProperty OnSequenceComplete;
    private SerializedProperty OnSequencePause;
    private SerializedProperty OnSequencePlay;
    private SerializedProperty OnSequenceKill;
    private SerializedProperty OnSequenceLoop;
    
    private bool showSequenceEvents = false;
    private bool showControlButtons = true;
    private bool showPreviewButtons = false;
    
    void OnEnable()
    {
        playOnStart = serializedObject.FindProperty("playOnStart");
        autoKill = serializedObject.FindProperty("autoKill");
        sequenceLoop = serializedObject.FindProperty("sequenceLoop");
        sequenceLoopCount = serializedObject.FindProperty("sequenceLoopCount");
        sequenceLoopType = serializedObject.FindProperty("sequenceLoopType");
        tweenSteps = serializedObject.FindProperty("tweenSteps");
        showDebugLogs = serializedObject.FindProperty("showDebugLogs");
        
        // Control buttons
        playSequence = serializedObject.FindProperty("playSequence");
        pauseSequence = serializedObject.FindProperty("pauseSequence");
        resumeSequence = serializedObject.FindProperty("resumeSequence");
        restartSequence = serializedObject.FindProperty("restartSequence");
        killSequence = serializedObject.FindProperty("killSequence");
        resetToStart = serializedObject.FindProperty("resetToStart");
        previewFirstStep = serializedObject.FindProperty("previewFirstStep");
        previewLastStep = serializedObject.FindProperty("previewLastStep");
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        DOTweenSequenceAnimator animator = (DOTweenSequenceAnimator)target;
        
        // Header
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("DOTween Sequence Animator", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        // Sequence Settings
        EditorGUILayout.LabelField("Sequence Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(playOnStart);
        EditorGUILayout.PropertyField(autoKill);
        EditorGUILayout.PropertyField(sequenceLoop);
        
        if (sequenceLoop.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(sequenceLoopCount, new GUIContent("Loop Count (-1 = infinite)"));
            EditorGUILayout.PropertyField(sequenceLoopType);
            EditorGUI.indentLevel--;
        }
        
        EditorGUILayout.Space();
        
        // Animation Steps
        EditorGUILayout.LabelField("Animation Steps", EditorStyles.boldLabel);
        
        // Steps info
        if (tweenSteps.arraySize > 0)
        {
            int joinGroups = animator.GetJoinGroupCount();
            EditorGUILayout.HelpBox($"Total Steps: {tweenSteps.arraySize} | Animation Groups: {joinGroups}", MessageType.Info);
        }
        
        EditorGUILayout.PropertyField(tweenSteps, true);
        
        EditorGUILayout.Space();
        
        // Runtime Info
        if (Application.isPlaying && animator.IsActive)
        {
            EditorGUILayout.LabelField("Runtime Info", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Toggle("Is Playing", animator.IsPlaying);
            EditorGUILayout.Toggle("Is Paused", animator.IsPaused);
            EditorGUILayout.Slider("Progress", animator.Progress, 0f, 1f);
            EditorGUILayout.FloatField("Elapsed Time", animator.ElapsedTime);
            EditorGUILayout.FloatField("Total Duration", animator.Duration);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space();
        }
        
        // Control Buttons
        showControlButtons = EditorGUILayout.Foldout(showControlButtons, "Control Buttons", true);
        if (showControlButtons)
        {
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("▶ Play"))
                animator.PlaySequence();
            if (GUILayout.Button("⏸ Pause"))
                animator.PauseSequence();
            if (GUILayout.Button("▶ Resume"))
                animator.ResumeSequence();
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("🔄 Restart"))
                animator.RestartSequence();
            if (GUILayout.Button("⏹ Kill"))
                animator.KillSequence();
            if (GUILayout.Button("🏠 Reset"))
                animator.ResetToStart();
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.EndDisabledGroup();
        }
        
        // Preview Buttons
        showPreviewButtons = EditorGUILayout.Foldout(showPreviewButtons, "Preview Helpers", true);
        if (showPreviewButtons)
        {
            EditorGUI.BeginDisabledGroup(!Application.isPlaying || tweenSteps.arraySize == 0);
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("👁 First Step"))
                animator.PreviewFirstStep();
            if (GUILayout.Button("👁 Last Step"))
                animator.PreviewLastStep();
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.EndDisabledGroup();
            
            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Preview buttons work only in Play Mode", MessageType.Info);
            }
        }
        
        EditorGUILayout.Space();
        
        // Debug
        EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(showDebugLogs);
        
        // Apply changes
        serializedObject.ApplyModifiedProperties();
        
        // Repaint in play mode for runtime info
        if (Application.isPlaying)
        {
            Repaint();
        }
    }
}

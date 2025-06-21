using System.Collections.Generic;
using Feature.AnimationModule.Scripts;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DOTweenStep))]
public class DOTweenStepDrawer : PropertyDrawer
{
    private const float lineHeight = 18f;
    private const float spacing = 2f;
    
    // Dictionary to store expanded state for each property
    private static Dictionary<string, bool> expandedStates = new Dictionary<string, bool>();
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        // Get unique key for this property
        string propertyKey = property.propertyPath;
        if (!expandedStates.ContainsKey(propertyKey))
            expandedStates[propertyKey] = false;
        
        var operation = property.FindPropertyRelative("operation");
        var tweenType = property.FindPropertyRelative("tweenType");
        var duration = property.FindPropertyRelative("duration");
        
        // Header with foldout
        var headerRect = new Rect(position.x, position.y, position.width, lineHeight);
        
        // Color code based on operation
        var originalColor = GUI.backgroundColor;
        if (operation.enumValueIndex == 0) // Append
            GUI.backgroundColor = new Color(0.8f, 1f, 0.8f); // Light green
        else // Join
            GUI.backgroundColor = new Color(0.8f, 0.8f, 1f); // Light blue
        
        string headerText = $"{((TweenType)tweenType.enumValueIndex)} ({duration.floatValue}s) - {((StepOperation)operation.enumValueIndex)}";
        expandedStates[propertyKey] = EditorGUI.Foldout(headerRect, expandedStates[propertyKey], headerText, true);
        
        GUI.backgroundColor = originalColor;
        
        if (expandedStates[propertyKey])
        {
            EditorGUI.indentLevel++;
            float yPos = position.y + lineHeight + spacing;
            
            // Step Operation
            var operationRect = new Rect(position.x, yPos, position.width, lineHeight);
            EditorGUI.PropertyField(operationRect, operation);
            yPos += lineHeight + spacing;
            
            // Show warning for first step if Join is selected
            if (GetStepIndex(property) == 0 && operation.enumValueIndex == 1)
            {
                var warningRect = new Rect(position.x, yPos, position.width, lineHeight);
                EditorGUI.HelpBox(warningRect, "First step is always Append!", MessageType.Warning);
                yPos += lineHeight + spacing;
            }
            
            // Tween Type
            var typeRect = new Rect(position.x, yPos, position.width, lineHeight);
            EditorGUI.PropertyField(typeRect, tweenType);
            yPos += lineHeight + spacing;
            
            // Duration
            var durationRect = new Rect(position.x, yPos, position.width, lineHeight);
            EditorGUI.PropertyField(durationRect, duration);
            yPos += lineHeight + spacing;
            
            // Delay
            var delay = property.FindPropertyRelative("delay");
            var delayRect = new Rect(position.x, yPos, position.width, lineHeight);
            EditorGUI.PropertyField(delayRect, delay);
            yPos += lineHeight + spacing;
            
            // Easing
            var useCustomCurve = property.FindPropertyRelative("useCustomCurve");
            var customCurveRect = new Rect(position.x, yPos, position.width, lineHeight);
            EditorGUI.PropertyField(customCurveRect, useCustomCurve);
            yPos += lineHeight + spacing;
            
            if (useCustomCurve.boolValue)
            {
                var customCurve = property.FindPropertyRelative("customCurve");
                var curveRect = new Rect(position.x, yPos, position.width, lineHeight);
                EditorGUI.PropertyField(curveRect, customCurve);
                yPos += lineHeight + spacing;
            }
            else
            {
                var easeType = property.FindPropertyRelative("easeType");
                var easeRect = new Rect(position.x, yPos, position.width, lineHeight);
                EditorGUI.PropertyField(easeRect, easeType);
                yPos += lineHeight + spacing;
            }
            
            // Target values based on tween type
            TweenType currentType = (TweenType)tweenType.enumValueIndex;
            yPos = DrawTargetFields(position, property, yPos, currentType);
            
            // Loop settings
            var enableLoop = property.FindPropertyRelative("enableLoop");
            var loopRect = new Rect(position.x, yPos, position.width, lineHeight);
            EditorGUI.PropertyField(loopRect, enableLoop);
            yPos += lineHeight + spacing;
            
            if (enableLoop.boolValue)
            {
                var loopCount = property.FindPropertyRelative("loopCount");
                var loopCountRect = new Rect(position.x, yPos, position.width, lineHeight);
                EditorGUI.PropertyField(loopCountRect, loopCount, new GUIContent("Loop Count (-1 = infinite)"));
                yPos += lineHeight + spacing;
                
                var loopType = property.FindPropertyRelative("loopType");
                var loopTypeRect = new Rect(position.x, yPos, position.width, lineHeight);
                EditorGUI.PropertyField(loopTypeRect, loopType);
                yPos += lineHeight + spacing;
            }
            
            // Special settings for Punch and Shake
            if (currentType == TweenType.Punch)
            {
                yPos = DrawPunchSettings(position, property, yPos);
            }
            else if (currentType == TweenType.Shake || currentType == TweenType.ShakeAnchor)
            {
                yPos = DrawShakeSettings(position, property, yPos);
            }
            
            // Target override
            var useCustomTarget = property.FindPropertyRelative("useCustomTarget");
            var customTargetRect = new Rect(position.x, yPos, position.width, lineHeight);
            EditorGUI.PropertyField(customTargetRect, useCustomTarget);
            yPos += lineHeight + spacing;
            
            if (useCustomTarget.boolValue)
            {
                // Component overrides for Fade and Color
                if (currentType == TweenType.Fade || currentType == TweenType.Color)
                {
                    yPos = DrawComponentOverrides(position, property, yPos, currentType);
                }
                else
                {
                    var customTarget = property.FindPropertyRelative("customTarget");
                    var targetRect = new Rect(position.x, yPos, position.width, lineHeight);
                    EditorGUI.PropertyField(targetRect, customTarget);
                    yPos += lineHeight + spacing;
                }
            }
            
            EditorGUI.indentLevel--;
        }
        
        EditorGUI.EndProperty();
    }
    
    private float DrawTargetFields(Rect position, SerializedProperty property, float yPos, TweenType tweenType)
    {
        switch (tweenType)
        {
            case TweenType.Move:
            case TweenType.MoveLocal:
            case TweenType.MoveAnchor: // Added support for new UI movement type
            case TweenType.Rotate:
            case TweenType.RotateLocal:
            case TweenType.Scale:
            case TweenType.Punch:
            case TweenType.Shake:
            case TweenType.ShakeAnchor: // Added support for new UI shake type
                var targetVector = property.FindPropertyRelative("targetVector");
                var vectorRect = new Rect(position.x, yPos, position.width, lineHeight);
                string label = GetVectorLabel(tweenType);
                EditorGUI.PropertyField(vectorRect, targetVector, new GUIContent(label));
                yPos += lineHeight + spacing;
                break;
                
            case TweenType.Fade:
                var targetFloat = property.FindPropertyRelative("targetFloat");
                var floatRect = new Rect(position.x, yPos, position.width, lineHeight);
                EditorGUI.Slider(floatRect, targetFloat, 0f, 1f, "Target Alpha");
                yPos += lineHeight + spacing;
                break;
                
            case TweenType.Color:
                var targetColor = property.FindPropertyRelative("targetColor");
                var colorRect = new Rect(position.x, yPos, position.width, lineHeight);
                EditorGUI.PropertyField(colorRect, targetColor, new GUIContent("Target Color"));
                yPos += lineHeight + spacing;
                break;
        }
        
        return yPos;
    }
    
    private float DrawPunchSettings(Rect position, SerializedProperty property, float yPos)
    {
        var punchVibrato = property.FindPropertyRelative("punchVibrato");
        var vibratoRect = new Rect(position.x, yPos, position.width, lineHeight);
        EditorGUI.PropertyField(vibratoRect, punchVibrato);
        yPos += lineHeight + spacing;
        
        var punchElasticity = property.FindPropertyRelative("punchElasticity");
        var elasticityRect = new Rect(position.x, yPos, position.width, lineHeight);
        EditorGUI.PropertyField(elasticityRect, punchElasticity);
        yPos += lineHeight + spacing;
        
        return yPos;
    }
    
    private float DrawShakeSettings(Rect position, SerializedProperty property, float yPos)
    {
        var shakeVibrato = property.FindPropertyRelative("shakeVibrato");
        var vibratoRect = new Rect(position.x, yPos, position.width, lineHeight);
        EditorGUI.PropertyField(vibratoRect, shakeVibrato);
        yPos += lineHeight + spacing;
        
        var shakeRandomness = property.FindPropertyRelative("shakeRandomness");
        var randomnessRect = new Rect(position.x, yPos, position.width, lineHeight);
        EditorGUI.PropertyField(randomnessRect, shakeRandomness);
        yPos += lineHeight + spacing;
        
        var shakeFadeOut = property.FindPropertyRelative("shakeFadeOut");
        var fadeOutRect = new Rect(position.x, yPos, position.width, lineHeight);
        EditorGUI.PropertyField(fadeOutRect, shakeFadeOut);
        yPos += lineHeight + spacing;
        
        return yPos;
    }
    
    private float DrawComponentOverrides(Rect position, SerializedProperty property, float yPos, TweenType tweenType)
    {
        if (tweenType == TweenType.Fade)
        {
            var canvasGroup = property.FindPropertyRelative("canvasGroup");
            var canvasRect = new Rect(position.x, yPos, position.width, lineHeight);
            EditorGUI.PropertyField(canvasRect, canvasGroup);
            yPos += lineHeight + spacing;
        }
        
        var spriteRenderer = property.FindPropertyRelative("spriteRenderer");
        var spriteRect = new Rect(position.x, yPos, position.width, lineHeight);
        EditorGUI.PropertyField(spriteRect, spriteRenderer);
        yPos += lineHeight + spacing;
        
        var uiImage = property.FindPropertyRelative("uiImage");
        var imageRect = new Rect(position.x, yPos, position.width, lineHeight);
        EditorGUI.PropertyField(imageRect, uiImage);
        yPos += lineHeight + spacing;
        
        var uiText = property.FindPropertyRelative("uiText");
        var textRect = new Rect(position.x, yPos, position.width, lineHeight);
        EditorGUI.PropertyField(textRect, uiText);
        yPos += lineHeight + spacing;
        
        return yPos;
    }
    
    private string GetVectorLabel(TweenType tweenType)
    {
        switch (tweenType)
        {
            case TweenType.Move:
            case TweenType.MoveLocal:
                return "Target Position";
            case TweenType.MoveAnchor: // Added label for new UI movement type
                return "Target Anchored Position";
            case TweenType.Rotate:
            case TweenType.RotateLocal:
                return "Target Rotation";
            case TweenType.Scale:
                return "Target Scale";
            case TweenType.Punch:
                return "Punch Strength";
            case TweenType.Shake:
                return "Shake Strength";
            case TweenType.ShakeAnchor: // Added label for new UI shake type
                return "Shake Anchor Strength";
            default:
                return "Target Vector";
        }
    }
    
    private int GetStepIndex(SerializedProperty property)
    {
        string path = property.propertyPath;
        int startIndex = path.LastIndexOf('[') + 1;
        int endIndex = path.LastIndexOf(']');
        if (startIndex > 0 && endIndex > startIndex)
        {
            string indexStr = path.Substring(startIndex, endIndex - startIndex);
            if (int.TryParse(indexStr, out int index))
                return index;
        }
        return 0;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Get unique key for this property
        string propertyKey = property.propertyPath;
        if (!expandedStates.ContainsKey(propertyKey))
            expandedStates[propertyKey] = false;

        if (!expandedStates[propertyKey])
            return lineHeight;

        float height = lineHeight + spacing; // Header

        var operation = property.FindPropertyRelative("operation");
        var tweenType = property.FindPropertyRelative("tweenType");
        var useCustomCurve = property.FindPropertyRelative("useCustomCurve");
        var enableLoop = property.FindPropertyRelative("enableLoop");
        var useCustomTarget = property.FindPropertyRelative("useCustomTarget");

        // Basic fields
        height += (lineHeight + spacing) * 4; // operation, tweenType, duration, delay

        // Warning for first step
        if (GetStepIndex(property) == 0 && operation.enumValueIndex == 1)
            height += lineHeight + spacing;

        // Easing
        height += lineHeight + spacing; // useCustomCurve
        height += lineHeight + spacing; // curve or ease type

        // Target values
        height += lineHeight + spacing;

        // Loop settings
        height += lineHeight + spacing; // enableLoop
        if (enableLoop.boolValue)
            height += (lineHeight + spacing) * 2; // loopCount, loopType

        // Special settings
        TweenType currentType = (TweenType)tweenType.enumValueIndex;
        if (currentType == TweenType.Shake || currentType == TweenType.ShakeAnchor) // Added ShakeAnchor support
            height += (lineHeight + spacing) * 3;
        else if (currentType == TweenType.Punch)
            height += (lineHeight + spacing) * 2;

        // Custom target
        height += lineHeight + spacing; // useCustomTarget
        if (useCustomTarget.boolValue)
            height += lineHeight + spacing;

        // Component overrides
        if (useCustomTarget.boolValue && (currentType == TweenType.Fade || currentType == TweenType.Color))
        {
            height += (lineHeight + spacing) * 2; // base components
            if (currentType == TweenType.Fade)
                height += lineHeight + spacing; // canvasGroup
        }

        return height;
    }
}

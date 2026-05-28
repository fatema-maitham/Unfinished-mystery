using UnityEngine;

/// <summary>
/// ScriptableObject holding data for a single instruction carousel slide.
/// Create assets via: Assets > Create > Instructions > Instruction Slide
/// </summary>
[CreateAssetMenu(fileName = "Instruction_00", menuName = "Instructions/Instruction Slide")]
public class InstructionData : ScriptableObject
{
    [Header("Slide Content")]
    [Tooltip("Title shown at the top of the slide (e.g. 'Place Your Bombs')")]
    public string title;

    [TextArea(3, 6)]
    [Tooltip("Body description shown beneath the image")]
    public string description;

    [Tooltip("Illustration shown in the centre of the slide")]
    public Sprite illustration;
}

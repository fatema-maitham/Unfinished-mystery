using UnityEngine;

/// <summary>
/// Attach to the Player. Detects nearby IActivatable objects and drives the UI prompt.
/// Scalable: teammates never touch this — they only implement IActivatable on their objects.
/// </summary>
public class PlayerActivationDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("How far the player can reach to interact")]
    [SerializeField] private float interactionRadius = 2.5f;

    [Tooltip("Layers that can contain activatables (set to your Interactable layer)")]
    [SerializeField] private LayerMask interactableLayer = ~0;

    [Tooltip("Max number of colliders checked per frame (keep low for performance)")]
    [SerializeField] private int maxColliders = 8;

    [Header("Input")]
    [Tooltip("Key to trigger activation")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("References")]
    [SerializeField] private ActivationPromptUI promptUI;

    // ── Runtime State ────────────────────────────────────────────────────────
    private IActivatable _currentTarget;
    private IActivatable _previousTarget;
    private readonly Collider[] _overlapResults = new Collider[8];

    // ── Unity ────────────────────────────────────────────────────────────────
    private void Update()
    {
        FindClosestActivatable();
        HandleInput();
    }

    // ── Detection ────────────────────────────────────────────────────────────
    private void FindClosestActivatable()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            interactionRadius,
            _overlapResults,
            interactableLayer
        );

        IActivatable closest = null;
        float closestDist = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            var activatable = _overlapResults[i].GetComponentInParent<IActivatable>();
            if (activatable == null || !activatable.CanActivate) continue;

            float dist = Vector3.Distance(transform.position, _overlapResults[i].transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = activatable;
            }
        }

        SetTarget(closest);
    }

    private void SetTarget(IActivatable newTarget)
    {
        if (newTarget == _currentTarget) return;

        _currentTarget?.OnActivatableBlur();

        _currentTarget = newTarget;

        if (_currentTarget != null)
        {
            _currentTarget.OnActivatableFocus();
            promptUI?.ShowPrompt(_currentTarget.ActivationLabel, _currentTarget.ActivationHint);
        }
        else
        {
            promptUI?.HidePrompt();
        }
    }

    // ── Input ────────────────────────────────────────────────────────────────
    private void HandleInput()
    {
        if (_currentTarget == null) return;
        if (!Input.GetKeyDown(interactKey)) return;
        if (!_currentTarget.CanActivate) return;

        _currentTarget.OnActivate(gameObject);
    }

    // ── Gizmos ───────────────────────────────────────────────────────────────
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.85f, 0f, 0.25f);
        Gizmos.DrawSphere(transform.position, interactionRadius);
        Gizmos.color = new Color(1f, 0.85f, 0f, 0.85f);
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
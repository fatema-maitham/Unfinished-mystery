using UnityEngine;

public class PlayerActivationDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float interactionRadius = 10f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private int maxColliders = 32;

    [Header("Input")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("References")]
    [SerializeField] private ActivationPromptUI promptUI;

    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;

    private IActivatable _currentTarget;
    private Collider[] _overlapResults;

    private void Awake()
    {
        _overlapResults = new Collider[maxColliders];
    }

    private void Update()
    {
        FindClosestActivatable();
        HandleInput();
    }

    private void FindClosestActivatable()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            interactionRadius,
            _overlapResults,
            interactableLayer,
            QueryTriggerInteraction.Collide
        );

        if (showDebugLogs)
        {
            Debug.Log("Found colliders: " + count);
        }

        IActivatable closest = null;
        float closestDist = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            if (_overlapResults[i] == null) continue;

            IActivatable activatable = _overlapResults[i].GetComponentInParent<IActivatable>();

            if (showDebugLogs)
            {
                Debug.Log(
                    "Hit: " + _overlapResults[i].name +
                    " | Layer: " + LayerMask.LayerToName(_overlapResults[i].gameObject.layer) +
                    " | Activatable: " + (activatable != null)
                );
            }

            if (activatable == null) continue;
            if (!activatable.CanActivate) continue;

            float dist = Vector3.Distance(
                transform.position,
                _overlapResults[i].ClosestPoint(transform.position)
            );

            if (showDebugLogs)
            {
                Debug.Log("Distance to activatable: " + dist + " | Allowed radius: " + activatable.ActivationRadius);
            }

            if (dist > activatable.ActivationRadius) continue;

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
            if (showDebugLogs)
            {
                Debug.Log("CURRENT TARGET FOUND: " + _currentTarget.ActivationHint);
            }

            _currentTarget.OnActivatableFocus();

            if (promptUI != null)
            {
                promptUI.ShowPrompt(_currentTarget.ActivationLabel, _currentTarget.ActivationHint);
            }
            else
            {
                Debug.LogError("Prompt UI is NOT assigned on PlayerActivationDetector.");
            }
        }
        else
        {
            if (promptUI != null)
            {
                promptUI.HidePrompt();
            }
        }
    }

    private void HandleInput()
    {
        if (_currentTarget == null) return;

        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log("Pressed E on target: " + _currentTarget.ActivationHint);
            _currentTarget.OnActivate(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
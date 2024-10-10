#region

using UnityEngine;

#endregion

namespace JadeLib.Features.Utility;

/// <summary>
///     A <see cref="MonoBehaviour" /> that includes a reference hub.
/// </summary>
public abstract class ReferenceHubBehaviour : MonoBehaviour
{
    /// <summary>
    ///     Gets or sets the reference hub for this behaviour, can be undefined.
    /// </summary>
    public ReferenceHub ReferenceHub { get; protected set; }

    /// <summary>
    ///     Gets or sets the unloadables for this behaviour, can be empty.
    /// </summary>
    public IUnloadable[] Unloadables { get; protected set; } = [];

    private void Start()
    {
        if (!ReferenceHub.TryGetHub(this.gameObject, out var referenceHub))
        {
            Destroy(this);
            throw new UnityException("ReferenceHub not found");
        }

        this.ReferenceHub = referenceHub;
    }

    private void OnDestroy()
    {
        this.Destroy();
        this.Unloadables.ForEach(unloadable => unloadable.Unload());
    }

    public abstract void Destroy();
}
using Content.Client.Light.EntitySystems;
using Content.Shared.Light.Components;

namespace Content.Client.Light.Components;

/// <summary>
/// Component that represents a handheld expendable light which can be activated and eventually dies over time.
/// </summary>
[RegisterComponent]
public sealed partial class ExpendableLightComponent : SharedExpendableLightComponent
{
    /// <summary>
    /// The icon state used by expendable lights when the they have been completely expended.
    /// </summary>
    [DataField]
    public string? IconStateSpent;

    /// <summary>
    /// The icon state used by expendable lights while they are lit.
    /// </summary>
    [DataField]
    public string? IconStateLit;

    /// <summary>
    /// The sprite layer shader used while the expendable light is lit.
    /// </summary>
    [DataField]
    public string? SpriteShaderLit = null;

    /// <summary>
    /// The sprite layer shader used after the expendable light has burnt out.
    /// </summary>
    [DataField]
    public string? SpriteShaderSpent = null;

    /// <summary>
    /// The sprite layer shader used after the expendable light has burnt out.
    /// </summary>
    [DataField]
    public Color? GlowColorLit = null;

    /// <summary>
    /// The sound that plays when the expendable light is lit.
    /// </summary>
    [Access(typeof(ExpendableLightSystem))]
    public EntityUid? PlayingStream;
}

public enum ExpendableLightVisualLayers : byte
{
    Base = 0,
    Glow = 1,
    Overlay = 2,
}

namespace Wagner.Drm;

[Flags]
public enum DrmAvailableNode
{
    Primary = 1 << DrmNode.Primary,
    Control = 1 << DrmNode.Control,
    Renderer = 1 << DrmNode.Renderer,
    Max = 1 << DrmNode.Max
}

using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace SkyEye;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool Overlay2D_Enabled = true;
    public bool Overlay2D_ShowCenter = false;
    public bool Overlay2D_ShowAssist = false;

    public bool Overlay2D_TextStroke = true;
    public float Overlay2D_DotSize = 5f;
    public float Overlay2D_DotStroke = 1f;

    [NonSerialized] private DalamudPluginInterface? PluginInterface;

    public void Initialize(DalamudPluginInterface pluginInterface)
    {
        PluginInterface = pluginInterface;
    }

    public void Save()
    {
        PluginInterface!.SavePluginConfig(this);
    }
}

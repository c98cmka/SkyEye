using Dalamud.Configuration;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;

namespace SkyEye;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool Overlay2D_Enabled = true;
    public bool Overlay2D_WeatherMap_Enabled = true;
    public bool Overlay2D_SpeedUp_Enabled = true;
    public bool NeedRabbit = false;
    public bool Overlay2D_ShowCenter = false;
    public bool Overlay2D_ShowAssist = false;

    public bool Overlay2D_TextStroke = true;
    public float Overlay2D_DotSize = 5f;
    public float Overlay2D_DotStroke = 1f;

    public bool Overlay3D_Enabled = true;

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

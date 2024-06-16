using System;
using System.Numerics;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;

namespace SkyEye;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration;

    private Plugin plu;

    // We give this window a constant ID using ###
    // This allows for labels being dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
    public ConfigWindow(Plugin plugin) : base("SkyEye")
    {
        Flags =  ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        SizeCondition = ImGuiCond.Always;
        plu = plugin;
        Configuration = Plugin.Configuration;
    }

    public void Dispose() { }

    public override void PreDraw()
    {
        // Flags must be added or removed before Draw() is being called, or they won't apply
        Configuration.Save();
    }

    public override void Draw()
    {
        if (ImGui.Checkbox("开关", ref Configuration.Overlay2D_Enabled)) {
            Configuration.Save();
        }
        if (ImGui.Checkbox("稀有天气时间开关", ref Configuration.Overlay2D_WeatherMap_Enabled)) {
            Configuration.Save();
        }
        if (ImGui.Checkbox("宝箱位置绘制开关", ref Configuration.Overlay3D_Enabled))
        {
            Configuration.Save();
        }
        if (ImGui.Checkbox("无人就加速", ref Configuration.Overlay2D_SpeedUp_Enabled))
        {
            Configuration.Save();
        }
        ImGui.SameLine();
        if (ImGui.Button("reset"))
        {
            Plugin.SetSpeed(1.0f * Plugin.speedOffset);
        }
        //if (ImGui.Checkbox("仅在幸福兔BUFF中启用", ref Configuration.NeedRabbit))
        //{
        //    Configuration.Save();
        //}
        ImGui.Text("周围人数：" + plu.OtherPlayer.Count);


        //if (ImGui.Button("123"))
        //{
        //    Camera.WorldToScreen(Plugin.clientState.LocalPlayer.Position, out Vector2 v);
        //}
    }
}

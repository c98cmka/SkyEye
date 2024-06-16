using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Dalamud.Interface;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Utility;
using ImGuiNET;

namespace SkyEye;

internal static class ImguiUtil
{
    //2D
    public static void DrawText(this ImDrawListPtr drawList, Vector2 pos, string text, uint col, bool stroke, bool centerAlignX = true, uint strokecol = 4278190080u)
    {
        if (centerAlignX)
        {
            pos -= new Vector2(ImGui.CalcTextSize(text).X, 0f) / 2f;
        }
        if (stroke)
        {
            drawList.AddText(pos + new Vector2(-1f, -1f), strokecol, text);
            drawList.AddText(pos + new Vector2(-1f, 1f), strokecol, text);
            drawList.AddText(pos + new Vector2(1f, -1f), strokecol, text);
            drawList.AddText(pos + new Vector2(1f, 1f), strokecol, text);
        }
        drawList.AddText(pos, col, text);
    }

    public static void DrawTextNoSwift(this ImDrawListPtr drawList, Vector2 pos, string text, uint col, bool stroke, bool centerAlignX = true, uint strokecol = 4278190080u)
    {
        if (centerAlignX)
        {
            pos -= new Vector2(ImGui.CalcTextSize(text).X, 0f) / 2f;
        }
        if (stroke)
        {
            drawList.AddText(pos + new Vector2(-1f, -1f), strokecol, text);
            drawList.AddText(pos + new Vector2(-1f, 1f), strokecol, text);
            drawList.AddText(pos + new Vector2(1f, -1f), strokecol, text);
            drawList.AddText(pos + new Vector2(1f, 1f), strokecol, text);
        }
        drawList.AddText(pos, col, text);
    }

    public static void DrawMapTextDot(this ImDrawListPtr drawList, Vector2 pos, string str, uint fgcolor, uint bgcolor)
    {
        if (!string.IsNullOrWhiteSpace(str))
        {
            drawList.DrawText(pos, str, fgcolor, Plugin.Configuration.Overlay2D_TextStroke, centerAlignX: true, bgcolor);
        }
        drawList.AddCircleFilled(pos, Plugin.Configuration.Overlay2D_DotSize, fgcolor);
        if (Plugin.Configuration.Overlay2D_DotStroke != 0f)
        {
            drawList.AddCircle(pos, Plugin.Configuration.Overlay2D_DotSize, bgcolor, 0, Plugin.Configuration.Overlay2D_DotStroke);
        }
    }

    public static void DrawMapDot(this ImDrawListPtr drawList, Vector2 pos, uint fgcolor, uint bgcolor)
    {
        drawList.AddCircleFilled(pos, 4f, fgcolor);
        if (Plugin.Configuration.Overlay2D_DotStroke != 0f)
        {
            drawList.AddCircle(pos, 2f, bgcolor, 0, Plugin.Configuration.Overlay2D_DotStroke);
        }
    }

    public static Vector2 Rotate(this Vector2 vin, float rotation)
    {
        return vin.Rotate(new Vector2((float)Math.Sin(rotation), (float)Math.Cos(rotation)));
    }
    public static Vector2 Rotate(this Vector2 vin, Vector2 rotation)
    {
        rotation = rotation.Normalize();
        return new Vector2(rotation.Y * vin.X + rotation.X * vin.Y, rotation.Y * vin.Y - rotation.X * vin.X);
    }
    public static Vector2 Normalize(this Vector2 v)
    {
        float num = v.Length();
        if (num != 0)
        {
            float num2 = 1f / num;
            v.X *= num2;
            v.Y *= num2;
            return v;
        }
        return v;
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Fates;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Game.Fate;
using FFXIVClientStructs.FFXIV.Client.System.Resource.Handle;
using FFXIVClientStructs.FFXIV.Component.GUI;
using FFXIVClientStructs.STD;
using ImGuiNET;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;
using SkyEye.Data;
using SkyEye.Data.Positions;
using static Lumina.Data.Parsing.Layer.LayerCommon;

namespace SkyEye;

public unsafe class UIBuilder : IDisposable
{
    private DalamudPluginInterface pi;

    private readonly Dictionary<uint, ushort> sizeFactorDict;
    //private Dictionary<ushort, Map[]> TerritoryMapsDictionary { get; }

    private ImDrawListPtr BDL;

    private Vector2? mapOrigin = Vector2.Zero;
    private Vector2[] MapPosSize = new Vector2[2];

    private List<(Vector3 worldpos, uint fgcolor, uint bgcolor, string name)> ObjectList2D = new List<(Vector3, uint, uint, string)>();

    private List<(Vector3 worldpos, uint fgcolor, uint bgcolor, string name, string fateId, EurekaWeather SpawnRequiredWeather, bool SpawnByRequiredNight)> EurekaList2D = new List<(Vector3, uint, uint, string, string, EurekaWeather, bool)>();
    private List<string> EurekaLiveIdList2D = new List<string>();
    private List<string> EurekaLiveIdList2D_old = new List<string>();

    EurekaAnemos eurekaAnemos = new EurekaAnemos(new List<EurekaFate>());
    EurekaPagos eurekaPagos = new EurekaPagos(new List<EurekaFate>());
    EurekaPyros eurekaPyros = new EurekaPyros(new List<EurekaFate>());
    EurekaHydatos eurekaHydatos = new EurekaHydatos(new List<EurekaFate>());

    public static SoundPlayer player1 = new SoundPlayer();
    public static SoundPlayer player2 = new SoundPlayer();

    List<(EurekaWeather Weather, TimeSpan Time)> weathers;
    (EurekaWeather Weather, TimeSpan Time) weatherNow;
    Dictionary<EurekaWeather, (string, string)> weatherDic;

    EorzeaTime eorzeaTime;

    public const uint Red = 4278190335u;

    public const uint Magenta = 4294902015u;

    public const uint Yellow = 4278255615u;

    public const uint Green = 4278255360u;

    public const uint GrassGreen = 4278247424u;

    public const uint Cyan = 4294967040u;

    public const uint DarkCyan = 4287664128u;

    public const uint LightCyan = 4294967200u;

    public const uint Blue = 4294901760u;

    public const uint Black = 4278190080u;

    public const uint TransBlack = 2147483648u;

    public const uint Grey = 4286611584u;

    public const uint White = uint.MaxValue;

    private float GlobalUIScale = 1f;
    private float WorldToMapScale => AreaMap.MapScale * sizeFactorDict[Plugin.clientState.TerritoryType] / 100f * GlobalUIScale;

    public UIBuilder(Plugin plugin, DalamudPluginInterface pluginInterface)
    {
        pi = pluginInterface;

        sizeFactorDict = Plugin.dataManager.GetExcelSheet<TerritoryType>().ToDictionary((TerritoryType k) => k.RowId, (TerritoryType v) => v.Map.Value.SizeFactor);
        //TerritoryMapsDictionary = (from i in Plugin.dataManager.GetExcelSheet<Map>().GroupBy(delegate (Map i)
        //{
        //    LazyRow<TerritoryType> territoryType = i.TerritoryType;
        //    if (territoryType == null)
        //    {
        //        return null;
        //    }
        //    TerritoryType value = territoryType.Value;
        //    return (value == null) ? null : new uint?(value.RowId);
        //}) where i.Key.HasValue && i.Key != 0 select i).ToDictionary((IGrouping<uint?, Map> i) => (ushort)i.Key.Value, (IGrouping<uint?, Map> j) => j.ToArray());
        Plugin.clientState.TerritoryChanged += TerritoryChanged;
        pi.UiBuilder.Draw += UiBuilder_OnBuildUi;
    }

    private void TerritoryChanged(ushort territoryId)
    {
        //切图清空记录
        Plugin.log.Error($"territory changed to: {territoryId}", Array.Empty<object>());
        EurekaAnemos.deadFateDic = new Dictionary<int, string>() { { 1332, "-1" }, { 1348, "-1" }, { 1333, "-1" }, { 1328, "-1" }, { 1344, "-1" }, { 1347, "-1" }, { 1345, "-1" }, { 1334, "-1" }, { 1335, "-1" }, { 1336, "-1" }, { 1339, "-1" }, { 1346, "-1" }, { 1343, "-1" }, { 1337, "-1" }, { 1342, "-1" }, { 1341, "-1" }, { 1331, "-1" }, { 1340, "-1" }, { 1338, "-1" }, { 1329, "-1" } };
        EurekaPagos.deadFateDic = new Dictionary<int, string>() { { 1351, "-1" }, { 1369, "-1" }, { 1353, "-1" }, { 1354, "-1" }, { 1355, "-1" }, { 1366, "-1" }, { 1357, "-1" }, { 1356, "-1" }, { 1352, "-1" }, { 1360, "-1" }, { 1358, "-1" }, { 1361, "-1" }, { 1362, "-1" }, { 1359, "-1" }, { 1363, "-1" }, { 1365, "-1" }, { 1364, "-1" }, { 1367, "-1" }, { 1368, "-1" } };
        EurekaPyros.deadFateDic = new Dictionary<int, string>() { { 1388, "-1" }, { 1389, "-1" }, { 1390, "-1" }, { 1391, "-1" }, { 1392, "-1" }, { 1393, "-1" }, { 1394, "-1" }, { 1395, "-1" }, { 1396, "-1" }, { 1397, "-1" }, { 1398, "-1" }, { 1399, "-1" }, { 1400, "-1" }, { 1401, "-1" }, { 1402, "-1" }, { 1403, "-1" }, { 1404, "-1" }, { 1407, "-1" }, { 1408, "-1" } };
        EurekaHydatos.deadFateDic = new Dictionary<int, string>() { { 1412, "-1" }, { 1413, "-1" }, { 1414, "-1" }, { 1415, "-1" }, { 1416, "-1" }, { 1417, "-1" }, { 1418, "-1" }, { 1419, "-1" }, { 1420, "-1" }, { 1421, "-1" }, { 1422, "-1" }, { 1423, "-1" }, { 1424, "-1" }, { 1425, "-1" } };
    }

    public void Dispose()
    {
        pi.UiBuilder.Draw -= UiBuilder_OnBuildUi;
        Plugin.clientState.TerritoryChanged -= TerritoryChanged;
    }

    private unsafe void UiBuilder_OnBuildUi()
    {
        bool flag = false;
        try
        {
            if (Plugin.clientState.LocalPlayer != null && 
                (Plugin.clientState.TerritoryType == 732 || Plugin.clientState.TerritoryType == 763 || Plugin.clientState.TerritoryType == 795 || Plugin.clientState.TerritoryType == 827) &&
                !(Plugin.condition[ConditionFlag.BetweenAreas] || Plugin.condition[ConditionFlag.BetweenAreas51]))
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
        }
        catch (Exception)
        {
            flag = false;
        }
        if (flag)
        {
            eorzeaTime = EorzeaTime.ToEorzeaTime(DateTime.Now);

            BDL = ImGui.GetBackgroundDrawList(ImGui.GetMainViewport());
            //RefreshObjects();
            RefreshEureka();
            if (Plugin.Configuration.Overlay2D_Enabled)
            {
                DrawMapOverlay();
            }
            //Plugin.log.Error(Convert.ToDateTime(DateTime.Now.ToString().Replace("/", "-")).ToString());
        }
        ObjectList2D.Clear();
        EurekaList2D.Clear();
        foreach (var item in EurekaLiveIdList2D) {
            EurekaLiveIdList2D_old.Add(item);
        }
        EurekaLiveIdList2D.Clear();
    }

    private void RefreshObjects()
    {
        foreach (var o in Plugin.objects)
        {
            if (o.Name is not null && !"".Equals(o.Name) && o.ObjectId != Plugin.clientState.LocalPlayer.ObjectId)
            {
                ObjectList2D.Add((o.Position, White, White, o.Name.ToString()));
            }
        }
    }

    private unsafe void RefreshEureka()
    {
        switch (Plugin.clientState.TerritoryType)
        {
            case 732:
                //now
                foreach (var o in Plugin.fates)
                {
                    if (o.Name is not null && !"".Equals(o.Name))
                    {
                        EurekaLiveIdList2D.Add(o.FateId.ToString());
                        if (!EurekaLiveIdList2D_old.Contains(o.FateId.ToString())) {
                            NMFound();
                        }
                        EurekaAnemos.deadFateDic[o.FateId] = "1";
                    }

                }
                EurekaLiveIdList2D_old.Clear();
                foreach (KeyValuePair<int, string> o in EurekaAnemos.deadFateDic)
                {
                    if (o.Value.Contains(":")) //时间够两小时的更新成-1
                    {
                        TimeSpan minuteSpan = new TimeSpan(DateTime.Now.Ticks - Convert.ToDateTime(o.Value).Ticks);
                        if (minuteSpan.TotalHours >= 2) //超过两小时
                        {
                            EurekaAnemos.deadFateDic[o.Key] = "-1"; //cd好了
                        }
                    }
                }
                break;
            case 763:
                //now
                foreach (var o in Plugin.fates)
                {
                    if (o.Name is not null && !"".Equals(o.Name))
                    {
                        EurekaLiveIdList2D.Add(o.FateId.ToString());
                        if (!EurekaLiveIdList2D_old.Contains(o.FateId.ToString()))
                        {
                            if (o.FateId == 1367 || o.FateId == 1368)
                            {
                                TZFound();
                            }
                            else {
                                NMFound();
                            }
                        }
                        EurekaPagos.deadFateDic[o.FateId] = "1";
                    }

                }
                EurekaLiveIdList2D_old.Clear();
                foreach (KeyValuePair<int, string> o in EurekaPagos.deadFateDic)
                {
                    if (o.Value.Contains(":")) //时间够两小时的更新成-1
                    {
                        TimeSpan minuteSpan = new TimeSpan(DateTime.Now.Ticks - Convert.ToDateTime(o.Value).Ticks);
                        if (o.Key == 1367 || o.Key == 1368)
                        { //兔子
                            if (minuteSpan.TotalMinutes >= 8) //超过8分钟
                            {
                                EurekaPagos.deadFateDic[o.Key] = "-1"; //cd好了
                            }
                        }
                        else
                        {
                            if (minuteSpan.TotalHours >= 2) //超过两小时
                            {
                                EurekaPagos.deadFateDic[o.Key] = "-1"; //cd好了
                            }
                        }
                    }
                }
                break;
            case 795:
                //now
                foreach (var o in Plugin.fates)
                {
                    if (o.Name is not null && !"".Equals(o.Name))
                    {
                        EurekaLiveIdList2D.Add(o.FateId.ToString());
                        if (!EurekaLiveIdList2D_old.Contains(o.FateId.ToString()))
                        {
                            if (o.FateId == 1407 || o.FateId == 1408)
                            {
                                TZFound();
                            }
                            else
                            {
                                NMFound();
                            }
                        }
                        EurekaPyros.deadFateDic[o.FateId] = "1";
                    }

                }
                EurekaLiveIdList2D_old.Clear();
                foreach (KeyValuePair<int, string> o in EurekaPyros.deadFateDic)
                {
                    if (o.Value.Contains(":")) //时间够两小时的更新成-1
                    {
                        TimeSpan minuteSpan = new TimeSpan(DateTime.Now.Ticks - Convert.ToDateTime(o.Value).Ticks);
                        if (o.Key == 1407 || o.Key == 1408)
                        { //兔子
                            if (minuteSpan.TotalMinutes >= 8) //超过8分钟
                            {
                                EurekaPyros.deadFateDic[o.Key] = "-1"; //cd好了
                            }
                        }
                        else
                        {
                            if (minuteSpan.TotalHours >= 2) //超过两小时
                            {
                                EurekaPyros.deadFateDic[o.Key] = "-1"; //cd好了
                            }
                        }
                    }
                }
                break;
            case 827:
                //now
                foreach (var o in Plugin.fates)
                {
                    if (o.Name is not null && !"".Equals(o.Name))
                    {
                        EurekaLiveIdList2D.Add(o.FateId.ToString());
                        if (!EurekaLiveIdList2D_old.Contains(o.FateId.ToString()))
                        {
                            if (o.FateId == 1425)
                            {
                                TZFound();
                            }
                            else
                            {
                                NMFound();
                            }
                        }
                        EurekaHydatos.deadFateDic[o.FateId] = "1";
                    }

                }
                EurekaLiveIdList2D_old.Clear();
                foreach (KeyValuePair<int, string> o in EurekaHydatos.deadFateDic)
                {
                    if (o.Value.Contains(":")) //时间够两小时的更新成-1
                    {
                        TimeSpan minuteSpan = new TimeSpan(DateTime.Now.Ticks - Convert.ToDateTime(o.Value).Ticks);
                        if (o.Key == 1425)
                        { //兔子
                            if (minuteSpan.TotalMinutes >= 10.5) //超过10.5分钟
                            {
                                EurekaHydatos.deadFateDic[o.Key] = "-1"; //cd好了
                            }
                        }
                        else
                        {
                            if (minuteSpan.TotalHours >= 2) //超过两小时
                            {
                                EurekaHydatos.deadFateDic[o.Key] = "-1"; //cd好了
                            }
                        }
                    }
                }
                break;
            default:
                Plugin.log.Error("not in Eureka.");
                break;
        }

        //all
        if (Plugin.clientState.TerritoryType == 732) //风岛 Anemos
        {
            //获取天气
            weathers = eurekaAnemos.GetAllNextWeatherTime();
            weatherNow = eurekaAnemos.GetCurrentWeatherInfo();
            weatherDic = new Dictionary<EurekaWeather, (string, string)>();
            foreach (var o in weathers)
            {
                (DateTime Start, DateTime End) timeFromNextWeather = EorzeaWeather.GetWeatherUptime(o.Weather, EurekaAnemos.Weathers, DateTime.Now);
                TimeSpan TimeLeft = timeFromNextWeather.End - DateTime.Now;
                weatherDic.Add(o.Weather, (o.Time.ToString(@"hh\:mm\:ss"), TimeLeft.ToString(@"hh\:mm\:ss")));
            }



            Map map = Plugin.dataManager.GetExcelSheet<Map>()?.GetRow(732);
            foreach (var o in EurekaAnemos.AnemosFates)
            {
                EurekaList2D.Add((ToVector3(MapToWorld(o.FatePosition, map)), White, White, o.BossShortName.ToString(), o.FateId.ToString(), o.SpawnRequiredWeather, o.SpawnByRequiredNight));
            }
        }
        else if (Plugin.clientState.TerritoryType == 763) //冰岛 Pagos
        {
            //获取天气
            weathers = eurekaPagos.GetAllNextWeatherTime();
            weatherNow = eurekaPagos.GetCurrentWeatherInfo();
            weatherDic = new Dictionary<EurekaWeather, (string, string)>();
            foreach (var o in weathers)
            {
                (DateTime Start, DateTime End) timeFromNextWeather = EorzeaWeather.GetWeatherUptime(o.Weather, EurekaPagos.Weathers, DateTime.Now);
                TimeSpan TimeLeft = timeFromNextWeather.End - DateTime.Now;
                weatherDic.Add(o.Weather, (o.Time.ToString(@"hh\:mm\:ss"), TimeLeft.ToString(@"hh\:mm\:ss")));
            }



            Map map = Plugin.dataManager.GetExcelSheet<Map>()?.GetRow(763);
            foreach (var o in EurekaPagos.PagosFates)
            {
                EurekaList2D.Add(((ToVector3(o.FatePosition)), White, White, o.BossShortName.ToString(), o.FateId.ToString(), o.SpawnRequiredWeather, o.SpawnByRequiredNight));
            }
        }
        else if (Plugin.clientState.TerritoryType == 795) //火岛 Pyros
        {
            //获取天气
            weathers = eurekaPyros.GetAllNextWeatherTime();
            weatherNow = eurekaPyros.GetCurrentWeatherInfo();
            weatherDic = new Dictionary<EurekaWeather, (string, string)>();
            foreach (var o in weathers)
            {
                (DateTime Start, DateTime End) timeFromNextWeather = EorzeaWeather.GetWeatherUptime(o.Weather, EurekaPyros.Weathers, DateTime.Now);
                TimeSpan TimeLeft = timeFromNextWeather.End - DateTime.Now;
                weatherDic.Add(o.Weather, (o.Time.ToString(@"hh\:mm\:ss"), TimeLeft.ToString(@"hh\:mm\:ss")));
            }



            Map map = Plugin.dataManager.GetExcelSheet<Map>()?.GetRow(795);
            foreach (var o in EurekaPyros.PyrosFates)
            {
                EurekaList2D.Add((ToVector3(o.FatePosition), White, White, o.BossShortName.ToString(), o.FateId.ToString(), o.SpawnRequiredWeather, o.SpawnByRequiredNight));
            }
        }
        else if (Plugin.clientState.TerritoryType == 827) //水岛 Hydatos
        {
            //获取天气
            weathers = eurekaHydatos.GetAllNextWeatherTime();
            weatherNow = eurekaHydatos.GetCurrentWeatherInfo();
            weatherDic = new Dictionary<EurekaWeather, (string, string)>();
            foreach (var o in weathers)
            {
                (DateTime Start, DateTime End) timeFromNextWeather = EorzeaWeather.GetWeatherUptime(o.Weather, EurekaHydatos.Weathers, DateTime.Now);
                TimeSpan TimeLeft = timeFromNextWeather.End - DateTime.Now;
                weatherDic.Add(o.Weather, (o.Time.ToString(@"hh\:mm\:ss"), TimeLeft.ToString(@"hh\:mm\:ss")));
            }



            Map map = Plugin.dataManager.GetExcelSheet<Map>()?.GetRow(827);
            foreach (var o in EurekaHydatos.HydatosFates)
            {
                EurekaList2D.Add((ToVector3(o.FatePosition), White, White, o.BossShortName.ToString(), o.FateId.ToString(), o.SpawnRequiredWeather, o.SpawnByRequiredNight));
            }
        }
    }

    private void DrawMapOverlay()
    {
        RefreshMapOrigin(); //刷新小地图位置
        Vector2? vector = mapOrigin;
        if (!vector.HasValue)
        {
            return;
        }
        Vector2 valueOrDefault = vector.GetValueOrDefault();
        if (!(valueOrDefault != Vector2.Zero) || Plugin.clientState.TerritoryType == 0)
        {
            return;
        }
        BDL.PushClipRect(MapPosSize[0], MapPosSize[1]); //仅在小地图区域内绘制

        foreach (var item in ObjectList2D)
        {
            Vector2 pos = WorldToMap(valueOrDefault, item.worldpos);
            BDL.DrawMapTextDot(pos, item.name, item.fgcolor, item.bgcolor);
        }


        foreach (var item in EurekaList2D)
        {
            Vector2 pos = WorldToMap(valueOrDefault, item.worldpos);
            if (EurekaLiveIdList2D.Contains(item.fateId)) //已经出了
            {
                //NM进度
                var fateProgress = FateManager.Instance()->GetFateById(ushort.Parse(item.fateId))->Progress;
                if (fateProgress > 0)
                {
                    BDL.DrawText(pos, item.name + "(" + fateProgress + "%)", GrassGreen, true);
                }
                else {
                    BDL.DrawText(pos, item.name, GrassGreen, true);
                }
                //NM死了更新
                if (fateProgress == 98 || fateProgress == 99 || fateProgress == 100)
                {
                    switch (Plugin.clientState.TerritoryType)
                    {
                        case 732:
                            EurekaAnemos.deadFateDic[ushort.Parse(item.fateId)] = DateTime.Now.ToString().Replace("/", "-");
                            break;
                        case 763:
                            EurekaPagos.deadFateDic[ushort.Parse(item.fateId)] = DateTime.Now.ToString().Replace("/", "-");
                            break;
                        case 795:
                            EurekaPyros.deadFateDic[ushort.Parse(item.fateId)] = DateTime.Now.ToString().Replace("/", "-");
                            break;
                        case 827:
                            EurekaHydatos.deadFateDic[ushort.Parse(item.fateId)] = DateTime.Now.ToString().Replace("/", "-");
                            break;
                        default:
                            Plugin.log.Error("not in Eureka.");
                            break;
                    }
                }
            }
            else
            { //还没出的
                if ((Plugin.clientState.TerritoryType == 732 && EurekaAnemos.deadFateDic[ushort.Parse(item.fateId)].Contains(":")) ||
                    (Plugin.clientState.TerritoryType == 763 && EurekaPagos.deadFateDic[ushort.Parse(item.fateId)].Contains(":")) ||
                    (Plugin.clientState.TerritoryType == 795 && EurekaPyros.deadFateDic[ushort.Parse(item.fateId)].Contains(":")) ||
                    (Plugin.clientState.TerritoryType == 827 && EurekaHydatos.deadFateDic[ushort.Parse(item.fateId)].Contains(":"))) //cd还没转好的
                {
                    TimeSpan timeFromCanTriggered;
                    switch (Plugin.clientState.TerritoryType)
                    {
                        case 732:
                            timeFromCanTriggered = new TimeSpan(2, 0, 0) - (DateTime.Now - Convert.ToDateTime(EurekaAnemos.deadFateDic[ushort.Parse(item.fateId)]));
                            break;
                        case 763:
                            if ("1367".Equals(item.fateId) || "1368".Equals(item.fateId))
                            {
                                timeFromCanTriggered = new TimeSpan(0, 8, 0) - (DateTime.Now - Convert.ToDateTime(EurekaPagos.deadFateDic[ushort.Parse(item.fateId)]));
                            }
                            else { 
                                timeFromCanTriggered = new TimeSpan(2, 0, 0) - (DateTime.Now - Convert.ToDateTime(EurekaPagos.deadFateDic[ushort.Parse(item.fateId)]));
                            }
                            break;
                        case 795:
                            if ("1407".Equals(item.fateId) || "1408".Equals(item.fateId))
                            {
                                timeFromCanTriggered = new TimeSpan(0, 8, 0) - (DateTime.Now - Convert.ToDateTime(EurekaPyros.deadFateDic[ushort.Parse(item.fateId)]));
                            }
                            else
                            {
                                timeFromCanTriggered = new TimeSpan(2, 0, 0) - (DateTime.Now - Convert.ToDateTime(EurekaPyros.deadFateDic[ushort.Parse(item.fateId)]));
                            }
                            break;
                        case 827:
                            if ("1425".Equals(item.fateId))
                            {
                                timeFromCanTriggered = new TimeSpan(0, 10, 30) - (DateTime.Now - Convert.ToDateTime(EurekaHydatos.deadFateDic[ushort.Parse(item.fateId)]));
                            }
                            else if ("1422".Equals(item.fateId) || "1424".Equals(item.fateId)) //UFO 支援
                            {
                                timeFromCanTriggered = new TimeSpan(0, 20, 0) - (DateTime.Now - Convert.ToDateTime(EurekaHydatos.deadFateDic[ushort.Parse(item.fateId)]));
                            }
                            else
                            {
                                timeFromCanTriggered = new TimeSpan(2, 0, 0) - (DateTime.Now - Convert.ToDateTime(EurekaHydatos.deadFateDic[ushort.Parse(item.fateId)]));
                            }
                            break;
                        default:
                            Plugin.log.Error("not in Eureka.");
                            timeFromCanTriggered = new TimeSpan();
                            break;
                    }
                    if (item.SpawnRequiredWeather == EurekaWeather.None && !item.SpawnByRequiredNight) //无条件触发
                    {
                        BDL.DrawText(pos, item.name + "\n" + timeFromCanTriggered.ToString(@"hh\:mm\:ss"), Grey, true);
                    }
                    else
                    {
                        if (item.SpawnRequiredWeather == EurekaWeather.None) //无需天气，需要夜晚
                        {
                            int etime_hour = int.Parse(eorzeaTime.EorzeaDateTime.ToString("%H"));
                            if (etime_hour < 6 || etime_hour >= 18) //是晚上
                            {
                                BDL.DrawText(pos, item.name + "\n" + timeFromCanTriggered.ToString(@"hh\:mm\:ss"), Grey, true);
                            }
                            else
                            {
                                //离夜晚还有多久
                                TimeSpan timeFromNight = eorzeaTime.TimeUntilNight();
                                if (timeFromNight < timeFromCanTriggered)
                                {
                                    BDL.DrawText(pos, item.name + "\n" + timeFromCanTriggered.ToString(@"hh\:mm\:ss"), Grey, true);
                                }
                                else
                                {
                                    BDL.DrawText(pos, item.name + "\n" + timeFromNight.ToString(@"hh\:mm\:ss"), Grey, true);
                                }

                            }
                        }
                        else //无需夜晚，需要天气
                        {
                            if (!weatherDic.ContainsKey(item.SpawnRequiredWeather) || weatherNow.Weather == item.SpawnRequiredWeather) //天气过关
                            {
                                BDL.DrawText(pos, item.name + "\n" + timeFromCanTriggered.ToString(@"hh\:mm\:ss"), Grey, true);
                            }
                            else
                            {
                                //离目标天气还有多久
                                if (TimeSpan.Parse(weatherDic[item.SpawnRequiredWeather].Item1) < timeFromCanTriggered)
                                {
                                    BDL.DrawText(pos, item.name + "\n" + timeFromCanTriggered.ToString(@"hh\:mm\:ss"), Grey, true);
                                }
                                else
                                {
                                    BDL.DrawText(pos, item.name + "\n" + weatherDic[item.SpawnRequiredWeather].Item1, Grey, true);
                                }
                            }
                        }
                    }
                }
                else //未知或cd转好了的
                {
                    if (item.SpawnRequiredWeather == EurekaWeather.None && !item.SpawnByRequiredNight) //无条件触发
                    {
                        BDL.DrawText(pos, item.name, item.fgcolor, true);
                    }
                    else
                    {
                        if (item.SpawnRequiredWeather == EurekaWeather.None) //无需天气，需要夜晚
                        {
                            int etime_hour = int.Parse(eorzeaTime.EorzeaDateTime.ToString("%H"));
                            if (etime_hour < 6 || etime_hour >= 18) //是晚上
                            {
                                //离白天还有多久
                                TimeSpan timeFromDay = eorzeaTime.TimeUntilDay();
                                BDL.DrawText(pos, item.name + "\n" + timeFromDay.ToString(@"hh\:mm\:ss"), item.fgcolor, true);
                            }
                            else
                            {
                                //离夜晚还有多久
                                TimeSpan timeFromNight = eorzeaTime.TimeUntilNight();
                                BDL.DrawText(pos, item.name + "\n" + timeFromNight.ToString(@"hh\:mm\:ss"), Grey, true);
                            }
                        }
                        else //无需夜晚，需要天气
                        {
                            if (!weatherDic.ContainsKey(item.SpawnRequiredWeather) || weatherNow.Weather == item.SpawnRequiredWeather) //天气过关
                            {
                                //天气还剩多久
                                BDL.DrawText(pos, item.name + "\n" + weatherDic[item.SpawnRequiredWeather].Item2, item.fgcolor, true);
                            }
                            else
                            {
                                //离目标天气还有多久
                                BDL.DrawText(pos, item.name + "\n" + weatherDic[item.SpawnRequiredWeather].Item1, Grey, true);
                            }
                        }
                    }
                }
            }
        }
        //if (Plugin.Configuration.Overlay2D_ShowCenter)
        //{
        //BDL.DrawMapTextDot(valueOrDefault, "ME", 4294967040u, 4278190080u); //ME
        //}
        //if (Plugin.Configuration.Overlay2D_ShowAssist)
        //{
        //    BDL.AddCircle(valueOrDefault, WorldToMapScale * 25f, 4294967040u, 0, 1f); //自身加载范围
        //    BDL.AddCircle(valueOrDefault, WorldToMapScale * 125f, 4286611584u, 0, 1f); //服务器加载范围
        //    //BDL.AddLine(valueOrDefault, valueOrDefault - new Vector2(0f, WorldToMapScale * 25f).Rotate((float)Math.PI / 4f + Plugin.HRotation), 4294967040u, 1f);
        //    //BDL.AddLine(valueOrDefault, valueOrDefault - new Vector2(0f, WorldToMapScale * 25f).Rotate(-(float)Math.PI / 4f + Plugin.HRotation), 4294967040u, 1f);
        //}
        BDL.PopClipRect();
    }
    private unsafe void RefreshMapOrigin()
    {
        mapOrigin = null;
        if (!AreaMap.MapVisible)
        {
            return;
        }
        AtkUnitBase* areaMapAddon = AreaMap.AreaMapAddon;
        GlobalUIScale = (*areaMapAddon).Scale;
        if (((*areaMapAddon).UldManager).NodeListCount <= 4)
        {
            return;
        }
        AtkComponentNode* ptr = (AtkComponentNode*)((*areaMapAddon).UldManager).NodeList[3];
        AtkResNode atkResNode = (*ptr).AtkResNode;
        if ((*(&(*(*ptr).Component).UldManager)).NodeListCount < 233)
        {
            return;
        }
        for (int i = 6; i < (*(&(*(*ptr).Component).UldManager)).NodeListCount - 1; i++)
        {
            if (!(*(*(&(*(*ptr).Component).UldManager)).NodeList[i]).IsVisible)
            {
                continue;
            }
            AtkComponentNode* ptr2 = (AtkComponentNode*)(*(&(*(*ptr).Component).UldManager)).NodeList[i];
            AtkImageNode* ptr3 = (AtkImageNode*)(*(&(*(*ptr2).Component).UldManager)).NodeList[4];
            string text = null;
            if ((*ptr3).PartsList != null && (*ptr3).PartId <= (*(*ptr3).PartsList).PartCount)
            {
                AtkUldAsset* uldAsset = (*(AtkUldPart*)((byte*)(*(*ptr3).PartsList).Parts + ((*ptr3).PartId * (nint)Unsafe.SizeOf<AtkUldPart>()))).UldAsset;
                if ((int)(*(&(*uldAsset).AtkTexture)).TextureType == 1)
                {
                    StdString fileName = ((*(*((*uldAsset).AtkTexture).Resource).TexFileResourceHandle).ResourceHandle).FileName;
                    text = Path.GetFileName(fileName.ToString());
                }
            }
            if (text == "060443.tex" || text == "060443_hr1.tex")
            {
                AtkComponentNode* ptr4 = (AtkComponentNode*)(*(&(*(*ptr).Component).UldManager)).NodeList[i];
                Plugin.log.Verbose($"node found {i}", Array.Empty<object>());
                AtkResNode atkResNode2 = (*ptr4).AtkResNode;
                Vector2 vector = new Vector2((*areaMapAddon).X, (*areaMapAddon).Y);
                ImGuiViewportPtr mainViewport = ImGui.GetMainViewport();
                mapOrigin = mainViewport.Pos + vector + (new Vector2(atkResNode.X, atkResNode.Y) + new Vector2(atkResNode2.X, atkResNode2.Y) + new Vector2(atkResNode2.OriginX, atkResNode2.OriginY)) * GlobalUIScale;
                Vector2[] mapPosSize = MapPosSize;
                mainViewport = ImGui.GetMainViewport();
                mapPosSize[0] = mainViewport.Pos + vector + new Vector2(atkResNode.X, atkResNode.Y) * GlobalUIScale;
                Vector2[] mapPosSize2 = MapPosSize;
                mainViewport = ImGui.GetMainViewport();
                mapPosSize2[1] = mainViewport.Pos + vector + new Vector2(atkResNode.X, atkResNode.Y) + new Vector2((int)atkResNode.Width, (int)atkResNode.Height) * GlobalUIScale;
                break;
            }
        }
    }

    private Vector2 WorldToMap(Vector2 origin, Vector3 worldVector3)
    {
        Vector2 vector = (ToVector2(worldVector3) - ToVector2(Plugin.clientState.LocalPlayer.Position)) * WorldToMapScale;
        return origin + vector;
    }
    public static Vector2 ToVector2(Vector3 v)
    {
        return new Vector2(v.X, v.Z);
    }
    public static Vector3 ToVector3(Vector2 v)
    {
        return new Vector3(v.X, 0, v.Y);
    }
    public static float MapToWorld(float value, uint scale, int offset = 0)
    {
        return offset * (scale / 100.0f) + 50.0f * (value - 1) * (scale / 100.0f);
    }
    public static Vector2 MapToWorld(Vector2 coordinates, Map map)
    {
        var scalar = map.SizeFactor / 100.0f;

        var xWorldCoord = MapToWorld(coordinates.X, map.SizeFactor, map.OffsetX);
        var yWorldCoord = MapToWorld(coordinates.Y, map.SizeFactor, map.OffsetY);

        var objectPosition = new Vector2(xWorldCoord, yWorldCoord);
        var center = new Vector2(1024.0f, 1024.0f);

        return objectPosition / scalar - center / scalar;
    }

    public void NMFound()
    {
        player1.Stop();
        Plugin.log.Error("nmFound");
        player1.SoundLocation = Path.Combine(pi.AssemblyLocation.Directory?.FullName!, "nm.wav");
        player1.Load();
        player1.Play();
    }

    public void TZFound()
    {
        player2.Stop();
        Plugin.log.Error("tzFound");
        player2.SoundLocation = Path.Combine(pi.AssemblyLocation.Directory?.FullName!, "tz.wav");
        player2.Load();
        player2.Play();
    }
}

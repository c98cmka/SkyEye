using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.Objects.Types;
using FFXIVClientStructs.Interop;
using FFXIVClientStructs.FFXIV.Client.Game.Fate;
using Dalamud.Game.ClientState.Objects.SubKinds;
using System;
using System.Linq;
using System.Numerics;
using Dalamud.Game;
using Dalamud;
using System.Runtime.InteropServices;
using static FFXIVClientStructs.FFXIV.Client.UI.Agent.AgentMJIFarmManagement;
using System.Collections.Generic;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using System.Text.RegularExpressions;
using static Lumina.Data.Parsing.Layer.LayerCommon;
using SkyEye.EurekaHelper.Positions;

namespace SkyEye;

public sealed class Plugin : IDalamudPlugin
{
    private const string CommandName = "/skyeye";

    internal UIBuilder Ui;
    public DalamudPluginInterface PluginInterface { get; init; }
    private ICommandManager CommandManager { get; init; }
    public static Configuration Configuration { get; private set; }
    [PluginService] internal static IClientState clientState { get; private set; }
    [PluginService] internal static IDataManager dataManager { get; private set; }
    [PluginService] internal static IPluginLog log { get; private set; }
    [PluginService] internal static ICondition condition { get; private set; }
    [PluginService] internal static IGameGui gui { get; private set; }
    [PluginService] internal static IObjectTable objects { get; private set; }
    [PluginService] internal static IFateTable fates { get; private set; }
    [PluginService] internal static IFramework Framework { get; private set; } = null!;
    [PluginService] public static ISigScanner SigScanner { get; private set; } = null!;
    [PluginService] public static IChatGui chatGui { get; private set; } = null!;

    public double CamAngleX;
    public double CamAngleY;

    public List<PlayerCharacter> OtherPlayer = new List<PlayerCharacter>();

    public float Dspeed = 1.0f;
    internal static float speedOffset = 6f;

    public List<Vector3> _detectedTreasurePositions = [];

    //public unsafe static GameObject** GameObjectList;

    internal static AddressResolver address;
    internal unsafe static ref float HRotation => ref *(float*)((nint)address.CamPtr + 304);

    public readonly WindowSystem WindowSystem = new("SkyEye");
    private ConfigWindow ConfigWindow { get; init; }
    //private MainWindow MainWindow { get; init; }

    public unsafe Plugin(
        [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
        [RequiredVersion("1.0")] ICommandManager commandManager,
        [RequiredVersion("1.0")] ITextureProvider textureProvider)
    {
        PluginInterface = pluginInterface;
        CommandManager = commandManager;

        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize(PluginInterface);

        Ui = new UIBuilder(this, pluginInterface);
        ConfigWindow = new ConfigWindow(this);
        //MainWindow = new MainWindow(this, goatImage);

        WindowSystem.AddWindow(ConfigWindow);
        //WindowSystem.AddWindow(MainWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "A useful message to display in /xlhelp"
        });

        PluginInterface.UiBuilder.Draw += DrawUI;

        Framework.Update += UpdateRoundPlayers;

        // This adds a button to the plugin installer entry of this plugin which allows
        // to toggle the display status of the configuration ui
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

        chatGui.ChatMessageUnhandled += OnChatMessage;

        // Adds another button that is doing the same but for the main ui of the plugin
        //PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();
        chatGui.ChatMessageUnhandled -= OnChatMessage;
        ConfigWindow.Dispose();
        //MainWindow.Dispose();
        Framework.Update -= UpdateRoundPlayers;
        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        // in response to the slash command, just toggle the display status of our main ui
        ToggleConfigUI();
    }

    private void OnChatMessage(XivChatType type, uint senderId, SeString sender, SeString message)
    {
        if (clientState.LocalPlayer == null || message == null || (clientState.TerritoryType != 732 && clientState.TerritoryType != 763 && clientState.TerritoryType != 795 && clientState.TerritoryType != 827))
        {
            return;
        }
        string msg = message.TextValue.Trim();
        if (msg.StartsWith("找到了财宝"))
        {
            _detectedTreasurePositions = new List<Vector3>();
        }
        if (!msg.StartsWith("财宝好像是在"))
        {
            return;
        }
        var result = Regex.Match(msg, "财宝好像是在(?<direction>正北|东北|正东|东南|正南|西南|正西|西北)方向(?<distance>(很远|稍远|不远|很近))的地方！");

        if (!result.Success)
        {
            return;
        }

        string direction = result.Groups["direction"].Value;
        string distanceT = result.Groups["distance"].Value;

        int minDistance;
        int maxDistance;

        if (distanceT == "很远")
        {
            minDistance = 200;
            maxDistance = int.MaxValue;
        }
        else if (distanceT == "稍远")
        {
            minDistance = 100;
            maxDistance = 200;
        }
        else if (distanceT == "不远")
        {
            minDistance = 25;
            maxDistance = 100;
        }
        else
        {
            minDistance = 0;
            maxDistance = 25;
        }

        var playerPos = clientState.LocalPlayer.Position;
        var TreasurePositions = RabbitTreasurePosition.RabbitTreasurePositions[clientState.TerritoryType];

        var Treasures = TreasurePositions
            .Where(
                c => {
                    float distance = Vector3.Distance(playerPos, c);
                    return distance >= minDistance && distance <= maxDistance;
                }
            )
            .OrderBy(c => Vector3.Distance(playerPos, c));

        if (direction.Equals("正南", StringComparison.OrdinalIgnoreCase))
        {
            _detectedTreasurePositions = Treasures
                .Where(c => c.Z > playerPos.Z && Math.Abs(c.X - playerPos.X) <= Math.Abs(c.Z - playerPos.Z))
                .ToList();
        }
        else if (direction.Equals("正北", StringComparison.OrdinalIgnoreCase))
        {
            _detectedTreasurePositions = Treasures
                .Where(c => c.Z < playerPos.Z && Math.Abs(c.X - playerPos.X) <= Math.Abs(c.Z - playerPos.Z))
                .ToList();
        }
        else if (direction.Equals("正东", StringComparison.OrdinalIgnoreCase))
        {
            _detectedTreasurePositions = Treasures
                .Where(c => c.X > playerPos.X && Math.Abs(c.X - playerPos.X) >= Math.Abs(c.Z - playerPos.Z))
                .ToList();
        }
        else if (direction.Equals("正西", StringComparison.OrdinalIgnoreCase))
        {
            _detectedTreasurePositions = Treasures
                .Where(c => c.X < playerPos.X && Math.Abs(c.X - playerPos.X) >= Math.Abs(c.Z - playerPos.Z))
                .ToList();
        }
        else if (direction.Equals("东南", StringComparison.OrdinalIgnoreCase))
        {
            _detectedTreasurePositions = Treasures.Where(c => c.Z >= playerPos.Z && c.X >= playerPos.X).ToList();
        }
        else if (direction.Equals("西南", StringComparison.OrdinalIgnoreCase))
        {
            _detectedTreasurePositions = Treasures.Where(c => c.Z >= playerPos.Z && c.X <= playerPos.X).ToList();
        }
        else if (direction.Equals("东北", StringComparison.OrdinalIgnoreCase))
        {
            _detectedTreasurePositions = Treasures.Where(c => c.Z <= playerPos.Z && c.X >= playerPos.X).ToList();
        }
        else if (direction.Equals("西北", StringComparison.OrdinalIgnoreCase))
        {
            _detectedTreasurePositions = Treasures.Where(c => c.Z <= playerPos.Z && c.X <= playerPos.X).ToList();
        }
    }

    private void UpdateRoundPlayers(IFramework framework)
    {
        if (clientState.LocalPlayer == null || (clientState.TerritoryType != 732 && clientState.TerritoryType != 763 && clientState.TerritoryType != 795 && clientState.TerritoryType != 827))
        {
            if (Dspeed != 1.0f)
            {
                Dspeed = 1.0f;
                SetSpeed(Dspeed * speedOffset);
            }
            return;
        }
        lock (OtherPlayer)
        {
            OtherPlayer.Clear();
            if (objects == null)
            {
                return;
            }
            foreach (var obj in objects)
            {
                try
                {
                    if (obj != null && (obj.ObjectId != clientState.LocalPlayer.ObjectId) & obj.Address.ToInt64() != 0)
                    {
                        PlayerCharacter rcTemp = obj as PlayerCharacter;
                        if (rcTemp != null)
                        {
                            var targetDistance = Vector3.Distance(clientState.LocalPlayer?.Position ?? Vector3.Zero, rcTemp.Position);
                            if (targetDistance <= 150)
                            {
                                OtherPlayer.Add(rcTemp);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    log.Error("error");
                    continue;
                }
            }
        }
        if (Configuration.Overlay2D_SpeedUp_Enabled)
        {
            if (OtherPlayer.Count() > 0)
            {
                //reset
                if (condition[ConditionFlag.Mounted])
                {
                    //log.Error("reset if");
                    if (Dspeed != 1.5f)
                    {
                        Dspeed = 1.5f;
                        SetSpeed(Dspeed * speedOffset);
                    }
                }
                else
                {
                    //log.Error("reset else");
                    if (Dspeed != 1.0f)
                    {
                        Dspeed = 1.0f;
                        SetSpeed(Dspeed * speedOffset);
                    }
                }
            }
            else
            {
                //没人了开冲
                if (condition[ConditionFlag.Mounted])
                {
                    //log.Error("开冲if");
                    if (Dspeed != 2.5f)
                    {
                        Dspeed = 2.5f;
                        SetSpeed(Dspeed * speedOffset);
                    }
                }
                else
                {
                    //log.Error("开冲else");
                    if (Dspeed != 1.0f)
                    {
                        Dspeed = 1.0f;
                        SetSpeed(Dspeed * speedOffset);
                    }

                }
            }
        }
        else
        {
            //reset
            if (condition[ConditionFlag.Mounted])
            {
                if (Dspeed != 1.5f)
                {
                    Dspeed = 1.5f;
                    SetSpeed(Dspeed * speedOffset);
                }
            }
            else
            {
                //log.Error(Dspeed + "");
                if (Dspeed != 1.0f)
                {
                    Dspeed = 1.0f;
                    SetSpeed(Dspeed * speedOffset);
                }

            }
        }
    }

    public static void SetSpeed(float speedBase)
    {
        SigScanner.TryScanText("f3 ?? ?? ?? ?? ?? ?? ?? e8 ?? ?? ?? ?? 48 ?? ?? ?? ?? ?? ?? 0f ?? ?? e8 ?? ?? ?? ?? f3 ?? ?? ?? ?? ?? ?? ?? f3 ?? ?? ?? ?? ?? ?? ?? f3 ?? ?? ?? f3", out var address);
        address = address + 4 + Marshal.ReadInt32(address + 4) + 4;
        SafeMemory.Write(address + 20, speedBase);
        SetMoveControlData(speedBase);
    }
    private unsafe static void SetMoveControlData(float speed)
    {
        SafeMemory.Write(((delegate* unmanaged[Stdcall]<byte, nint>)SigScanner.ScanText("E8 ?? ?? ?? ?? 48 ?? ?? 74 ?? 83 ?? ?? 75 ?? 0F ?? ?? ?? 66"))(1) + 8, speed);
    }

    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle();
    //public void ToggleMainUI() => MainWindow.Toggle();
}

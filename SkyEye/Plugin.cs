using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.Objects.Types;
using FFXIVClientStructs.Interop;
using FFXIVClientStructs.FFXIV.Client.Game.Fate;

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

        // This adds a button to the plugin installer entry of this plugin which allows
        // to toggle the display status of the configuration ui
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

        // Adds another button that is doing the same but for the main ui of the plugin
        //PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        //MainWindow.Dispose();

        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        // in response to the slash command, just toggle the display status of our main ui
        ToggleConfigUI();
    }

    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle();
    //public void ToggleMainUI() => MainWindow.Toggle();
}

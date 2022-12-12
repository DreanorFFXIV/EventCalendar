using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;

// ReSharper disable PossibleNullReferenceException

namespace EventCalendar
{
    public class Plugin : IDalamudPlugin
    {
        public string Name => "EventCalendar";
        private const string CommandName = "/events";
        private PluginUI PluginUi { get; init; }

        [PluginService] private DalamudPluginInterface PluginInterface { get; set; }

        [PluginService] private CommandManager CommandManager { get; set; }

        public Plugin()
        {
            PluginUi = new PluginUI();
            PluginInterface.UiBuilder.Draw += DrawUI;

            CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Displays all current and upcoming events."
            });
        }

        public void Dispose()
        {
            PluginInterface.UiBuilder.Draw -= DrawUI;
            PluginUi.Dispose();
            CommandManager.RemoveHandler(CommandName);
        }

        private void DrawUI()
        {
            PluginUi.Draw();
        }

        private void OnCommand(string command, string args)
        {
            PluginUi.Visible = true;
        }
    }
}
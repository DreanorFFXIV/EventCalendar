using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;

// ReSharper disable PossibleNullReferenceException

namespace EventCalendar
{
    public class Plugin : IDalamudPlugin
    {
        public string Name => "EventCalendar";
        private const string CommandName = "/evt";
        private PluginUI PluginUI { get; init; }

        [PluginService]
        private DalamudPluginInterface PluginInterface { get; set; }

        [PluginService] 
        private CommandManager CommandManager { get; set; }

        public Plugin()
        {
            PluginUI = new PluginUI();
            PluginInterface.UiBuilder.Draw += DrawUI;

            CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Displays all current and upcoming events."
            });
        }

        public void Dispose()
        {
            PluginInterface.UiBuilder.Draw -= DrawUI;
            PluginUI.Dispose();
            CommandManager.RemoveHandler(CommandName);
        }

        private void DrawUI()
        {
            PluginUI.Draw();
        }

        private void OnCommand(string command, string args)
        {
            PluginUI.Visible = true;
        }
    }
}
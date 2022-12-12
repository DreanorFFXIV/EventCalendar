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
        private PluginUI _pluginUi { get; init; }
        
        [PluginService] 
        private DalamudPluginInterface _pluginInterface { get; set; }

        [PluginService]
        private CommandManager _commandManager { get; set; }

        public Plugin()
        {
            _pluginUi = new PluginUI(this);
            _pluginInterface.UiBuilder.Draw += DrawUI;

            _commandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });
        }

        public void Dispose()
        {
            _pluginInterface.UiBuilder.Draw -= DrawUI;
            _pluginUi.Dispose();
            _commandManager.RemoveHandler(CommandName);
        }

        private void DrawUI()
        {
            _pluginUi.Draw();
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            _pluginUi.Visible = true;
        }
    }
}
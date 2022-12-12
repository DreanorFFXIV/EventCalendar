using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using ImGuiNET;
using ImGuiScene;

namespace EventCalendar
{
	public class PluginUI : IDisposable
	{
		private Plugin Plugin;
		
		// this extra bool exists for ImGui, since you can't ref a property
		private bool visible = false;
		public bool Visible
		{
			get { return this.visible; }
			set { this.visible = value; }
		}
		
		public PluginUI(Plugin plugin)
		{

			Plugin = plugin;
		}

		public void Draw()
		{
			string publicId = "5de3023ee82a703ccd7c4b71f0e34418967b28f0001cdb722cb261e11d51dc8d@group.calendar.google.com";
			DrawCalendarEntry("title", "desc", "url");
		}

		public void DrawCalendarEntry(string title, string description, string imgUrl)
		{
			ImGui.TableNextColumn();

			if (description.Length > 200)
			{
				description = description[..200] + "...";
			}

			ImGui.TextWrapped(description);
    
            ImGui.TableNextRow();
        }
		
		public void Dispose()
		{
			Plugin?.Dispose();
		}
	}
}
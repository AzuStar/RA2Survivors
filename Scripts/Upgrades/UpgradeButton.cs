using Godot;

namespace RA2Survivors
{
	public partial class UpgradeButton : Button
	{
		[Export]
		public RichTextLabel title;

		[Export]
		public RichTextLabel description;

		public void SetText(string title, string description)
		{
			this.title.Text = title;
			this.description.Text = description;
		}
	}
}

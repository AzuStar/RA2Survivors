using System;
using System.Collections.Generic;
using Godot;

namespace RA2Survivors
{
    public partial class UpgradeSelector : Node
    {
        public const string RESOURCE_PATH = "Upgrades/StandardUpgradeChoice.tscn";
        public static UpgradeSelector instance { get; private set; }

        private List<UpgradeButton> buttons = new List<UpgradeButton>();

        public UpgradeSelector()
        {
            instance = this;
        }

        public static void CreateSelection(
            UpgradeButtonSettings[] createdButtons,
            Action<UpgradeButtonSettings> callbackHandler = null
        )
        {
            PauseService.PauseGame();
            foreach (var button in createdButtons)
            {
                UpgradeButton uButton = ResourceProvider.CreateResource<UpgradeButton>(
                    RESOURCE_PATH
                );
                uButton.SetText(button.title, button.description);
                uButton.Pressed += _CallBackLogic + button.callback;
                if (callbackHandler != null)
                {
                    uButton.Pressed += () => callbackHandler(button);
                }
                instance.buttons.Add(uButton);
                instance.AddChild(uButton);
            }
        }

        private static void _CallBackLogic()
        {
            PauseService.UnpauseGame();
            foreach (var button in instance.buttons)
            {
                button.QueueFree();
            }
            instance.buttons.Clear();
        }
    }
}

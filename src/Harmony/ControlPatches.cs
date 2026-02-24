using HarmonyLib;
using UnityEngine;

namespace HildirOverhaul
{
    public static class TraderPatches
    {
        private static TraderUI _traderUI;
        private static BankUI _bankUI;

        internal static void SetTraderUI(TraderUI ui) => _traderUI = ui;
        internal static void SetBankUI(BankUI ui) => _bankUI = ui;
        internal static TraderUI GetTraderUI() => _traderUI;

        /// <summary>
        /// Returns true only when the trader being interacted with is specifically Hildir.
        /// Checked by prefab name so any other trader mod's NPCs fall through to vanilla UI.
        /// </summary>
        private static bool IsHildir(Trader trader)
        {
            if (trader == null) return false;
            string prefab = Utils.GetPrefabName(trader.gameObject);
            return string.Equals(prefab, "Hildir", System.StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Intercept StoreGui.Show — prevent vanilla UI, show our custom UI instead.
        /// Only fires for Hildir; all other traders use vanilla StoreGui.
        /// </summary>
        [HarmonyPatch(typeof(StoreGui), "Show")]
        [HarmonyPrefix]
        private static bool StoreGui_Show_Prefix(StoreGui __instance, Trader trader)
        {
            if (_traderUI == null) return true;
            if (!IsHildir(trader)) return true; // let vanilla handle every non-Hildir trader
            _traderUI.Show(trader, __instance);
            return false; // skip vanilla StoreGui for Hildir
        }

        /// <summary>
        /// When StoreGui.Hide is called, also close our UI.
        /// </summary>
        [HarmonyPatch(typeof(StoreGui), "Hide")]
        [HarmonyPostfix]
        private static void StoreGui_Hide_Postfix()
        {
            if (_traderUI != null && _traderUI.IsVisible)
                _traderUI.Hide();
        }

        /// <summary>
        /// Report visible when our UI is open so the game knows a store UI is active.
        /// </summary>
        [HarmonyPatch(typeof(StoreGui), "IsVisible")]
        [HarmonyPostfix]
        private static void StoreGui_IsVisible_Postfix(ref bool __result)
        {
            if (_traderUI != null && _traderUI.IsVisible)
                __result = true;
            if (_bankUI != null && _bankUI.IsVisible)
                __result = true;
        }

        /// <summary>
        /// Suppress Hildir's random talk bubbles while our UI is open.
        /// </summary>
        [HarmonyPatch(typeof(Chat), "SetNpcText")]
        [HarmonyPrefix]
        private static bool Chat_SetNpcText_Prefix()
        {
            if (_traderUI != null && _traderUI.IsVisible)
                return false;
            return true;
        }

        /// <summary>
        /// Block keyboard input when the search box is focused.
        /// </summary>
        [HarmonyPatch(typeof(Chat), "HasFocus")]
        [HarmonyPostfix]
        private static void Chat_HasFocus_Postfix(ref bool __result)
        {
            if (!__result && _traderUI != null && _traderUI.IsSearchFocused)
                __result = true;
        }

        /// <summary>
        /// Block player movement/input when our UI is open (trader or bank).
        /// </summary>
        [HarmonyPatch(typeof(Player), "TakeInput")]
        [HarmonyPrefix]
        private static bool Player_TakeInput_Prefix(ref bool __result)
        {
            if ((_traderUI != null && _traderUI.IsVisible) ||
                (_bankUI != null && _bankUI.IsVisible))
            {
                __result = false;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Register console commands after Terminal.InitTerminal so the command
        /// dictionary isn't wiped out after registration.
        /// </summary>
        [HarmonyPatch(typeof(Terminal), "InitTerminal")]
        [HarmonyPostfix]
        private static void Terminal_InitTerminal_Postfix()
        {
            new Terminal.ConsoleCommand("setbankbalance",
                "Set Hildir's bank balance. Usage: setbankbalance <amount>  or  setbankbalance = <amount>",
                (Terminal.ConsoleEventArgs args) =>
                {
                    string raw = null;
                    if (args.Length >= 3 && args[1] == "=") raw = args[2];
                    else if (args.Length >= 2 && args[1] != "=") raw = args[1];

                    if (!int.TryParse(raw, out int amount) || amount < 0)
                    {
                        args.Context.AddString("Usage: setbankbalance <amount>");
                        return;
                    }

                    var player = Player.m_localPlayer;
                    if (player == null) { args.Context.AddString("No player found."); return; }

                    int previous = BankBalanceStore.Read(player);
                    BankBalanceStore.Write(player, amount);
                    GetTraderUI()?.ReloadBankBalance();

                    ((Character)player).Message(MessageHud.MessageType.Center, $"Bank balance set to {amount:N0}");
                    args.Context.AddString($"Bank balance set to {amount:N0} (was {previous:N0})");
                });
        }

    }
}

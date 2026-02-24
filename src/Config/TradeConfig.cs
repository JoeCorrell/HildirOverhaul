using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using Newtonsoft.Json;

namespace HildirOverhaul
{
    public static class ConfigLoader
    {
        private static string SellFile => Path.Combine(Paths.ConfigPath, "HildirOverhaul.hildir.sell.json");

        public static List<TradeEntry> SellEntries { get; private set; } = new List<TradeEntry>();

        public static void Initialize()
        {
            EnsureConfigFilesExist();
            LoadSellConfig();
            ValidateAndLogStats();
        }

        private static void ValidateAndLogStats()
        {
            int sellValid = 0, sellInvalid = 0;

            foreach (var entry in SellEntries.ToList())
            {
                if (!ValidateEntry(entry, "SELL"))
                {
                    SellEntries.Remove(entry);
                    sellInvalid++;
                }
                else
                {
                    sellValid++;
                }
            }

            if (sellInvalid > 0)
            {
                HildirOverhaul.Log.LogWarning($"[ConfigLoader] Removed {sellInvalid} invalid SELL entries.");
            }

            HildirOverhaul.Log.LogInfo($"{"[ConfigLoader]"} Config validated: {sellValid} SELL entries.");
        }

        private static bool ValidateEntry(TradeEntry entry, string type)
        {
            if (entry == null)
            {
                HildirOverhaul.Log.LogWarning($"{"[ConfigLoader]"} {type} entry is null, skipping.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(entry.prefab))
            {
                HildirOverhaul.Log.LogWarning($"{"[ConfigLoader]"} {type} entry has empty prefab name, skipping.");
                return false;
            }

            if (entry.price < 0)
            {
                HildirOverhaul.Log.LogWarning($"{"[ConfigLoader]"} {type} entry '{entry.prefab}' has negative price ({entry.price}), setting to 0.");
                entry.price = 0;
            }

            if (entry.stack <= 0)
            {
                HildirOverhaul.Log.LogWarning($"{"[ConfigLoader]"} {type} entry '{entry.prefab}' has invalid stack ({entry.stack}), setting to 1.");
                entry.stack = 1;
            }

            return true;
        }

        private static void EnsureConfigFilesExist()
        {
            if (!File.Exists(SellFile))
            {
                File.WriteAllText(SellFile, DefaultSellJson());
                HildirOverhaul.Log.LogInfo("[ConfigLoader] Created default SELL config.");
            }
        }

        private static void LoadSellConfig()
        {
            try
            {
                SellEntries = JsonConvert.DeserializeObject<List<TradeEntry>>(File.ReadAllText(SellFile)) ?? new List<TradeEntry>();
                HildirOverhaul.Log.LogInfo($"[ConfigLoader] Loaded {SellEntries.Count} SELL entries.");
            }
            catch (Exception ex)
            {
                HildirOverhaul.Log.LogError($"[ConfigLoader] Error loading SELL config: {ex}");
                SellEntries = new List<TradeEntry>();
            }
        }

        private static string DefaultSellJson() => JsonConvert.SerializeObject(new List<TradeEntry>
        {
            new TradeEntry() { prefab = "Wood", stack = 1, price = 1 }
        }, Formatting.Indented);
    }

    public class TradeEntry
    {
        [JsonProperty("item_prefab")]
        public string prefab = "";

        [JsonProperty("item_quantity")]
        public int stack = 1;

        [JsonProperty("item_price")]
        public int price = 1;

        [JsonProperty("must_defeated_boss")]
        public string requiredGlobalKey = "";
    }
}

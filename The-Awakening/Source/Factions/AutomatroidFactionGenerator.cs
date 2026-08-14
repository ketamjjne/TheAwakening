using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace TheAwakening
{
    [StaticConstructorOnStartup]
    public static class AutomatroidFactionGenerator
    {
        static AutomatroidFactionGenerator()
        {
            var harmony = new Harmony("ketamjne.theawakening");

            harmony.Patch(
                AccessTools.Method(typeof(Game), "InitNewGame"),
                postfix: new HarmonyMethod(
                    typeof(AutomatroidFactionGenerator),
                    nameof(InitNewGamePostfix)
                )
            );
        }

        private static void InitNewGamePostfix()
        {
            EnsureAutomatroidFactionExists();
        }

        private static void EnsureAutomatroidFactionExists()
        {
            if (Current.Game == null)
            {
                Log.Error("[The Awakening] Current.Game is null.");
                return;
            }

            if (Current.Game.World == null)
            {
                Log.Error("[The Awakening] Current.Game.World is null.");
                return;
            }

            FactionDef def = DefDatabase<FactionDef>.GetNamedSilentFail(
                "DMS_Automatroid_Hostile"
            );

            if (def == null)
            {
                Log.Error(
                    "[The Awakening] DMS_Automatroid_Hostile FactionDef not found."
                );
                return;
            }

            FactionManager factionManager =
                Current.Game.World.factionManager;

            if (factionManager == null)
            {
                Log.Error("[The Awakening] FactionManager is null.");
                return;
            }

            if (factionManager.AllFactions.Any(f => f.def == def))
            {
                Log.Message(
                    "[The Awakening] Automatroid faction already exists."
                );
                return;
            }

            Faction faction = new Faction
            {
                def = def,
                loadID = Find.UniqueIDsManager.GetNextFactionID()
            };

            factionManager.Add(faction);

            Log.Message(
                "[The Awakening] Created hidden Automatroid faction: "
                + faction.Name
            );
        }
    }
}
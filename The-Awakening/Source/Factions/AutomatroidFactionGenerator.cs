using System.Linq;
using RimWorld;
using Verse;

namespace TheAwakening
{
    [StaticConstructorOnStartup]
    public static class AutomatroidFactionGenerator
    {
        static AutomatroidFactionGenerator()
        {
            LongEventHandler.ExecuteWhenFinished(EnsureAutomatroidFactionExists);
        }

        private static void EnsureAutomatroidFactionExists()
        {
            if (Current.Game == null)
                return;

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

            FactionManager factionManager = Current.Game.World.factionManager;

            if (factionManager == null)
                return;

            if (factionManager.AllFactions.Any(f => f.def == def))
                return;

            Faction faction = FactionGenerator.NewGeneratedFaction(
                new FactionGeneratorParms
                {
                    factionDef = def
                }
            );

            factionManager.Add(faction);

            Log.Message(
                "[The Awakening] Created hidden Automatroid faction: "
                + faction.Name
            );
        }
    }
}
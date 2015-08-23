using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using Verse.AI;
using RimWorld;

namespace ContainersForStuff
{
    public class Container : Building_Storage
    {
        public List<Thing> storage = new List<Thing>();
        public ThingFilter allowances = new ThingFilter();
        public string label = "";
        public int itemsCap = 10;

        public List<Thing> itemsList { get { return Find.ThingGrid.ThingsListAtFast(this.Position).Where(thing => thing.def.thingClass.Name != "Container").ToList(); } }
        // doesn't count container as thing
        public int itemsCount { get { return Find.ThingGrid.ThingsListAt(this.Position).Count - 1; } }


        public override void TickRare()
        {
            foreach (Thing thing in itemsList)
            {
                if ( thing.TryGetComp<CompRottable>().GetType() == typeof(CompRottable))
                {
                    thing.TryGetComp<CompRottable>().rotProgress = 0f;
                }
            }
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            ScatterItemsAround();
            base.Destroy(mode);
        }

        public void ScatterItemsAround()
        {
            foreach (Thing thing in itemsList)
            {
                IntVec3 bestSpot = IntVec3.Invalid;
                if (JobDriver_HaulToCell.TryFindPlaceSpotNear(this.Position, thing, out bestSpot))
                {
                    thing.Position = bestSpot;
                }
                else
                {
                    Log.Error("No free spot for " + thing);
                }
            }
        }
    }
}
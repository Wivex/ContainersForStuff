﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RimWorld;
using Verse;
using Verse.AI;

namespace ContainersForStuff
{
    public class CompContainer : ThingComp
    {
        public List<Thing> itemsList { get { return Find.ThingGrid.ThingsListAtFast(parent.Position).Where(thing => thing.def.thingClass != typeof(Building_Storage)).ToList(); } }

        // doesn't count container as thing
        public int itemsCount { get { return Find.ThingGrid.ThingsListAt(parent.Position).Count - 1; } }
        
        public CompProperties_Container compProps
        {
            get
            {
                return (CompProperties_Container)props;
            }
        }

        // Executed when building is spawned on map (after loading too)
        public override void PostSpawnSetup()
        {
            base.PostSpawnSetup();

            // Restrict itemsCap to prevent unsafe behaviour
            if (compProps.itemsCap < 1 || compProps.itemsCap > 15)
            {
                compProps.itemsCap = 10;
                Log.Error("CompContainer's itemsCap value should be between 1 and 15");
            }
        }

        public override void PostDestroy(DestroyMode mode = DestroyMode.Vanish)
        {
            // Scatter items to provide easy access
            ScatterItemsAround();
            base.PostDestroy(mode);
        }

        public void ScatterItemsAround()
        {
            foreach (Thing thing in itemsList)
            {
                IntVec3 bestSpot = IntVec3.Invalid;
                if (JobDriver_HaulToCell.TryFindPlaceSpotNear(parent.Position, thing, out bestSpot))
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

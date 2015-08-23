using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RimWorld;
using Verse;
using Verse.AI;

namespace ContainersForStuff
{
    public class CompProperties_Container : CompProperties
    {
        // Default value
        public int itemsCap = 10;

        // Default requirement
        public CompProperties_Container()
        {
            this.compClass = typeof(CompProperties_Container);
        }
    }
}

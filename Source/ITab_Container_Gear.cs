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
    public class ITab_Container_Gear : ITab_Pawn_Gear
    {
        public string labelTitle;
        public Container container;

        public ITab_Container_Gear()
        {
            this.size = new Vector2(300f, 355f);
            this.labelKey = "Items";
        }
        public override bool IsVisible
        {
            get { return true; }
        }
        public string GetTitle()
        {
            labelTitle = "Stored items count: " + container.itemsCount + "/" + container.itemsCap;
            return labelTitle;
        }

        protected override void FillTab()
        {
            float fieldHeight = 30.0f;

            this.container = this.SelThing as Container;

            List<Thing> list = container.itemsList;

            ConceptDatabase.KnowledgeDemonstrated(ConceptDefOf.PrisonerTab, KnowledgeAmount.GuiFrame);
            Text.Font = GameFont.Small;

            Rect innerRect = GenUI.ContractedBy(new Rect(0.0f, 0.0f, this.size.x, this.size.y), 10f);
            float innerRectX = innerRect.x;
            GUI.BeginGroup(innerRect);
            {
                Widgets.TextField(new Rect(0.0f, 0.0f, this.size.x - 40f, fieldHeight), GetTitle());
                
                Rect thingIconRect = new Rect(10f, fieldHeight + 5f, 30f, fieldHeight);
                Rect thingLabelRect = new Rect(thingIconRect.x + 35f, thingIconRect.y + 5.0f, innerRect.width - 35f, fieldHeight);
                Rect thingButtonRect = new Rect(thingIconRect.x, thingIconRect.y, innerRect.width, fieldHeight);

                //float startY = 0.0f;
                //Widgets.ListSeparator(ref startY, innerRect.width, GetTitle());

                foreach (var thing in list)
                {
                    Widgets.ThingIcon(thingIconRect, thing);
                    Widgets.Label(thingLabelRect, thing.Label);

                    if (Widgets.InvisibleButton(thingButtonRect))
                    {
                        List<FloatMenuOption> options = new List<FloatMenuOption>();
                        options.Add(new FloatMenuOption("Info", () =>
                        {
                            Find.LayerStack.Add((Layer)new Dialog_InfoCard(thing));
                        }));
                        options.Add(new FloatMenuOption("Drop", () =>
                        {
                            IntVec3 bestSpot = IntVec3.Invalid;
                            if (JobDriver_HaulToCell.TryFindPlaceSpotNear(container.Position, thing, out bestSpot))
                            {
                                thing.Position = bestSpot;
                            }
                            else
                            {
                                Log.Error("No free spot for " + thing);
                            }
                        }));

                        Find.LayerStack.Add((Layer)new Layer_FloatMenu(options, "", false, false));
                    }

                    thingIconRect.y += fieldHeight;
                    thingLabelRect.y += fieldHeight;
                    thingButtonRect.y += fieldHeight;
                }
            }
            GUI.EndGroup();
        }
    }
}

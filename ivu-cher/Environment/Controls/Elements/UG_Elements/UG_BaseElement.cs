using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AvengersUtd.Explore.Environment.Controls.Elements.UG_Elements
{
    public class UG_BaseElement : Element
    {
        public AvengersUtd.Explore.Data.Elements.ElementType ElementType { get; set; }

        public UG_BaseElement()
        {

            BuildingBlockLabel = "UG";
            ElementHeight = 300;

        }


        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            base.ElementContentLoaded(sender, e);
            CircleAdorner.SetVisibility(false);
            CircleAdorner.SetVisibility(ThumbPosition.Top, true);
            //CircleAdorner.SetVisibility(ThumbPosition.Right, true);
            WatermarkLabel.Content = "Maximize to edit options...";

        }

        protected override void OnGotFocus(System.Windows.RoutedEventArgs e)
        {

            base.OnGotFocus(e);

            try
            {
                switch (Global.MainWindow.ActivePreview)
                {
                    case "And":
                        AndPreview();
                        break;
                    default:
                        break;

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            

        }

        private void AndPreview()
        {
            System.Windows.UIElement template=null;

            switch (this.ElementType)
            {
                case Data.Elements.ElementType.UG_ContentItem:
                    template = new Templates.UGTemplateContenuti();
                   ((Templates.UGTemplateContenuti)template).Caption = ((UG_ContentItem)this).Nome;
                   ((Templates.UGTemplateContenuti)template).Description= ((UG_ContentItem)this).Descrizione;
                    break;
                case Data.Elements.ElementType.UG_GoalElement:
                    break;
                case Data.Elements.ElementType.UG_CityElement:
                    template = new Templates.UGTemplateWelcomeScreen(((UG_CityElement)this).Citta, ((UG_CityElement)this).Descrizione, null);
                    break;
                case Data.Elements.ElementType.UG_ResourceElement:
                    break;
                default:
                    break;
            }


            CircleAdorner.SetVisibility(ThumbPosition.Top, false);
            Global.MainWindow.ANDPreview.Child = template;
        }



    }
}

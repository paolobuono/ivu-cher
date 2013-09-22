using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Explore.Environment.Controls.Elements.UG_Elements
{
    class UG_CityElement: UG_BaseElement
    {
        private UI.UG_CityControl cControl;


        public string Citta { get { return cControl != null ? cControl.Nome : string.Empty; } }
        public string Descrizione { get { return cControl != null ? cControl.Descrizione : string.Empty; } }

        public UG_CityElement()
        {
            ElementType = Data.Elements.ElementType.UG_CityElement;
            ConnectionType acceptedBottom = new ConnectionType(ThumbPosition.Bottom,
                                                            typeof(UG_ContentItem),
                                                            ThumbPosition.Top,
                                                            new Type[] { typeof(UG_ContentItem), typeof(Connector) });
            Connections.Add(ThumbPosition.Bottom, acceptedBottom);
            ElementHeight = 300;
        //   Height = 170;
        }

        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            base.ElementContentLoaded(sender, e);
            CircleAdorner.SetVisibility(ThumbPosition.Top, false);
            CircleAdorner.SetVisibility(ThumbPosition.Bottom, true);
            
            cControl = new UI.UG_CityControl();
            ContentPanel.Children.Add(cControl);
        }


        internal Data.Templates.UrbanGameContextWrapper ConvertToWrapper()
        {
            Data.Templates.UrbanGameContextWrapper ug_wrapper = new Data.Templates.UrbanGameContextWrapper();
            ug_wrapper.nome = this.Citta;
            ug_wrapper.descrizione = this.Descrizione;
            return ug_wrapper;
        }


        internal Data.Templates.UrbanGameTemplateWrapper ConvertToTemplate()
        {
            Data.Templates.UrbanGameTemplateWrapper template = new Data.Templates.UrbanGameTemplateWrapper();
            return template;
        }
    }
}

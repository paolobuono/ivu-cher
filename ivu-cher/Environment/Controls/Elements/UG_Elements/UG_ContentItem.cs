using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Explore.Data.Resources;
using System.IO;
using AvengersUtd.Explore.Data;

namespace AvengersUtd.Explore.Environment.Controls.Elements.UG_Elements
{

    // in xml -> Monumento
    public class UG_ContentItem : UG_BaseElement
    {
        public UI.UG_MonumentControl mControl;

        public string Nome { get { return mControl != null ? mControl.NomeMonumento : string.Empty; } }
        public string Latitudine { get { return mControl != null ? mControl.Latitudine : string.Empty; } }
        public string Longitudine { get { return mControl != null ? mControl.Longitudine : string.Empty; } }
        public string Storia { get { return mControl != null ? mControl.Storia : string.Empty; } }
        public string Info { get { return mControl != null ? mControl.Info : string.Empty; } }
        public string Descrizione { get { return mControl != null ? mControl.Descrizione : string.Empty; } }
        public string Tipo { get { return mControl != null ? mControl.Tipo : string.Empty; } }




        public UG_ContentItem()
        {
            ElementType = Data.Elements.ElementType.UG_ContentItem;
            //accept goal on the bottom
            ConnectionType acceptedBottom = new ConnectionType(ThumbPosition.Bottom,
                                                            typeof(UG_ContentItem),
                                                            ThumbPosition.Top,
                                                            new Type[] { typeof(Connector), typeof(QuestionElement) });

            Connections.Add(ThumbPosition.Bottom, acceptedBottom);

            //accept resources on the right
            ConnectionType acceptedRight = new ConnectionType(ThumbPosition.Right,
                                                            typeof(UG_ContentItem),
                                                            ThumbPosition.Top,
                                                            new Type[] { typeof(AvengersUtd.Explore.Environment.Controls.Elements.ResourceElement) });
            Connections.Add(ThumbPosition.Right, acceptedRight);

            ElementHeight = 450;
        }


        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            base.ElementContentLoaded(sender, e);

            mControl = new UI.UG_MonumentControl();
            ContentPanel.Children.Add(mControl);
            CircleAdorner.SetVisibility(ThumbPosition.Top, true);
            CircleAdorner.SetVisibility(ThumbPosition.Bottom, true);
            CircleAdorner.SetVisibility(ThumbPosition.Right, true);


        }


        internal Data.Templates.MonumentXml ConvertToWrapper(GridCanvas canvas, AvengersUtd.Explore.Data.Templates.UrbanGameContextWrapper ug_wrapper)
        {
            Data.Templates.MonumentXml t_ret = new Data.Templates.MonumentXml();
            t_ret.latitudine = Latitudine.ToString().toWrapper();
            t_ret.longitudine = Longitudine.ToString().toWrapper();
            t_ret.id = this.elementIndex;
            t_ret.informazione = this.Info.toWrapper();
            t_ret.nome = this.Nome.toWrapper();
            t_ret.storia = this.Storia.toWrapper();

            #region external Elements
            //foto audio e modelli 3d...

            string DataFolder = "contents\\";
            foreach (var item in canvas.GetElementsConnectedTo(this).OfType<ResourceElement>())
            {
                foreach (AbstractResource res in item.ResourceBox.Items)
                {
                    switch (res.Type)
                    {
                        case ResourceType.Image:
                            t_ret.FotoResId.Add(res.Id);
                            ug_wrapper.ListFoto.Add(new AvengersUtd.Explore.Data.Templates.Fotografia{ id= res.Id, file = DataFolder+ new FileInfo(res.UriString).Name});
                            break;
                        case ResourceType.Audio:
                            t_ret.AudioResId.Add(res.Id);
                            ug_wrapper.ListAudioResources.Add(new AvengersUtd.Explore.Data.Templates.RisorsaAudio { id = res.Id, fileAudio = DataFolder + new FileInfo(res.UriString).Name });
                            break;
                        case ResourceType.Model:
                            t_ret.Ricostruzioni3dId.Add(res.Id);
                            break;
                        default:
                            break;
                    }
                }
            }
            // domande
            foreach (var item in canvas.GetElementsConnectedTo(this).OfType<QuestionElement>())
            {
                ug_wrapper.ListDomande.Add(item.ConvertToWrapper());
            }
            #endregion
            return t_ret;
        }

        internal Data.Templates.Tappa ConvertToTemplate(GridCanvas gridCanvas, Data.Templates.UrbanGameTemplateWrapper template)
        {
            Data.Templates.Tappa t_tappa = new Data.Templates.Tappa();
            t_tappa.descrizione = this.Descrizione.toWrapper();
            t_tappa.id = this.elementIndex.ToString();
            t_tappa.tipo = this.Tipo;
            foreach (var item in gridCanvas.GetElementsConnectedTo(this).OfType<QuestionElement>())
            {
                t_tappa.iddomanda.Add(item.QuestionId);
            }
            return t_tappa;
        }

    }
}

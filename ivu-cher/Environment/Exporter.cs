using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Explore.Environment.Controls.Elements;
using AvengersUtd.Explore.Data.Resources;
using AvengersUtd.Explore.Data.Templates;
using AvengersUtd.Explore.Environment.Controls;
using AvengersUtd.Explore.Data;
using System.IO;
using System.Web;
using System.Windows;
using AvengersUtd.Explore.Environment.Controls.Elements.UG_Elements;

namespace AvengersUtd.Explore.Environment
{
    public class Exporter
    {

        const string htmlHeader = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\">\n<html>\n"
                + "<head>\n"
                + "\t<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">"
                + "\t<meta name=\"author\" content=\"CHe Workshop 1.0\">"
                + "\t<meta name=\"description\" content=\"CHe Application\">\n";

        List<Element> elements;
        List<ImageResource> images;
        List<TextResource> texts;
        List<AudioResource> audios;
        List<Theme> themes;
        GridCanvas gridCanvas;

        string appName;
        private string basePath;

        public Exporter(GridCanvas gridCanvas)
        {
            this.gridCanvas = gridCanvas;
            elements = gridCanvas.Elements;
            images = new List<ImageResource>();
            texts = new List<TextResource>();
            audios = new List<AudioResource>();
            themes = new List<Theme>();
        }

        void AddResource(AbstractResource resource)
        {

            switch (resource.Type)
            {
                case ResourceType.Audio:
                    audios.Add((AudioResource)resource);
                    break;

                case ResourceType.Image:
                    images.Add((ImageResource)resource);
                    break;

                default:

                case ResourceType.Text:
                    texts.Add((TextResource)resource);
                    break;
            }
        }


        public bool ExportUrbanGameXML(string appName)
        {
            UrbanGameTemplateWrapper ug_template = new UrbanGameTemplateWrapper();
            UrbanGameContextWrapper ug_context = new UrbanGameContextWrapper();

            

            LoadUgWrappers(gridCanvas, out ug_template, out ug_context);

            if (ug_context == null || ug_template == null)
            { return false; }

            DataManager.Serialize<UrbanGameContextWrapper>(ug_context, appName+"\\context.xml" );
            DataManager.Serialize<UrbanGameTemplateWrapper>(ug_template, appName+"\\template.xml");

            ExportUGResources(gridCanvas, appName);

           
            return true;
        }
        /// <summary>
        /// Export all Urban GameResources in folder /Data/
        /// </summary>
        /// <param name="gridCanvas"></param>
        /// <param name="appName"></param>
        private void ExportUGResources(GridCanvas canvas, string appName)
        {
            List<string> resourceStrings = new List<string>();
            foreach (var item in elements.OfType<ResourceElement>())
            {
                foreach (AbstractResource res in item.ResourceBox.Items)
                {
                    resourceStrings.Add(res.UriString);
                }
            }
            // Create Folder
            DirectoryInfo dir  = new DirectoryInfo(appName+"\\Data");
            if (!dir.Exists)
                dir.Create();

            //save all;
            foreach (var item in resourceStrings)
            {
                try
                {



                    FileInfo res = new FileInfo(item);
                    res.CopyTo(string.Format("{0}\\{1}", dir.FullName, res.Name), true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format
                    ("Errore nella copia del file {0}", item));
                }
            }
        }

        private UrbanGameContextWrapper LoadUgWrappers(GridCanvas gridCanvas,out  UrbanGameTemplateWrapper template,out  UrbanGameContextWrapper context)
        {
            template = null;
            context = null;
                
            UG_CityElement city = elements.OfType<UG_CityElement>().FirstOrDefault();
            #region controllo prerequisiti
            if (city == null)
            {
                MessageBox.Show("City element not found.");
                return null;
            }

            #endregion

            context = city.ConvertToWrapper();
            template = city.ConvertToTemplate();
            //adding monuments ..
            foreach (var item in gridCanvas.GetElementsConnectedTo(city).OfType<UG_ContentItem>())
            {
                context.Luoghi.Add(item.ConvertToWrapper(gridCanvas, context));
                template.Tappe.Add(item.ConvertToTemplate(gridCanvas, template));
            }
            //todo: stile ???? 
            context.stile = new stile { id = "1" };
            return context;
        }



        public bool ExportExcursionGameXML(string appName)
        {
            ExcursionGameElement egElement = null;
            ExcursionGameWrapper egWrapper = new ExcursionGameWrapper();
            List<MissionElementXml> missions = new List<MissionElementXml>();
            PrologueElement pElement = null;
            MissionListElement mlElement;
            try
            {
                egElement = elements.OfType<ExcursionGameElement>().First();
                egWrapper.GameTitle = egElement.Caption;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("No Excursion-Game building block found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                pElement = gridCanvas.GetElementsConnectedTo(egElement).OfType<PrologueElement>().First();
                egWrapper.PrologueTitle = pElement.Caption;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("No Prologue building block found connected to the root.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                mlElement = gridCanvas.GetElementsConnectedTo(pElement).OfType<MissionListElement>().First();
                foreach (KeyValuePair<int, MissionElement> pair in mlElement.MissionList)
                {
                    GoalElement goal = null;
                    OracleElement oracleHint = null;
                    try
                    {
                        goal = gridCanvas.GetElementsConnectedTo(pair.Value).OfType<GoalElement>().First();
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("No Goal building block found connected to Mission block '" + pair.Value.Caption + "'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                    try
                    {
                        oracleHint = gridCanvas.GetElementsConnectedTo(pair.Value).OfType<OracleElement>().First();
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("No OracleHint building block found connected to Mission block '" + pair.Value.Caption + "'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }


                    ResourceElementXml rxGoal = EncodeResourceElement(goal);
                    ResourceElementXml rxHint = EncodeResourceElement(oracleHint);
                    missions.Add(new MissionElementXml()
                    {
                        ID = pair.Value.Caption,
                        Index = pair.Key,
                        OracleHint = rxHint,
                        Goal = rxGoal,
                        PlaceId = pair.Value.PlaceId
                    });
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("No Missions or MissionList building blocks found connected to the Prologue.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            egWrapper.Missions = missions.ToArray();

            List<ContextualSoundXml> cxSounds = new List<ContextualSoundXml>();
            try
            {
                var csData = gridCanvas.GetElementsConnectedTo(egElement).OfType<ContextSoundElement>();
                foreach (ContextSoundElement csElement in csData)
                {
                    ResourceElementXml cxElement = EncodeResourceElement(csElement);

                    cxSounds.Add(new ContextualSoundXml() { ID = cxElement.ID, ResourceIDs = cxElement.ResourceIDs });
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("No Contextual Sounds building blocks found connected to the root.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            egWrapper.ContextualSounds = cxSounds.ToArray();

            ResourceContainer rc = new ResourceContainer()
            {
                AudioResources = audios.ToArray(),
                ImageResources = images.ToArray(),
                TextResources = texts.ToArray()
            };

            DataManager.Serialize(rc, appName.Insert(appName.Length - 4, "_resources"));
            DataManager.Serialize(egWrapper, appName.Insert(appName.Length - 4, "_excursiongame"));

            return true;

        }

        ResourceElementXml EncodeResourceElement(ResourceElement rElement)
        {
            List<AbstractResource> resources = new List<AbstractResource>();
            List<string> resourceIDs = new List<string>();
            try
            {
                foreach (AbstractResource resource in rElement.ResourceBox.Items)
                {
                    AddResource(resource);
                    resourceIDs.Add(resource.Id);
                }
            }
            catch (InvalidCastException)
            {
                MessageBox.Show("Element: " + rElement.Caption + " has no resources.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return new ResourceElementXml() { ID = rElement.Caption, ResourceIDs = resourceIDs.ToArray() };
        }

        #region Museum XML
        public void ExportMuseumGuideXML(string appName)
        {
            foreach (ThemeElement themeElement in elements.OfType<ThemeElement>())
            {
                List<ResourceElementXml> items = new List<ResourceElementXml>();

                foreach (ResourceElement resourceElement in gridCanvas.GetElementsConnectedTo(themeElement).OfType<ResourceElement>())
                {
                    List<string> resourceIDs = new List<string>();

                    foreach (AbstractResource resource in resourceElement.ResourceBox.Items)
                    {

                        AddResource(resource);

                        resourceIDs.Add(resource.Id);
                    }
                    items.Add(new ResourceElementXml()
                    {
                        ID = resourceElement.Caption,
                        ResourceIDs = resourceIDs.ToArray()
                    });
                }
                themes.Add(new Theme() { Id = themeElement.Caption, Items = items.ToArray() });
            }

            ResourceContainer rc = new ResourceContainer()
            {
                AudioResources = audios.ToArray(),
                ImageResources = images.ToArray(),
                TextResources = texts.ToArray()
            };

            MuseumGuideElement mge = elements.OfType<MuseumGuideElement>().First();
            List<string> mgeResourceIds = new List<string>();
            foreach (AbstractResource resource in mge.ResourceBox.Items)
            {
                AddResource(resource);
                mgeResourceIds.Add(resource.Id);
            }
            MuseumGuide mg = new MuseumGuide()
            {
                Id = mge.Caption,
                ResourceIDs = mgeResourceIds.ToArray(),
                Themes = themes.ToArray()
            };

            DataManager.Serialize(rc, appName.Insert(appName.Length - 4, "_resources"));
            DataManager.Serialize(mg, appName.Insert(appName.Length - 4, "_museumguide"));
        }
        #endregion

        #region Museum HTML export
        public void ExportMuseumGuideHTML(string appName, string outputPath)
        {
            this.appName = appName;
            this.basePath = outputPath + "\\" + appName;

            Directory.CreateDirectory(basePath + "\\resources");
            StringBuilder sb = new StringBuilder();
            ExportMuseumGuideMainPageHTML(sb);


            foreach (ResourceElement rElement in elements.OfType<ResourceElement>())
            {
                if (rElement is MuseumGuideElement)
                    continue;
                ExportMuseumGuideItemsHTML(rElement);
            }

            using (StreamWriter sw = File.CreateText(basePath + "\\index.htm"))
            {
                sw.Write(sb.ToString());
            }
        }

        void ExportMuseumGuideMainPageHTML(StringBuilder sb)
        {
            MuseumGuideElement mgElement = elements.OfType<MuseumGuideElement>().First();
            sb.Append(htmlHeader + "\t<title>" + Common.HtmlEncode(mgElement.Caption) + "</title>\n"
                + "\t<link href=\"style.css\" rel=\"stylesheet\" type=\"text/css\"/>"
                + "</head>");

            sb.AppendLine("<body>");


            if (mgElement.ResourceBox.Items.Count > 0)
            {
                foreach (AbstractResource resource in mgElement.ResourceBox.Items.OfType<AbstractResource>())
                {
                    EncodeResource(resource, sb);
                }
            }
            sb.AppendLine("\t<ul>");
            foreach (ThemeElement themeElement in elements.OfType<ThemeElement>())
            {

                sb.AppendLine("\t\t<li>" + themeElement.Caption + "</li>");
                IEnumerable<ResourceElement> rElements = gridCanvas.GetElementsConnectedTo(themeElement).OfType<ResourceElement>();

                if (rElements.Count() == 0)
                    continue;

                sb.AppendLine("\t\t<ul>");
                foreach (ResourceElement pageElement in rElements)
                {
                    sb.AppendLine("\t\t\t<li><a href=\"" + GetHtmlFilename(pageElement.Caption) + "\">" + pageElement.Caption + "</a></li>");
                }
                sb.AppendLine("\t\t</ul>");
            }
            sb.AppendLine("\t</ul>");
            sb.AppendLine("</body>\n</html>");
        }

        void ExportMuseumGuideItemsHTML(ResourceElement rElement)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(htmlHeader + "\t<title>" + Common.HtmlEncode(rElement.Caption) + "</title>\n"
                + "\t<link href=\"style.css\" rel=\"stylesheet\" type=\"text/css\"/>"
                + "</head>");

            sb.AppendLine("<body>");
            foreach (AbstractResource resource in rElement.ResourceBox.Items.OfType<AbstractResource>())
            {
                EncodeResource(resource, sb);
            }
            sb.AppendLine("</body>\n</html>");

            using (StreamWriter sw = File.CreateText(basePath + "\\" + GetHtmlFilename(rElement.Caption)))
            {
                sw.Write(sb.ToString());
            }

        }

        void EncodeResource(AbstractResource resource, StringBuilder sb)
        {
            switch (resource.Type)
            {
                case ResourceType.Image:
                    ImageResource imgResource = (ImageResource)resource;
                    string outputFilePath = basePath + "\\resources\\" + GetFileName(imgResource.Uri.LocalPath);
                    File.Copy(imgResource.Uri.LocalPath, outputFilePath, true);
                    sb.AppendLine("\t<img class=\"image\" src=\"" + outputFilePath + "\"/>");
                    sb.AppendLine("\t<h1 class=\"pageTitle\">" + Common.HtmlEncode(imgResource.Title) + "</h1>");
                    break;

                case ResourceType.Text:
                    TextResource trResource = (TextResource)resource;
                    using (StreamReader sr = File.OpenText(trResource.Uri.LocalPath))
                    {
                        sb.AppendLine("\t<p class=\"text\">" + Common.HtmlEncode(sr.ReadToEnd()) + "</p>");
                    }
                    break;

            }

        }

        static string GetHtmlFilename(string caption)
        {
            return caption.Replace(' ', '-') + ".html";
        }

        static String GetFileName(String hrefLink)
        {
            return Path.GetFileName(Uri.UnescapeDataString(hrefLink).Replace("/", "\\"));
        }

        #endregion



    }
}

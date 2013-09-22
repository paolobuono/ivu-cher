using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AvengersUtd.Explore.Data.Resources;
using System.Collections.Generic;
using AvengersUtd.Explore.Data.Elements;
using System.Linq;
using System.Windows.Input;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    public class ResourceElement : Element
    {
        internal const string ResourceTag ="Resource";
        private ListBox resourceBox;

        protected internal TextBlock InfoText
        {
            get;
            set;
        }
        
        protected internal ListBox ResourceBox
        {
            get { return resourceBox; }
        }

        protected bool ResourceBoxMode
        { get; set; }

        protected internal Dictionary<ResourceType, ResourceRule> ResourceRules { get; set; }

        public ResourceElement()
        {
            Background = Brushes.LightYellow;
            ResourceRules = new Dictionary<ResourceType, ResourceRule>();
        }

        #region Resource Rules

        public bool ContainsRule(ResourceType type)
        {
            return ResourceRules.Keys.Contains(type);
        }

        public bool MatchRule(ResourceType type)
        {
            

            int currentResources = ResourceBoxMode ? resourceBox.Items.Cast<AbstractResource>().Count(resource => resource.Type == type)
                : 0 ;
            ResourceRule rule = ResourceRules[type];

            if (currentResources < rule.Max)
                return true;
            else return false;
        }

        #endregion

        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            base.ElementContentLoaded(sender, e);

            InfoText = new TextBlock { Text = "Drag resources inside:", FontSize = 14, TextWrapping = TextWrapping.Wrap };
            resourceBox = new System.Windows.Controls.ListBox {Style = (Style) FindResource("RuleBox")};
            ResourceBoxMode = false;
            resourceBox.Drop += resourceBox_Drop;
            resourceBox.DragEnter += resourceBox_DragEnter;
            resourceBox.DragOver += resourceBox_DragEnter;
            resourceBox.DragLeave += resourceBox_DragEnter;
            resourceBox.KeyUp += resourceBox_KeyUp;
            resourceBox.MaxHeight = ContentPanel.Height - 30;
            ContentPanel.Children.Add(InfoText);
            ContentPanel.Children.Add(resourceBox);
        }

        void resourceBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!ResourceBoxMode)
                return;

            if (e.Key == Key.Delete || e.Key == Key.Back)
                resourceBox.Items.RemoveAt(resourceBox.SelectedIndex);
        }

        private void resourceBox_DragEnter(object sender, DragEventArgs e)
        {
            base.OnDragOver(e);
            if (!e.Data.GetDataPresent(ResourceTag)) return;

            AbstractResource resource = (AbstractResource)e.Data.GetData(ResourceTag);

            if (ContainsRule(resource.Type))
            {
                if (MatchRule(resource.Type))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
                else
                    e.Effects = DragDropEffects.None;
            }
            
        }

        private void resourceBox_Drop(object sender, DragEventArgs e)
        {
            if (!ResourceBoxMode)
            {
                resourceBox.Style = (Style)FindResource("ResourceBox");
                resourceBox.ItemsSource = null;
                ResourceBoxMode = true;
            }
            resourceBox.Items.Add(e.Data.GetData(ResourceTag));
        }

    }
}

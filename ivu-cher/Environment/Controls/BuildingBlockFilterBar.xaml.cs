using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AvengersUtd.Explore.Data.Resources;
using System;
using AvengersUtd.Explore.Data.Elements;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace AvengersUtd.Explore.Environment.Controls
{
    /// <summary>
    /// Interaction logic for FilterBar.xaml
    /// </summary>
    public partial class BuildingBlockFilterBar : UserControl
    {
        protected internal bool IsDragInProgress { get; set; }

        // public BuildingBlockFilterBar()
        //{
        //    InitializeComponent();           
        //}

        public BuildingBlockFilterBar()
        {
            InitializeComponent();            
        }

        void FillToolBar()
        {
            switch (Global.Template)
            {
                case TemplateType.Unknown:
                    break;
                case TemplateType.HistoryPuzzle:
                    AddHistoryPuzzleElements();
                    break;
                case TemplateType.MuseumGuide:
                    AddMuseumGuideElements();
                    break;
                case TemplateType.ExcursionGame:
                    AddExcursionGameElements();
                    break;
                case TemplateType.UrbanGame:
                    AddUrbanGameElements();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddUrbanGameElements()
        {
            BuildingBlockFilterButton cityItem = new BuildingBlockFilterButton("City") { ToolTip = "City", ElementType = ElementType.UG_CityElement, Height = 62, Width = 62 };
            AddButton(cityItem);
            BuildingBlockFilterButton ebConnector = new BuildingBlockFilterButton("Connector") { ToolTip = "Connector", ElementType = ElementType.Connector, Height = 62, Width = 62 };
            AddButton(ebConnector);
            BuildingBlockFilterButton monumentItem = new BuildingBlockFilterButton("Monument") { ToolTip = "Monument", ElementType = ElementType.UG_ContentItem, Height = 62, Width = 62 };
            AddButton(monumentItem);
            BuildingBlockFilterButton qAItem = new BuildingBlockFilterButton("Questions") { ToolTip = "Questions", ElementType = ElementType.QuestionsAnswers, Height = 62, Width = 62 };
            AddButton(qAItem);
            BuildingBlockFilterButton resourceItem = new BuildingBlockFilterButton("Resource") { ToolTip = "Resource", ElementType = ElementType.UG_ResourceElement, Height = 62, Width = 62 };
            AddButton(resourceItem);

        }

        void AddHistoryPuzzleElements()
        {
            BuildingBlockFilterButton ebApp = new BuildingBlockFilterButton("History-Puzzle") { ToolTip = "History-Puzzle",  ElementType = ElementType.HistoryPuzzleApp, Height = 62, Width = 62 };
            AddButton(ebApp);
            BuildingBlockFilterButton ebResource = new BuildingBlockFilterButton("PuzzleBtn") { ToolTip = "Puzzle Button", ElementType = ElementType.Puzzle, Height = 62, Width = 62 };
            AddButton(ebResource);
            BuildingBlockFilterButton ebQA = new BuildingBlockFilterButton("QeA") { ToolTip = "Q&A", ElementType = ElementType.QuestionsAnswers, Height = 62, Width = 62 };
            AddButton(ebQA);
            BuildingBlockFilterButton ebMap = new BuildingBlockFilterButton("Map") { ToolTip = "Map", ElementType = ElementType.Map, Height = 62, Width = 62 };
            AddButton(ebMap);
            BuildingBlockFilterButton ebConnector = new BuildingBlockFilterButton("Connector") { ToolTip = "Connector", ElementType = ElementType.Connector, Height = 62, Width = 62 };
            AddButton(ebConnector);
        }

        void AddMuseumGuideElements()
        {
            BuildingBlockFilterButton ebApp = new BuildingBlockFilterButton("Museum Guide") { ToolTip = "Museum Guide", ElementType = ElementType.MuseumGuideApp, Height = 62, Width = 62 };
            AddButton(ebApp);
            BuildingBlockFilterButton ebMap = new BuildingBlockFilterButton("Map") { ToolTip = "Map", ElementType = ElementType.Map, Height = 62, Width = 62 };
            AddButton(ebMap);
            BuildingBlockFilterButton ebTheme = new BuildingBlockFilterButton("Theme") { ToolTip = "Theme", ElementType = ElementType.Theme, Height = 62, Width = 62 };
            AddButton(ebTheme);
            BuildingBlockFilterButton ebItem = new BuildingBlockFilterButton("Item") { ToolTip = "Item", ElementType = ElementType.Item, Height = 62, Width = 62 };
            AddButton(ebItem);
            BuildingBlockFilterButton ebConnector = new BuildingBlockFilterButton("Connector") { ToolTip = "Connector", ElementType = ElementType.Connector, Height = 62, Width = 62 };
            AddButton(ebConnector);
        }
         
        void AddExcursionGameElements()
        {
            BuildingBlockFilterButton ebApp = new BuildingBlockFilterButton("Excursion-Game") { ToolTip = "Excursion-Game", ElementType = ElementType.ExcursionGameApp, Height = 62, Width = 62 };
            AddButton(ebApp);
            BuildingBlockFilterButton ebPrologue = new BuildingBlockFilterButton("Prologue") { ToolTip = "Prologue", ElementType = ElementType.Prologue, Height = 62, Width = 62 };
            AddButton(ebPrologue);
            BuildingBlockFilterButton ebMission = new BuildingBlockFilterButton("Mission") { ToolTip = "Mission", ElementType = ElementType.Mission, Height = 62, Width = 62 };
            AddButton(ebMission);
            BuildingBlockFilterButton ebCSound = new BuildingBlockFilterButton("Cont. Sounds") { ToolTip = "Cont. Sounds", ElementType = ElementType.CSound, Height = 62, Width = 62 };
            AddButton(ebCSound);
            BuildingBlockFilterButton ebGoal = new BuildingBlockFilterButton("Goal") { ToolTip = "Goal", ElementType = ElementType.Goal, Height = 62, Width = 62 };
            AddButton(ebGoal);
            BuildingBlockFilterButton ebMList = new BuildingBlockFilterButton("Mission List") { ToolTip = "Mission List", ElementType = Data.Elements.ElementType.MissionList, Height = 62, Width = 62 };
            AddButton(ebMList);
            BuildingBlockFilterButton ebOracle = new BuildingBlockFilterButton("Oracle Hint") { ToolTip = "Oracle Hint", ElementType = Data.Elements.ElementType.OracleHint, Height = 62, Width = 62 };
            AddButton(ebOracle);
            BuildingBlockFilterButton ebConnector = new BuildingBlockFilterButton("Connector") { ToolTip = "Connector", ElementType = ElementType.Connector, Height = 62, Width = 62 };
            AddButton(ebConnector);

        }

        void AddButton(BuildingBlockFilterButton button)
        {
            button.OwnerToolbar = this;
            buttonPanel.Children.Add(button);
        } 

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillToolBar();
        }

        public IEnumerable<FilterButton> Filters
        {
            get
            {
                return buttonPanel.Children.OfType<FilterButton>();
            }
        }

        internal UIElementCollection Controls
        {
            get { return buttonPanel.Children; }
        }

        private void FilterButton_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = sender as ToggleButton;

            foreach (ToggleButton othersToggleButton in buttonPanel.Children)
            {
                if (othersToggleButton.Equals(toggleButton))
                    continue;

                othersToggleButton.IsChecked = false;

            }
            if (toggleButton.IsChecked.HasValue && toggleButton.IsChecked.Value)
            {
                Global.WorkArea.DesignMode = true;
                Global.WorkArea.Cursor = Cursors.Cross;
            }
        }

        private void FilterButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Global.WorkArea.DesignMode = false;
            Global.WorkArea.Cursor = Cursors.Arrow;
        }


    }
}

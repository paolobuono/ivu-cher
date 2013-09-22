    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using AvengersUtd.Explore.Data.Resources;
using AvengersUtd.Explore.Data.Elements;

namespace AvengersUtd.Explore.Environment.Controls
{
    /// <summary>
    /// Interaction logic for ToolBar.xaml
    /// </summary>
    public partial class ToolBar : UserControl
    {
        protected internal bool IsDragInProgress { get; set; }

        public ToolBar()
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
            ElementButton CityItem = new ElementButton { Content = "City", ElementType = ElementType.UG_CityElement };
            AddButton(CityItem);
            ElementButton ebConnector = new ElementButton { Content = "Connector", ElementType = ElementType.Connector };
            AddButton(ebConnector);
            ElementButton contentItem = new ElementButton { Content = "Monument", ElementType = ElementType.UG_ContentItem };
            AddButton(contentItem);
            //BuildingBlockFilterButton goalItem= new BuildingBlockFilterButton { Content = "Goal", ElementType = ElementType.UG_GoalElement };
            //AddButton(goalItem);

            ElementButton QAItem= new ElementButton { Content = "Questions", ElementType = ElementType.QuestionsAnswers };
            AddButton(QAItem);
            ElementButton resourceItem= new ElementButton { Content = "Resource", ElementType = ElementType.UG_ResourceElement};
            AddButton(resourceItem);

        }

        void AddMuseumGuideElements()
        {
            ElementButton ebApp = new ElementButton
            {
                Content = "Museum Guide",
                ElementType = ElementType.MuseumGuideApp
            };
            ebApp.Width = 96;

            ElementButton ebMap = new ElementButton {Content = "Map", ElementType = ElementType.Map};
            ElementButton ebTheme= new ElementButton{Content ="Theme", ElementType = ElementType.Theme};
            ElementButton ebItem = new ElementButton {Content = "Item", ElementType = ElementType.Item};
            ElementButton ebConnector = new ElementButton { Content = "Connector", ElementType = ElementType.Connector };

            AddButton(ebApp);
            //AddButton(ebMap);
            AddButton(ebTheme);
            AddButton(ebItem);
            AddButton(ebConnector);
        }

        void AddHistoryPuzzleElements()
        {
            ElementButton ebApp = new ElementButton
                                      {
                                          Content = "History-Puzzle",
                                          ElementType = ElementType.HistoryPuzzleApp
                                      };
            ElementButton ebResource = new ElementButton {Content = "Puzzle", ElementType = ElementType.Puzzle};
            ElementButton ebQA = new ElementButton {Content = "Q&A", ElementType = ElementType.QuestionsAnswers};
            ElementButton ebMap = new ElementButton { Content = "Map", ElementType = ElementType.Map };
            ElementButton ebConnector = new ElementButton {Content = "Connector", ElementType = ElementType.Connector};
            AddButton(ebApp);
            AddButton(ebResource);
            AddButton(ebQA);
            //AddButton(ebMap);
            AddButton(ebConnector);
        }

        void AddExcursionGameElements()
        {
            ElementButton ebApp = new ElementButton
            {
                Content = "Excursion-Game",
                ElementType = ElementType.ExcursionGameApp
            };

            ElementButton ebPrologue = new ElementButton { Content = "Prologue", ElementType = ElementType.Prologue };
            ElementButton ebMission = new ElementButton { Content = "Mission", ElementType = ElementType.Mission };
            ElementButton ebCSound = new ElementButton { Content = "Cont. Sounds", ElementType = ElementType.CSound };
            ElementButton ebGoal = new ElementButton { Content = "Goal", ElementType = ElementType.Goal };
            ElementButton ebMList = new ElementButton { Content = "Mission List", ElementType = Data.Elements.ElementType.MissionList };
            ElementButton ebOracle = new ElementButton { Content = "Oracle Hint", ElementType = Data.Elements.ElementType.OracleHint };
            ElementButton ebConnector = new ElementButton { Content = "Connector", ElementType = ElementType.Connector };

            AddButton(ebApp);
            AddButton(ebPrologue);
            AddButton(ebMission);
            AddButton(ebCSound);
            AddButton(ebGoal);
            AddButton(ebMList);
            AddButton(ebOracle);
            AddButton(ebConnector);
        }

        void AddButton(ElementButton button)
        {
            button.OwnerToolbar = this;
            buttonPanel.Children.Add(button);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillToolBar();
        }
    }
}

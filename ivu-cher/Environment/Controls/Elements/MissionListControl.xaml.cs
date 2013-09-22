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
using System.Collections.ObjectModel;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    /// <summary>
    /// Interaction logic for MissionListControl.xaml
    /// </summary>
    public partial class MissionListControl : UserControl
    {
        ObservableCollection<string> missions;

        public void AddMission(MissionElement mElement)
        {
            missions.Add(mElement.Caption);
        }

        public int FindMissionIndex(string caption)
        {
            return missions.IndexOf(caption);
        }

        public void EditMission(string oldCaption, string newCaption)
        {
            int index = FindMissionIndex(oldCaption);
            
            missions[index] = newCaption;
        }

        public void RemoveMission(string caption)
        {
            int index = FindMissionIndex(caption);
            missions.Remove(caption);
        }

        public bool HasMission(string caption)
        {
            return missions.Contains(caption);
        }

        public MissionListControl()
        {
            InitializeComponent();
            missions = new ObservableCollection<string>();
            missionList.ItemsSource = missions;
        }

        public string[] Missions
        {
            get { return missions.ToArray(); }
        }

        private void upButton_Click(object sender, RoutedEventArgs e)
        {
            int location = missionList.SelectedIndex;
            if (location > 0)
            {
                object rememberMe = missionList.SelectedItem;
                missions.RemoveAt(location);
                missions.Insert(location - 1, (string)rememberMe);
                missionList.SelectedIndex = location - 1;
            }
        }

        private void downButton_Click(object sender, RoutedEventArgs e)
        {
            int location = missionList.SelectedIndex;
            if (location < missionList.Items.Count-1)
            {
                object rememberMe = missionList.SelectedItem;
                missions.RemoveAt(location);
                missions.Insert(location + 1, (string)rememberMe);
                missionList.SelectedIndex = location + 1;
            }
        }
    }
}

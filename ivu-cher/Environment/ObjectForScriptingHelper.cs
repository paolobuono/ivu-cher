using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using AvengersUtd.Explore.Environment.Controls;

namespace AvengersUtd.Explore.Environment
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class ObjectForScriptingHelper
    {
        MapPreviewPanel mExternalWPF;
        public ObjectForScriptingHelper(MapPreviewPanel w)
        {
            this.mExternalWPF = w;
        }

        public void ActionChanged(string action)
        {
          this.mExternalWPF.ActionChanged(action);
        }

        public void PinAdded(string latitude, string longitude)
        {
          this.mExternalWPF.PinAdded(latitude, longitude);
        }

        public void RetrieveCoordsFromMouse()
        {
          this.mExternalWPF.RetrieveCoordsFromMousePosition();
        }

    }
}

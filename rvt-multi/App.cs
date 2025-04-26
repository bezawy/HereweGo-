using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using System;
using System.Reflection;

namespace rvt_multi
{
    [Transaction(TransactionMode.Manual)]
    public class App : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            // Create ribbon tab
            string tabName = "Here we go!";
            application.CreateRibbonTab(tabName);

            // Create ribbon panel
            RibbonPanel panel = application.CreateRibbonPanel(tabName, "Multi Elements");

            // Get assembly path
            string assemblyPath = Assembly.GetExecutingAssembly().Location;


            #region push_btns
            //// Create push button
            PushButtonData buttonData = new PushButtonData(
                "MultiElements",
                "Multi Elements",
                assemblyPath,
                "rvt_Multielements.ShowMultiElementsWindow");

            // Add button to panel
            panel.AddItem(buttonData);
            #endregion


            return Result.Succeeded;
        }
    }
}
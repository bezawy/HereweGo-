using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using GenericSelection;
using System.Linq;

namespace rvt_Multielements
{
    public class DeleteMultipleElementsHandler : IExternalEventHandler
    {
        public UIApplication UiApp { get; set; }

        public void Execute(UIApplication app)
        {
            try
            {
                UIDocument uidoc = app.ActiveUIDocument;
                Document doc = uidoc.Document;

                BuiltInCategory categoryfilter = BuiltInCategory.OST_Windows;
                GenericSelections filter = new GenericSelections(categoryfilter);

                var selectedElements = uidoc.Selection.PickElementsByRectangle(filter, "Select Windows to delete")
                    .Select(e => e.Id)
                    .ToList();

                using (Transaction trans = new Transaction(doc, "Delete Windows"))
                {
                    trans.Start();
                    doc.Delete(selectedElements);
                    trans.Commit();
                }

                TaskDialog.Show("Success", $"{selectedElements.Count} windows deleted successfully.");
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                // User canceled selection - do nothing
            }
            catch (System.Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
        }

        public string GetName()
        {
            return "Delete Multiple Elements Handler";
        }
    }
}

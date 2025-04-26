using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;

namespace rvt_Multielements
{

    public class DeleteElementHandler : IExternalEventHandler
    {
        public UIApplication UiApp { get; set; }
        public void Execute(UIApplication app)
        {
            try
            {
                UIDocument uidoc = app.ActiveUIDocument;
                Document doc = uidoc.Document;

                using (Transaction trans = new Transaction(doc, "Delete Element"))
                {
                    

                    Reference pickedRef = uidoc.Selection.PickObject(ObjectType.Element, "Select an element to delete");
                    ElementId elementId = pickedRef.ElementId;

                    trans.Start();
                    doc.Delete(elementId);
                    trans.Commit();

                    TaskDialog.Show("Success", "Element deleted successfully");
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
        }

        public string GetName()
        {
            return "Delete Element Handler";
        }
    }



}

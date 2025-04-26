using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selection
{
    [Transaction(TransactionMode.Manual)]
    public class ManuallyDeleteItems : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            using (Transaction trans = new Transaction(doc, "Delete Windows"))
            {
                try
                {
                    // Select an element
                    Reference pickedRef = uidoc.Selection.PickObject(ObjectType.Element, "Select manually Element to Delete!");
                    ElementId elementId = pickedRef.ElementId;

                    trans.Start();
                    doc.Delete(elementId);
                    trans.Commit();

                    return Result.Succeeded;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return Result.Failed;
                }
            }
        }
    }
}
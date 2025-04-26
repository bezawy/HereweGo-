using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using GenericSelection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace rvt_Multielements
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    [Transaction(TransactionMode.Manual)]
    public class ShowMultiElementsWindow : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                //create and show wpf window
                var mainWindow = new MainWindow(commandData.Application);
                mainWindow.Show();

                return Result.Succeeded;
            }
            catch (Exception EX)
            {
                message = EX.Message;
                return Result.Failed;
            }
        }


    }

    public partial class MainWindow : Window
    {
        private readonly UIDocument _uidoc;
        private readonly Document _doc;
        private readonly UIApplication _uiapp;
        private ExternalEvent _deleteElementEvent;
        private DeleteElementHandler _deleteElementHandler;
        private ExternalEvent _deleteMultipleElementsEvent;
        private DeleteMultipleElementsHandler _deleteMultipleElementsHandler;



        public MainWindow(UIApplication uiapp) // modify our constructor 
        {
            InitializeComponent();
            _uiapp = uiapp;
            _uidoc = uiapp.ActiveUIDocument;
            _doc = _uidoc.Document;
            _deleteElementHandler = new DeleteElementHandler();
            _deleteElementHandler.UiApp = _uiapp;
            _deleteElementEvent = ExternalEvent.Create(_deleteElementHandler);

            // Multiple elements delete
            _deleteMultipleElementsHandler = new DeleteMultipleElementsHandler();
            _deleteMultipleElementsHandler.UiApp = _uiapp;
            _deleteMultipleElementsEvent = ExternalEvent.Create(_deleteMultipleElementsHandler);


            // Set up button click handlers
            DltMltItems.Click += DeleteMultipleItems_Click;
            DltManualItems.Click += DeleteManualItem_Click;
            CreateWall.Click += CreateWall_Click;
        }

        private void DeleteMultipleItems_Click(object sender, RoutedEventArgs e)
        {
            using (Transaction trs = new Transaction(_doc, "DeleteWindows"))
            {
                _deleteMultipleElementsEvent.Raise();


                try
                {



                    BuiltInCategory categoryfilter = BuiltInCategory.OST_Windows;
                    GenericSelections filter = new GenericSelections(categoryfilter);


                    var element1 = _uidoc.Selection.PickElementsByRectangle(filter, "just select Windows").
                        Select(drs => drs.Id).ToList();


                    trs.Start();
                    _doc.Delete(element1);
                    trs.Commit();




                }
                catch (Exception ex)
                {

                    TaskDialog.Show("Warning Message", ex.Message);



                }


                }
            }

        private void DeleteManualItem_Click(object sender, RoutedEventArgs e)
        {
            using (Transaction trans = new Transaction(_doc, "delete element"))
            {
                _deleteElementEvent.Raise();

                try
                {
                    Reference pickedRef = _uidoc.Selection.PickObject(ObjectType.Element, "Delete the element");
                    ElementId elementId = pickedRef.ElementId;

                    trans.Start();
                    _doc.Delete(elementId);
                    trans.Commit();

                    TaskDialog.Show("Success", "Element deleted successfully");
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", ex.Message);
                }
            }
        }

        private void CreateWall_Click(object sender, RoutedEventArgs e)
        {
            // Implementation remains empty as per original
        }
    }
}
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingLibrary
{
    public class DoorsUtils
    {
        public static int CountDoors(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<FamilyInstance> doors = new FilteredElementCollector(doc)
                                                        .OfCategory(BuiltInCategory.OST_Doors)
                                                        .WhereElementIsNotElementType()
                                                        .Cast<FamilyInstance>()
                                                        .ToList();
            int result = doors.Count();
            return result;
        }
    }
}

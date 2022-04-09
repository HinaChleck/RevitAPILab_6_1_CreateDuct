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
    public class WallsUtils
    {
        public static double WallsVolume(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<Wall> walls = new FilteredElementCollector(doc)
                                                        .OfClass(typeof(Wall))
                                                        .Cast<Wall>()
                                                        .ToList();

            double result = 0;
            foreach (Wall wall in walls)
            {
                result+= UnitUtils.ConvertFromInternalUnits(wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble(), DisplayUnitType.DUT_CUBIC_METERS);
            }
            return result;
        }
    }
}

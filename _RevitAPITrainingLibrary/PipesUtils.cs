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
    public class PipesUtils
    {
        public static int CountPipes(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<Pipe> pipes = new FilteredElementCollector(doc)
                                                        .OfClass(typeof(Pipe))
                                                        .Cast<Pipe>()
                                                        .ToList();
            int result = pipes.Count();
            return result;
        }
    }
}

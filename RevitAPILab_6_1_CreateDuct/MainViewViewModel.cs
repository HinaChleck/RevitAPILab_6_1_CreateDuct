using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using RevitAPITrainingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPILab_6_1_CreateDuct
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        private DuctType selectedDuctType;

        public List<DuctType> DuctTypes { get; } = new List<DuctType>();//списки лучше создавать пустыми, а не null, во избежание ошибок
        public MEPSystemType mEPSystemType { get; set; }
        public List<Level> Levels { get; } = new List<Level>();
        public DelegateCommand SaveCommand { get; }
        public double MoveCenter { get; set; }
        public List<XYZ> Points { get; } = new List<XYZ>();
        public DuctType SelectedDuctType { get => selectedDuctType; set => selectedDuctType = value; }
        public Level SelectedLevel { get; set; }
        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            DuctTypes = DuctUtils.GetDuctTypes(commandData);
            Levels = LevelUtils.GetLevels(commandData);
            SaveCommand = new DelegateCommand(OnSaveCommand);
            MoveCenter = 0;
            mEPSystemType = DuctUtils.GetSystemType(commandData);
            Points = SelectionUtils.GetPoints(_commandData, "Выберите точки", ObjectSnapTypes.Endpoints);


        }
        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (Points.Count<2 || SelectedDuctType == null || SelectedLevel == null)    
                    return;
            var curves = new List<Curve>();
            for (int i= 0; i<Points.Count; i++)
            {
                if (i == 0)
                    continue;

                XYZ prevPoint = Points[i-1];
                XYZ currentPoint = Points[i];

                Curve curve = Line.CreateBound(prevPoint, currentPoint);
                curves.Add(curve);
            }

            using (var ts = new Transaction(doc, "Создать воздуховод"))
            {
                ts.Start();

                foreach (var curve in curves)
               
                {
                    Duct.Create(doc, mEPSystemType.Id, selectedDuctType.Id, SelectedLevel.Id, curve.GetEndPoint(0), curve.GetEndPoint(1));
                }

                ts.Commit();
            }
            RaiseCloseRequest();

        }
        public event EventHandler CloseRequest;

        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }

}

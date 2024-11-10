using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{


    public class Root
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string SeriesType { get; set; }
        public List<CabinetGroup> CabinetGroups { get; set; }
    }

    public class CabinetGroup
    {
        public int Id { get; set; }
        public string SortOrder { get; set; }
        public string Name { get; set; }
        public List<Component> Components { get; set; }
        public List<CabinetSingle> CabinetSingles { get; set; }
    }

    public class Component
    {
        public string Type { get; set; }
        public string MaterialCode { get; set; }
        public string Name { get; set; }
        public string Num { get; set; }
        public string StructType { get; set; }
        public string StructId { get; set; }
        public string UnMatchedOptionError { get; set; }
    }

    public class CabinetSingle
    {
        public List<Component> Components { get; set; }
        public List<FunctionUnit> FunctionUnits { get; set; }
    }

    public class FunctionUnit
    {
        public string Type { get; set; }
        public string MaterialCode { get; set; }
        public string Name { get; set; }
        public string Num { get; set; }
        public string StructType { get; set; }
        public string StructId { get; set; }
        public string UnMatchedOptionError { get; set; }
    }


}

using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD;

namespace AsposeDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string licensePath = @"D:\Aspose\Aspose.CADfor.NET.lic";
            License svgLicense = new License();
            svgLicense.SetLicense(licensePath);

            short newWidth = 50;

            string fileName = @"D:\Aspose\小视图.dxf";
            string outFile = @"D:\Aspose\小视图.svg";
            // Load DXF file
            using (CadImage cadImage = (CadImage)Aspose.CAD.Image.Load(fileName))
            {
                foreach (var entity in cadImage.Entities)
                {
                    entity.LineWeight = newWidth;

                    if (entity.TypeName == CadEntityTypeName.INSERT)
                    {
                        CadInsertObject cadInsert = (CadInsertObject)entity;
                        SetLineWeight(cadImage, cadInsert, newWidth);
                    }
                }

                foreach (Aspose.CAD.FileFormats.Cad.CadTables.CadLayerTable layer in cadImage.Layers)
                {
                    layer.LineWeight = newWidth;
                }

                CadRasterizationOptions rasterizationOptions = new CadRasterizationOptions();
                rasterizationOptions.PageHeight = 1000;
                rasterizationOptions.PageWidth = (int)(rasterizationOptions.PageHeight * cadImage.Width / cadImage.Height);

                rasterizationOptions.DrawType = CadDrawTypeMode.UseObjectColor;

                // this option could be used to decrease text width
                rasterizationOptions.Quality.TextThicknessNormalization = true;

                SvgOptions svgOptions = new Aspose.CAD.ImageOptions.SvgOptions();
                svgOptions.VectorRasterizationOptions = rasterizationOptions;

                // Save DXF as SVG
                cadImage.Save(outFile, svgOptions);
            }

            static void SetLineWeight(CadImage cadImage, CadInsertObject cadInsert, short newWeight)
            {
                CadBlockEntity block = cadImage.BlockEntities[cadInsert.Name];

                foreach (CadEntityBase blockEntity in block.Entities)
                {
                    blockEntity.LineWeight = newWeight;

                    if (blockEntity.TypeName == CadEntityTypeName.INSERT)
                    {
                        SetLineWeight(cadImage, (CadInsertObject)blockEntity, newWeight);
                    }
                }
            }
        }
    }
}
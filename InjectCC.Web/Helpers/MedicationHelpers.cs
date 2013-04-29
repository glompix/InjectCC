using System.Web;
using System.Web.Mvc;
using InjectCC.Web.ViewModels.Medication;
using System.Text;
using System.IO;
using InjectCC.Web;

public static class MedicationHelpers
{
    public static IHtmlString ReferenceImageClasses<T>(this HtmlHelper<T> Html)
        where T : MedicationModel
    {
        var sb = new StringBuilder();
        foreach (var refImage in Html.ViewData.Model.ReferenceImages)
        {
            var path = UrlHelper.GenerateContentUrl(Path.Combine(PathConfig.ReferenceImagePath, refImage), Html.ViewContext.HttpContext);
            var filename = Path.GetFileNameWithoutExtension(refImage);
            sb.AppendFormat(@".reference-image-{0} {{ background-image: url('{1}'); }}", filename, path);
        }
        return new HtmlString(sb.ToString());
    }
}
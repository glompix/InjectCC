using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace InjectCC.Web.Helpers
{
    public static class SiteHelpers
    {
        public static IHtmlString NavActionLink(this HtmlHelper Html, string linkText, string action, string controller,
            object attributes = null)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (controller == null)
                throw new ArgumentNullException("controller");

            var htmlAttributes = GetHtmlAttributes(attributes);
            var actionMatches = action.Equals(Html.ViewContext.RequestContext.RouteData.Values["Action"]);
            var controllerMatches = controller.Equals(Html.ViewContext.RequestContext.RouteData.Values["Controller"]);
            if (actionMatches && controllerMatches)
                htmlAttributes["class"] = htmlAttributes.ContainsKey("class")
                    ? htmlAttributes["class"] + " active"
                    : "active";

            var link = Html.ActionLink(linkText, action, controller).ToString();
            return new HtmlString(string.Format("<li{1}>{0}</li>", link, ToAttributesString(htmlAttributes)));
        }

        /// <summary>
        /// Renders an image to the page.
        /// </summary>
        /// <param name="url">The URL for the image.</param>
        /// <param name="alt">The alt text for the image.</param>
        /// <param name="class_">A CSS class to be applied to the image.</param>
        /// <returns>HTML markup which displays the image.</returns>
        public static IHtmlString Image(this HtmlHelper Html, string url, object attributes = null)
        {
            var htmlAttributes = GetHtmlAttributes(attributes);
            htmlAttributes.Remove("src"); // Remove attributes that we will provide.

            string html = "<img src=\"{0}\"{1} />";
            return new HtmlString(string.Format(html, Html.Content(url), ToAttributesString(htmlAttributes)));
        }

        /// <summary>
        /// Loads an external script into the page.
        /// </summary>
        /// <param name="url">The path to the script.</param>
        /// <returns>HTML markup which loads the script.</returns>
        public static IHtmlString Script(this HtmlHelper Html, string url)
        {
            url = Html.FixProtocolForHref(url);
            url = AddVersionation(url);

            string html = "<script type=\"text/javascript\" src=\"{0}\"></script>";
            return new HtmlString(string.Format(html, Html.Content(url)));
        }

        /// <summary>
        /// Loads an external style sheet into the page.
        /// </summary>
        /// <param name="url">The path to the style sheet.</param>
        /// <param name="media">The media type to which the style sheet applies.</param>
        /// <returns>HTML markup which loads the style sheet.</returns>
        public static IHtmlString Stylesheet(this HtmlHelper Html, string url, string media = null)
        {
            url = Html.FixProtocolForHref(url);
            url = AddVersionation(url);

            if (!string.IsNullOrEmpty(media))
                media = "media=\"" + media + "\"";

            string html = "<link type=\"text/css\" rel=\"stylesheet\" href=\"{0}\" {1}/>";
            return new HtmlString(string.Format(html, Html.Content(url), media));
        }

        private static string AddVersionation(string url)
        {
            var versionNumber = Version();
            if (versionNumber != null && url.Contains("?"))
                return string.Format("{0}&v={1}", url, versionNumber);
            else if (versionNumber != null)
                return string.Format("{0}?v={1}", url, versionNumber);
            else
                return url;
        }

        /// <summary>
        /// Creates a link to external metadata resources.
        /// </summary>
        public static IHtmlString MetaLink(this HtmlHelper Html, string rel, string href, string type)
        {
            href = Html.FixProtocolForHref(href);
            href = Html.Content(href);

            var html = "<link rel=\"{0}\" href=\"{1}\" type=\"{2}\" />";
            return new HtmlString(string.Format(html, rel, href, type));
        }

        public static bool IsSSL(this HtmlHelper Html) { return Html.ViewContext.HttpContext.Request.IsSecureConnection; }
        private static IEnumerable<string> _trustedSecureDomains = new string[] { 
            "webgenesis.yourcls.com",
            "*.google.com",
            "*.googleapis.com"
        };

        /// <summary>
        /// Switches a URL's protocol to HTTPS if the current request is over a secure connection and the
        /// URL comes from a domain in PresentationExtensions._trustedSecureDomains.
        /// </summary>
        private static string FixProtocolForHref(this HtmlHelper Html, string url)
        {
            Func<string, string> regexEncode = (d) => "^http://" + d.Replace(".", "\\.").Replace("*", "[^.]+");

            if (Html.IsSSL() && _trustedSecureDomains.Any(d => Regex.IsMatch(url, regexEncode(d))))
                url = url.Replace("http://", "https://");

            return url;
        }

        private static string Content(this HtmlHelper Html, string url)
        {
            UrlHelper Url = new UrlHelper(new RequestContext(Html.ViewContext.HttpContext, Html.ViewContext.RouteData));
            return Url.Content(url);
        }

        private static IDictionary<string, string> GetHtmlAttributes(object attrs)
        {
            if (attrs is IDictionary<string, string>)
                return (IDictionary<string, string>)attrs;
            else
            {
                var htmlAttrs = new Dictionary<string, string>();
                foreach (var prop in GetProperties(attrs))
                {
                    var normalizedName = prop.Key.Trim('@', '_').ToLower(); // HTML attribute names should be lowercase.
                    var normalizedValue = Convert.ToString(prop.Value) ?? "";
                    htmlAttrs[normalizedName] = normalizedValue;
                }
                return htmlAttrs;
            }
        }

        private const string _attrFormat = " {0}=\"{1}\"";
        private static string ToAttributesString(IDictionary<string, string> d)
        {
            var attrsb = new StringBuilder();
            foreach (var entry in d)
                attrsb.AppendFormat(_attrFormat, entry.Key, entry.Value);
            return attrsb.ToString();
        }

        /// <summary>
        /// Inspects the given object and returns a sequence of properties existing on the object.
        /// </summary>
        private static IEnumerable<KeyValuePair<string, object>> GetProperties(object o)
        {
            if (o != null)
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(o);
                foreach (PropertyDescriptor prop in props)
                {
                    object val = prop.GetValue(o);
                    if (val != null)
                    {
                        yield return new KeyValuePair<string, object>(prop.Name, val);
                    }
                }
            }
        }

        public static string Version()
        {
            if (_version == null)
            {
                var assembly = System.Reflection.Assembly.GetEntryAssembly();
                if (assembly != null)
                {
                    var version = assembly.GetName().Version;
                    _version = version.ToString();
                }
            }
            return _version ?? "";
        }
        private static string _version = null;

        public static DateTime BuildDate()
        {
            if (_buildDate == null)
            {
                // See: http://stackoverflow.com/questions/1600962/displaying-the-build-date
                var assembly = System.Reflection.Assembly.GetEntryAssembly();
                if (assembly != null && File.Exists(assembly.Location))
                {
                    var buffer = new byte[Math.Max(Marshal.SizeOf(typeof(_IMAGE_FILE_HEADER)), 4)];
                    using (var fileStream = new FileStream(assembly.Location, FileMode.Open, FileAccess.Read))
                    {
                        fileStream.Position = 0x3C;
                        fileStream.Read(buffer, 0, 4);
                        fileStream.Position = BitConverter.ToUInt32(buffer, 0); // COFF header offset
                        fileStream.Read(buffer, 0, 4); // "PE\0\0"
                        fileStream.Read(buffer, 0, buffer.Length);
                    }
                    var pinnedBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    try
                    {
                        var coffHeader = (_IMAGE_FILE_HEADER)Marshal.PtrToStructure(pinnedBuffer.AddrOfPinnedObject(), typeof(_IMAGE_FILE_HEADER));
                        _buildDate = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1) + new TimeSpan(coffHeader.TimeDateStamp * TimeSpan.TicksPerSecond));
                    }
                    finally
                    {
                        pinnedBuffer.Free();
                    }
                }
            }

            return _buildDate ?? DateTime.Now;
        }
        private static DateTime? _buildDate = null;
        private struct _IMAGE_FILE_HEADER
        {
            public ushort Machine;
            public ushort NumberOfSections;
            public uint TimeDateStamp;
            public uint PointerToSymbolTable;
            public uint NumberOfSymbols;
            public ushort SizeOfOptionalHeader;
            public ushort Characteristics;
        };
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Test
{
    public partial class Default : System.Web.UI.Page
    {

        bool showOKFiles = true;
        string tableRow = "<tr><th scope=\"row\">{0}</th><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>";
        string mainDirectory = "C:\\Dev\\Altero\\src\\Assets\\Cutscenes\\";

        int dirCount = 0;


        public string getFiles(string directory)
        {

            StringBuilder SbThisDir = new StringBuilder();

            foreach (var dir in new DirectoryInfo(directory).GetDirectories())
            {
                dirCount++;

                StringBuilder sbDir = new StringBuilder();
                //sbDir.AppendFormat("{0}", dir.FullName);
                sbDir.AppendFormat("<div class=\"collapse\" id=\"collapse"+dirCount+"\"><div class=\"card card-body\"><table class=\"table table-sm table-hover \"><thead><tr><th scope=\"col\">#</th><th scope=\"col\">File </th><th scope=\"col\">Dimension</th><th scope=\"col\">Good Dimension</th><th scope=\"col\">File Size</th></tr></thead><tbody>", "");

                int fileCount = 0;
                int fileNotOkCount = 0;
                foreach (var file in dir.GetFiles())
                {
                    if (file.Extension.ToLower().Contains("png"))
                    {
                        fileCount++;
                        bool fileBad = false;

                        System.Drawing.Image img = System.Drawing.Image.FromFile(file.FullName);

                        int width = img.Width;
                        while (width % 4 != 0) width++;

                        int height = img.Height;
                        while (height % 4 != 0) height++;

                        fileBad = (width != img.Width || height != img.Height);

                        if (fileBad) fileNotOkCount++;

                        if (fileBad || showOKFiles)
                            sbDir.AppendFormat(tableRow, fileCount, file.Name, img.Width + "x" + img.Height, (width == img.Width ? width.ToString() : "<span style=\"color:red; font-weight:bold\">" + width.ToString() + "</span>") + "x" + (height == img.Height ? height.ToString() : "<span style=\"color:red; font-weight:bold\">" + height.ToString() + "</span>"), file.Length.ToString());
                    }
                }
                sbDir.AppendLine("</table></div></div>");

                if (fileCount > 0)
                {


                    SbThisDir.AppendFormat("<p><a class=\"btn btn-primary\" data-toggle=\"collapse\" href=\"#collapse"+dirCount+"\" role=\"button\" aria-expanded=\"false\" aria-controls=\"collapse"+dirCount+"\">{0} -> {1}/{2} files to fix</a></p>", dir.FullName, fileCount.ToString(), fileNotOkCount.ToString(),dirCount);
                    //SbThisDir.AppendFormat("{0} -> {1}/{2} files to fix", dir.FullName, fileCount.ToString(), fileNotOkCount.ToString());
                    SbThisDir.AppendLine(sbDir.ToString());
                }
                //else SbThisDir.AppendFormat("{0} -> NO IMAGE FOUND <br />", dir.FullName);

                SbThisDir.Append(getFiles(dir.FullName));
            }

            return SbThisDir.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string test = getFiles(mainDirectory);
            ltBody.Text = test;
        }
    }
}
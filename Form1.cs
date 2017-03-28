using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        const string htmlPath = @"C:\temp\sortedData\";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count = 0;

            if (!System.IO.Directory.Exists(htmlPath))
            {
                System.IO.Directory.CreateDirectory(htmlPath);
            }
            foreach (string a in System.IO.Directory.GetFiles(htmlPath))
            {
                if (a.EndsWith(".dat"))
                {
                    //if (count == 0)
                    {
                        readfile(a);
                        count++;
                        //break;
                    }
                }
            }

            //generateOnePixel();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new ColorGenerator().generateOnePixel();
        }
        void readfile(string filePath)
        {
            ColorGenerator cg = new ColorGenerator();
            if (System.IO.File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Read the stream to a string, and write the string to the console.
                    string line = sr.ReadToEnd();
                    List<string> lines = line.Split('\n').ToList();


                    //                var data = google.visualization.arrayToDataTable([
                    //          ['Lat', 'Long'],
                    //[31.278732, 121.463296]
                    //        ]);


                    StringBuilder sb = new StringBuilder();
                    string strdt = "2016-01-01";
                    string strcenter = "";
                    int daySeq = 0;
                    string carName = "";
                    for (int i = 0; i < lines.Count() - 1; i++)//
                    {
                        string[] rec = lines[i].Split(',');

                        if (i == 0)
                        {
                            carName = rec[1];
                            strcenter = string.Format("{0}, {1}", rec[80], rec[81]);
                        }

                        if (strdt == "2016-01-01")
                        {
                            strdt = rec[2].Split(' ')[0];
                            daySeq = 0;
                            cg.resetFileSeq();

                            sb.Append(string.Format("[{0}, {1}],\n", rec[80], rec[81]));
                        }
                        else if (strdt == rec[2].Split(' ')[0])
                        {
                            sb.Append(string.Format("[{0}, {1}],\n", rec[80], rec[81]));
                        }
                        else
                        {
                            strdt = rec[2].Split(' ')[0];
                            daySeq++;
                            cg.resetFileSeq();

                            //writeFile(rec[1], rec[2], sb.ToString(), strcenter);
                            //sb.Clear();
                            sb.Append(string.Format("[{0}, {1}],\n", rec[80], rec[81]));
                        }
                        cg.generateCarPixel(i, daySeq, carName);
                    }
                    writeFile(carName, "", sb.ToString(), strcenter);
                    sb.Clear();
                    string[] lastRec = lines[lines.Count - 2].Split(',');
                    if (!string.IsNullOrWhiteSpace(sb.ToString()))
                    {
                        try
                        {
                            writeFile(lastRec[1], lastRec[2], sb.ToString(), strcenter);
                        }
                        catch (Exception ex) { }
                    }
                }
            }
        }

        void writeFile(string strCarNo, string strDate, string strContent, string strcentre)
        {
            string str1 = @"<!DOCTYPE html>
<html> 
<head> 
  <meta http-equiv=""content-type"" content=""text/html; charset=UTF-8"" /> 
  <title>Google Maps Multiple Markers</title> 
  <script src=""http://maps.google.com/maps/api/js?sensor=false"" 
          type=""text/javascript""></script>
</head> 
<body>
  <div id=""map"" style=""width: 800px; height: 600px;""></div>

  <script type=""text/javascript"">
    var locations = [
";

            string str2 = @"
];

    var map = new google.maps.Map(document.getElementById('map')
, {
      zoom: 10,
      center: new google.maps.LatLng(";


            string str3 = @"),
      mapTypeId: google.maps.MapTypeId.ROADMAP,
	useMapTypeControl: true
    }
);

    var infowindow = new google.maps.InfoWindow();

    var marker, i;

    for (i = 0; i < locations.length; i++) {  
      marker = new google.maps.Marker({
        position: new google.maps.LatLng(locations[i][0], locations[i][1]),
        map: map,
        //icon: 'https://maps.gstatic.com/intl/en_us/mapfiles/markers2/measle_blue.png'
        icon: 'color/' + i + '.png'
      });

      google.maps.event.addListener(marker, 'click', (function(marker, i) {
        return function() {
          //infowindow.setContent(locations[i][0]);
          infowindow.open(map, marker);
        }
      })(marker, i));
    }
  </script>
</body>
</html>";
            if (!System.IO.Directory.Exists(@"C:\temp\cardata\"))
            {
                System.IO.Directory.CreateDirectory(@"C:\temp\cardata\");
            }
            if (!string.IsNullOrEmpty(strContent))
                System.IO.File.WriteAllText(string.Format(@"C:\temp\cardata\{0}\{0}_{1}.html", strCarNo, strDate.Split(' ')[0].Replace("-", "_")), str1 + strContent.Substring(0, strContent.Length - 2) + str2 + "31.2206928,121.4872501" + str3);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int count = 0;

            if (!System.IO.Directory.Exists(htmlPath))
            {
                System.IO.Directory.CreateDirectory(htmlPath);
            }
            foreach (string a in System.IO.Directory.GetFiles(htmlPath))
            {
                if (a.EndsWith(".dat"))
                {
                    //if (count == 0)
                    {
                        readandplotImage(a);
                        count++;
                        //break;
                    }
                }
            }

        }
        void readandplotImage(string filePath)
        {
            string carName = (new DirectoryInfo(filePath)).Name.Split('.')[0];

            if (!System.IO.File.Exists(string.Format(@"c:\\temp\cardata\{0}.png", carName)))
            {
                ColorGenerator cg = new ColorGenerator();
                if (System.IO.File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        // Read the stream to a string, and write the string to the console.
                        string line = sr.ReadToEnd();
                        List<string> lines = line.Split('\n').ToList();

                        int width = 800;
                        int height = 600;
                        StringBuilder sb = new StringBuilder();
                        string strdt = "2016-01-01";
                        int daySeq = -1;
                        List<float> xli = new List<float>();
                        List<float> yli = new List<float>();
                        List<string> dtli = new List<string>();

                        for (int i = 0; i < lines.Count(); i++)
                        {
                            if (!string.IsNullOrEmpty(lines[i]))
                            {
                                string[] rec = lines[i].Split(',');
                                float x = float.Parse(rec[81]);
                                float y = float.Parse(rec[80]);
                                if (x >= 70 && x <= 150 && y >= 10 && y <= 60)
                                {
                                    xli.Add(x);
                                    yli.Add(y);
                                    dtli.Add(rec[2].Split(' ')[0]);
                                }
                            }
                        }
                        float minx = xli.Min();
                        float maxx = xli.Max();
                        float miny = yli.Min();
                        float maxy = yli.Max();
                        float diffx = maxx - minx;
                        float diffy = maxy - miny;

                        float xscale = width / diffx;
                        float yscale = height / diffy;
                        float scale = Math.Min(xscale, yscale);

                        int countInaDay = 0;
                        int seqInaDay = -1;
                        Bitmap bmp = new Bitmap(width, height);
                        Color crbg = Color.Black;
                        using (Graphics graphics = Graphics.FromImage(bmp))
                        {
                            graphics.FillRectangle(new SolidBrush(crbg), 0, 0, bmp.Width, bmp.Height);
                            for (int i = 0; i < xli.Count; i++)
                            {
                                if (strdt == dtli[i])
                                {
                                    seqInaDay++;
                                }
                                else
                                {
                                    strdt = dtli[i];
                                    countInaDay = (from a in dtli where a == strdt select a).ToList().Count;
                                    daySeq++;
                                    seqInaDay = 0;
                                }
                                Color cr1 = cg.GenerateColor(countInaDay, seqInaDay, daySeq);

                                int xpos = Convert.ToInt32((xli[i] - minx) * scale);
                                int ypos = Convert.ToInt32((yli[i] - miny) * scale);
                                //if (daySeq == 1)
                                {
                                    if (seqInaDay == 0)
                                    {
                                        var seed = Convert.ToInt32(Regex.Match(Guid.NewGuid().ToString(), @"\d+").Value);
                                        int bias = new Random(seed).Next(40) - 20;
                                        Font font = new Font("Arial", 10);
                                        Color crfont = Color.White;
                                        graphics.FillRectangle(new SolidBrush(cr1), xpos + bias, ypos + bias, 20, 20);
                                        graphics.DrawString(daySeq.ToString(), font, new SolidBrush(crfont), xpos + bias, ypos + bias);
                                    }
                                    else
                                    {
                                        graphics.FillRectangle(new SolidBrush(cr1), xpos, ypos, 2, 2);
                                    }
                                }


                            }
                            graphics.Flush();
                            graphics.Dispose();
                            bmp.Save(string.Format(@"c:\\temp\cardata\{0}.png", carName), System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }
                }
            }
        }
    }
}
using System.IO;
using System;

namespace GCode2xml
{
    class readGcode
    {
        public static string ParseGcodeFile(ref Ply[] plys, string filename)
        {
            try
            {
                int countPly = -1;
                int prevCountPly = 0;
                Point[] points = new Point[0];
                
                // open GCode file for read access
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);

                bool endHeader = false; // signifies that the header information has ended and a rapid positioining (G0) or start of feed cutting (G1) command has been found
                decimal Xcoord_Prev = 0;
                decimal Ycoord_Prev = 0;

                while (sr.EndOfStream != true)
                {
                    string linetext = sr.ReadLine(); // read each line from the GCode file
                    string[] splitChar = { " " };
                    string[] coords = linetext.ToString().Split(splitChar, StringSplitOptions.RemoveEmptyEntries); // split the line text into coordinate elements, separated by spaces

                    bool readCoords = true; // reset with each line in case a comment is found

                    if (linetext.StartsWith("(") == false) // skip comment lines (i.e. lines with bracket character)
                    {
                        // loop through the gcodes in each line
                        foreach (string Gcode in coords)
                        {
                            if (plys.Length == 0 && Gcode == "G91") // if no points have been read and G91 code is found (i.e. relative postioining) return error
                            { return "GCode file must use absolute positioning"; } // skip return to home position

                            if (Gcode == "G0" || Gcode.StartsWith("M") == true)
                            {
                                countPly += 1;

                                if (countPly > prevCountPly)
                                {
                                    Ply ply = new Ply();
                                    ply.NAME = "L" + countPly + "-Ply" + countPly;
                                    ply.POINT = points;
                                    Array.Resize(ref plys, countPly + 1);
                                    plys[countPly - 1] = ply;

                                    if (Gcode.StartsWith("M") == true) { return ""; } // exit without error message when an "M" code is found (i.e. successfully parsed all coordinate data)

                                    prevCountPly = countPly;
                                }

                                points = new Point[0];
                            }

                            if (Gcode == "G0" || Gcode == "G1") { endHeader = true; }  // rapid positioining (G0) or the start of the feed cutting (G1)
                        }

                        if (endHeader == true && readCoords == true)
                        {
                            bool bXVal = false;
                            bool bYVal = false;

                            Point point = new Point(); // create new Point object

                            point.NUMBER = points.Length; // assign point ID number
                            point.NAME = "";

                            foreach (string Gcode in coords)
                            {
                                if (Gcode.ToString().StartsWith("X") == true)
                                {
                                    bXVal = true;
                                    decimal XcoordVal = Convert.ToDecimal(Gcode.Substring(1, Gcode.ToString().Length - 1));
                                    point.XVAL = XcoordVal;
                                    Xcoord_Prev = XcoordVal;
                                }
                                else if (Gcode.ToString().StartsWith("Y") == true)
                                {
                                    bYVal = true;
                                    decimal YcoordVal = Convert.ToDecimal(Gcode.Substring(1, Gcode.ToString().Length - 1));
                                    point.YVAL = YcoordVal;
                                    Ycoord_Prev = YcoordVal;
                                }
                            }

                            // if the line of GCode contains only one coordinate, assign the previous value for the other coordinate
                            if (bXVal == false)
                            {
                                point.XVAL = Xcoord_Prev;
                            }
                            if (bYVal == false)
                            {
                                point.YVAL = Ycoord_Prev;
                            }

                            Array.Resize(ref points, points.Length + 1); // re-size the points array, increasing the count by 1
                            points[points.Length - 1] = point; // add the X, Y coordinate data from the next point to the point array     
                        }
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}

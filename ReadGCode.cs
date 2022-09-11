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
                int countPly = 0;
                int prevCountPly = 0;
                Point[] points = new Point[0];

                decimal Xcoord_Prev = 0;
                decimal Ycoord_Prev = 0;

                // open GCode file for read access
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);

                while (sr.EndOfStream != true)
                {
                    string linetext = sr.ReadLine(); // read each line from the GCode file
                    string[] splitChar = { " " };
                    string[] coords = linetext.ToString().Split(splitChar, StringSplitOptions.RemoveEmptyEntries); // split the line text into coordinate elements, separated by spaces

                    bool readCoords = true; // reset with each line in case a comment is found

                    if (linetext.StartsWith("(") == false) // skip comment lines (i.e. lines with bracket character)
                    {
                        foreach (string Gcode in coords) // loop through the gcodes in each line
                        {
                            // if no coordinate points have been read and G91 code is found (i.e. relative postioining) return error
                            if (plys.Length == 0 && Gcode == "G91") 
                            { return "GCode file must use absolute positioning"; }

                            if (Gcode == "G0" || Gcode.StartsWith("M") == true) // for each new G0 (rapid) or 'M' code (tool change)
                            {
                                // create new PLY layer after each G0 (or M) gcode following the first set of coordinates
                                if (countPly > prevCountPly)  
                                {
                                    Ply ply = new Ply(); // create a new ply object
                                    ply.NAME = "L" + countPly + "-Ply" + countPly; // assign the tag of the form L#-Name (where #'s are sequential)
                                    ply.POINT = points;
                                    Array.Resize(ref plys, countPly + 1); // re-size the plys array, increasing the count by 1
                                    plys[countPly - 1] = ply; // add the ply (containing the group of points) the ply array

                                    if (Gcode.StartsWith("M") == true) { return ""; } // exit without error message when an "M" code is found (i.e. a tool change which is assumed to be the end of the coordinate data, if the GCode was created with a single tool)

                                    prevCountPly = countPly;
                                }

                                countPly += 1;
                                points = new Point[0]; // reset the array of Point data
                            }
                        }

                        if (readCoords == true)
                        {
                            bool bXVal = false;
                            bool bYVal = false;

                            Point point = new Point(); // create new Point object

                            point.NUMBER = points.Length; // assign point ID number
                            point.NAME = "";

                            foreach (string Gcode in coords) // loop through the gcodes in each line
                            {
                                if (Gcode.StartsWith("X") == true)
                                {                                   
                                    // strip off the X character and convert the coordinate to decimal
                                    decimal XcoordVal = Convert.ToDecimal(Gcode.Substring(1, Gcode.ToString().Length - 1));

                                    bXVal = true;
                                    point.XVAL = XcoordVal;
                                    Xcoord_Prev = XcoordVal;
                                }
                                else if (Gcode.StartsWith("Y") == true)
                                {
                                    // strip off the Y character and convert the coordinate to decimal
                                    decimal YcoordVal = Convert.ToDecimal(Gcode.Substring(1, Gcode.ToString().Length - 1));
                                    
                                    bYVal = true;
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

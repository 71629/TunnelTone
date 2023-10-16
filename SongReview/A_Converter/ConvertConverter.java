import java.io.*;
import java.util.Scanner;
import java.util.Random;
import java.lang.Math;
class ConvertConverter {
	public static void main (String args[]) throws Exception{
		Scanner input = new Scanner(System.in);
		File file = new File(
		 	"..\\A_Converter\\2.aff");
		BufferedReader br = new BufferedReader(new FileReader(file));
		int count = 0; int counttt = 0;
		String st;
		String arcSave[] = new String [9999];
		String arcTapSave[] = new String [9999];
		String narcSave[] = new String [9999];
		String carcSave[] = new String [9999];
		//System.out.print("Remember change the location in line 9, 70 and 74 of this program to read and write the file!!!");
		//System.out.println();
		//Read per line until end
		while ((st = br.readLine()) != null) {
			if(counttt >=3) {
				arcSave[count] = st;
				count++;
			} else {counttt++;}
		}
		for(int i=0; i<arcSave.length; i++) {
			if(arcSave[i]!=null) {
				//System.out.println(arcSave[i]);
				for(int j=0; j<= arcSave[i].length(); j++) {
					if(arcSave[i].substring(j,j+1).equals(")")) {
						arcTapSave[i] = arcSave[i].substring(0,j+1) + ";";
						//System.out.println(arcTapSave[i]);
						break;
					}
				}
			}
		}
		for(int i=0; i<arcSave.length; i++) {
			if(arcSave[i]!=null) {
				for(int j=0; j<= arcSave[i].length(); j++) {
					if(arcSave[i].substring(j,j+1).equals("[")) {
						narcSave[i] = arcSave[i].substring(j+1,arcSave[i].length() - 2) + ";";
						//System.out.println(narcSave[i]);
						break;
					}
				}
			}
		}
		for(int i=0; i<narcSave.length; i++) {
			if(narcSave[i]!=null) {
				narcSave[i] = narcSave[i].replace(",",";");
				//System.out.println(narcSave[i]);
			}
		}
		for(int i=0; i<narcSave.length; i++) {
			if(narcSave[i]!=null) {
				arcSave[i] = arcTapSave[i] + narcSave[i];
			}
		}
		File files = new File("..\\A_Converter\\Program.cs");
		System.out.println("");
		System.out.println("ConvertEnd.txt has been create in Desktop!");
		System.out.println("");
		FileWriter fw = new FileWriter("..\\A_Converter\\Converter\\Converter\\Program.cs");
		System.out.println("Writing Program.cs...");

		fw.write("using System.Diagnostics;" + "\r\n");
		fw.write("using System.Numerics;" + "\r\n");
		fw.write("using System.Text.Json;" + "\r\n");
		fw.write("using System.Text.Json.Serialization;" + "\r\n");
		fw.write("\r\n");
		fw.write("namespace DefaultNamespace" + "\r\n");
		fw.write("{" + "\r\n");
		fw.write("    public class Converter" + "\r\n");
		fw.write("    {" + "\r\n");
		fw.write("        public const int s = 0;" + "\r\n");
		fw.write("        public const int si = 1;" + "\r\n");
		fw.write("        public const int sisi = 1;" + "\r\n");
		fw.write("        public const int so = 2;" + "\r\n");
		fw.write("        public const int soso = 2;" + "\r\n");
		fw.write("        public const int sosi = 4;" + "\r\n");
		fw.write("        public const int b = 5;" + "\r\n");
		fw.write("\r\n");
		fw.write("        public const int none = 10;" + "\r\n");
		fw.write("\r\n");
		fw.write("        public static List<Trail> trails = new();" + "\r\n");
		fw.write("        public static Chart chart = new()" + "\r\n");
		fw.write("        {" + "\r\n");
		fw.write("            trails = Array.Empty<Trail>()" + "\r\n");
		fw.write("        };" + "\r\n");
		fw.write("\r\n");
		fw.write("        public static void Main()" + "\r\n");
		fw.write("        {" + "\r\n");
		fw.write("            // Insert .aff (.lua)" + "\r\n");
		counttt = 0;
		do {
			fw.write(arcSave[counttt] + "\r\n");
			System.out.println("Write Line" + counttt + ": " + arcSave[counttt]);
			counttt++;
		}while(arcSave[counttt] != null);

		fw.write("\r\n");
		fw.write("            // Convert" + "\r\n");
		fw.write("            chart.trails = trails.ToArray();" + "\r\n");
		fw.write("            var ret = JsonSerializer.Serialize(chart, new JsonSerializerOptions{WriteIndented = true});" + "\r\n");
		fw.write("\r\n");
		fw.write("            // Output the result to a new json file" + "\r\n");
		fw.write("            File.WriteAllText(@" + "\"D:\\Prismatic.json\"" +", ret);" + "\r\n");
		fw.write("\r\n");
		fw.write("            //Open explorer" + "\r\n");
		fw.write("            Process.Start(" + "\"explorer.exe\"" + ", @" +"\"D:\\\");" + "\r\n");
		fw.write("        }" + "\r\n");
		fw.write("\r\n");
		fw.write("        private static void arc(int startTime, int endTime, double startX, double endX, int easingMode, double startY, double endY, int color, int fx, bool virtualArc)" + "\r\n");
		fw.write("        {" + "\r\n");
		fw.write("            var trail = new Trail" + "\r\n");
		fw.write("            {" + "\r\n");
		fw.write("                startTime = startTime," + "\r\n");
		fw.write("                endTime = endTime," + "\r\n");
		fw.write("                startX = startX," + "\r\n");
		fw.write("                startY = startY," + "\r\n");
		fw.write("                endX = endX," + "\r\n");
		fw.write("                endY = endY," + "\r\n");
		fw.write("                easing = easingMode," + "\r\n");
		fw.write("                easingRatio = 0.65f," + "\r\n");
		fw.write("                color = color," + "\r\n");
		fw.write("                virtualTrail = virtualArc," + "\r\n");
		fw.write("                taps = new List<Tap>()" + "\r\n");
		fw.write("            };" + "\r\n");
		fw.write("\r\n");
		fw.write("            trails.Add(trail);" + "\r\n");
		fw.write("        }" + "\r\n");
		fw.write("\r\n");
		fw.write("        private static void arctap(int time)" + "\r\n");
		fw.write("        {" + "\r\n");
		fw.write("            var tap = new Tap" + "\r\n");
		fw.write("            {" + "\r\n");
		fw.write("                time = time" + "\r\n");
		fw.write("            };" + "\r\n");
		fw.write("\r\n");
		fw.write("            trails.Last().taps.Add(tap);" + "\r\n");
		fw.write("        }" + "\r\n");
		fw.write("    }" + "\r\n");
		fw.write("\r\n");
		fw.write("    public class Chart" + "\r\n");
		fw.write("    {" + "\r\n");
		fw.write("        public Trail[] trails { get; set; }" + "\r\n");
		fw.write("    }" + "\r\n");
		fw.write("\r\n");
		fw.write("    public class Trail" + "\r\n");
		fw.write("    {" + "\r\n");
		fw.write("        public double startX { get; set; }     " + "\r\n");
		fw.write("        public double startY { get; set; }" + "\r\n");
		fw.write("\r\n");
		fw.write("        public double endX { get; set; }" + "\r\n");
		fw.write("        public double endY { get; set; }" + "\r\n");
		fw.write("        public int startTime { get; set; }" + "\r\n");
		fw.write("        public int endTime { get; set; }" + "\r\n");
		fw.write("        public int color { get; set; }" + "\r\n");
		fw.write("        public int easing { get; set; }" + "\r\n");
		fw.write("        public float easingRatio { get; set; }" + "\r\n");
		fw.write("        public bool virtualTrail { get; set; }" + "\r\n");
		fw.write("\r\n");
		fw.write("        public List<Tap> taps { get; set; }" + "\r\n");
		fw.write("    }" + "\r\n");
		fw.write("\r\n");
		fw.write("    public class Tap" + "\r\n");
		fw.write("    {" + "\r\n");
		fw.write("        public int time { get; set; }" + "\r\n");
		fw.write("    }" + "\r\n");
		fw.write("}" + "\r\n");
		fw.close();
		System.out.println("Writing Success!");
		System.out.println("Please open TunnelTone\\SongReview\\A_Converter\\Converter.sin to finish convert.");
		//System.out.print("Remember change the file location in line 9, 70 and 74 of this program to read and write the file!!!");
	}
}
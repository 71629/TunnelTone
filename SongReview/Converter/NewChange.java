import java.io.*;
import java.util.Scanner;
import java.util.Random;
import java.lang.Math;
public class NewChange {
	public static void main(String[] args) throws Exception
	{
		Scanner input = new Scanner(System.in);
		File file = new File(
		 	"C:\\Users\\jacky\\Desktop\\Sample.osu");
		BufferedReader br = new BufferedReader(new FileReader(file));
		
		int loop = 87; int checkloop = 0; int count = 0; int counttt = 0; int doublecount = 0; int saveI = 0;

		float rand = 0; float looptemp = 0;

		String save = ""; String tempsave = ""; String st; String temp; String storetemp = "s";
		String arcSave[] = new String [9999];
		
		boolean trigger = true;
			
		System.out.print("Remember change the location in line 10, 144 and 148 of this program to read and write the file!!!");
		System.out.println();
		loop = CheckLooping(br);
		//Read per line until end
		while ((st = br.readLine()) != null) {

			try {	
				arcSave[count] = st.substring(0, 15);
				//System.out.println("Substring" + 15); //Normal
			}
			catch(Exception a) {
				try {
					arcSave[count] = st.substring(0, 14);
					System.out.println("Substring : " + 14 + " Line: " + (count + 1));
				}
				catch (Exception b) {
					try {
						arcSave[count] = st.substring(0, 13);
						System.out.println("Substring : " + 13 + " Line: " + (count + 1));
					}
					catch (Exception c) {
						try {
							arcSave[count] = st.substring(0, 12);
							System.out.println("Substring : " + 12 + " Line: " + (count + 1));	
						}
						catch (Exception d) {
							try {
								arcSave[count] = st.substring(0, 11);
								System.out.println("Substring : " + 11 + " Line: " + (count + 1));
							}
							catch (Exception e) {
								arcSave[count] = st.substring(0, 10);
								System.out.println("Substring : " + 10 + " Line: " + (count + 1));
							}
							
						}
						
					}
					
				}
			}
			
			//System.out.println(arcSave[count]);
			count++;
		}
		for(int f=0; f<2; f++) {
			for(int i=0; i<arcSave.length; i++) {
				if(arcSave[i] !=null){
					if(arcSave[i].substring(0,1).equals(",")){
						arcSave[i] = arcSave[i].substring(1,arcSave[i].length());
					} else {
						if(arcSave[i].substring(1,2).equals(",")) {
							arcSave[i] = arcSave[i].substring(2,arcSave[i].length());
						} else {
							if(arcSave[i].substring(2,3).equals(",")) {
							arcSave[i] = arcSave[i].substring(3,arcSave[i].length());
							} else {
								if(arcSave[i].substring(3,4).equals(",")) {
									arcSave[i] = arcSave[i].substring(4,arcSave[i].length());
								} else {
									break;
								}
							}
						}
					}
				//System.out.println(arcSave[i]); (Remove first,)
				}
			}
		}
		for(int i=0; i<arcSave.length; i++) {
			if(arcSave[i] != null){
			if(arcSave[i].substring(3,4).equals(",")){
				arcSave[i] = arcSave[i].substring(0,3);
			} else {
				if(arcSave[i].substring(4,5).equals(",")) {
					arcSave[i] = arcSave[i].substring(0,4);
				} else {
					if(arcSave[i].substring(5,6).equals(",")) {
						arcSave[i] = arcSave[i].substring(0,5);
					} else {
						if(arcSave[i].substring(6,7).equals(",")) {
						arcSave[i] = arcSave[i].substring(0,6);
						}
					}
				}
			}
			//System.out.println(arcSave[i]); (It should be number only)
			}
		}
		//for(int i=count;i<arcSave.length; i++) {
		//	arcSave[count] = "0";
		//	count++;
		//}
		counttt = 0;
		System.out.println("AudioOffset:0");
		System.out.println("-");
		System.out.println("timing(0,100.00,4.00)");
		String anotherArcSave[] = new String[9999];
		int anotherCount = 0;
		int arraySaveLengthFirst;
		int arraySaveLengthLast;

		for(int i=0; i<arcSave.length; i++) {
			if(RandomTF() == true || counttt == 0) {
				counttt++;
			} else {
				if(arcSave[i] != null) {
					anotherArcSave[anotherCount] = "arc("+ arcSave[i-counttt] + "," + arcSave[i] + "," + leftRightStart() + "," + leftRightEnd() + "," + Type() + "," + upDownStart() + "," + upDownEnd() + ",0,none,true)[";			
					for(int j = i - counttt; j<=i; j++) {
						if(j == i) {
							anotherArcSave[anotherCount] = anotherArcSave[anotherCount] + "arctap(" + arcSave[j] + ")];";
						} else {
							anotherArcSave[anotherCount] = anotherArcSave[anotherCount] +"arctap(" + arcSave[j] + "),";
						}			
					}
					counttt = 0;
					//System.out.println(anotherArcSave[anotherCount]); //Print a full type of arc
					anotherCount++;		
				}	
			}
		}
		//File Write in to .aff
		File files = new File("C:\\Users\\jacky\\Desktop\\2.aff");
		System.out.println("");
		System.out.println("0.aff has been create in Desktop!");
		System.out.println("");
		FileWriter fw = new FileWriter("C:\\Users\\jacky\\Desktop\\2.aff");
		System.out.println("Writing 2.aff...");
		fw.write("AudioOffset:0" + "\r\n");
		fw.write("-" + "\r\n");
		fw.write("timing(0,166.00,0.00);" + "\r\n");
		counttt = 0;
		do {
			fw.write(anotherArcSave[counttt] + "\r\n");
			System.out.println("Write Line" + counttt + ": " + anotherArcSave[counttt]);
			counttt++;
		}while(anotherArcSave[counttt] != null);
		fw.close();
		System.out.println("Writing Success!");
		System.out.print("Remember change the file location in line 10 and 112 of this program to read and write the file!!!");
	}	
	public static float upDownStart() {
		float input = 0;
		float set = (float)1.5;
		float f = (float)1.5;
		do {
			input = (float)Math.random() * (float)1.5;
			f = (float)Math.random() * (float)(-0.5);
			input = input - f;
		}while (input <= -0.15 || input >= 0.95 );
		String temp = Float.toString(input);
		input = Float.parseFloat(temp.substring(0, 4));
		return input;
	}
	public static float upDownEnd() {
		float input = 0;
		float set = (float)1.5;
		float f = (float)1.5;
		do {
			input = (float)Math.random() * (float)1.5;
			f = (float)Math.random() * (float)(-0.5);
			input = input - f;
		}while (input <= -0.15 || input >= 0.95 );
		String temp = Float.toString(input);
		input = Float.parseFloat(temp.substring(0, 4));
		return input;
	}
	public static float leftRightStart() {
		float input = 0;
		float set = (float)1.5;
		float f = (float)1.5;
		do {
			input = (float)Math.random() * (float)1.5;
			f = (float)Math.random() * (float)(-0.5);
			input = input - f;
		}while (input <= -0.45 || input >= 1.45 || ((input < 0.400) && (input > 0.600)));
		String temp = Float.toString(input);
		input = Float.parseFloat(temp.substring(0, 4));
		return input;
	}
	public static float leftRightEnd() {
		float input = 0;
		float set = (float)1.5;
		float f = (float)1.5;
		do {
			input = (float)Math.random() * (float)1.5;
			f = (float)Math.random() * (float)(-0.5);
			input = input - f;
		}while (input <= -0.45 || input >= 1.45 || ((input < 0.400) && (input > 0.600)));
		String temp = Float.toString(input);
		input = Float.parseFloat(temp.substring(0, 4));
		return input;
	}
	public static boolean RandomTF() {
		boolean tf = false;
		float random = (float)Math.random() * 10;
		if(random >5) { return true; } 
		else { return false;}
	}
	public static int CheckLooping(BufferedReader bbr ) throws Exception {
		int countss = 1;
		String sst;
		while ((sst = bbr.readLine()) != null)
			if(sst.equals("[HitObjects]")) {
				//System.out.println("File Start Line" + countss);
				return countss;
			} else { countss++; }
		return countss;
	}
	public static String Type() { //Return a random string type.
		float input = new Random().nextFloat(1);
		if(input < 0.125) { return "b"; }
		if(input < 0.25) { return "s"; }
		if(input < 0.375) { return "si"; }
		if(input < 0.5) { return "so"; }
		if(input < 0.625) { return "sisi"; }
		if(input < 0.75) { return "soso"; }
		if(input < 0.875) { return "sosi"; }
		if(input > 0.875) { return "s"; }
		return "s";
	}
}
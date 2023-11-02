import java.io.*;
import java.util.Scanner;
import java.util.Random;
import java.lang.Math;
public class NewChange {
	public static void main(String[] args) throws Exception
	{
		Scanner input = new Scanner(System.in);
		File file = new File(
		 	"..\\A_Converter\\Sample.osu");
		BufferedReader br = new BufferedReader(new FileReader(file));
		int count = 0; int counttt = 15; int anotherCount = 0; int checkOneButton; int checkOneButtonTemp; int loop = CheckLooping(br);
		String leftRightStartOne; String upDownStartOne; String st;
		String arcSave[] = new String [9999]; String anotherArcSave[] = new String[9999];
		boolean trigger = true; boolean exceptionNull = false;
		while ((st = br.readLine()) != null) { //Read per line until end
			do {
				try{
					arcSave[count] = st.substring(0, counttt); //If 
					//System.out.println("Substring" + counttt); //Normal
					exceptionNull = false;
					counttt = 15;
					break;
					} 
				catch (Exception a) {
					counttt--;
					exceptionNull = true;
				}
			}while(exceptionNull != false);
			count++;
			//System.out.println(arcSave[count]); //It should be cut it into the array
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
					} //System.out.println(arcSave[i]); (Remove first,)
				}
			}
		}
		for(int i=0; i<arcSave.length; i++) {
			if(arcSave[i] != null){
				try{
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
				}
				catch (Exception e) { continue; } //System.out.println(arcSave[i]); //(It should be number only)
			}
		}
		counttt = 0;
		for(int i=0; i<arcSave.length; i++) {
			if(RandomTF() == true || counttt == 0) {
				counttt++;
			} else {
				if(arcSave[i] != null) {

					checkOneButton = Integer.valueOf(arcSave[i]);
					checkOneButtonTemp = Integer.valueOf(arcSave[i-counttt]);
					leftRightStartOne = Float.toString(leftRightAxis());
					upDownStartOne = Float.toString(upDownAxis());
					if(checkOneButton == checkOneButtonTemp) {
						checkOneButton++;
						anotherArcSave[anotherCount] = "arc("+ arcSave[i-counttt] + "," + checkOneButton + "," + leftRightStartOne + "," + leftRightStartOne + "," + Type() + "," + upDownStartOne + "," + upDownStartOne + ",0,none,true)[";
					} else { anotherArcSave[anotherCount] = "arc("+ arcSave[i-counttt] + "," + arcSave[i] + "," + leftRightAxis() + "," + leftRightAxis() + "," + Type() + "," + upDownAxis() + "," + upDownAxis() + ",0,none,true)["; }
					//anotherArcSave[anotherCount] = "arc("+ arcSave[i-counttt] + "," + arcSave[i] + "," + leftRightStart() + "," + leftRightEnd() + "," + Type() + "," + upDownStart() + "," + upDownEnd() + ",0,none,true)[";
					for(int j = i - counttt; j<=i; j++) {
						if(j == i) { anotherArcSave[anotherCount] = anotherArcSave[anotherCount] + "arctap(" + arcSave[j] + ")];";}
						else { anotherArcSave[anotherCount] = anotherArcSave[anotherCount] +"arctap(" + arcSave[j] + "),"; }			
					}
					//System.out.println(anotherArcSave[anotherCount]); //Print a full type of arc
					counttt = 0;
					anotherCount++;		
				}	
			}
		}
		File files = new File("..\\A_Converter\\2.aff"); //File Write in to .aff
		System.out.println("");
		System.out.println("2.aff has been create in TunnelTone\\SongReview\\A_Converter !");
		System.out.println("");
		FileWriter fw = new FileWriter("..\\A_Converter\\2.aff");
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
	}	
	public static float upDownAxis() {
		float input = 0;
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
	public static float leftRightAxis() {
		float input = 0;
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
		int random = new Random().nextInt(2);
		if(random == 1) { return true; } 
		return false;
	}
	public static int CheckLooping(BufferedReader bbr ) throws Exception {
		int countss = 1;
		String sst;
		while ((sst = bbr.readLine()) != null)
			if(sst.equals("[HitObjects]")) {
				//System.out.println("File Start Line" + countss); //It should be print the first note timeline.
				return countss;
			} else { countss++; }
		return countss;
	}
	public static String Type() { //Return a random string type.
		int random = new Random().nextInt(7);
		String[] type = new String[7];
		type[0] = "b";
		type[1] = "s";
		type[2] = "si";
		type[3] = "so";
		type[4] = "sisi";
		type[5] = "soso";
		type[6] = "sosi";
		return type[random];
	}
}
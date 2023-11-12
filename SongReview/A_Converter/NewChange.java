import java.io.*;
import java.util.Scanner;
import java.util.Random;
import java.lang.Math;
public class NewChange {
	public static void main(String[] args) throws Exception
	{
		Scanner input = new Scanner(System.in);
		File file = new File("..\\A_Converter\\Sample.osu");
		BufferedReader br = new BufferedReader(new FileReader(file));
		short count = 0; short counttt = 15; int anotherCount = 0; int checkOneButton; int checkOneButtonTemp; int loop = CheckLooping(br);
		String leftRightStartOne; String upDownStartOne; String st;
		String arcTapSave[] = new String [9999]; String anotherArcSave[] = new String[9999];
		boolean trigger = true; boolean exceptionNull = false;
		String[] type = new String[7];
		type[0] = "b"; type[1] = "s"; type[2] = "si"; type[3] = "so"; type[4] = "sisi"; type[5] = "soso"; type[6] = "sosi";
		while ((st = br.readLine()) != null) { //Read per line until end
			do {
				try{
					arcTapSave[count] = st.substring(0, counttt); //If 
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
			//System.out.println(arcTapSave[count]); //It should be cut it into the array
		}

		counttt = 1;
		count = 0;
		do {
			try{
				if(arcTapSave[count].substring((counttt-1),counttt).equals(",")) {
					arcTapSave[count] = arcTapSave[count].substring(counttt,arcTapSave[count].length());
					//System.out.println(arcTapSave[count]); //Remove first,
					count++;
					counttt = 1;
				} else { counttt++; }
			}catch (Exception b) {
				count++;
				counttt = 1;
			}
		}while(arcTapSave[count] != null);

		counttt = 1;
		count = 0;
		do {
			try{
				if(arcTapSave[count].substring((counttt-1),counttt).equals(",")) {
					arcTapSave[count] = arcTapSave[count].substring(counttt,arcTapSave[count].length());
					//System.out.println(arcTapSave[count]); //Remove Second,
					count++;
					counttt = 1;
				} else { counttt++; }
			}catch (Exception b) {
				count++;
				counttt = 1;
			}
		}while(arcTapSave[count] != null);

		counttt = 0;
		count = 0;
		do {
			try{
				if(arcTapSave[count].substring(counttt,(counttt+1)).equals(",")) {
					arcTapSave[count] = arcTapSave[count].substring(0,counttt);
					count++;
					counttt = 0;
					//System.out.println(arcTapSave[count]); //It should be number only
				} else { counttt++; }
			}catch (Exception e) {
				count++;
				counttt = 0;
			}
		}while(arcTapSave[count] != null);

		counttt = 0;
		for(int i=0; i<arcTapSave.length; i++) { //It will progress one arc then progress another arc. So it will not generate a lot of arc in one time
			if(IsArcEndByRandom() == true || counttt == 0) {
				counttt++;
			} else {
				if(arcTapSave[i] != null) {
					checkOneButton = Integer.valueOf(arcTapSave[i]);
					checkOneButtonTemp = Integer.valueOf(arcTapSave[i-counttt]);
					leftRightStartOne = Float.toString(leftRightAxis());
					upDownStartOne = Float.toString(upDownAxis());
					if(checkOneButton == checkOneButtonTemp) {
						checkOneButton++;
						anotherArcSave[anotherCount] = "arc("+ arcTapSave[i-counttt] + "," + checkOneButton + "," + leftRightStartOne + "," + leftRightStartOne + "," + type[new Random().nextInt(7)] + "," + upDownStartOne + "," + upDownStartOne + ",0,none,true)[";
					} else { anotherArcSave[anotherCount] = "arc("+ arcTapSave[i-counttt] + "," + arcTapSave[i] + "," + leftRightAxis() + "," + leftRightAxis() + "," + type[new Random().nextInt(7)] + "," + upDownAxis() + "," + upDownAxis() + ",0,none,true)["; }
					//anotherArcSave[anotherCount] = "arc("+ arcTapSave[i-counttt] + "," + arcTapSave[i] + "," + leftRightStart() + "," + leftRightEnd() + "," + Type() + "," + upDownStart() + "," + upDownEnd() + ",0,none,true)[";
					for(int j = i - counttt; j<=i; j++) {
						if(j == i) { anotherArcSave[anotherCount] = anotherArcSave[anotherCount] + "arctap(" + arcTapSave[j] + ")];";}
						else { anotherArcSave[anotherCount] = anotherArcSave[anotherCount] +"arctap(" + arcTapSave[j] + "),"; }			
					}
					//System.out.println(anotherArcSave[anotherCount]); //Print a full type of arc
					counttt = 0;
					anotherCount++;		
				}	
			}
		}
		File files = new File("..\\A_Converter\\2.aff"); //File Write in to 2.aff under this java program folder.
		System.out.println("2.aff has been create in TunnelTone\\SongReview\\A_Converter !");
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
		return input; //return -0.15 to 0.95 random number
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
		return input; //return -0.45 to 1.45 random number
	}
	public static boolean IsArcEndByRandom() {
		boolean bool = false;
		byte random = (byte)new Random().nextInt(2);
		return (random==1);
	}
	public static short CheckLooping(BufferedReader bbr ) throws Exception {
		short countss = 1;
		String sst;
		while ((sst = bbr.readLine()) != null)
			if(sst.equals("[HitObjects]")) {
				//System.out.println("File Start Line" + countss); //It should be print the first note timeline.
				return countss;
			} else { countss++; }
		return countss;
	}
}
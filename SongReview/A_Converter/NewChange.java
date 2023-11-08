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
		short count = 0; short counttt = 15; int anotherCount = 0; int checkOneButton; int checkOneButtonTemp; int loop = CheckLooping(br);
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

		counttt = 1;
		count = 0;
		do {
			try{
				if(arcSave[count].substring((counttt-1),counttt).equals(",")) {
					arcSave[count] = arcSave[count].substring(counttt,arcSave[count].length());
					System.out.println(arcSave[count]); //Remove first,
					count++;
					counttt = 1;
				} else { counttt++; }
			}catch (Exception b) {
				count++;
				counttt = 1;
			}
		}while(arcSave[count] != null);


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
					} //System.out.print(i+": "); System.out.println(arcSave[i]); //(Remove first,)
				}
			}
		}

		counttt = 0;
		count = 0;
		do {
			try{
				if(arcSave[count].substring(counttt,(counttt+1)).equals(",")) {
					arcSave[count] = arcSave[count].substring(0,counttt);
					count++;
					counttt = 0;
					//System.out.println(arcSave[count]); //It should be number only
				} else { counttt++; }
			}catch (Exception e) {
				count++;
				counttt = 0;
			}
		}while(arcSave[count] != null);

		counttt = 0;
		for(int i=0; i<arcSave.length; i++) {
			if(IsArcEndByRandom() == true || counttt == 0) {
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
		File files = new File("..\\A_Converter\\2.aff"); //File Write in to 2.aff under this java program folder.
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
	public static String Type() { //Return a random string type.
		byte random = (byte)new Random().nextInt(7);
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
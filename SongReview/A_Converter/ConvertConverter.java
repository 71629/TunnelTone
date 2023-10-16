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
		File files = new File("..\\A_Converter\\ConvertEnd.txt");
		System.out.println("");
		System.out.println("ConvertEnd.txt has been create in Desktop!");
		System.out.println("");
		FileWriter fw = new FileWriter("..\\A_Converter\\ConvertEnd.txt");
		System.out.println("Writing ConvertEnd.txt...");
		counttt = 0;
		do {
			fw.write(arcSave[counttt] + "\r\n");
			System.out.println("Write Line" + counttt + ": " + arcSave[counttt]);
			counttt++;
		}while(arcSave[counttt] != null);
		fw.close();
		System.out.println("Writing Success!");
		System.out.println("Please directly copy the word in ConvertEnd.txt File");
		//System.out.print("Remember change the file location in line 9, 70 and 74 of this program to read and write the file!!!");
	}
}
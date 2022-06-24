import java.io.FileWriter;
import java.io.IOException;
import java.util.concurrent.ThreadLocalRandom;

/*
 Generates the terrain file 
*/
public class CVSGEN2 {

    public static void main(String[] args) {
        int x = 100;
        int y = 100;
        int count = x * y;
        int minwater = 30;
        int maxwater = 100;
        int minnut = 10;
        int maxnut = 100;

        int[][] csv = new int[count][4];

        for (int i = 0; i < count; i++) {
            int[] cur = csv[i];
            cur[2] = ThreadLocalRandom.current().nextInt(minwater,maxwater);
            cur[3]= ThreadLocalRandom.current().nextInt(minnut,maxnut);
        }
        for (int i = 0; i < x; i++) {
            for (int j = 0; j < y; j++) {
                csv[count-1][0]=i;
                csv[count-1][1]=j;
                count--;
            }
        }
        StringBuilder sb = new StringBuilder("x;y;waterlevel;nutrients\n");

        for (int[] row: csv) {
            for (int cell: row) {
                sb.append(cell);
                sb.append(";");
            }
            sb.deleteCharAt(sb.length()-1);
            sb.append("\n");
        }
        try(FileWriter fw = new FileWriter("resource/terrain_gen.csv")){
            fw.write(sb.toString());
        }catch (IOException e){e.printStackTrace();}


    }
}

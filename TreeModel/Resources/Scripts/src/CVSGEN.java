import java.io.FileWriter;
import java.io.IOException;
import java.util.concurrent.ThreadLocalRandom;

public class CVSGEN {
    public static void main(String[] args) {
        int x = 100;
        int y = 100;
        int count = 400;
        int[][] csv = new int[x][y];

        while (count > 0){
            int rx = ThreadLocalRandom.current().nextInt(x);
            int ry = ThreadLocalRandom.current().nextInt(y);
            if (csv[rx][ry] == 0){
                csv[rx][ry] = ThreadLocalRandom.current().nextInt(1,4);
                count--;
            }
        }
        StringBuilder sb = new StringBuilder();
        for (int[] row: csv) {
            for (int cell: row) {
                sb.append(cell);
                sb.append(";");
            }
            sb.deleteCharAt(sb.length()-1);
            sb.append("\n");
        }
        try(FileWriter fw = new FileWriter("resource/g.csv")){
            fw.write(sb.toString());
        }catch (IOException e){e.printStackTrace();}
    }
}

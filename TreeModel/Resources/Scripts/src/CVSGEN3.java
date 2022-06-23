import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ThreadLocalRandom;

public class CVSGEN3 {

    public static void main(String[] args) {
        int x = 100;
        int y = 100;
        int humanAmount = 0;
        int treeAmount = 250;
        int animalAmount = 100;
       int [][] humans = new int[humanAmount][2];
       int [][] trees = new int[treeAmount][2];
       int [][] animals = new int[animalAmount][2];
        List<int[]> usedCoordinates = new ArrayList<>();

        for (int i = 0; i < humanAmount; i++) {
            humans[i][0] = ThreadLocalRandom.current().nextInt(x);
            humans[i][1] = ThreadLocalRandom.current().nextInt(y);

        }
        for (int i = 0; i < animalAmount; i++) {
            animals[i][0] = ThreadLocalRandom.current().nextInt(x);
            animals[i][1] = ThreadLocalRandom.current().nextInt(y);

        }
        for (int i = 0; i < treeAmount; i++) {
            int rx = ThreadLocalRandom.current().nextInt(x);
            int ry = ThreadLocalRandom.current().nextInt(y);
            while (usedCoordinates.contains(new int[]{rx,ry})){
                 rx = ThreadLocalRandom.current().nextInt(x);
                 ry = ThreadLocalRandom.current().nextInt(y);
            }
            trees[i][0] = rx;
            trees[i][1] = ry;
            usedCoordinates.add(trees[i]);

        }

        StringBuilder sb = new StringBuilder("x;y;type\n");

        for (int[] row: trees) {
            for (int cell: row) {
                sb.append(cell);
                sb.append(";");
            }
            sb.append("tree\n");
        }

        for (int[] row: animals) {
            for (int cell: row) {
                sb.append(cell);
                sb.append(";");
            }
            sb.append("animal\n");
        }

        for (int[] row: humans) {
            for (int cell: row) {
                sb.append(cell);
                sb.append(";");
            }
            sb.append("human\n");
        }
        try(FileWriter fw = new FileWriter("resource/g3.csv")){
            fw.write(sb.toString());
        }catch (IOException e){e.printStackTrace();}


    }
}

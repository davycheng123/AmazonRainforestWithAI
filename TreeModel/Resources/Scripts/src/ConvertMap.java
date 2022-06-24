import java.io.BufferedReader;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.Objects;
/*
Converts a geodata map to a initialposition file and back.
Needs to be compiled!
*/
public class ConvertMap {

    public static void main(String[] args) {
        switch (args.length){
            case 3 -> ConvertTo(args[0],args[1],args[2]);
            case 4 -> ConvertBack(args[0],args[1],args[2],args[3]);
            default -> System.out.println("usage: java ConvertMap [map_with_geodata] {optional: map_with_rasterdata} [x] [y]");
        }
    }
    
    private static void ConvertTo(String file, String xS, String yS){
        int x = Integer.parseInt(xS);
        int y = Integer.parseInt(yS);
        try(FileWriter fw = new FileWriter("converted_map.csv");
            BufferedReader br = new BufferedReader(new FileReader(file))){
            fw.write("x;y;type\n");
            br.readLine();
            for (int i = 0; i < y; i++) {
                for (int j = 0; j < x; j++) {
                    String height = br.readLine().split(";")[0];
                    if (Objects.equals(height, "0.00000000")) continue;
                    fw.write(j+";"+i+";"+"tree\n");
                }
            }

        }catch (IOException e){e.printStackTrace();}
    }
    private static void ConvertBack(String file0, String file1, String xS,String yS){

        int x = Integer.parseInt(xS);
        int y = Integer.parseInt(yS);
        String[][][] dict = new String[y][x][2];
        try(BufferedReader br = new BufferedReader(new FileReader(file0))){
            br.readLine();
            for (int i = 0; i < y; i++) {
                for (int j = 0; j < x; j++) {
                    String[] line = br.readLine().split(";");
                    dict[i][j][0] = line[2];
                    dict[i][j][1] = line[3];
                }
            }
        }catch (IOException e){e.printStackTrace();}

        try(BufferedReader br = new BufferedReader(new FileReader(file1));
            FileWriter fw = new FileWriter("converted_tree.csv")){
            String header = br.readLine();
            fw.write(header +"\n");
            String line = br.readLine();
            while (line != null){
                String[] nline = line.split(";");
                if (nline.length == 20) {
                    int i = Integer.parseInt(nline[18]);
                    int j = Integer.parseInt(nline[17]);
                    nline[17] = dict[i][j][0];
                    nline[18] = dict[i][j][1];
                    fw.write(String.join(";", nline) + "\n");
                }
                line = br.readLine();
            }
        }catch (IOException e){e.printStackTrace();}
    }
}

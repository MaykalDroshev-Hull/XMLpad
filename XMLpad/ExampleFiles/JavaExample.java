import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.Scanner;

public class AverageCalculator {
    public static void main(String[] args) {
        String filename = "numbers.txt";
        ArrayList<Double> numbers = new ArrayList<>();
        double sum = 0.0;

        try {
            Scanner scanner = new Scanner(new File(filename));
            while (scanner.hasNextDouble()) {
                double x = scanner.nextDouble();
                numbers.add(x);
                sum += x;
            }
            scanner.close();
        } catch (FileNotFoundException e) {
            System.err.println("File not found: " + filename);
            System.exit(1);
        }

        if (numbers.size() > 0) {
            double average = sum / numbers.size();
            System.out.println("The average of the numbers is " + average);
        } else {
            System.out.println("No numbers found in file.");
        }
    }
}

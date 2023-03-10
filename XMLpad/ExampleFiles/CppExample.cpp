#include <iostream>
#include <fstream>
#include <vector>

int main() {
    std::ifstream infile("numbers.txt");
    std::vector<double> numbers;
    double sum = 0.0;

    double x;
    while (infile >> x) {
        numbers.push_back(x);
        sum += x;
    }

    if (numbers.size() > 0) {
        double average = sum / numbers.size();
        std::cout << "The average of the numbers is " << average << std::endl;
    } else {
        std::cout << "No numbers found in file." << std::endl;
    }

    return 0;
}

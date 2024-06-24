# DiceRoller

DiceRoller is a simple C# console application that simulates rolling multiple dice, calculates statistics on the results, and displays the output.

## Installation
1. Go to the releases.
2. Download the latest version of DiceRoller.zip.
3. Extract the contents of the zip file.
4. Open the DiceRoller.exe file to run the application.

## Running the Application
When you run the application, you will be prompted for the following inputs:

1. **Number of dice:** Enter the total number of dice to be rolled in each round.
2. **Number of sides on each die:** Enter the number of sides on each die (e.g., 6 for a standard six-sided die).
3. **Number of dice values to sum:** Enter the number of highest dice values to sum. For example, if you roll 4 dice and choose to sum 3, the program will select the highest 3 values from each roll and sum them. This value must be between 1 and the total number of dice rolled.
4. **Number of rounds:** Enter the number of rounds the simulation should run.

Example input sequence:
```
Enter the number of dice: 4
Enter the number of sides on each die: 6
Enter the number of dice values to sum (always takes the highest values, must be between 1 and 4): 3
Enter the number of rounds to test: 1000000
```
Example Output
```
Average result after 1,000,000 rounds: 12.247899
Minimum roll: 3
Maximum roll: 18
Median roll: 12
Standard deviation: 2.845837501616551
95% Confidence Interval: [12.242321213977789, 12.253476896984125]
Time taken: 494 ms

Frequency Distribution and Graph:
 3:     777 (0.08%)
 4:    3020 (0.30%)
 5: #   7663 (0.77%)
 6: ##  16186 (1.62%)
 7: ####  29203 (2.92%)
 8: #######  47849 (4.78%)
 9: ##########  70062 (7.01%)
10: ##############  94306 (9.43%)
11: ################# 114005 (11.40%)
12: ################### 129079 (12.91%)
13: #################### 132380 (13.24%)
14: ################## 123726 (12.37%)
15: ############### 100949 (10.09%)
16: ###########  72820 (7.28%)
17: ######  41786 (4.18%)
18: ##  16189 (1.62%)
```

# WiFiAnalyzer
This is a console utility that scans for WiFi networks and writes the details to a text file. It can also connect to a specified open network.

## Usage

1. Open a command prompt as administrator.
2. Navigate to the directory containing the utility.
3. Run the utility as a command-line argument. For example:

    ```
    .\WiFiAnalyzer.exe
    ```
4. You will be prompted with the following options:
    - Enter 'all' to see all SSIDs.
    - Enter 'open' to see only open networks.
    - Enter 'connect' to connect to a selected network (only for open networks).
5. If you choose to connect to a network, you will be asked to enter the SSID of the network you want to connect to.
6. The application will attempt to connect to the specified network and print a success or failure message.

## Output

The application writes the details of the found networks to a text file on the Desktop in the "WiFiAnalyzer" folder. The filename includes the current date and time.

## Author

Bohdan Harabadzhyu

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Note

This application uses the NativeWifi API and requires the appropriate permissions to scan for WiFi networks and connect to them.

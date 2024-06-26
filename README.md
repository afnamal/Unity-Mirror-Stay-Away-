## Stay Away - README

![Stay Away](https://firebasestorage.googleapis.com/v0/b/chat-api-aa04a.appspot.com/o/stayAwayPhotos%2Fstay.PNG?alt=media&token=852faee6-27bc-4abf-a40d-ad8cd159e2f5)

**Stay Away** is a multiplayer game built with Unity and Mirror, where two players can join and play over a local network using different devices. The game consists of 4 levels, each becoming progressively more challenging as the height of the central pillar increases. The user interface is created using Unity's UI Toolkit.

### How to Play

1. **Starting the Game:**
   - The first player to click the "Start Game" button becomes the host.
   - The second player to click the "Start Game" button joins as a client.
   - The host waits for the client to join before starting the game.

2. **Gameplay:**
   - The game involves two tanks trying to shoot each other until one wins.
   - Both tanks have 100 health points.
   - Each hit reduces the health by 10 points.
   - Ten hits result in a loss.
   - The game includes 4 levels, with the difficulty increasing as the height of the central pillar increases.

3. **Controls:**
   - **Android:**
     - Tap on the left half of the screen to move left.
     - Tap on the right half of the screen to move right.
     - Use the fire button to shoot.
   - **Windows:**
     - Use the arrow keys to move your tank.
     - Press `Ctrl` to shoot.

### Installation and Setup

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/yourusername/stay-away.git
   ```

2. **Open the Project:**
   - Open Unity Hub.
   - Click on "Open" and navigate to the cloned repository's folder.
   - Select the folder to open the project in Unity.

3. **Build and Run:**
   - Open `File > Build Settings`.
   - Ensure that all the required scenes are added to the build.
   - Select your target platform and click on "Build and Run".

### Network Configuration

- **Host and Client:**
  - The first player to start the game becomes the host.
  - The second player to join becomes the client.
  - Ensure both devices are connected to the same local network.

### Technologies Used

- **Unity**: Game engine used to develop Stay Away.
- **Mirror**: Networking library used for local multiplayer functionality.
- **UI Toolkit**: Used to create the user interface.

### Screenshots

![Screenshot 1](https://firebasestorage.googleapis.com/v0/b/chat-api-aa04a.appspot.com/o/stayAwayPhotos%2Fstay2.PNG?alt=media&token=0fcf3953-c04f-42c6-b0fe-7e9ee1d48ebd)
![Screenshot 2](https://firebasestorage.googleapis.com/v0/b/chat-api-aa04a.appspot.com/o/stayAwayPhotos%2Fstay3.PNG?alt=media&token=4e6fc758-a9be-4313-b508-ba04c8b45ec5)

### Troubleshooting

- Ensure both devices are on the same local network.
- Check firewall settings to allow Unity to communicate over the network.
- Restart the game if the connection drops unexpectedly.

### Contributions

Contributions are welcome! Please fork the repository and submit pull requests.

### License

This project is licensed under the MIT License.

### Contact

For any inquiries or support, please contact me at afnamal@hotmail.com.

---

Enjoy playing Stay Away!

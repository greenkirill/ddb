using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using msg.lib;

namespace msg.client {
    public class Messenger {

        public Messenger(string hostname, int port) {
            client = new Client(hostname, port);

            client.ProfileRecieved += ProfileRecieve;

            clientTask = new Task(client.Start);
            clientTask.Start();
        }
        public byte Screen { get; private set; } = 1;
        public Client client { get; private set; }
        public Task clientTask { get; private set; }

        private bool go = true;

        private Profile Profile;

        public void ProfileRecieve(Profile Profile) {
            Console.WriteLine("dasd");
            this.Profile = Profile;
            ToScreen2();
        }

        private void ToScreen2() {
            Screen2_Help();
            Screen = 2;
        }

        public void Start() {
            string inp;
            do {
                inp = Console.ReadLine();
                switch (Screen) {
                    case 1:
                        Screen1(inp);
                        break;
                    case 2:
                        Screen2(inp);
                        break;
                    case 3:
                        Screen3(inp);
                        break;
                    case 4:
                        break;
                    default:
                        go = false;
                        break;
                }
            } while (go);
        }

        public void Screen1(string inp) {
            var spl = inp.Split(" ");
            switch (spl[0].ToLower()) {
                case "auth":
                    Screen1_Auth(inp);
                    break;
                case "reg":
                    Screen1_Reg(inp);
                    break;
                case "help":
                    Screen1_Help();
                    break;
                default:
                    Screen1_Help();
                    break;
            }
        }

        public void Screen1_Reg(string inp) {
            var spl = inp.Split(" ");
            if (spl.Length < 3)
                Console.WriteLine("Try that: \nreg USERNAME PASSWORD");
            else {
                Screen = 4;
                client.Registration(spl[1], spl[2]);
            }
        }
        public void Screen1_Auth(string inp) {
            var spl = inp.Split(" ");
            if (spl.Length < 3)
                Console.WriteLine("Try that: \auth USERNAME PASSWORD");
            else {
                Screen = 4;
                client.Auth(spl[1], spl[2]);
            }
        }

        public void Screen1_Help() {
            Console.WriteLine("help\nauth USERNAME PASSWORD\nreg USERNAME PASSWORD");
        }
        public void Screen2_Help() {
            Console.WriteLine("help\nuser list\ndialogue create USERNAME1 USERNAME2 USERNAME3 ... USERNAMEN\ndialogue list\ndialogue join DIALOGUE_NUMBER");
        }
        public void Screen3_Help() {
            Console.WriteLine("press esc for exit");
        }

        public void Screen2(string inp) {

        }
        public void Screen3(string inp) {

        }

    }
}
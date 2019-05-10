using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using msg.lib;

namespace msg.client {
    public class Messenger {

        public Messenger(string hostname, int port) {
            client = new Client(hostname, port);

            client.ProfileRecieved += ProfileRecieve;

            client.UserListRecieved += UserListRecieve;

            client.DialogueListRecieved += DialogueListRecieve;

            // client.MsgListRecieved += ProfileRecieve;

            // client.MsgRecieved += ProfileRecieve;

            clientTask = new Task(client.Start);
            clientTask.Start();

        }
        public byte Screen { get; private set; } = 1;
        public Client client { get; private set; }
        public Task clientTask { get; private set; }

        private bool go = true;

        private Profile Profile;

        private List<Profile> users;

        private List<Dialogue> Dialogues;

        private Guid CurrentDialogueId = Guid.NewGuid();



        public void ProfileRecieve(Profile Profile) {
            this.Profile = Profile;
            ToScreen2();
        }
        public void UserListRecieve(List<Profile> Profile) {
            this.users = Profile;
            Console.WriteLine("User list:");
            for (int i = 0; i < Profile.Count; i++) {
                Console.WriteLine($"{i,3}. {Profile[i].Username}");
            }
        }
        public void DialogueListRecieve(List<Dialogue> Dialogues) {
            this.Dialogues = Dialogues;
            Console.WriteLine("User list:");
            for (int i = 0; i < Dialogues.Count; i++) {
                Console.Write($"{i,3}. members: ");
                var s = "";
                foreach (var u in Dialogues[i].Members)
                    s += $"{u.profile.Username} ";
                Console.WriteLine(s);
            }
        }

        private void ToScreen2() {
            Console.WriteLine("successfully authorised! \n\n");
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
                Console.WriteLine("Try that: \nreg USERNAME PASSWORD\n\n");
            else {
                Screen = 4;
                client.Registration(spl[1], spl[2]);
            }
        }
        public void Screen1_Auth(string inp) {
            var spl = inp.Split(" ");
            if (spl.Length < 3)
                Console.WriteLine("Try that: \nauth USERNAME PASSWORD\n\n");
            else {
                Screen = 4;
                client.Auth(spl[1], spl[2]);
            }
        }

        public void Screen1_Help() {
            Console.WriteLine("Command list:\nhelp\nauth USERNAME PASSWORD\nreg USERNAME PASSWORD\n\n");
        }
        public void Screen2_Help() {
            Console.WriteLine("Command list:\nhelp\nuser-list\ndialogue-create USERNAME1 USERNAME2 USERNAME3 ... USERNAMEN\ndialogue-list\ndialogue-open DIALOGUE_NUMBER\n\n");
        }
        public void Screen3_Help() {
            Console.WriteLine("Command list:\npress esc for exit\n\n");
        }

        public void Screen2(string inp) {

            var spl = inp.Split(" ");
            switch (spl[0].ToLower()) {
                case "user-list":
                    Screen2_UserList(inp);
                    break;
                case "dialogue-create":
                    Screen2_DialogueCreate(inp);
                    break;
                case "dialogue-list":
                    Screen2_DialogueList(inp);
                    break;
                case "dialogue-open":
                    Screen2_DialogueOpen(inp);
                    break;
                case "help":
                    Screen2_Help();
                    break;
                default:
                    Screen2_Help();
                    break;
            }

        }

        private void Screen2_UserList(string inp) {
            client.RUserList();
        }
        private void Screen2_DialogueCreate(string inp) {
            var spl = inp.Split(" ").ToList();
            if (spl.Count < 2) {
                Console.WriteLine("Try that: \ndialogue-create USERNAME1 USERNAME2 USERNAME3 ... USERNAMEN\n\n");

            } else {
                client.DialogueCreate(spl.Skip(1).ToList());
            }
        }
        private void Screen2_DialogueList(string inp) {
            client.RDialogueList();

        }
        private void Screen2_DialogueOpen(string inp) {

        }

        public void Screen3(string inp) {

        }

    }
}
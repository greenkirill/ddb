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

            client.MsgListRecieved += MsgListRecieve;

            client.MsgRecieved += MsgRecieve;

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

        private List<Message> CurrentMessages = new List<Message>();



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

        public void MsgListRecieve(List<Message> messages) {
            CurrentMessages = messages;
            Screen = 3;
            RePaintScreen3();
        }

        public void MsgRecieve(Message message) {
            if (message.Dialogue.ID == CurrentDialogueId) {
                CurrentMessages.Add(message);
                RePaintScreen3();
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
            Console.WriteLine("Command list:");
            Console.WriteLine("help");
            Console.WriteLine("auth USERNAME PASSWORD");
            Console.WriteLine("reg USERNAME PASSWORD\n\n");
        }
        public void Screen2_Help() {
            Console.WriteLine("Command list:");
            Console.WriteLine("help");
            Console.WriteLine("User List: ul");
            Console.WriteLine("Create Dialogue: cd USERNAME1 USERNAME2 USERNAME3 ... USERNAMEN");
            Console.WriteLine("Dialogue List: dl");
            Console.WriteLine("Open Dialogue: od DIALOGUE_NUMBER\n\n");
        }
        public void Screen3_Help() {
            Console.WriteLine("Command list:\npress esc for exit\n\n");
        }

        public void Screen2(string inp) {

            var spl = inp.Split(" ");
            switch (spl[0].ToLower()) {
                case "ul":
                    Screen2_UserList(inp);
                    break;
                case "cd":
                    Screen2_DialogueCreate(inp);
                    break;
                case "dl":
                    Screen2_DialogueList(inp);
                    break;
                case "od":
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
            var spl = inp.Split(" ").ToList();
            if (spl.Count < 2) {
                Console.WriteLine("Try that: \ndialogue-open DIALOGUE_NUMBER\n\n");

            } else {
                Screen = 4;
                var index = int.Parse(spl[1]);
                var D = Dialogues[index];
                CurrentDialogueId = D.ID;
                client.RMsgList(D.ID);
            }
        }

        public void Screen3(string inp) {
            Screen3_SendMsg(inp);
        }

        private void Screen3_SendMsg(string text) {
            client.SendMsg(CurrentDialogueId, text);
            CurrentMessages.Add(new Message {
                ID = Guid.NewGuid(),
                Text = text,
                SentAt = DateTime.Now,
                SentBy = Profile.ID,
                Dialogue = new Dialogue {
                    ID = CurrentDialogueId
                }
            });
            RePaintScreen3();
        }


        private void RePaintScreen3() {
            MessageComparer comparer = new MessageComparer();
            CurrentMessages.Sort(comparer);
            foreach (var m in CurrentMessages) {
                Console.WriteLine($"{m.SentAt.ToString("HH:mm:ss")} {GetUsernameById(m.SentBy),5}: {m.Text}");
            }
            Console.Write("> ");
        }

        private string GetUsernameById(Guid id) {
            foreach (var p in users) {
                if (p.ID == id)
                    return p.Username;
            }
            return "noname";
        }

        class MessageComparer : IComparer<Message> {
            // Compare two Persons.
            public int Compare(Message m1, Message m2) {
                return m1.SentAt.CompareTo(m2.SentAt);
            }
        }
    }
}
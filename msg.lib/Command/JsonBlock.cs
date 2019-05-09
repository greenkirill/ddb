using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace msg.lib {
    public class MetaJson : IBlock {

        public const byte Type = BlockTypeConstants.MetaJson;
        public int Size { get; private set; } = 0;
        public dynamic DJson { get; private set; } = null;

        public MetaJson(string json) {
            DJson = (dynamic)JsonConvert.DeserializeObject(json);
            Size = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(DJson)).Length;
        }
        public MetaJson(dynamic json) {
            DJson = json;
            Size = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(DJson)).Length;
        }
        public MetaJson() { }
        public byte[] GetBytes() {
            var PieceType_bytes = new byte[] { Type };
            var PieceSize_bytes = BitConverter.GetBytes(Size);
            var jString = JsonConvert.SerializeObject(DJson);
            var Json_bytes = Encoding.UTF8.GetBytes(jString);
            var buffer = new byte[5 + Json_bytes.Length];
            Buffer.BlockCopy(PieceType_bytes, 0, buffer, 0, 1);
            Buffer.BlockCopy(PieceSize_bytes, 0, buffer, 1, 4);
            Buffer.BlockCopy(Json_bytes, 0, buffer, 5, Json_bytes.Length);
            return buffer;
        }

        public void SetSize(int size) {
            Size = size;
        }
        public void SetJson(string json) {
            DJson = (dynamic)JsonConvert.DeserializeObject(json);
        }
        public void SetJson(dynamic json) {
            DJson = json;
        }

        public void SetBody(byte[] bd) {
            var json = Encoding.UTF8.GetString(bd);
            DJson = (dynamic)JsonConvert.DeserializeObject(json);
        }
    }
}

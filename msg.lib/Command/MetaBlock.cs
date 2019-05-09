using System;
using System.Collections.Generic;
using System.Text;

namespace msg.lib {
    public interface IBlock {

        byte[] GetBytes();

        void SetSize(int size);

        void SetBody(byte[] body);

    }
}
